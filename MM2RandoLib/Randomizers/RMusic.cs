using MM2Randomizer.Enums;
using MM2Randomizer.Patcher;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace MM2Randomizer.Randomizers
{
    public class Song
    {
        public Song(string songname, string originalStartAddress, string songBytesStr)
        {
            this.OriginalStartAddress = originalStartAddress;
            this.SongName = songname;


            // Parse song bytes from hex string
            List<byte> songBytes = new List<byte>();
            while (songBytesStr.Length > 0)
            {
                string twoCharByte = songBytesStr.Substring(0, 2);
                songBytes.Add(byte.Parse(twoCharByte, NumberStyles.HexNumber));
                songBytesStr = songBytesStr.Remove(0, 2);
            }

            // Header consists of the first 11 bytes, followed by the song data
            this.SongHeader = songBytes.GetRange(0, 11);
            this.SongData = songBytes.GetRange(11, songBytes.Count - 11);

            // Parse channel information
            byte byteSmall = SongHeader[1];
            byte byteLarge = SongHeader[2];
            int absolutePos = byteSmall + (byteLarge * 256);
            Channel1Index = absolutePos - OriginalStartAddressInt - 11;

            byteSmall = SongHeader[3];
            byteLarge = SongHeader[4];
            absolutePos = byteSmall + (byteLarge * 256);
            Channel2Index = absolutePos - OriginalStartAddressInt - 11;

            byteSmall = SongHeader[5];
            byteLarge = SongHeader[6];
            absolutePos = byteSmall + (byteLarge * 256);
            Channel3Index = absolutePos - OriginalStartAddressInt - 11;

            byteSmall = SongHeader[7];
            byteLarge = SongHeader[8];
            if (byteSmall != 0 || byteLarge != 0) // Ignore some songs with no noise channel
            {
                absolutePos = byteSmall + (byteLarge * 256);
                Channel4Index = absolutePos - OriginalStartAddressInt - 11;
            }

            // Parse vibrato information
            byteSmall = songBytes[9];
            byteLarge = songBytes[10];
            absolutePos = byteSmall + (byteLarge * 256);
            VibratoIndex = absolutePos - OriginalStartAddressInt - 11;

            // Count length of vibrato section
            int i = VibratoIndex;
            while (true)
            {
                if (i >= SongData.Count ||
                    i == Channel1Index ||
                    i == Channel2Index ||
                    i == Channel3Index ||
                    i == Channel4Index)
                {
                    VibratoLength = i - VibratoIndex;
                    break;
                }
                i++;
            }
        }

        public enum SongIndex
        {
            Flash,
            Wood,
            Crash,
            Heat,
            Air,
            Metal,
            Quick,
            Bubble,
            Wily12,
            Wily345
        }

        public string SongName { get; set; }
        public byte SongStartPtr1stByte { get; set; }
        public byte SongStartPtr2ndByte { get; set; }
        public List<byte> SongHeader { get; set; }
        public List<byte> SongData { get; set; }

        public int Channel1Index { get; set; }
        public int Channel2Index { get; set; }
        public int Channel3Index { get; set; }
        public int Channel4Index { get; set; }
        public int VibratoIndex { get; set; }
        public int VibratoLength { get; set; }

        public string OriginalStartAddress { get; set; }
        public int OriginalStartAddressInt
        {
            get
            {
                byte byteSmall = byte.Parse(OriginalStartAddress.Substring(2, 2), NumberStyles.HexNumber);
                byte byteLarge = byte.Parse(OriginalStartAddress.Substring(0, 2), NumberStyles.HexNumber);
                return byteSmall + (byteLarge * 256);
            }
        }

        public int TotalLength
        {
            get
            {
                return SongHeader.Count + SongData.Count;
            }
        }
    }

    public class RMusic : IRandomizer
    {
        /// <summary>
        /// The maximum number of bytes that the list of stage songs can be.
        /// </summary>
        //public const int StageSongsSize = 0x20CA; // 8394
        public const int StageSongsSize = 0x24DE; // 8394 + 1044 Expanded size from music prepatch
        
        private StringBuilder debug = new StringBuilder();

        public RMusic() { }

        public override string ToString()
        {
            return debug.ToString();
        }

        public void Randomize(Patch p, Random r)
        {
            debug.AppendLine();
            debug.AppendLine("Random Music Module");
            debug.AppendLine("--------------------------------------------");

            ImportMusic(p, r);
            //OldRando(p, r);
        }

        public void ImportMusic(Patch p, Random r)
        {
            List<Song> songs = new List<Song>();
            List<Song> stageSongs = new List<Song>();
            string[] lines = Properties.Resources.music.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            // Read songs from file, parse and add to list
            foreach (string line in lines)
            {
                if (line.StartsWith("#")) continue; // Ignore comment lines
                string[] lineParts = line.Split(',');

                // Add song to list of songs
                Song song = new Song(lineParts[0], lineParts[1], lineParts[2]);
                songs.Add(song);

                // DEBUG ONLY: TEST ONE SONG AT A TIME
                //for (int i = 0; i < 10; i++)
                //{
                //    songs.Add(new Song(lineParts[0], lineParts[1], lineParts[2]));
                //}
            }
            debug.AppendLine($"{songs.Count} stage songs loaded.");

            // Create a shuffled list of songs
            bool checkBytes = true;
            const int numTracks = 11;
            while (checkBytes)
            {
                // Shuffle and get first 11 songs
                songs.Shuffle(r);
                stageSongs = songs.GetRange(0, numTracks);

                // Count bytes 
                int totalBytes = 0;
                int i = 0;
                foreach (Song song in stageSongs)
                {
                    totalBytes += song.SongHeader.Count;
                    totalBytes += song.SongData.Count;
                    i++;
                }

                // Break if within limit (Redo shuffle if over limit)
                // DEBUG DEBUG
                if (totalBytes <= StageSongsSize)
                {
                    checkBytes = false;
                    debug.AppendLine($"{numTracks} songs selected. {totalBytes} bytes used out of {StageSongsSize} limit.");
                }
                else
                {
                    debug.AppendLine($"{numTracks} songs selected. {totalBytes} bytes, greater than {StageSongsSize} limit. Reshuffled songs.");
                }
            }

            // Write the songs and song info
            int songStartRom = 0x030AE6;
            int TblPtrOffset = 0x30A60; // Start of address pairs which point to song locations
            for (int k = 0; k < stageSongs.Count; k++)
            {
                Song song = stageSongs[k];

                // Calculate start addresses from song lengths and write to address table
                int addressTwoBytes = songStartRom - 0x30010 + 0x8000;
                string addressHex = addressTwoBytes.ToString("X");
                song.SongStartPtr1stByte = byte.Parse(addressHex.Substring(0, 2), NumberStyles.HexNumber);
                song.SongStartPtr2ndByte = byte.Parse(addressHex.Substring(2, 2), NumberStyles.HexNumber);
                
                if (k >= 10) // Must use a different location for the 11th track; use Stage Select at 0x30A78
                {
                    p.Add(0x30A78, song.SongStartPtr2ndByte, $"Song {k} Pointer Offset Byte 1");
                    p.Add(0x30A79, song.SongStartPtr1stByte, $"Song {k} Pointer Offset Byte 2");

                    byte songIndex = 0x0C; // Wily 5 will play the song at the Stage Select address
                    p.Add(0x0381EC, songIndex, $"Wily 5 Song"); 
                    debug.AppendLine($"{Enum.GetName(typeof(EMusicID), (EMusicID)songIndex)} stage song: {song.SongName}, {song.OriginalStartAddress}");
                }
                else
                {
                    p.Add(TblPtrOffset++, song.SongStartPtr2ndByte, $"Song {k} Pointer Offset Byte 1");
                    p.Add(TblPtrOffset++, song.SongStartPtr1stByte, $"Song {k} Pointer Offset Byte 2");
                    debug.AppendLine($"{Enum.GetName(typeof(EMusicID), (EMusicID)k)} stage song: {song.SongName}, {song.OriginalStartAddress}");
                }

                // Header: Calculate 4 channel addresses and vibrato address
                byte origChannel1ByteSmall = song.SongHeader[1];
                byte origChannel1ByteLarge = song.SongHeader[2];
                int origChannel1Offset = origChannel1ByteSmall + (origChannel1ByteLarge * 256);
                int relChannel1Offset = origChannel1Offset - song.OriginalStartAddressInt;
                int newChannel1Offset = addressTwoBytes + relChannel1Offset;
                song.SongHeader[1] = (byte)(newChannel1Offset % 256);
                song.SongHeader[2] = (byte)(newChannel1Offset / 256);

                byte origChannel2ByteSmall = song.SongHeader[3];
                byte origChannel2ByteLarge = song.SongHeader[4];
                int origChannel2Offset = origChannel2ByteSmall + (origChannel2ByteLarge * 256);
                int relChannel2Offset = origChannel2Offset - song.OriginalStartAddressInt;
                int newChannel2Offset = addressTwoBytes + relChannel2Offset;
                song.SongHeader[3] = (byte)(newChannel2Offset % 256);
                song.SongHeader[4] = (byte)(newChannel2Offset / 256);

                byte origChannel3ByteSmall = song.SongHeader[5];
                byte origChannel3ByteLarge = song.SongHeader[6];
                int origChannel3Offset = origChannel3ByteSmall + (origChannel3ByteLarge * 256);
                int relChannel3Offset = origChannel3Offset - song.OriginalStartAddressInt;
                int newChannel3Offset = addressTwoBytes + relChannel3Offset;
                song.SongHeader[5] = (byte)(newChannel3Offset % 256);
                song.SongHeader[6] = (byte)(newChannel3Offset / 256);

                byte origChannel4ByteSmall = song.SongHeader[7];
                byte origChannel4ByteLarge = song.SongHeader[8];
                if (origChannel4ByteSmall > 0 || origChannel4ByteLarge > 0)
                {
                    int origChannel4Offset = origChannel4ByteSmall + (origChannel4ByteLarge * 256);
                    int relChannel4Offset = origChannel4Offset - song.OriginalStartAddressInt;
                    int newChannel4Offset = addressTwoBytes + relChannel4Offset;
                    song.SongHeader[7] = (byte)(newChannel4Offset % 256);
                    song.SongHeader[8] = (byte)(newChannel4Offset / 256);

                    if (relChannel4Offset > song.TotalLength || relChannel4Offset < 0)
                        debug.AppendLine($"WARNING: Song {song.SongName} channel 4 points to a shared location.");
                }

                byte origVibratoByteSmall = song.SongHeader[9];
                byte origVibratoByteLarge = song.SongHeader[10];
                int origVibratoOffset = origVibratoByteSmall + (origVibratoByteLarge * 256);
                int relVibratoOffset = origVibratoOffset - song.OriginalStartAddressInt;
                int newVibratoOffset = addressTwoBytes + relVibratoOffset;
                song.SongHeader[9] = (byte)(newVibratoOffset % 256);
                song.SongHeader[10] = (byte)(newVibratoOffset / 256);

                if (relChannel1Offset > song.TotalLength || relChannel1Offset < 0)
                    debug.AppendLine($"WARNING: Song {song.SongName} channel 1 points to a shared location.");
                if (relChannel2Offset > song.TotalLength || relChannel2Offset < 0)
                    debug.AppendLine($"WARNING: Song {song.SongName} channel 2 points to a shared location.");
                if (relChannel3Offset > song.TotalLength || relChannel3Offset < 0)
                    debug.AppendLine($"WARNING: Song {song.SongName} channel 3 points to a shared location.");
                if (relVibratoOffset > song.TotalLength || relVibratoOffset < 0)
                    debug.AppendLine($"WARNING: Song {song.SongName} vibrato points to a shared location.");


                // Write song header
                foreach (byte b in song.SongHeader)
                {
                    p.Add(songStartRom, b, $"Song Header Byte for {song.SongName}");
                    songStartRom++;
                }

                // Song Data: Traverse stream and change loop pointers
                for (int i = 0; i < song.SongData.Count; i++)
                {
                    //Do not parse loop pointers for vibrato
                    //TODO: Check the length of the vibrato string, or even better, use separate lists for

                    //each channel!
                    if (i >= song.VibratoIndex && i < song.VibratoLength)
                    {
                        continue;
                    }

                    byte b0 = song.SongData[i];

                    // Bisqwit is awesome.
                    // http://www.romhacking.net/forum/index.php?topic=16383.0
                    // http://bisqwit.iki.fi/jutut/megamansource/mm2music.txt
                    switch (b0)
                    {
                        case 0x00: // Two-byte encoding $00 n.  Song speed is set as n frames per tick.
                            i += 1;
                            break;
                        case 0x01: // Two-byte encoding $01 n. Adjusts vibrato parameters by n. Affects all following notes.
                            i += 1;
                            break;
                        case 0x02: // Two-byte encoding $02 n. Selects duty cycle settings. Valid values for n: $00,$40,$80,$C0. Only applicable for squarewave channels.
                            i += 1;
                            break;
                        case 0x03: // Two-byte encoding $03 n. Selects volume and envelope settings. Value n is passed directly to the soundchip; Affects all following notes.
                            i += 1;
                            break;
                        case 0x04: // Four-byte encoding $04 n w. Ends a loop. If n=0, loop is infinite. Otherwise the marked section plays for n+1 times. w is a 16-bit pointer to the beginning of the loop. Finite loops cannot be nested.
                            byte origLoopPtrSmall = song.SongData[i + 2];
                            byte origLoopPtrLarge = song.SongData[i + 3];

                            // Get the loop destination pointer by converting the two bytes to a 16-bit int
                            int origLoopOffset = origLoopPtrSmall + (origLoopPtrLarge * 256);
                            // Find index of destination of the loop with respect to the start of the song
                            int relLoopOffset = origLoopOffset - song.OriginalStartAddressInt; 
                            // Make new loop destination with respect to the new starting location of this song
                            int newLoopOffset = addressTwoBytes + relLoopOffset;

                            // Put new hex bytes back into song data array
                            song.SongData[i + 2] = (byte)(newLoopOffset % 256);
                            song.SongData[i + 3] = (byte)(newLoopOffset / 256);

                            if (relLoopOffset > song.TotalLength || relLoopOffset < 0)
                                debug.AppendLine($"WARNING: Song {song.SongName} has external loop point.");

                            i += 3;
                            break;
                        case 0x05: // Two-byte encoding $05 n. Sets note base to n. Value n is added to the note index for any notes (excluding pauses) played on this channel from now.
                            i += 1;
                            break;
                        case 0x06: // One-byte encoding $06. Dotted note: The next note will be played 50% longer than otherwise, i.e. 3/2 of its stated duration.
                            break;
                        case 0x07: // Three-byte encoding $07 n m. Sets volume curve settings. Byte n controls the attack, and byte m controls the decay. Affects all following notes.
                            i += 2;
                            break;
                        case 0x08: // Two-byte encoding $08 n. Select vibrato entry n from the vibrato table referred to by the song header. Affects all following notes.
                            i += 1;
                            break;
                        case 0x09: // One-byte encoding $09. Ends the track. Can be omitted if the track ends in an infinite loop instead.
                            break;
                        default:
                            // One - byte encoding $20 + n.Note delay(n = 0 - 7): Delays the next note by n ticks, without affecting its overall timing. (I.e.plays silence for the first n ticks of the note.)
                            // One - byte encoding $30.Triplet: The next note will be played at 2 / 3 of its stated duration.
                            // One - byte encoding: m * 0x20 + n.Play note(m = 2..7).If n = 0, plays pause. Otherwise plays note n(note base is added to n). The lowest note that can be played is C - 0(n + base = 0).Note or pause length is 2m−1 ticks, possibly altered by the triplet / dotted modifiers.The next event will be read only after this note/pause is done playing.
                            break;
                    }
                }
                
                // Write song data
                foreach (byte b in song.SongData)
                {
                    p.Add(songStartRom, b, $"Song Data Byte for {song.SongName}");
                    songStartRom++;
                }
            }

            // Play a random stage song during the credits
            Song creditsSong = stageSongs[r.Next(numTracks)];
            p.Add(0x30A88, creditsSong.SongStartPtr2ndByte, $"Song Credits 2 Byte 0 ({creditsSong.SongName})");
            p.Add(0x30A89, creditsSong.SongStartPtr1stByte, $"Song Credits 2 Byte 1 ({creditsSong.SongName})");
            debug.AppendLine($"Credits song: {creditsSong.SongName}");
        }

        public void OldRando(Patch p, Random r)
        {
            List<EMusicID> newBGMOrder = new List<EMusicID>();
            List<EMusicID> robos = new List<EMusicID>();

            // Select 2 replacement tracks for the 2 extra instance of the boring W3/4/5 theme
            robos.Add(EMusicID.Flash);
            robos.Add(EMusicID.Heat);
            robos.Add(EMusicID.Air);
            robos.Add(EMusicID.Wood);
            robos.Add(EMusicID.Quick);
            robos.Add(EMusicID.Metal);
            robos.Add(EMusicID.Clash);
            robos.Add(EMusicID.Bubble);
            robos.Shuffle(r);

            newBGMOrder.Add(EMusicID.Flash);
            newBGMOrder.Add(EMusicID.Heat);
            newBGMOrder.Add(EMusicID.Air);
            newBGMOrder.Add(EMusicID.Wood);
            newBGMOrder.Add(EMusicID.Quick);
            newBGMOrder.Add(EMusicID.Metal);
            newBGMOrder.Add(EMusicID.Clash);
            newBGMOrder.Add(EMusicID.Bubble);
            newBGMOrder.Add(EMusicID.Wily12);
            newBGMOrder.Add(EMusicID.Wily12);  // Wily 1/2 track will play twice
            newBGMOrder.Add(EMusicID.Wily345); // Wily 3/4/5 track only plays once
            newBGMOrder.Add(robos[0]);         // Add extra Robot Master tracks to the set
            newBGMOrder.Add(robos[1]);

            // Randomize tracks
            newBGMOrder.Shuffle(r);

            // Start writing at Heatman BGM ID, both J and U
            // Loop through BGM addresses Heatman to Wily 5 (Wily 6 still silent)
            for (int i = 0; i < newBGMOrder.Count; i++)
            {
                EMusicID bgm = newBGMOrder[i];
                p.Add(0x0381E0 + i, (byte)bgm, String.Format("BGM Stage {0}", i));
            }

            // Finally, fix Wily 5 track when exiting a Teleporter to be the selected Wily 5 track instead of default
            p.Add(0x038489, (byte)newBGMOrder.Last(), "BGM Wily 5 Teleporter Exit Fix");
        }
    }
}

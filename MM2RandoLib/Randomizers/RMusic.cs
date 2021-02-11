using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using MM2Randomizer.Data;
using MM2Randomizer.Enums;
using MM2Randomizer.Extensions;
using MM2Randomizer.Patcher;

namespace MM2Randomizer.Randomizers
{
    public class Song
    {
        public Song(String in_SongName, String in_OriginalStartAddress, String in_SongBytesStr)
        {
            this.OriginalStartAddress = in_OriginalStartAddress;
            this.SongName = in_SongName;

            // Parse song bytes from hex String
            List<Byte> songBytes = new List<Byte>();
            while (in_SongBytesStr.Length > 0)
            {
                String twoCharByte = in_SongBytesStr.Substring(0, 2);
                songBytes.Add(Byte.Parse(twoCharByte, NumberStyles.HexNumber));
                in_SongBytesStr = in_SongBytesStr.Remove(0, 2);
            }

            // Header consists of the first 11 bytes, followed by the song data
            this.SongHeader = songBytes.GetRange(0, 11);
            this.SongData = songBytes.GetRange(11, songBytes.Count - 11);

            // Parse channel information
            Byte byteSmall = SongHeader[1];
            Byte byteLarge = SongHeader[2];
            Int32 absolutePos = byteSmall + (byteLarge * 256);
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
            Int32 i = VibratoIndex;

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

        public String SongName { get; set; }
        public Byte SongStartPtr1stByte { get; set; }
        public Byte SongStartPtr2ndByte { get; set; }
        public List<Byte> SongHeader { get; set; }
        public List<Byte> SongData { get; set; }

        public Int32 Channel1Index { get; set; }
        public Int32 Channel2Index { get; set; }
        public Int32 Channel3Index { get; set; }
        public Int32 Channel4Index { get; set; }
        public Int32 VibratoIndex { get; set; }
        public Int32 VibratoLength { get; set; }

        public String OriginalStartAddress { get; set; }

        public Int32 OriginalStartAddressInt
        {
            get
            {
                Byte byteSmall = Byte.Parse(OriginalStartAddress.Substring(2, 2), NumberStyles.HexNumber);
                Byte byteLarge = Byte.Parse(OriginalStartAddress.Substring(0, 2), NumberStyles.HexNumber);
                return byteSmall + (byteLarge * 256);
            }
        }

        public Int32 TotalLength
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
        private const Int32 StageSongsSize = 0x24DE; // 8394 + 1044 Expanded size from music prepatch

        private StringBuilder debug = new StringBuilder();

        public RMusic() { }

        public override String ToString()
        {
            return debug.ToString();
        }

        public void Randomize(Patch p, Random r)
        {
            debug.AppendLine();
            debug.AppendLine("Random Music Module");
            debug.AppendLine("--------------------------------------------");

            ImportMusic(p, r);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="p"></param>
        /// <param name="r"></param>
        public void ImportMusic(Patch p, Random r)
        {
            List<Song> songs = new List<Song>();
            List<Song> stageSongs = new List<Song>();

            SoundTrackSet soundTrackSet = Properties.Resources.SoundTrackConfiguration.Deserialize<SoundTrackSet>();

            // Read songs from file, parse and add to list
            foreach (SoundTrack soundTrack in soundTrackSet)
            {
                if (true == soundTrack.Enabled)
                {
                    songs.Add(new Song(soundTrack.Title, soundTrack.StartAddress, soundTrack.TrackData));
                }
            }

            debug.AppendLine($"{songs.Count} stage songs loaded.");

            // Create a shuffled list of songs
            Boolean checkBytes = true;
            const Int32 numTracks = 11;

            while (checkBytes)
            {
                // Shuffle and get first 11 songs
                songs.Shuffle(r);
                stageSongs = songs.GetRange(0, numTracks);

                // Count bytes 
                Int32 totalBytes = 0;
                Int32 i = 0;

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
            Int32 songStartRom = 0x030AE6;
            Int32 TblPtrOffset = 0x30A60; // Start of address pairs which point to song locations

            for (Int32 k = 0; k < stageSongs.Count; k++)
            {
                Song song = stageSongs[k];

                // Calculate start addresses from song lengths and write to address table
                Int32 addressTwoBytes = songStartRom - 0x30010 + 0x8000;
                String addressHex = addressTwoBytes.ToString("X");
                song.SongStartPtr1stByte = Byte.Parse(addressHex.Substring(0, 2), NumberStyles.HexNumber);
                song.SongStartPtr2ndByte = Byte.Parse(addressHex.Substring(2, 2), NumberStyles.HexNumber);
                
                if (k >= 10) // Must use a different location for the 11th track; use Stage Select at 0x30A78
                {
                    p.Add(0x30A78, song.SongStartPtr2ndByte, $"Song {k} Pointer Offset Byte 1");
                    p.Add(0x30A79, song.SongStartPtr1stByte, $"Song {k} Pointer Offset Byte 2");

                    Byte songIndex = 0x0C; // Wily 5 will play the song at the Stage Select address
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
                Byte origChannel1ByteSmall = song.SongHeader[1];
                Byte origChannel1ByteLarge = song.SongHeader[2];
                Int32 origChannel1Offset = origChannel1ByteSmall + (origChannel1ByteLarge * 256);
                Int32 relChannel1Offset = origChannel1Offset - song.OriginalStartAddressInt;
                Int32 newChannel1Offset = addressTwoBytes + relChannel1Offset;
                song.SongHeader[1] = (Byte)(newChannel1Offset % 256);
                song.SongHeader[2] = (Byte)(newChannel1Offset / 256);

                Byte origChannel2ByteSmall = song.SongHeader[3];
                Byte origChannel2ByteLarge = song.SongHeader[4];
                Int32 origChannel2Offset = origChannel2ByteSmall + (origChannel2ByteLarge * 256);
                Int32 relChannel2Offset = origChannel2Offset - song.OriginalStartAddressInt;
                Int32 newChannel2Offset = addressTwoBytes + relChannel2Offset;
                song.SongHeader[3] = (Byte)(newChannel2Offset % 256);
                song.SongHeader[4] = (Byte)(newChannel2Offset / 256);

                Byte origChannel3ByteSmall = song.SongHeader[5];
                Byte origChannel3ByteLarge = song.SongHeader[6];
                Int32 origChannel3Offset = origChannel3ByteSmall + (origChannel3ByteLarge * 256);
                Int32 relChannel3Offset = origChannel3Offset - song.OriginalStartAddressInt;
                Int32 newChannel3Offset = addressTwoBytes + relChannel3Offset;
                song.SongHeader[5] = (Byte)(newChannel3Offset % 256);
                song.SongHeader[6] = (Byte)(newChannel3Offset / 256);

                Byte origChannel4ByteSmall = song.SongHeader[7];
                Byte origChannel4ByteLarge = song.SongHeader[8];

                if (origChannel4ByteSmall > 0 || origChannel4ByteLarge > 0)
                {
                    Int32 origChannel4Offset = origChannel4ByteSmall + (origChannel4ByteLarge * 256);
                    Int32 relChannel4Offset = origChannel4Offset - song.OriginalStartAddressInt;
                    Int32 newChannel4Offset = addressTwoBytes + relChannel4Offset;
                    song.SongHeader[7] = (Byte)(newChannel4Offset % 256);
                    song.SongHeader[8] = (Byte)(newChannel4Offset / 256);

                    if (relChannel4Offset > song.TotalLength || relChannel4Offset < 0)
                    {
                        debug.AppendLine($"WARNING: Song {song.SongName} channel 4 points to a shared location.");
                    }
                }

                Byte origVibratoByteSmall = song.SongHeader[9];
                Byte origVibratoByteLarge = song.SongHeader[10];
                Int32 origVibratoOffset = origVibratoByteSmall + (origVibratoByteLarge * 256);
                Int32 relVibratoOffset = origVibratoOffset - song.OriginalStartAddressInt;
                Int32 newVibratoOffset = addressTwoBytes + relVibratoOffset;
                song.SongHeader[9] = (Byte)(newVibratoOffset % 256);
                song.SongHeader[10] = (Byte)(newVibratoOffset / 256);

                if (relChannel1Offset > song.TotalLength || relChannel1Offset < 0)
                {
                    debug.AppendLine($"WARNING: Song {song.SongName} channel 1 points to a shared location.");
                }

                if (relChannel2Offset > song.TotalLength || relChannel2Offset < 0)
                {
                    debug.AppendLine($"WARNING: Song {song.SongName} channel 2 points to a shared location.");
                }

                if (relChannel3Offset > song.TotalLength || relChannel3Offset < 0)
                {
                    debug.AppendLine($"WARNING: Song {song.SongName} channel 3 points to a shared location.");
                }

                if (relVibratoOffset > song.TotalLength || relVibratoOffset < 0)
                {
                    debug.AppendLine($"WARNING: Song {song.SongName} vibrato points to a shared location.");
                }


                // Write song header
                foreach (Byte b in song.SongHeader)
                {
                    p.Add(songStartRom, b, $"Song Header Byte for {song.SongName}");
                    songStartRom++;
                }

                // Song Data: Traverse stream and change loop pointers
                for (Int32 i = 0; i < song.SongData.Count; i++)
                {
                    // Do not parse loop pointers for vibrato
                    // TODO: Check the length of the vibrato String, or even better, use separate lists for each channel!
                    if (i >= song.VibratoIndex && i < song.VibratoLength)
                    {
                        continue;
                    }

                    Byte b0 = song.SongData[i];

                    // Bisqwit is awesome.
                    // http://www.romhacking.net/forum/index.php?topic=16383.0
                    // http://bisqwit.iki.fi/jutut/megamansource/mm2music.txt
                    switch (b0)
                    {
                        // Two-Byte encoding $00 n.  Song speed is set as n
                        // frames per tick.
                        case 0x00:
                        {
                            i += 1;
                            break;
                        }

                        // Two-Byte encoding $01 n. Adjusts vibrato parameters
                        // by n. Affects all following notes.
                        case 0x01:
                        {
                            i += 1;
                            break;
                        }

                        // Two-Byte encoding $02 n. Selects duty cycle settings.
                        // Valid values for n: $00,$40,$80,$C0. Only applicable
                        // for squarewave channels.
                        case 0x02:
                        {
                            i += 1;
                            break;
                        }

                        // Two-Byte encoding $03 n. Selects volume and envelope
                        // settings. Value n is passed directly to the soundchip;
                        // Affects all following notes.
                        case 0x03:
                        {
                            i += 1;
                            break;
                        }

                        // Four-Byte encoding $04 n w. Ends a loop. If n=0, loop is
                        // infinite. Otherwise the marked section plays for n+1 times.
                        // w is a 16-bit pointer to the beginning of the loop.
                        // Finite loops cannot be nested.
                        case 0x04:
                        {
                            Byte origLoopPtrSmall = song.SongData[i + 2];
                            Byte origLoopPtrLarge = song.SongData[i + 3];

                            // Get the loop destination pointer by converting the two bytes to a 16-bit Int32
                            Int32 origLoopOffset = origLoopPtrSmall + (origLoopPtrLarge * 256);
                            // Find index of destination of the loop with respect to the start of the song
                            Int32 relLoopOffset = origLoopOffset - song.OriginalStartAddressInt;
                            // Make new loop destination with respect to the new starting location of this song
                            Int32 newLoopOffset = addressTwoBytes + relLoopOffset;

                            // Put new hex bytes back into song data array
                            song.SongData[i + 2] = (Byte)(newLoopOffset % 256);
                            song.SongData[i + 3] = (Byte)(newLoopOffset / 256);

                            // Validation check when testing out newly ripped songs to make sure I didn't miss any loops
                            if (relLoopOffset > song.TotalLength || relLoopOffset < 0)
                            {
                                debug.AppendLine($"WARNING: Song {song.SongName} has external loop point.");
                            }

                            i += 3;

                            break;
                        }

                        // Two-Byte encoding $05 n. Sets note base to n. Value
                        // n is added to the note index for any notes
                        // (excluding pauses) played on this channel from now.
                        case 0x05:
                        {
                            i += 1;
                            break;
                        }

                        // One-Byte encoding $06. Dotted note: The next note will
                        // be played 50% longer than otherwise, i.e. 3/2 of its
                        // stated duration.
                        case 0x06:
                        {
                            break;
                        }

                        // Three-Byte encoding $07 n m. Sets volume curve settings.
                        // Byte n controls the attack, and Byte m controls the decay.
                        // Affects all following notes.
                        case 0x07:
                        {
                            i += 2;
                            break;
                        }

                        // Two-Byte encoding $08 n. Select vibrato entry n from the
                        // vibrato table referred to by the song header. Affects all
                        // following notes.
                        case 0x08:
                        {
                            i += 1;
                            break;
                        }

                        // One-Byte encoding $09. Ends the track. Can be omitted if
                        // the track ends in an infinite loop instead.
                        case 0x09:
                        {
                            break;
                        }

                        // One - Byte encoding $20 + n.Note delay(n = 0 - 7):
                        //      Delays the next note by n ticks, without affecting
                        //      its overall timing. (I.e.plays silence for the
                        //      first n ticks of the note.)
                        //
                        // One - Byte encoding $30.Triplet:
                        //      The next note will be played at 2 / 3 of its
                        //      stated duration.
                        //
                        // One - Byte encoding:
                        //      m * 0x20 + n.Play note(m = 2..7).If n = 0, plays pause.
                        //      Otherwise, plays note n(note base is added to n). The
                        //      lowest note that can be played is C - 0(n + base = 0).
                        //      Note or pause length is 2m−1 ticks, possibly altered by
                        //      the triplet / dotted modifiers.The next event will be
                        //      read only after this note/pause is done playing.
                        default:
                        {
                            break;
                        }
                    }
                }
                
                // Write song data
                foreach (Byte b in song.SongData)
                {
                    p.Add(songStartRom, b, $"Song Data Byte for {song.SongName}");
                    songStartRom++;
                }
            }

            // Play a random song during wily 6
            Int32 w6song = r.Next(11);
            if (w6song == 10)
            {
                // Wily 5 song was saved to 0x0C, don't use 0x0A  
                p.Add(0x0381ED, 0x0C, $"Random Wily 6 Song: Song #{w6song}, {stageSongs[w6song].SongName}");
            }
            else
            {
                p.Add(0x0381ED, (Byte)w6song, $"Random Wily 6 Song: Song #{w6song}, {stageSongs[w6song].SongName}");
            }
            debug.AppendLine($"Wily 6 stage song: {stageSongs[w6song].SongName} (#{w6song})");

            // Play a random stage song during the credits
            Song creditsSong = stageSongs[r.Next(numTracks)];
            p.Add(0x30A88, creditsSong.SongStartPtr2ndByte, $"Song Credits 2 Byte 0 ({creditsSong.SongName})");
            p.Add(0x30A89, creditsSong.SongStartPtr1stByte, $"Song Credits 2 Byte 1 ({creditsSong.SongName})");
            debug.AppendLine($"Credits song: {creditsSong.SongName}");
        }
    }
}

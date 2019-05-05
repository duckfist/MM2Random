using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MM2Randomizer.Patcher
{
    public class Patch
    {
        public Dictionary<int, ChangeByteRecord> Bytes { get; set; }

        public Patch()
        {
            Bytes = new Dictionary<int, ChangeByteRecord>();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="note"></param>
        public void Add(int address, byte value, string note = "")
        {
            ChangeByteRecord newByte = new ChangeByteRecord(address, value, note);

            // Either replace the byte at the given address, or add it if it doesn't exist
            if (Bytes.ContainsKey(address))
            {
                Bytes[address] = newByte;
            }
            else
            {
                Bytes.Add(address, newByte);
            }
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="filename"></param>
        public void ApplyRandoPatch(string filename)
        {
            using (var stream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
            { 
                //GetStringSortedByAddress();

                foreach (KeyValuePair<int, ChangeByteRecord> kvp in Bytes)
                {
                    stream.Position = kvp.Key;
                    stream.WriteByte(kvp.Value.Value);
                }
            }
        }
        public string GetStringSortedByAddress()
        {
            var sortDict = from kvp in Bytes orderby kvp.Key ascending select kvp;
            return ConvertDictToString(sortDict);
        }

        public string GetString()
        {
            return ConvertDictToString((IOrderedEnumerable<KeyValuePair<int, ChangeByteRecord>>)Bytes);
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private string ConvertDictToString(IOrderedEnumerable<KeyValuePair<int, ChangeByteRecord>> dict)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<int, ChangeByteRecord> kvp in dict)
            {
                ChangeByteRecord b = kvp.Value;
                sb.Append($"0x{b.Address:X6}\t{b.Value:X2}\t{b.Note}");
                sb.Append(Environment.NewLine);
            }
            return sb.ToString();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="romname"></param>
        /// <param name="patchBytes"></param>
        public void ApplyIPSPatch(string romname, byte[] patchBytes)
        {
            // Noobish Noobsicle wrote this IPS patching code
            // romname is the original ROM, patchname is the patch to apply
            FileStream romstream = new FileStream(romname, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            int lint = patchBytes.Length;
            byte[] ipsbyte = patchBytes;
            byte[] rombyte = new byte[romstream.Length];
            IAsyncResult romresult;
            int ipson = 5;
            int totalrepeats = 0;
            int offset = 0;
            bool keepgoing = true;
            while (keepgoing == true)
            {
                offset = ipsbyte[ipson] * 0x10000 + ipsbyte[ipson + 1] * 0x100 + ipsbyte[ipson + 2];
                ipson++;
                ipson++;
                ipson++;
                if (ipsbyte[ipson] * 256 + ipsbyte[ipson + 1] == 0)
                {
                    ipson++;
                    ipson++;
                    totalrepeats = ipsbyte[ipson] * 256 + ipsbyte[ipson + 1];
                    ipson++;
                    ipson++;
                    byte[] repeatbyte = new byte[totalrepeats];
                    for (int ontime = 0; ontime < totalrepeats; ontime++)
                        repeatbyte[ontime] = ipsbyte[ipson];
                    romstream.Seek(offset, SeekOrigin.Begin);
                    romresult = romstream.BeginWrite(repeatbyte, 0, totalrepeats, null, null);
                    romstream.EndWrite(romresult);
                    ipson++;
                }
                else
                {
                    totalrepeats = ipsbyte[ipson] * 256 + ipsbyte[ipson + 1];
                    ipson++;
                    ipson++;
                    romstream.Seek(offset, SeekOrigin.Begin);
                    romresult = romstream.BeginWrite(ipsbyte, ipson, totalrepeats, null, null);
                    romstream.EndWrite(romresult);
                    ipson = ipson + totalrepeats;
                }
                if (ipsbyte[ipson] == 69 && ipsbyte[ipson + 1] == 79 && ipsbyte[ipson + 2] == 70)
                    keepgoing = false;
            }
            romstream.Close();
        }
    }
}

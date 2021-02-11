using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace MM2Randomizer.Patcher
{
    public class Patch
    {
        public Dictionary<Int32, ChangeByteRecord> Bytes { get; set; }

        public Patch()
        {
            Bytes = new Dictionary<Int32, ChangeByteRecord>();
        }

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="note"></param>
        public void Add(Int32 address, Byte value, String note = "")
        {
            ChangeByteRecord newByte = new ChangeByteRecord(address, value, note);

            // Either replace the Byte at the given address, or add it if it doesn't exist
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
        /// <param name="address"></param>
        /// <param name="value"></param>
        /// <param name="note"></param>
        public Int32 Add(Int32 in_StartAddress, Byte[] in_Value, String note = "")
        {
            Int32 index = 0;

            foreach (Byte b in in_Value)
            {
                this.Add(in_StartAddress++, b, $"{note}[{index++}]");
            }

            return in_StartAddress;
        }


        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="filename"></param>
        public void ApplyRandoPatch(String filename)
        {
            using (FileStream stream = new FileStream(filename, FileMode.Open, FileAccess.ReadWrite))
            {
                //GetStringSortedByAddress();

                foreach (KeyValuePair<Int32, ChangeByteRecord> kvp in Bytes)
                {
                    stream.Position = kvp.Key;
                    stream.WriteByte(kvp.Value.Value);
                }
            }
        }


        public String GetStringSortedByAddress()
        {
            IOrderedEnumerable<KeyValuePair<Int32, ChangeByteRecord>> sortDict =
                from kvp in Bytes orderby kvp.Key ascending select kvp;

            return ConvertDictToString(sortDict);
        }


        public String GetString()
        {
            return ConvertDictToString((IOrderedEnumerable<KeyValuePair<Int32, ChangeByteRecord>>)Bytes);
        }


        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private String ConvertDictToString(IOrderedEnumerable<KeyValuePair<Int32, ChangeByteRecord>> dict)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<Int32, ChangeByteRecord> kvp in dict)
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
        public void ApplyIPSPatch(String romname, Byte[] patchBytes)
        {
            // Noobish Noobsicle wrote this IPS patching code
            // romname is the original ROM, patchname is the patch to apply
            FileStream romstream = new FileStream(romname, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            Int32 lint = patchBytes.Length;
            Byte[] ipsbyte = patchBytes;
            Byte[] rombyte = new Byte[romstream.Length];
            IAsyncResult romresult;
            Int32 ipson = 5;
            Int32 totalrepeats = 0;
            Int32 offset = 0;
            Boolean keepgoing = true;

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
                    Byte[] repeatbyte = new Byte[totalrepeats];

                    for (Int32 ontime = 0; ontime < totalrepeats; ontime++)
                    {
                        repeatbyte[ontime] = ipsbyte[ipson];
                    }

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
                {
                    keepgoing = false;
                }
            }
            romstream.Close();
        }
    }
}

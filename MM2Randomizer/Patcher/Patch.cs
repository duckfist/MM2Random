using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MM2Randomizer.Patcher
{
    public class Patch
    {
        public Dictionary<int, ChangeByteRecord> Bytes { get; set; }

        public Patch()
        {
            Bytes = new Dictionary<int, ChangeByteRecord>();
        }

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

        public void GetStringSortedByAddress()
        {
            var sortDict = from kvp in Bytes orderby kvp.Key ascending select kvp;
            foreach (KeyValuePair<int, ChangeByteRecord> kvp in sortDict)
            {
                ChangeByteRecord b = kvp.Value;
                System.Diagnostics.Debug.WriteLine(
                    "0x{0:X6}\t{1:X2}\t{2}",
                    b.Address,
                    b.Value,
                    b.Note);
            }
        }

        public void GetString()
        {
            foreach (KeyValuePair<int, ChangeByteRecord> kvp in Bytes)
            {
                ChangeByteRecord b = kvp.Value;
                System.Diagnostics.Debug.WriteLine(
                    "0x{0:X6}\t{1:X2}\t{2}",
                    b.Address,
                    b.Value,
                    b.Note);
            }
        }

        public static void ApplyIPSPatch(string romname, string patchname)
        {
            // Noobish Noobsicle wrote this IPS patching code
            // romname is the original ROM, patchname is the patch to apply
            FileStream romstream = new FileStream(romname, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            FileStream ipsstream = new FileStream(patchname, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            int lint = (int)ipsstream.Length;
            byte[] ipsbyte = new byte[ipsstream.Length];
            byte[] rombyte = new byte[romstream.Length];
            IAsyncResult romresult;
            IAsyncResult ipsresult = ipsstream.BeginRead(ipsbyte, 0, lint, null, null);
            ipsstream.EndRead(ipsresult);
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
            ipsstream.Close();
        }
    }
}

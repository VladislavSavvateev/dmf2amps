using System;
using System.IO;
using System.Text;

namespace dmf2amps.Models {
    public static class StaticBinaryReader {
        public static string ReadString(Stream stream, int length) {
            var buf = new byte[length];
            stream.Read(buf, 0, length);
            return Encoding.ASCII.GetString(buf);
        }

        public static uint ReadUInt32(Stream stream) {
            var buf = new byte[4];
            stream.Read(buf, 0, 4);
            return BitConverter.ToUInt32(buf, 0);
        }
    }
}
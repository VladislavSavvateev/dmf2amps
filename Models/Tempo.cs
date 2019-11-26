using System.IO;

namespace dmf2amps.Models {
    public class Tempo {
        public byte TimeBase { get; }
        public byte EvenTime { get; }
        public byte OddTime { get; }

        public Tempo(Stream br) {
            TimeBase = (byte) (br.ReadByte() + 1);
            EvenTime = (byte) (br.ReadByte() * TimeBase);
            OddTime = (byte) (br.ReadByte() * TimeBase);
        }
    }
}
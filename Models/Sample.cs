using System.IO;

namespace dmf2amps.Models {
    public class Sample {
        public string SampleName { get; }
        public byte SampleRate { get; }
        public byte SamplePitch { get; }
        public byte SampleAmp { get; }
        public byte SampleBits { get; }
        
        public short[] SampleData { get; }

        public Sample(BinaryReader br) {
            SampleData = new short[br.ReadUInt32()];

            SampleName = StaticBinaryReader.ReadString(br.BaseStream, br.ReadByte());
            SampleRate = br.ReadByte();
            SamplePitch = br.ReadByte();
            SampleAmp = br.ReadByte();
            SampleBits = br.ReadByte();

            for (var i = 0; i < SampleData.Length; i++) SampleData[i] = br.ReadInt16();
        }
    }
}
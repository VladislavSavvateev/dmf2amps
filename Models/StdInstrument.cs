using System.IO;

namespace dmf2amps.Models {
    public class StdInstrument {
        public uint[] VolumeMacro { get; }
        public sbyte VolumeMacroLoopPosition { get; }
        
        public int[] ArpeggioMacro { get; }
        public sbyte ArpeggioLoopPosition { get; }
        public byte ArpeggioMacroMode { get; }
        
        public uint[] DutyNoiseMacro { get; }
        public sbyte DutyNoiseMacroLoopPosition { get; }
        
        public uint[] WavetableMacro { get; }
        public sbyte WavetableMacroLoopPosition { get; }
        
        public StdInstrument(BinaryReader br) {
            VolumeMacro = new uint[br.ReadByte()];
            for (var i = 0; i < VolumeMacro.Length; i++) VolumeMacro[i] = br.ReadUInt32();
            VolumeMacroLoopPosition = (sbyte)(VolumeMacro.Length != 0 ? br.ReadSByte() : -1);

            ArpeggioMacro = new int[br.ReadByte()];
            for (var i = 0; i < ArpeggioMacro.Length; i++) ArpeggioMacro[i] = br.ReadInt32();
            ArpeggioLoopPosition = (sbyte)(ArpeggioMacro.Length != 0 ? br.ReadByte() : -1);
            ArpeggioMacroMode = br.ReadByte();
            
            DutyNoiseMacro = new uint[br.ReadByte()];
            for (var i = 0; i < DutyNoiseMacro.Length; i++) DutyNoiseMacro[i] = br.ReadUInt32();
            DutyNoiseMacroLoopPosition = (sbyte)(DutyNoiseMacro.Length != 0 ? br.ReadSByte() : -1);
            
            WavetableMacro = new uint[br.ReadByte()];
            for (var i = 0; i < WavetableMacro.Length; i++) WavetableMacro[i] = br.ReadUInt32();
            WavetableMacroLoopPosition = (sbyte)(WavetableMacro.Length != 0 ? br.ReadSByte() : -1);
        }
    }
}
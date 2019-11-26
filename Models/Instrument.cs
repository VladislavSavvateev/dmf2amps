using System;
using System.IO;
using System.Text;

namespace dmf2amps.Models {
	public class Instrument {
		public enum InstrumentType { FM = 1, STD = 0 }

		public string Name { get; }
		public InstrumentType Type { get; }
		public FMInstrument FM { get; }
		public StdInstrument Std { get; }

		public Instrument(BinaryReader br) {
			Name = GetString(br);

			Type = (InstrumentType) br.ReadByte();
			if (Type == InstrumentType.FM) FM = new FMInstrument(br);
			else Std = new StdInstrument(br);
		}

		public override string ToString() {
			return Name;
		}

		private static string GetString(BinaryReader br) {
			var length = br.ReadByte();
			var raw = new byte[length];
			br.Read(raw, 0, length);
			return Encoding.ASCII.GetString(raw);
		}
	}
}

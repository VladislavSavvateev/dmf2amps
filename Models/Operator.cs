using System.IO;

namespace dmf2amps.Models {
	public class Operator {
		public byte AM { get; }
		public byte AR { get; }
		public byte DR { get; }
		public byte MULT { get; }
		public byte RR { get; }
		public byte SL { get; }
		public byte TL { get; }
		public byte DT2 { get; }
		public byte RS { get; }
		public byte DT { get; }
		public byte D2R { get; }
		public byte SsgMode { get; }

		public Operator(BinaryReader br) {
			AM = br.ReadByte();
			AR = br.ReadByte();
			DR = br.ReadByte();
			MULT = br.ReadByte();
			RR = br.ReadByte();
			SL = br.ReadByte();
			TL = br.ReadByte();
			DT2 = br.ReadByte();
			RS = br.ReadByte();
			DT = br.ReadByte();
			D2R = br.ReadByte();
			SsgMode = br.ReadByte();
		}

	}
}

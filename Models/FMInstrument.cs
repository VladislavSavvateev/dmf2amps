using System.IO;

namespace dmf2amps.Models {
	public class FMInstrument {
		public byte ALG { get; }
		public byte FB { get; }
		public byte PMS { get; }
		public byte AMS { get; }
		public Operator[] Operators { get; }

		public FMInstrument(BinaryReader br) {
			ALG = br.ReadByte();
			FB = br.ReadByte();
			PMS = br.ReadByte();
			AMS = br.ReadByte();

			Operators = new Operator[4];
			for (var i = 0; i < 4; i++)
				Operators[i] = new Operator(br);
		}
	}
}

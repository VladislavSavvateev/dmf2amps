using System.IO;

namespace dmf2amps.Models {
	public class FMInstrument {
		public byte ALG { get; }
		public byte FB { get; }
		public byte PMS { get; }
		public byte AMS { get; }
		public Operator[] Operators { get; }
		public byte Offset { get; set; }

		public FMInstrument(BinaryReader br) {
			ALG = br.ReadByte();
			FB = br.ReadByte();
			PMS = br.ReadByte();
			AMS = br.ReadByte();

			Operators = new Operator[4];
			for (var i = 0; i < 4; i++)
				Operators[i] = new Operator(br);
		}

		public string GenerateDetune()
			=> $"${DetuneFixes[Operators[0].DT]:X2}, ${DetuneFixes[Operators[1].DT]:X2}, ${DetuneFixes[Operators[2].DT]:X2}, ${DetuneFixes[Operators[3].DT]:X2}";

		private string GenerateMultiple() => $"${Operators[0].MULT:X2}, ${Operators[1].MULT:X2}, ${Operators[2].MULT:X2}, ${Operators[3].MULT:X2}";
		private string GenerateRateScale() => $"${Operators[0].RS:X2}, ${Operators[1].RS:X2}, ${Operators[2].RS:X2}, ${Operators[3].RS:X2}";
		private string GenerateAttackRate() => $"${Operators[0].AR:X2}, ${Operators[1].AR:X2}, ${Operators[2].AR:X2}, ${Operators[3].AR:X2}";
		private string GenerateAmpMod() => $"${Operators[0].AM:X2}, ${Operators[1].AM:X2}, ${Operators[2].AM:X2}, ${Operators[3].AM:X2}";
		private string GenerateSustainRate() => $"${Operators[0].D2R:X2}, ${Operators[1].D2R:X2}, ${Operators[2].D2R:X2}, ${Operators[3].D2R:X2}";
		private string GenerateSustainLevel() => $"${Operators[0].SL:X2}, ${Operators[1].SL:X2}, ${Operators[2].SL:X2}, ${Operators[3].SL:X2}";
		private string GenerateDecayRate() => $"${Operators[0].DR:X2}, ${Operators[1].DR:X2}, ${Operators[2].DR:X2}, ${Operators[3].DR:X2}";
		private string GenerateReleaseRate() => $"${Operators[0].RR:X2}, ${Operators[1].RR:X2}, ${Operators[2].RR:X2}, ${Operators[3].RR:X2}";
		private string GenerateSSGEG() => $"${Operators[0].SsgMode:X2}, ${Operators[1].SsgMode:X2}, ${Operators[2].SsgMode:X2}, ${Operators[3].SsgMode:X2}";
		private string GenerateTotalLevel() => $"${Operators[0].TL:X2}, ${Operators[1].TL:X2}, ${Operators[2].TL:X2}, ${Operators[3].TL:X2}";

		public override string ToString()
			=> $"\tspAlgorithm\t${ALG:X2}\n" +
			   $"\tspFeedback\t${FB:X2}\n" +
			   $"\tspDetune\t{GenerateDetune()}\n" +
			   $"\tspMultiple\t{GenerateMultiple()}\n" +
			   $"\tspRateScale\t{GenerateRateScale()}\n" +
			   $"\tspAttackRt\t{GenerateAttackRate()}\n" +
			   $"\tspAmpMod\t{GenerateAmpMod()}\n" +
			   $"\tspDecayRt\t{GenerateSustainRate()}\n" +
			   $"\tspSustainLv\t{GenerateSustainLevel()}\n" +
			   $"\tspSustainRt\t{GenerateDecayRate()}\n" +
			   $"\tspReleaseRt\t{GenerateReleaseRate()}\n" +
			   $"\tspSSGEG\t{GenerateSSGEG()}\n" +
			   $"\tspTotalLv\t{GenerateTotalLevel()}\n\n";

		private static byte[] DetuneFixes = new byte[] {7, 6, 5, 4, 1, 2, 3};
		
		/*
		 * public override string ToString()
			=> $"\tspAlgorithm\t${ALG:X2}\n" +
			   $"\tspFeedback\t${FB:X2}\n" +
			   $"\tspDetune\t{GenerateDetune()}\n" +
			   $"\tspMultiple\t{GenerateMultiple()}\n" +
			   $"\tspRateScale\t{GenerateRateScale()}\n" +
			   $"\tspAttackRt\t{GenerateAttackRate()}\n" +
			   $"\tspAmpMod\t{GenerateAmpMod()}\n" +
			   $"\tspSustainRt\t{GenerateSustainRate()}\n" +
			   $"\tspSustainLv\t{GenerateSustainLevel()}\n" +
			   $"\tspDecayRt\t{GenerateDecayRate()}\n" +
			   $"\tspReleaseRt\t{GenerateReleaseRate()}\n" +
			   $"\tspSSGEG\t{GenerateSSGEG()}\n" +
			   $"\tspTotalLv\t{GenerateTotalLevel()}\n\n";
		 */
	}
}

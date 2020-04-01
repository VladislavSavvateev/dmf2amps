﻿using System.IO;

namespace dmf2amps.Models {
	public class Channel {
		public Pattern[] Patterns { get; }

		public Channel(DMF dmf, BinaryReader br, byte[] matrixValues) {
			Patterns = new Pattern[dmf.TotalRowsInPatternMatrix];

			var effectsCount = br.ReadByte();
			for (var i = 0; i < dmf.TotalRowsInPatternMatrix; i++) 
				Patterns[i] = new Pattern(dmf, br, effectsCount, matrixValues[i]);
		}
	}
}

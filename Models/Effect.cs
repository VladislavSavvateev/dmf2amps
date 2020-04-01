﻿using System;
 using System.IO;

namespace dmf2amps.Models {
	public class Effect {
		public short Code { get; }
		public short Value { get; }

		public Effect(BinaryReader br) {
			Code = br.ReadInt16();
			Value = br.ReadInt16();
		}

		public override bool Equals(object obj) {
			if (!(obj is Effect e)) throw new ArgumentException();
			
			return e.Code == Code && e.Value == Value;
		}

		public override int GetHashCode() {
			var hashCode = -1218297442;
			hashCode = hashCode * -1521134295 + Code.GetHashCode();
			hashCode = hashCode * -1521134295 + Value.GetHashCode();
			return hashCode;
		}

		public string ToAmpsCoord() {
			if (Code == 8) {
				var pan = "\tsPan\t";
				
				if ((Value & 0b1111) != 0 && Value >> 4 == 0) pan += "spRight\n";
				else if ((Value & 0b1111) == 0 && Value >> 4 != 0) pan += "spLeft\n";
				else pan += "spCenter\n";

				return pan;
			}

			return null;
		}
	}
}

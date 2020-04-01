﻿using System;
 using System.IO;
 using dmf2amps.Models.Amps;
 using dmf2amps.Models.Amps.Commands;

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

		public IEntity ToAmpsCoord() {
			switch (Code) {
				case 8: return new SetPan(Value);
			}
			
			return null;
		}
	}
}

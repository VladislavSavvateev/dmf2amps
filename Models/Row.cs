﻿using System;
using System.Collections.Generic;
using System.IO;
 using System.Linq;

 namespace dmf2amps.Models {
	public class Row {
		public UInt16 Note { get; }
		public UInt16 Octave { get; }
		public short Volume { get; }
		public Effect[] Effects { get; }
		public short Instrument { get; }

		public Row(byte effectsCount, BinaryReader br) {
			Effects = new Effect[effectsCount];

			Note = br.ReadUInt16();
			Octave = br.ReadUInt16();
			Volume = br.ReadInt16();
			for (var i = 0; i < effectsCount; i++) 
				Effects[i] = new Effect(br);
			
			Instrument = br.ReadInt16();
		}

		public override string ToString() {
			var result =
				$"{(Note == 100 ? "NOFF" : Note != 0 ? Amps.Note.Values[Note] : "")}{(Note == 100 ? "" : Note != 0 ? Octave.ToString() : "")} " +
				$"{(Volume != -1 ? Volume.ToString("X") : "")} " +
				$"{(Instrument != -1 ? Instrument.ToString("X") : "")}";

			return Effects.Where(t => t.Code != -1 && t.Value != -1).Aggregate(result, (current, t) => current + $" {t.Code:X}|{t.Value:X}");
		}

		public override bool Equals(object obj) {
			if (!(obj is Row r)) throw new ArgumentException();

			if (r.Note != Note || r.Octave != Octave || r.Volume != Volume || r.Instrument != Instrument ||
			    r.Effects.Length != Effects.Length)
				return false;

			return !Effects.Where((t, i) => !r.Effects[i].Equals(t)).Any();
		}

		public override int GetHashCode() {
			var hashCode = 721073902;
			hashCode = hashCode * -1521134295 + Note.GetHashCode();
			hashCode = hashCode * -1521134295 + Octave.GetHashCode();
			hashCode = hashCode * -1521134295 + Volume.GetHashCode();
			hashCode = hashCode * -1521134295 + EqualityComparer<Effect[]>.Default.GetHashCode(Effects);
			hashCode = hashCode * -1521134295 + Instrument.GetHashCode();
			return hashCode;
		}
	}
}

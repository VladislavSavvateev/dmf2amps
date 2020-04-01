﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace dmf2amps.Models {
	public class Pattern {
		public Row[] Rows { get; }
		public string AmpsName { get; set; }
		
		public byte MatrixValue { get; }

		public Pattern(DMF dmf, BinaryReader br, byte effectsCount, byte matrixValue) {
			Rows = new Row[dmf.TotalRowsPerPattern];
			MatrixValue = matrixValue;

			for (var i = 0; i < dmf.TotalRowsPerPattern; i++) 
				Rows[i] = new Row(effectsCount, br);
		}

		public override bool Equals(object obj) {
			if (!(obj is Pattern p)) throw new ArgumentException();
			
			if (p.Rows.Length != Rows.Length) return false;
			return !p.Rows.Where((t, i) => !t.Equals(Rows[i])).Any();
		}

		public override int GetHashCode() {
			return 1393792888 + EqualityComparer<Row[]>.Default.GetHashCode(Rows);
		}
	}
}

﻿using System;

namespace dmf2amps.Models {
	public class PatternPair {
		public Pattern p;
		public String name;

		public PatternPair(Pattern p, string name) {
			this.p = p;
			this.name = name;
		}
	}
}

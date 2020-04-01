namespace dmf2amps.Models.Amps {
    public class Note : IEntity {
        public static readonly string[] Values = {
            "",
            "nCs",
            "nD",
            "nEb",
            "nE",
            "nF",
            "nFs",
            "nG",
            "nAb",
            "nA",
            "nBb",
            "nB",
            "nC"
        };

        public const string nRst = "nRst";
        public const string sHold = "sHold";
        
        public string Value { get; set; }
        
        public int Octave { get; }
        
        public int Duration { get; set; }

        public Note(string value, int duration) {
            Value = value;
            Duration = duration;
            Octave = -1;
        }

        public Note(int value, int octave, int duration) {
            Value = Values[value];
            Octave = octave;
            Duration = duration;

            if (value == 12) Octave++;
        }

        public override string Generate() => !string.IsNullOrWhiteSpace(Value)
            ? $"dc.b\t{Value}{(Octave != -1 ? Octave.ToString() : "")}, ${Duration:X2}"
            : $"dc.b\t${Duration:X2}";

        public string[] ToDcB() => string.IsNullOrWhiteSpace(Value)
            ? new[] {$"${Duration:X2}"}
            : new[] {$"{Value}{(Octave != -1 ? Octave.ToString() : "")}", $"${Duration:X2}"};

        public override string ToString() => Generate();
    }
}
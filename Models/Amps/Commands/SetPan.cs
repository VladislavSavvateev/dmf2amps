namespace dmf2amps.Models.Amps.Commands {
    public class SetPan : ICommand {
        public string Value { get; }

        public SetPan(int value) {
            if ((value & 0b1111) != 0 && value >> 4 == 0)
                Value = "spRight";
            else if ((value & 0b1111) == 0 && value >> 4 != 0)
                Value = "spLeft";
            else
                Value = "spCenter\n";
        }

        public override string Generate() => $"sPan\t{Value}";
    }
}
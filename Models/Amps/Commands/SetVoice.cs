namespace dmf2amps.Models.Amps.Commands {
    public class SetVoice : ICommand {
        public int Value { get; }

        public SetVoice(int value) { Value = value; }

        public override string Generate() => $"sVoice\t${Value:X2}";
    }
}
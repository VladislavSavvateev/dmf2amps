namespace dmf2amps.Models.Amps.Commands {
    public class SetVolume : ICommand {
        public int Value { get; }

        public SetVolume(int value) { Value = value; }

        public override string Generate() => $"ssVol\t${Value:X2}";
    }
}
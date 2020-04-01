namespace dmf2amps.Models.Amps {
    public abstract class IEntity {
        public abstract string Generate();

        public override string ToString() => Generate();
    }
}
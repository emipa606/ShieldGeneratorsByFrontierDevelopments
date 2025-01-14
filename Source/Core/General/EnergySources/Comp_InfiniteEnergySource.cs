namespace FrontierDevelopments.General.EnergySources
{
    public class Comp_InfiniteEnergySource : BaseEnergySource
    {
        protected override string SaveKey => "InfiniteSource";

        private CompProperties_InfiniteEnergySource Props => (CompProperties_InfiniteEnergySource) props;

        public override float AmountAvailable => float.PositiveInfinity;

        public override float TotalAvailable => float.PositiveInfinity;

        public override float MaxRate => Props.rate;

        public override float Provide(float amount)
        {
            return 0f;
        }

        public override float Consume(float amount)
        {
            amount = base.Consume(amount);
            return amount > RateAvailable ? RateAvailable : amount;
        }
    }
}
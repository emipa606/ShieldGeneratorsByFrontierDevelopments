using Verse;

namespace FrontierDevelopments.General.EnergySources
{
    public class Comp_SingleUseEnergySource : BaseEnergySource
    {
        protected float _charge;
        protected override string SaveKey => "SingleUseSource";

        private CompProperties_SingleUseEnergySource Props => (CompProperties_SingleUseEnergySource) props;

        public virtual float MinimumCharge => 0f;

        public override float AmountAvailable => _charge;

        public override float TotalAvailable => Props.charge;

        public override float MaxRate => Props.rate;

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            if (_charge < 0)
            {
                _charge = Props.charge;
            }
        }

        public override string CompInspectStringExtra()
        {
            return "Charge: " + _charge;
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref _charge, "charge");
        }

        public override float Consume(float amount)
        {
            amount = base.Consume(amount);
            if (amount >= _charge)
            {
                _charge = 0f;
                OnEmpty();
                return _charge;
            }

            _charge -= amount;
            return amount;
        }

        protected virtual void OnEmpty()
        {
        }
    }
}
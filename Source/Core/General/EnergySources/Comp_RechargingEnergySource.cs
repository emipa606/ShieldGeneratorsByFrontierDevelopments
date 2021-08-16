using Verse;

namespace FrontierDevelopments.General.EnergySources
{
    public class Comp_RechargingEnergySource : Comp_SingleUseEnergySource
    {
        private bool _offlineRecharging;
        protected override string SaveKey => "RechargingSource";

        private CompProperties_RechargingEnergySource Props => (CompProperties_RechargingEnergySource) props;

        protected virtual float ChargeRate => Props.rate;

        public override float MinimumCharge => Props.minimum;

        public override void CompTick()
        {
            if (_charge + ChargeRate <= Props.charge)
            {
                _charge += ChargeRate;
            }
            else
            {
                _charge = Props.charge;
            }

            if (_offlineRecharging && _charge >= Props.onlineRechargeAmount)
            {
                _offlineRecharging = false;
            }
        }

        public override void PostExposeData()
        {
            base.PostExposeData();
            Scribe_Values.Look(ref _offlineRecharging, "offlineRecharging");
        }

        protected override void OnEmpty()
        {
            _offlineRecharging = true;
        }
    }
}
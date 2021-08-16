using System;
using System.Linq;
using Verse;

namespace FrontierDevelopments.Shields.Linear
{
    public class ShieldLinearWantLinkUiComponent : ShieldLinearUiComponent
    {
        public ShieldLinearWantLinkUiComponent(ILinearShield self, ILinearShield other,
            Action<bool> fieldRenderOverride, Action buttonAction)
        {
            Self = self;
            Other = other;
            FieldRenderOverride = fieldRenderOverride;
            ButtonAction = buttonAction;

            var distance = self.Position.DistanceTo(other.Position);
            PowerNeeded = LinearShieldUtility.CellProtectionFactorCost(distance) * distance;
            Efficiency =
                LinearShieldUtility.CalculateFieldEfficiency(distance, self.EfficientRange, other.EfficientRange);
        }

        private ILinearShield Self { get; }
        protected override ILinearShield Other { get; }

        protected override bool Blocked => LinearShieldUtility.BlockingBetween(Self, Other).Any();

        protected override string ButtonKey => "FrontierDevelopments.General.Cancel";
        protected override float PowerNeeded { get; }
        protected override float Efficiency { get; }

        protected override Action<bool> FieldRenderOverride { get; }

        protected override Action ButtonAction { get; }
    }
}
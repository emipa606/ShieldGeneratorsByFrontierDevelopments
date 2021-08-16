using System;
using HarmonyLib;

namespace FrontierDevelopments.Shields.Linear
{
    public class ShieldLinearLinkUiComponent : ShieldLinearUiComponent
    {
        public ShieldLinearLinkUiComponent(LinearShieldLink link, ILinearShield self)
        {
            Link = link;
            Self = self;
        }

        private LinearShieldLink Link { get; }

        private ILinearShield Self { get; }
        protected override ILinearShield Other => Link.Other(Self);
        protected override string ButtonKey => "fd.shields.linear.unlink.label";
        protected override float PowerNeeded => Link.CellProtectionFactor * Link.Distance;
        protected override float Efficiency => Link.Efficiency;

        protected override bool Blocked => Link.FieldBlocked;

        protected override Action<bool> FieldRenderOverride => value => { Link.FieldRenderOverride = value; };

        protected override Action ButtonAction => () =>
        {
            Link.WantUnlink = true;
            Link.Emitters.Do(emitter => emitter.NotifyWantSettings());
        };
    }
}
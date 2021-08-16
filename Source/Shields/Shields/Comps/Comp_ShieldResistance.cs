using System.Collections.Generic;
using System.Linq;
using Verse;

namespace FrontierDevelopments.Shields.Comps
{
    public class Comp_ShieldResistance : ThingComp, IShieldResists
    {
        private readonly Dictionary<string, ShieldResistance> resists = new Dictionary<string, ShieldResistance>();

        public virtual float? Apply(ShieldDamages damages)
        {
            var primaryResist = CanResist(damages.Primary);
            if (primaryResist == null)
            {
                return null;
            }

            return primaryResist.Value + CannotResist(damages.Secondaries);
        }

        public virtual float? CanResist(ShieldDamage damage)
        {
            if (!resists.ContainsKey(damage.def.defName))
            {
                return damage.Damage;
            }

            var resist = resists[damage.def.defName];
            if (resist.resist)
            {
                return damage.Damage * resist.multiplier;
            }

            return null;
        }

        public virtual float CannotResist(ShieldDamage damage)
        {
            var multiplier = 1.0f;
            if (resists.ContainsKey(damage.def.defName))
            {
                multiplier = resists[damage.def.defName].multiplier;
            }

            return damage.Damage * multiplier;
        }

        public virtual float CannotResist(List<ShieldDamage> damages)
        {
            return damages.Aggregate(0f, (sum, damage) => sum + CannotResist(damage));
        }

        public override void Initialize(CompProperties props)
        {
            base.Initialize(props);
            ((CompProperties_ShieldResistance) props)?.resists?.ForEach(resist =>
            {
                resists.Add(resist.damageDefName, resist);
            });
        }
    }
}
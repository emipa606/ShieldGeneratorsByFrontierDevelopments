using System.Collections.Generic;
using System.Linq;

namespace FrontierDevelopments.Shields
{
    public class ShieldDamages
    {
        public ShieldDamages(ShieldDamage primary)
        {
            Primary = primary;
        }

        public float Factor { get; set; }

        public ShieldDamage Primary { get; }

        public List<ShieldDamage> Secondaries { get; } = new List<ShieldDamage>();

        public int OverrideDamage { get; set; }

        public int DamageLimit { get; set; } = 500;

        public float Damage => Secondaries.Aggregate(Primary.Damage, (sum, damage) => sum + damage.Damage) * Factor;

        public void Add(ShieldDamage damage)
        {
            Secondaries.Add(damage);
        }
    }
}
using Verse;

namespace FrontierDevelopments.Shields
{
    public class ShieldDamage
    {
        public readonly DamageDef def;

        public ShieldDamage(DamageDef def, float amount)
        {
            this.def = def;
            Damage = amount;
        }

        public float Damage { get; }
    }
}
namespace FrontierDevelopments.Shields.Comps
{
    public class CompProperties_ShieldRadial : CompProperties_ShieldBase
    {
        public int maxRadius;
        public int minRadius;

        public float powerPerTile = 0.1f;

        public int ticksPerExpansion;

        public CompProperties_ShieldRadial()
        {
            compClass = typeof(Comp_ShieldRadial);
        }
    }
}
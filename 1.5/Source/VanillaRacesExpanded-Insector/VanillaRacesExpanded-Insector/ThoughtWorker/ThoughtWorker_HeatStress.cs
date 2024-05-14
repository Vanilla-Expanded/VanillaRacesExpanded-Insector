
using RimWorld;
using Verse;
namespace VanillaRacesExpandedInsector
{
    public class ThoughtWorker_HeatStress : ThoughtWorker
    {
        public const float FireRadius = 20.9f;

        protected override ThoughtState CurrentStateInternal(Pawn p)
        {
            if (!ModsConfig.BiotechActive)
            {
                return ThoughtState.Inactive;
            }
            if (p.genes == null || !p.genes.HasGene(InternalDefOf.VRE_Heatstress) || !ThoughtWorker_Pyrophobia.NearFire(p))
            {
                return ThoughtState.Inactive;
            }
            return ThoughtState.ActiveAtStage(0);
        }

       
    }
}
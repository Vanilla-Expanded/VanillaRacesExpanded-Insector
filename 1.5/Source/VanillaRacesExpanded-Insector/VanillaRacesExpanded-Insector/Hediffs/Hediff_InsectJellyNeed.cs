
using System.Collections.Generic;
using RimWorld;
using Verse;
namespace VanillaRacesExpandedInsector
{
    public class Hediff_InsectJellyNeed : HediffWithComps
    {

        public override bool ShouldRemove
        {
            get
            {
                return !this.pawn.HasActiveGene(InternalDefOf.VRE_InsectJellyDependency);
            }
        }

        public override void Tick()
        {
            base.Tick();
            if (pawn.IsHashIntervalTick(500))
            {
                var gene = pawn.genes.GetFirstGeneOfType<Gene_Resource_InsectJelly>();
                if (gene == null) { return; }
                if (gene.Resource.Value>0) {
                    pawn.health.RemoveHediff(this);

                }
                
                
            }
        }



    }
}
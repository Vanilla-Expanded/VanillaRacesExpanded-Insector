
using System.Collections.Generic;
using RimWorld;
using Verse;
namespace VanillaRacesExpandedInsector
{
    public class Hediff_PollutionNeed : HediffWithComps
    {
    
        public override bool ShouldRemove
        {
            get
            {          
                    return !this.pawn.HasActiveGene(InternalDefOf.VRE_PollutionDependency); 
            }
        }

        public override void Tick()
        {
            base.Tick();
            if (pawn.IsHashIntervalTick(500))
            {
                if (pawn.needs?.TryGetNeed<Need_Pollution>()?.CurLevel <= 0)
                {
                    // Just chill. God this code looks awful, but how else to catch the null
                }
                else { 
                    pawn.health.RemoveHediff(this); 
                }
            }
        }



    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;
using UnityEngine;


namespace VanillaRacesExpandedInsector
{
    public class JobGiver_ConsumeInsectJelly : ThinkNode_JobGiver
    {
        public override float GetPriority(Pawn pawn)
        {

            Pawn_GeneTracker genes = pawn.genes;
            if (genes != null && genes.GetFirstGeneOfType<Gene_Resource_InsectJelly>() == null)
            {
                return 0f;
            }
            return 9.1f;
        }
        protected override Job TryGiveJob(Pawn pawn)
        {

      
            var gene = pawn.genes.GetFirstGeneOfType<Gene_Resource_InsectJelly>();
            if (gene == null) { return null; }
            if (!gene.ShouldConsumeNow()) { return null; }

            Thing jelly = GetInsectJelly(pawn);
            if (jelly == null) { return null; }
            Job job = JobMaker.MakeJob(InternalDefOf.VRE_ConsumeInsectJelly, jelly);
            job.count = 1;
            return job;
        }
        public static Thing GetInsectJelly(Pawn pawn)
        {

            return GenClosest.ClosestThing_Global_Reachable(pawn.Position, pawn.Map, pawn.Map.listerThings.ThingsOfDef(ThingDefOf.InsectJelly), PathEndMode.Touch, TraverseParms.For(pawn), validator: (Thing t) =>
            {
                return pawn.CanReserve(t) && !t.IsForbidden(pawn);
            });
        }
    }
}

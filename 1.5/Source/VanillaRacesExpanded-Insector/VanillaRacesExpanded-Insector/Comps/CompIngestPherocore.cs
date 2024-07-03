using System.Collections.Generic;
using System;
using System.Text;
using Verse;
using Verse.AI;

namespace VanillaRacesExpandedInsector
{



    public class CompIngestPherocore : ThingComp
    {


        public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
        {

            if (selPawn != null && selPawn.genes?.HasActiveGene(InternalDefOf.VRE_GenelineEvolution)==true)
            {
                yield return new FloatMenuOption("VRE_IngestPherocore".Translate(this.parent.LabelCap), () =>
                {

                    if (selPawn.CanReserveAndReach(this.parent, PathEndMode.OnCell, Danger.Deadly))
                    {
                        Job makeJob = JobMaker.MakeJob(InternalDefOf.VRE_IngestPherocore, this.parent);
                        makeJob.haulMode = HaulMode.ToCellNonStorage;
                        makeJob.count = 1;
                        selPawn.jobs?.TryTakeOrderedJob(makeJob);


                    }

                });
            }

        }


    }
}

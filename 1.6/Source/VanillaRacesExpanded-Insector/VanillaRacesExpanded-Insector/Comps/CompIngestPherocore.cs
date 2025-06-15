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

                if((this.parent.def == InternalDefOf.VFEI2_PherocoreSorne && !GameComponent_UnlockedGenes.Instance.allSorneGenesUnlocked)||
                    (this.parent.def == InternalDefOf.VFEI2_PherocoreNuchadus && !GameComponent_UnlockedGenes.Instance.allNuchadusGenesUnlocked)||
                    (this.parent.def == InternalDefOf.VFEI2_PherocoreChelis && !GameComponent_UnlockedGenes.Instance.allChelisGenesUnlocked) ||
                    (this.parent.def == InternalDefOf.VFEI2_PherocoreKemian && !GameComponent_UnlockedGenes.Instance.allKemiaGenesUnlocked) ||
                    (this.parent.def == InternalDefOf.VFEI2_PherocoreXanides && !GameComponent_UnlockedGenes.Instance.allXanidesGenesUnlocked) ||
                    (this.parent.def == DefDatabase<ThingDef>.GetNamedSilentFail("VFEI2_PherocoreBlack") && !GameComponent_UnlockedGenes.Instance.allBlackGenesUnlocked)
                    )
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
}

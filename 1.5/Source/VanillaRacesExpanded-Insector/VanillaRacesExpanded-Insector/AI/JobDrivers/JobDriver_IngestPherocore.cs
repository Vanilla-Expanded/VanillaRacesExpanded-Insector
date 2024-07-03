
using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaRacesExpandedInsector
{
    public class JobDriver_IngestPherocore : JobDriver
    {
        private int useDuration = 100;

      

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            job.count = 1;
            this.FailOnIncapable(PawnCapacityDefOf.Manipulation);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
            

            Toil toil = Toils_General.Wait(useDuration);
            toil.WithProgressBarToilDelay(TargetIndex.A);
            toil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            if (job.targetA.IsValid)
            {
                toil.FailOnDespawnedOrNull(TargetIndex.A);

            }
            yield return toil;
            Toil use = new Toil();

            use.initAction = delegate
            {
                Pawn actor = use.actor;
                Messages.Message("VRE_PherocoreConsumed".Translate(), pawn, MessageTypeDefOf.PositiveEvent, true);
                TargetA.Thing.Destroy();

            };
            use.defaultCompleteMode = ToilCompleteMode.Instant;
            yield return use;
            yield break;

        }
    }
}

using RimWorld;
using System.Linq;
using Verse;
namespace VanillaRacesExpandedInsector
{
    [HotSwappable]
    public class MetapodHediff : HediffWithComps
    {
        public float growthPercent;
        public bool jellyFueled;
        public int ticksSpent;
        public Geneline curGeneline;

        public int TicksEmergesIn
        {
            get
            {
                var duration = FullEmergingDuration;
                if (jellyFueled)
                {
                    duration /= 5f;
                }

                return (int)((1f - growthPercent) * duration);
            }
        }
        public float FullEmergingDuration => GenDate.TicksPerDay * 5f;

        public override void Notify_Spawned()
        {
            base.Notify_Spawned();
            TryAddToMetapod();
        }

        public override string LabelInBrackets => GetProgress().ToStringPercent();

        public float GetProgress()
        {
            return growthPercent;
        }

        public override void CopyFrom(Hediff other)
        {
            base.CopyFrom(other);
            var metapod = other as MetapodHediff;
            growthPercent = metapod.growthPercent;
            ticksSpent = metapod.ticksSpent;
            jellyFueled = metapod.jellyFueled;
            curGeneline = metapod.curGeneline;
        }

        public override void PostAdd(DamageInfo? dinfo)
        {
            base.PostAdd(dinfo);
            var map = pawn.Map;
            if (map != null)
            {
                SpawnMetapod(map);
            }
        }

        private void SpawnMetapod(Map map)
        {
            var pos = pawn.Position;
            bool selected = Find.Selector.SelectedPawns.Contains(pawn);
            var metapod = pawn.MakeMetapod();
            GenPlace.TryPlaceThing(metapod, pos, map, ThingPlaceMode.Direct);
            if (selected) Find.Selector.Select(metapod);
        }

        public override void Tick()
        {
            base.Tick();
            ticksSpent++;
            DoGrowth();
            if (growthPercent >= 1f)
            {
                Complete();
            }
            else
            {
                TryAddToMetapod();
            }
        }

        public void DoGrowth()
        {
            var growthThisTick = 1f / FullEmergingDuration;
            if (jellyFueled)
            {
                growthThisTick *= 5f;
            }
            growthPercent += growthThisTick;
        }

        public void Complete()
        {
            var genelineEvolution = pawn.genes.GetFirstGeneOfType<Gene_GenelineEvolution>();
            if (genelineEvolution != null && curGeneline != null)
            {
                curGeneline.AddPawnDirectly(pawn, genelineEvolution);
            }
            else
            {
                var allGenelineGenes = pawn.genes.GenesListForReading.Where(x => x.def is GenelineGeneDef).ToList();
                foreach (var gene in allGenelineGenes)
                {
                    pawn.genes.RemoveGene(gene);
                }
            }
            genelineEvolution.geneline = curGeneline;
            pawn.health.RemoveHediff(this);
        }

        public override void PostRemoved()
        {
            base.PostRemoved();
            var sickness = pawn.health.AddHediff(InternalDefOf.VRE_MetapodSickness);
            var compDisappears = sickness.TryGetComp<HediffComp_Disappears>();
            compDisappears.ticksToDisappear = compDisappears.Props.disappearsAfterTicks.Lerped(ticksSpent / FullEmergingDuration);
        }

        private void TryAddToMetapod()
        {
            var parentHolder = pawn.ParentHolder as Metapod;
            if (parentHolder is null && pawn.Spawned)
            {
                SpawnMetapod(pawn.Map);
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref growthPercent, "growthPercent");
            Scribe_Values.Look(ref ticksSpent, "ticksSpent");
            Scribe_Values.Look(ref jellyFueled, "jellyFueled");
            Scribe_References.Look(ref curGeneline, "curGeneline");
        }
    }
}
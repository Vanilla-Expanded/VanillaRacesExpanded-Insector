using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
namespace VanillaRacesExpandedInsector
{
    [HotSwappable]
    public class MetapodHediff : HediffWithComps
    {
        public int ticksComplete;
        public Geneline curGeneline;
        public override void Notify_Spawned()
        {
            base.Notify_Spawned();
            TryAddToMetapod();
        }

        public override string LabelInBrackets => GetProgress().ToStringPercent();

        public float GetProgress()
        {
            var diff = ticksComplete - Find.TickManager.TicksGame;
            return 1f - (diff / (float)GenDate.TicksPerDay);
        }

        public override void CopyFrom(Hediff other)
        {
            base.CopyFrom(other);
            var metapod = other as MetapodHediff;
            ticksComplete = metapod.ticksComplete;
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
            if (Find.TickManager.TicksGame >= ticksComplete)
            {
                Complete();
            }
            else
            {
                TryAddToMetapod();
            }
        }

        public void Complete()
        {
            var genelineEvolution = pawn.genes.GetFirstGeneOfType<Gene_GenelineEvolution>();
            var allGenelineGenes = pawn.genes.GenesListForReading.Where(x => x.def is GenelineGeneDef).ToList();
            if (genelineEvolution != null && curGeneline != null)
            {
                MatchGenelineGenes(allGenelineGenes);
            }
            else
            {
                RemoveGeneLineGenes(allGenelineGenes);
            }
            pawn.health.RemoveHediff(this);
        }

        private void MatchGenelineGenes(List<Gene> allGenelineGenes)
        {
            foreach (var gene in allGenelineGenes)
            {
                if (curGeneline.genes.Contains(gene.def) is false)
                {
                    pawn.genes.RemoveGene(gene);
                }
            }
            foreach (var gene in curGeneline.genes)
            {
                if (pawn.genes.GetGene(gene) is null)
                {
                    pawn.genes.AddGene(gene, true);
                }
            }
        }

        private void RemoveGeneLineGenes(List<Gene> allGenelineGenes)
        {
            foreach (var gene in allGenelineGenes)
            {
                pawn.genes.RemoveGene(gene);
            }
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
            Scribe_Values.Look(ref ticksComplete, "ticksComplete");
            Scribe_References.Look(ref curGeneline, "curGeneline");
        }
    }
}
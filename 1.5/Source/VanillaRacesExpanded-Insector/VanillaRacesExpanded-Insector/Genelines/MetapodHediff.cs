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
        public Geneline genelineToRemove;
        public Geneline genelineToAdd;
        public List<GenelineGeneDef> oldGenes = new();
        public List<GenelineGeneDef> newGenes = new();
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
            genelineToRemove = metapod.genelineToRemove;
            genelineToAdd = metapod.genelineToAdd;
            oldGenes = metapod.oldGenes.ToList();
            newGenes = metapod.newGenes.ToList();
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
            if (genelineEvolution != null)
            {
                genelineToAdd?.AddPawnDirectly(pawn, genelineEvolution);
                genelineToRemove?.RemovePawnDirectly(pawn, genelineEvolution);
                if (newGenes.Any())
                {
                    genelineEvolution.geneline.EditGenesDirectly(newGenes, oldGenes, pawn);
                }
            }
            pawn.health.RemoveHediff(this);
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
            Scribe_References.Look(ref genelineToRemove, "genelineToRemove");
            Scribe_References.Look(ref genelineToAdd, "genelineToAdd");
            Scribe_Collections.Look(ref newGenes, "newGenes", LookMode.Def);
            Scribe_Collections.Look(ref oldGenes, "oldGenes", LookMode.Def);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                newGenes ??= new List<GenelineGeneDef>();
                oldGenes ??= new List<GenelineGeneDef>();
            }
        }
    }
}
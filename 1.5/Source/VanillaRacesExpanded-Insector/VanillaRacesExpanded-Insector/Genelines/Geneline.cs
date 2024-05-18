using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
namespace VanillaRacesExpandedInsector
{
    public class Geneline : IExposable, ILoadReferenceable
    {
        public string name;
        public int id;
        public List<GenelineGeneDef> genes = new();
        public List<Pawn> pawns = new();

        public void ExposeData()
        {
            Scribe_Values.Look(ref name, "name");
            Scribe_Values.Look(ref id, "id");
            Scribe_Collections.Look(ref genes, "genes", LookMode.Def);
            Scribe_Collections.Look(ref pawns, "pawns", LookMode.Reference);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                genes ??= new List<GenelineGeneDef>();
                pawns ??= new List<Pawn>();
            }
        }

        public MetapodHediff SetToMetapodStage(Pawn pawn)
        {
            var metapodHediff = pawn.health.hediffSet.GetFirstHediff<MetapodHediff>();
            if (metapodHediff is null)
            {
                metapodHediff = HediffMaker.MakeHediff(InternalDefOf.VRE_MetapodHediff, pawn) as MetapodHediff;
                pawn.health.AddHediff(metapodHediff);
            }
            metapodHediff.ticksComplete = Find.TickManager.TicksGame + GenDate.TicksPerDay;
            return metapodHediff;
        }

        public void AddPawnWithMetapod(Pawn pawn)
        {
            var genelineEvolution = pawn.genes.GetFirstGeneOfType<Gene_GenelineEvolution>();
            if (pawn.Dead is false)
            {
                var metapodHediff = SetToMetapodStage(pawn);
                metapodHediff.curGeneline = this;
            }
            else
            {
                AddPawnDirectly(pawn, genelineEvolution);
            }
        }

        public void AddPawnDirectly(Pawn pawn, Gene_GenelineEvolution genelineEvolution)
        {
            if (genelineEvolution.geneline != null)
            {
                genelineEvolution.geneline.RemovePawnDirectly(pawn, genelineEvolution);
            }
            genelineEvolution.geneline = this;
            foreach (var gene in genes)
            {
                var newGene = pawn.genes.GetGene(gene);
                if (newGene is null)
                {
                    pawn.genes.AddGene(gene, true);
                }
            }
            pawns.Add(pawn);
        }

        public void RemovePawn(Pawn pawn)
        {
            if (pawn.Dead is false)
            {
                var metapodHediff = SetToMetapodStage(pawn);
                metapodHediff.curGeneline = null;
            }
            else
            {
                var genelineEvolution = pawn.genes.GetFirstGeneOfType<Gene_GenelineEvolution>();
                RemovePawnDirectly(pawn, genelineEvolution);
            }
        }

        public void RemovePawnDirectly(Pawn pawn, Gene_GenelineEvolution genelineEvolution)
        {
            foreach (var gene in genes)
            {
                var oldGene = pawn.genes.GetGene(gene);
                if (oldGene != null)
                {
                    pawn.genes.RemoveGene(oldGene);
                }
            }
            if (genelineEvolution != null)
            {
                genelineEvolution.geneline = null;
            }
            pawns.Remove(pawn);
        }

        public void EditGeneline(List<GenelineGeneDef> genes, string genelineName)
        {
            this.name = genelineName;
            var oldGenes = genes.ToList();
            this.genes = genes.ToList();
            foreach (var pawn in pawns)
            {
                if (pawn.Dead is false)
                {
                    var metapod = SetToMetapodStage(pawn);
                    metapod.curGeneline = this;
                }
                else
                {
                    EditGenesDirectly(genes, oldGenes, pawn);
                }
            }
        }

        public void EditGenesDirectly(List<GenelineGeneDef> newGenes, List<GenelineGeneDef> oldGenes, Pawn pawn)
        {
            foreach (var gene in oldGenes)
            {
                if (newGenes.Contains(gene) is false)
                {
                    var oldGene = pawn.genes.GetGene(gene);
                    if (oldGene != null)
                    {
                        pawn.genes.RemoveGene(oldGene);
                    }
                }
            }
            foreach (var gene in newGenes)
            {
                var newGene = pawn.genes.GetGene(gene);
                if (newGene is null)
                {
                    pawn.genes.AddGene(gene, true);
                }
            }
        }

        public string GetUniqueLoadID()
        {
            return "Geneline_" + name + "_" + id;
        }
    }
}
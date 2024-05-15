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

        public void SetToMetapodStage(Pawn pawn)
        {

        }

        public void AddPawn(Pawn pawn)
        {
            if (pawn.Dead is false)
            {
                var genelineEvolution = pawn.genes.GetFirstGeneOfType<Gene_GenelineEvolution>();
                genelineEvolution.geneline?.RemovePawn(pawn);
                genelineEvolution.geneline = this;
                foreach (var gene in genes)
                {
                    var newGene = pawn.genes.GetGene(gene);
                    if (newGene is null)
                    {
                        pawn.genes.AddGene(gene, true);
                    }
                }
                SetToMetapodStage(pawn);
            }
            pawns.Add(pawn);
        }

        public void RemovePawn(Pawn pawn)
        {
            if (pawn.Dead is false)
            {
                foreach (var gene in genes)
                {
                    var oldGene = pawn.genes.GetGene(gene);
                    if (oldGene != null)
                    {
                        pawn.genes.RemoveGene(oldGene);
                    }
                }
                var genelineEvolution = pawn.genes.GetFirstGeneOfType<Gene_GenelineEvolution>();
                if (genelineEvolution != null)
                {
                    genelineEvolution.geneline = null;
                    SetToMetapodStage(pawn);
                }
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
                    foreach (var gene in oldGenes)
                    {
                        if (genes.Contains(gene) is false)
                        {
                            var oldGene = pawn.genes.GetGene(gene);
                            if (oldGene != null)
                            {
                                pawn.genes.RemoveGene(oldGene);
                            }
                        }
                    }
                    foreach (var gene in genes)
                    {
                        var newGene = pawn.genes.GetGene(gene);
                        if (newGene is null)
                        {
                            pawn.genes.AddGene(gene, true);
                        }
                    }
                    SetToMetapodStage(pawn);
                }
            }
        }

        public string GetUniqueLoadID()
        {
            return "Geneline_" + name + "_" + id;
        }
    }
}
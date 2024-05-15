using System.Collections.Generic;
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

        public void AddGene(GenelineGeneDef gene)
        {
            genes.Add(gene);
            // make pawns go to metapod stage
        }

        public void RemoveGene(GenelineGeneDef gene)
        {
            genes.Remove(gene);
            // make pawns go to metapod stage
        }

        public void AddPawn(Pawn pawn)
        {
            pawns.Add(pawn);
        }

        public void RemovePawn(Pawn pawn)
        {
            pawns.Remove(pawn);
        }

        public string GetUniqueLoadID()
        {
            return "Geneline_" + name + "_" + id;
        }
    }
}
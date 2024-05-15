using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VanillaRacesExpandedInsector
{
    public static class Utils
    {
        private static List<GenelineGeneDef> cachedGeneDefsInOrder = null;

        public static List<GenelineGeneDef> GenelineGenesInOrder
        {
            get
            {
                if (cachedGeneDefsInOrder == null)
                {
                    cachedGeneDefsInOrder = new List<GenelineGeneDef>();
                    foreach (var allDef in DefDatabase<GenelineGeneDef>.AllDefs)
                    {
                        cachedGeneDefsInOrder.Add(allDef);
                    }
                    cachedGeneDefsInOrder.SortBy((GenelineGeneDef x) => 0f - x.displayCategory.displayPriorityInXenotype, (GenelineGeneDef x) => x.displayCategory.label, (GenelineGeneDef x) => x.displayOrderInCategory);
                }
                return cachedGeneDefsInOrder;
            }
        }

        public static void SortGeneDefs(this List<GenelineGeneDef> geneDefs)
        {
            geneDefs.SortBy((GenelineGeneDef x) => 0f - x.displayCategory.displayPriorityInXenotype, (GenelineGeneDef x) => x.displayOrderInCategory, (GenelineGeneDef y) => y.label);
        }

        public static bool HasActiveGene(this Pawn pawn, GeneDef geneDef)
        {
            if (pawn.genes is null) return false;
            return pawn.genes.GetGene(geneDef)?.Active ?? false;
        }
    }
}
using RimWorld;
using System.Collections.Generic;
using Verse;

namespace VanillaRacesExpandedInsector
{
    public static class Utils
    {
        private static List<GeneDef> cachedGeneDefsInOrder = null;

        public static List<GeneDef> GenelineGenesInOrder
        {
            get
            {
                if (cachedGeneDefsInOrder == null)
                {
                    cachedGeneDefsInOrder = new List<GeneDef>();
                    foreach (GeneDef allDef in DefDatabase<GenelineGeneDef>.AllDefs)
                    {
                        cachedGeneDefsInOrder.Add(allDef);
                    }
                    cachedGeneDefsInOrder.SortBy((GeneDef x) => 0f - x.displayCategory.displayPriorityInXenotype, (GeneDef x) => x.displayCategory.label, (GeneDef x) => x.displayOrderInCategory);
                }
                return cachedGeneDefsInOrder;
            }
        }

        public static bool HasActiveGene(this Pawn pawn, GeneDef geneDef)
        {
            if (pawn.genes is null) return false;
            return pawn.genes.GetGene(geneDef)?.Active ?? false;
        }
    }
}
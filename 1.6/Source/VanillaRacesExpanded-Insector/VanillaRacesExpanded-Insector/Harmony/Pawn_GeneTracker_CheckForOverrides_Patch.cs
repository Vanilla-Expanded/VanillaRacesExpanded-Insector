using HarmonyLib;
using RimWorld;

namespace VanillaRacesExpandedInsector
{
    [HarmonyPatch(typeof(Pawn_GeneTracker), "CheckForOverrides")]
    public static class Pawn_GeneTracker_CheckForOverrides_Patch
    {
        public static void Postfix(Pawn_GeneTracker __instance)
        {
            foreach (var gene in __instance.GenesListForReading)
            {
                if (gene.def is GenelineGeneDef)
                {
                    foreach (var otherGene in __instance.GenesListForReading)
                    {
                        if (otherGene != gene && gene.def.ConflictsWith(otherGene.def))
                        {
                            gene.OverrideBy(null);
                            otherGene.OverrideBy(gene);
                        }
                    }
                }
            }
        }
    }
}


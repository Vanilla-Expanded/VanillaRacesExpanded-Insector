using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using HarmonyLib;


namespace VanillaRacesExpandedInsector
{
    [HarmonyPatch(typeof(FoodUtility), "TryGetFoodPoisoningChanceOverrideFromTraits")]
    public static class VanillaRacesExpandedInsector_FoodUtility_TryGetFoodPoisoningChanceOverrideFromTraits
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn pawn, Thing ingestible, ref float poisonChanceOverride)
        {
            if (pawn.genes?.HasActiveGene(InternalDefOf.VRE_InsectJellyDependency)==true && ingestible?.def == ThingDefOf.InsectJelly)
            {
                poisonChanceOverride = 0;
                
            }
          
        }
    }
}

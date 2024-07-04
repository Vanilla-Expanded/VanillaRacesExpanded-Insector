using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using HarmonyLib;
using Verse.AI;


namespace VanillaRacesExpandedInsector
{
    [HarmonyPatch(typeof(JobGiver_Wander), "TryGiveJob")]
    public static class VanillaRacesExpandedInsector_JobGiver_Wander_TryGiveJob
    {
        [HarmonyPostfix]
        public static void Postfix(Pawn pawn, ref Job __result, JobGiver_Wander __instance)
        {
            
            if(__instance.GetType() == typeof(JobGiver_RunRandom))
            {
                if (pawn.genes?.HasActiveGene(InternalDefOf.VRE_PyroResistantChitin) == true)
                {
                    __result = null;
                }
               
            }
            
        
        }
        [HarmonyPrefix]
        public static void Prefix(Pawn pawn, JobGiver_Wander __instance, ref IntRange ___ticksBetweenWandersRange)
        {

            if (__instance.GetType() == typeof(JobGiver_RunRandom))
            {
                if (pawn.genes?.HasActiveGene(InternalDefOf.VRE_ChemfuelSacks) == true)
                {
                    ___ticksBetweenWandersRange =   new IntRange(10, 20);
                }

            }


        }

    }
}

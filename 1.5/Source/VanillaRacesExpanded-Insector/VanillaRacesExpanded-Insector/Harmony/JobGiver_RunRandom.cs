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
        public static void Postfix(Pawn pawn, float ___wanderRadius, ref Job __result)
        {
            
            if(___wanderRadius == 7f)
            {
                if (pawn.genes?.HasActiveGene(InternalDefOf.VRE_PyroResistantChitin) == true)
                {
                    __result = null;
                }
            }
            
        
        }
    }
}

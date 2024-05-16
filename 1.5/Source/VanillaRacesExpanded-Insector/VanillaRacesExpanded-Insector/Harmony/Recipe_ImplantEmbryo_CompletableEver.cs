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
    [HarmonyPatch(typeof(Recipe_ImplantEmbryo), "CompletableEver")]
    public static class VanillaRacesExpandedInsector_Recipe_ImplantEmbryo_CompletableEver
    {
        [HarmonyPostfix]
        public static void Postfix(Recipe_ImplantEmbryo __instance, Pawn surgeryTarget, ref bool __result)
        {
            if (__result && surgeryTarget.HasActiveGene(InternalDefOf.VRE_ChestburstPregnancy))
            {
                __result = false;
                return;
            }
        }
    }
}

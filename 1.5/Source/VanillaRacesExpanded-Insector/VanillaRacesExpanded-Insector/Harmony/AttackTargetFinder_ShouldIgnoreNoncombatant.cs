using HarmonyLib;
using RimWorld;
using Verse;
using Verse.AI;
using System.Collections.Generic;

namespace VanillaRacesExpandedInsector
{
    [HarmonyPatch(typeof(AttackTargetFinder))]
    [HarmonyPatch("ShouldIgnoreNoncombatant")]
    public static class VanillaRacesExpandedInsector_AttackTargetFinder_ShouldIgnoreNoncombatant_Patch
    {

   

        [HarmonyPostfix]
        public static void DisableInsectoidTargetting(Thing searcherThing, IAttackTarget t, ref bool __result)
        {
            Pawn pawn;
            Pawn insectoid;
            if ((insectoid = (searcherThing as Pawn)) != null && insectoid.RaceProps.Insect && (pawn = (t as Pawn)) != null)
            {
                if (pawn.HasActiveGene(InternalDefOf.VRE_InsectPheromones))
                {
                    __result = false;
                }


            }


        }

       
    }













}


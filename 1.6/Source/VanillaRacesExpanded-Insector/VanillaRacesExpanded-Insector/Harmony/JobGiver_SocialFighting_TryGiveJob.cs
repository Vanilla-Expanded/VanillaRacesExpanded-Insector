using HarmonyLib;
using RimWorld;
using RimWorld.Planet;
using Verse;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Verse.AI;
namespace VanillaRacesExpandedInsector
{


    [HarmonyPatch(typeof(JobGiver_SocialFighting))]
    [HarmonyPatch("TryGiveJob")]
    public static class VanillaRacesExpandedInsector_JobGiver_SocialFighting_TryGiveJob_Patch
    {
        [HarmonyPrefix]
        static bool GoNuts(Pawn pawn, ref Job __result)
        {


            if (pawn.HasActiveGene(InternalDefOf.VRE_InsectVolatile))
            {
                if (pawn.RaceProps.Humanlike && pawn.WorkTagIsDisabled(WorkTags.Violent))
                {
                    __result = null;
                }
                Pawn otherPawn = ((MentalState_SocialFighting)pawn.MentalState).otherPawn;
                if (pawn.TryGetAttackVerb(null) == null)
                {
                    __result = null;
                }
                Job job = JobMaker.MakeJob(InternalDefOf.VRE_ArmedSocialFightInsectors, otherPawn);
                job.maxNumMeleeAttacks = 1;

                __result = job;
                return false;

            }
            return true;

        }


    }








}

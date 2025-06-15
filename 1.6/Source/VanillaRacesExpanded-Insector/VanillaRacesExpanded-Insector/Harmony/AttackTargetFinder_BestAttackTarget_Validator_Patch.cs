using HarmonyLib;
using RimWorld;
using System.Linq;
using System.Reflection;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace VanillaRacesExpandedInsector
{
    [HarmonyPatch(typeof(Pawn), "ThreatDisabledBecauseNonAggressiveRoamer")]
    public static class Pawn_ThreatDisabledBecauseNonAggressiveRoamer_Patch
    {
        public static void Postfix(ref bool __result, Pawn __instance, Pawn otherPawn)
        {
            if (__result is false && AttackTargetFinder_BestAttackTarget_Validator_Patch.CheckHostility(true,
                __instance, otherPawn) is false)
            {
                __result = true;
            }
        }
    }

    [HarmonyPatch]
    public static class AttackTargetFinder_BestAttackTarget_Validator_Patch
    {
        public static MethodBase TargetMethod()
        {
            foreach (var nestedType in typeof(AttackTargetFinder).GetNestedTypes(AccessTools.all))
            {
                var methods = nestedType.GetMethods(AccessTools.all);
                var method = methods.FirstOrDefault(x => x.Name.Contains("<BestAttackTarget>") 
                && x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == typeof(IAttackTarget));
                if (method != null)
                {
                    return method;
                }
            }
            return null;
        }

        public static void Postfix(ref bool __result, IAttackTarget t, Thing ___searcherThing)
        {
            if (__result)
            {
                var pawn1 = t.Thing as Pawn;
                var pawn2 = ___searcherThing as Pawn;
                __result = CheckHostility(__result, pawn1, pawn2);
            }
        }

        public static bool CheckHostility(bool __result, Pawn pawn1, Pawn pawn2)
        {
            if (__result)
            {
                if (pawn1 != null && pawn2 != null)
                {
                    if (pawn1.RaceProps.Insect && pawn2.HasActiveGene(InternalDefOf.VRE_InsectPheromones)
                        && PreventHostility(pawn2, pawn1))
                    {
                        __result = false;
                    }
                    else if (pawn2.RaceProps.Insect && pawn1.HasActiveGene(InternalDefOf.VRE_InsectPheromones) 
                        && PreventHostility(pawn1, pawn2))
                    {
                        __result = false;
                    }
                }
            }
            return __result;
        }

        public static bool PreventHostility(Pawn humanlike, Pawn insect)
        {
            if (humanlike.mindState.enemyTarget == insect
                || insect.mindState.enemyTarget == humanlike 
                || insect.GetLord()?.LordJob is LordJob_AssaultColony && humanlike.IsColonist
                || CheckEnemyTargetFaction(humanlike.mindState.enemyTarget, insect)
                || CheckEnemyTargetFaction(insect.mindState.enemyTarget, humanlike)
                || CheckEnemyTargetFaction(humanlike.mindState.lastAttackedTarget.Thing, insect)
                || CheckEnemyTargetFaction(insect.mindState.lastAttackedTarget.Thing, humanlike)
                || CheckEnemyTarget(humanlike.mindState.enemyTarget, insect)
                || CheckEnemyTarget(insect.mindState.enemyTarget, humanlike)
                || CheckEnemyTarget(humanlike.mindState.lastAttackedTarget.Thing, insect)
                || CheckEnemyTarget(insect.mindState.lastAttackedTarget.Thing, humanlike))
            {
                return false;
            }
            return true;
        }

        private static bool CheckEnemyTargetFaction(Thing enemyTarget, Pawn otherPawn)
        {
            return enemyTarget is not null && otherPawn.Faction != null && enemyTarget.Faction == otherPawn.Faction;
        }

        private static bool CheckEnemyTarget(Thing enemyTarget, Pawn otherPawn)
        {
            return enemyTarget is not null && enemyTarget is Pawn enemy && enemy.GetLord() == otherPawn.GetLord();
        }
    }
}


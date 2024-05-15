using HarmonyLib;
using RimWorld;
using Verse;
using System.Collections.Generic;

namespace VanillaRacesExpandedInsector
{

    [HarmonyPatch(typeof(FoodUtility))]
    [HarmonyPatch("ThoughtsFromIngesting")]
    public static class VanillaRacesExpandedInsector_FoodUtility_ThoughtsFromIngesting_Patch
    {

        public static List<PreceptDef> listOfPrecepts = new List<PreceptDef>(){InternalDefOf.Cannibalism_Acceptable, InternalDefOf.Cannibalism_Preferred,
                InternalDefOf.Cannibalism_RequiredStrong, InternalDefOf.Cannibalism_RequiredRavenous };

        [HarmonyPostfix]
        public static void AddPorkThought(Pawn ingester, Thing foodSource, ThingDef foodDef)
        {
            if (ingester.Ideo != null && ingester.genes?.HasGene(InternalDefOf.VRE_InsectFlesh) == true)
            {
                if (foodDef== InternalDefOf.Meat_Megaspider && IdeoNotCannibal(ingester))
                {
                    Thought_Memory thought_Memory = ThoughtMaker.MakeThought(InternalDefOf.AteHumanlikeMeatDirectCannibal, null);
                    ingester.needs.mood.thoughts.memories.TryGainMemory(thought_Memory);

                    ingester.mindState.lastHumanMeatIngestedTick = Find.TickManager.TicksGame;
                    Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.AteHumanMeat, ingester.Named(HistoryEventArgsNames.Doer)), canApplySelfTookThoughts: false);
                }
            }
        }

        public static bool IdeoNotCannibal(Pawn pawn)
        {
            foreach (PreceptDef precept in listOfPrecepts)
            {
                if (pawn.Ideo.HasPrecept(precept))
                {
                    return false;
                }
            }
            return true;

        }
    }













}


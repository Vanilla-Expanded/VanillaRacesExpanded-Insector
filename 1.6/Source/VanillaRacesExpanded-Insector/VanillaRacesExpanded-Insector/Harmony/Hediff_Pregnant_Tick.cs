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
    [HarmonyPatch(typeof(Hediff_Pregnant), nameof(Hediff_Pregnant.TickInterval))]
    public static class VanillaRacesExpandedInsector_Hediff_Pregnant_Tick
    {
        [HarmonyPrefix]
        public static bool Prefix(Hediff_Pregnant __instance, int delta)
        {
            if (__instance.pawn != null && __instance.pawn.IsHashIntervalTick(1200, delta) && __instance.pawn.HasActiveGene(InternalDefOf.VRE_ChestburstPregnancy) && !__instance.pawn.HasActiveGene(InternalDefOf.VRE_SpawningSack))
            {
                try
                {
                    if(__instance.pawn?.health?.hediffSet != null)
                    {
                        Hediff hediff = HediffMaker.MakeHediff(InternalDefOf.VREInsector_TempSterile, __instance.pawn);
                        __instance.pawn.health.AddHediff(hediff);

                        Hediff hediff2 = HediffMaker.MakeHediff(InternalDefOf.VRE_ChestburstPregnancyHediff, __instance.pawn);
                        __instance.pawn.health.AddHediff(hediff2);

                        Hediff pregnancy = __instance.pawn.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.VRE_ChestburstPregnancyHediff);
                        HediffComp_ChestburstPregnancy comp = pregnancy.TryGetComp<HediffComp_ChestburstPregnancy>();
                        comp.genes = __instance.geneSet;
                        comp.mother = __instance.pawn;
                        comp.father = __instance.Father;

                        ChoiceLetter letter = LetterMaker.MakeLetter("VRE_ChestburstPregnancyReady".Translate(__instance.pawn.LabelShort), "VRE_ChestburstPregnancyReadyDesc".Translate(__instance.pawn.LabelShort), LetterDefOf.PositiveEvent);
                        Find.LetterStack.ReceiveLetter(letter);

                        __instance.pawn.health.RemoveHediff(__instance);
                    }

                }
                catch (Exception ex)
                {
                    Log.Message("Error Suppressed: " + ex.ToString());
                }
                return false;
            }
            return true;
        }


    }
}

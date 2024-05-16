
using RimWorld;
using System.Linq;
using UnityEngine;
using Verse;

namespace VanillaRacesExpandedInsector
{
    public class HediffGiver_HypothermicHibernation : HediffGiver
    {


        public override void OnIntervalPassed(Pawn pawn, Hediff cause)
        {
            if (!pawn.HasActiveGene(InternalDefOf.VRE_HypothermicHibernation))
            {
                return;
            }

            float ambientTemperature = pawn.AmbientTemperature;
            FloatRange floatRange = pawn.ComfortableTemperatureRange();
            FloatRange floatRange2 = pawn.SafeTemperatureRange();
            HediffSet hediffSet = pawn.health.hediffSet;

            HediffDef hediffDefToRemove = InternalDefOf.Hypothermia;
            Hediff firstHediffOfDefToRemove = hediffSet.GetFirstHediffOfDef(hediffDefToRemove);
            if (firstHediffOfDefToRemove != null)
            {
                pawn.health.RemoveHediff(firstHediffOfDefToRemove);
            }


            HediffDef hediffDef = InternalDefOf.VRE_HypothermicHibernationHediff;
            Hediff firstHediffOfDef = hediffSet.GetFirstHediffOfDef(hediffDef);
            if (ambientTemperature < floatRange2.min)
            {
                float a = Mathf.Abs(ambientTemperature - floatRange2.min) * 6.45E-05f;
                a = Mathf.Max(a, 0.00075f);
                HealthUtility.AdjustSeverity(pawn, hediffDef, a);
                if (pawn.Dead)
                {
                    return;
                }
            }
            if (firstHediffOfDef == null)
            {
                return;
            }
            if (ambientTemperature > floatRange.min)
            {
                float value = firstHediffOfDef.Severity * 0.027f;
                value = Mathf.Clamp(value, 0.0015f, 0.015f);
                firstHediffOfDef.Severity -= value;
            }

        }
    }
}
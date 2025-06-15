
using RimWorld;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;
namespace VanillaRacesExpandedInsector
{
    public class HediffGiver_Heatstress : HediffGiver
    {

        public static readonly SimpleCurve TemperatureOverageAdjustmentCurve = new SimpleCurve
    {
        new CurvePoint(0f, 0f),
        new CurvePoint(25f, 25f),
        new CurvePoint(50f, 40f),
        new CurvePoint(100f, 60f),
        new CurvePoint(200f, 80f),
        new CurvePoint(400f, 100f),
        new CurvePoint(4000f, 1000f)
    };

        public override void OnIntervalPassed(Pawn pawn, Hediff cause)
        {

            if (!pawn.HasActiveGene(InternalDefOf.VRE_Heatstress))
            {
                return;
            }

            HediffDef hediffDefToRemove = InternalDefOf.Heatstroke;
            Hediff firstHediffOfDefToRemove = pawn.health?.hediffSet?.GetFirstHediffOfDef(hediffDefToRemove);
            if (firstHediffOfDefToRemove != null)
            {
                pawn.health.RemoveHediff(firstHediffOfDefToRemove);
            }

            float ambientTemperature = pawn.AmbientTemperature;
            FloatRange floatRange = pawn.ComfortableTemperatureRange();
            FloatRange floatRange2 = pawn.SafeTemperatureRange();
            HediffDef hediffForHeatshock = InternalDefOf.VRE_HeatstressHediff;
            Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(hediffForHeatshock);

            if (ambientTemperature > floatRange2.max)
            {
                float x = ambientTemperature - floatRange2.max;
                x = TemperatureOverageAdjustmentCurve.Evaluate(x);
                float a = x * 6.45E-05f;
                a = Mathf.Max(a, 0.000375f);
                HealthUtility.AdjustSeverity(pawn, hediffForHeatshock, a);

                if (firstHediffOfDef?.Severity > 0.37f)
                {
                    float num = 0.025f * firstHediffOfDef.Severity;
                    if (Rand.Value < num && pawn.RaceProps.body.AllPartsVulnerableToFrostbite.Where((BodyPartRecord y) => !pawn.health.hediffSet.PartIsMissing(y)).TryRandomElementByWeight((BodyPartRecord y) => y.def.frostbiteVulnerability, out BodyPartRecord result))
                    {
                        int num2 = Mathf.CeilToInt((float)result.def.hitPoints * 0.5f);
                        DamageInfo dinfo = new DamageInfo(DamageDefOf.Burn, num2, 0f, -1f, null, result);
                        pawn.TakeDamage(dinfo);
                    }
                }


            }
            else if (firstHediffOfDef != null && ambientTemperature < floatRange.max)
            {
                float value = firstHediffOfDef.Severity * 0.027f;
                value = Mathf.Clamp(value, 0.0015f, 0.015f);
                firstHediffOfDef.Severity -= value;
            }

            



        }
    }
}
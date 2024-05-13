
using RimWorld;
using UnityEngine;
using Verse;
using Verse.AI.Group;
namespace VanillaRacesExpandedInsector
{
    public class HediffGiver_Heatshock : HediffGiver
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

            if (!pawn.HasActiveGene(InternalDefOf.VRE_Heatshock))
            {
                return;
            }
            float ambientTemperature = pawn.AmbientTemperature;
            FloatRange floatRange = pawn.ComfortableTemperatureRange();
            FloatRange floatRange2 = pawn.SafeTemperatureRange();
            HediffDef hediffForHeatshock = InternalDefOf.VRE_HeatshockHediff; 
            Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(hediffForHeatshock);
            if (ambientTemperature > floatRange2.max)
            {
                float x = ambientTemperature - floatRange2.max;
                x = TemperatureOverageAdjustmentCurve.Evaluate(x);
                float a = x * 6.45E-05f;
                a = Mathf.Max(a, 0.000375f);
                HealthUtility.AdjustSeverity(pawn, hediffForHeatshock, a);
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
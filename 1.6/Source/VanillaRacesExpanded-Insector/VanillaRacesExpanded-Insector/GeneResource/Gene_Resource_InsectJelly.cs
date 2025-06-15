using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

using Verse;
using Verse.AI;

namespace VanillaRacesExpandedInsector
{

    public class Gene_Resource_InsectJelly : Gene_Resource, IGeneResourceDrain
    {
        public Gene_Resource Resource => this;

        public bool CanOffset => pawn.Spawned && Active;

        public float ResourceLossPerDay => def.resourceLossPerDay;

        public Pawn Pawn => pawn;

        public string DisplayLabel => def.resourceLabel;
        private int lastConsumed;
        public override float InitialResourceMax => 1f;

        public override float MinLevelForAlert => 0.15f;

        public override float MaxLevelOffset => base.MaxLevelOffset;


        public bool ShouldConsumeNow()
        {
            if (!Active) return false;
            return Value < 0.1f;
        }


        public override void Tick()
        {

            if (CanOffset)
            {
                float value = Resource.Value;
                Resource.Value += ((0f - ResourceLossPerDay) / 60000f);


            }
            if(Resource.Value<=0 && this.pawn?.health?.hediffSet?.GetFirstHediffOfDef(InternalDefOf.VRE_InsectJellyDependencyHediff) == null)
            {
                this.pawn.health.AddHediff(InternalDefOf.VRE_InsectJellyDependencyHediff);
            }

        }


        protected override Color BarColor => new Color(0.53f, 0.53f, 0.35f);

        protected override Color BarHighlightColor => new Color(0.6f, 0.67f, 0.4f);


        public override void Notify_IngestedThing(Thing thing, int numTaken)
        {

            if (thing.def == ThingDefOf.InsectJelly)
            {
                Value += 0.5f;
                lastConsumed = Find.TickManager.TicksGame;
            }



        }
        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (Gizmo gizmo in base.GetGizmos())
            {
                yield return gizmo;
            }
            foreach (Gizmo gizmo in GeneResourceDrainUtility.GetResourceDrainGizmos(this))
            {
                yield return gizmo;
            }
            yield break;
        }



        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref lastConsumed, "lastConsumed");
        }

        public override bool Active
        {
            get
            {
                if (base.Active && pawn != null)
                {
                    return !pawn.IsGhoul;
                }
                return false;
            }
        }

    }
}

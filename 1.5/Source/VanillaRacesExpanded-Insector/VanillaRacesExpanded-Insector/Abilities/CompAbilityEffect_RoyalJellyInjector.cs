using RimWorld;
using System;
using System.Collections.Generic;
using UnityEngine;
using VanillaRacesExpandedInsector;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace VanillaRacesExpandedInsector
{
    public class CompAbilityEffect_RoyalJellyInjector : CompAbilityEffect
    {

        List<HediffDef> hediffs => new List<HediffDef> { InternalDefOf.VFEI2_EmpressSpawn, InternalDefOf.VFEI2_GigamiteSpawn,
        InternalDefOf.VFEI2_TitantickSpawn,InternalDefOf.VFEI2_TeramantisSpawn,InternalDefOf.VFEI2_SilverfishSpawn};


        new public CompProperties_RoyalJellyInjector Props
        {
            get
            {
                return (CompProperties_RoyalJellyInjector)this.props;
            }
        }

        public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
        {
            
            base.Apply(target, dest);
            Pawn pawn = target.Pawn;
            if (pawn != null)
            {

                pawn.TakeDamage(new DamageInfo(DamageDefOf.Cut, 1, 0f, -1f, this.parent.pawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown));
              
                IntVec3 c;
                CellFinder.TryFindRandomReachableCellNearPosition(pawn.Position, pawn.Position, pawn.Map, 1, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), null, null, out c);
                FilthMaker.TryMakeFilth(c, pawn.Map, ThingDefOf.Filth_Blood);

                FleckMaker.AttachedOverlay(pawn, FleckDefOf.FlashHollow, new Vector3(0f, 0f, 0.26f));

                pawn.needs?.mood?.thoughts?.memories?.TryGainMemory((Thought_Memory)ThoughtMaker.MakeThought(InternalDefOf.VFEI2_AteRoyalJelly), parent.pawn);
                pawn.health.AddHediff(InternalDefOf.VFEI2_RoyalJellyHigh);
                pawn.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.VFEI2_RoyalJellyHigh).Severity = 1f;

                if (pawn.health != null && pawn.health.hediffSet.HasImmunizableNotImmuneHediff() && pawn.health.hediffSet.HasHediff(HediffDefOf.WoundInfection) && pawn.health.immunity != null && pawn.health.immunity.GetImmunity(HediffDefOf.WoundInfection) != 1)
                {
                    ImmunityRecord cPawn = pawn.health.immunity.GetImmunityRecord(HediffDefOf.WoundInfection);
                    cPawn.immunity += 0.05f;
                }

                foreach (HediffDef hediffdef in hediffs)
                {
                    Hediff hediff = pawn.health.hediffSet.GetFirstHediffOfDef(hediffdef);
                    if (hediff != null)
                    {
                        hediff.Severity += 0.0334f;
                    }

                }


            }
        }

       


      




    }
}
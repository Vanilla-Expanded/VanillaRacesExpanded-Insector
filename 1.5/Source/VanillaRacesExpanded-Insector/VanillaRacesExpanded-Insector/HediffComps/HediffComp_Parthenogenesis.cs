using RimWorld;
using Verse;
using Verse.Sound;
using System;
using Verse.AI;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;


namespace VanillaRacesExpandedInsector
{
    public class HediffComp_Parthenogenesis : HediffComp
    {
     

        public HediffCompProperties_Parthenogenesis Props
        {
            get
            {
                return (HediffCompProperties_Parthenogenesis)this.props;
            }
        }

        public override void CompExposeData()
        {
            base.CompExposeData();
           
        }

      

        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            base.CompGetGizmos();
            yield return new Command_Target
            {
                defaultLabel = "VRE_SelfImpregnate".Translate(),
                defaultDesc = "VRE_SelfImpregnateDesc".Translate(),
                icon = ContentFinder<Texture2D>.Get("UI/Abilities/Ability_SelfImpregnate", true),
                targetingParams = new TargetingParameters { canTargetSelf = true },
                Disabled = this.parent.pawn.gender != Gender.Female,
                disabledReason = "VRE_PawnIsMale".Translate(),
                
                action = delegate (LocalTargetInfo target)
                {
                    TryParthenogenesis(target);
                }
            };



        }

        public void TryParthenogenesis(LocalTargetInfo target)
        {
            if (target.Pawn != null)
            {
                if(target.Pawn != this.parent.pawn)
                {
                    Messages.Message("VRE_OnlyOnSelf".Translate(), parent.pawn, MessageTypeDefOf.NegativeEvent, true);

                }
                else if(target.Pawn?.health?.hediffSet?.HasHediff(HediffDefOf.Pregnant) == true){

                    Messages.Message("VRE_AlreadyPregnant".Translate(), parent.pawn, MessageTypeDefOf.NegativeEvent, true);


                }
                else
                {
                    Hediff_Pregnant hediff_Pregnant = (Hediff_Pregnant)HediffMaker.MakeHediff(HediffDefOf.Pregnant, parent.pawn);
                    hediff_Pregnant.SetParents(parent.pawn, parent.pawn, null);
                    parent.pawn.health.AddHediff(hediff_Pregnant);
                    Messages.Message("VRE_GotPregnant".Translate(parent.pawn), parent.pawn, MessageTypeDefOf.PositiveEvent, true);
                }
                

            }


        }


    }
}


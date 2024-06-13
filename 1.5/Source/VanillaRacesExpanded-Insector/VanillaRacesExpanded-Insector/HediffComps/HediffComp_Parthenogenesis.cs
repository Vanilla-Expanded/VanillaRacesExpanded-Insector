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

            Command_Action command_Action = new Command_Action();
            command_Action.defaultLabel = "VRE_SelfImpregnate".Translate();
            command_Action.defaultDesc = "VRE_SelfImpregnateDesc".Translate();
            command_Action.icon = ContentFinder<Texture2D>.Get("UI/Abilities/Ability_SelfImpregnate", true);
            command_Action.action = delegate ()
            {
                TryParthenogenesis();
            };
            if(this.parent.pawn.gender != Gender.Female)
            {
                command_Action.Disable("VRE_PawnIsMale".Translate());
            }
            if (this.parent.pawn.ageTracker.AgeBiologicalYears < 14) { 
          
                command_Action.Disable("VRE_PawnIsTooYoung".Translate(parent.pawn));
            }

            yield return command_Action;



        }

        public void TryParthenogenesis()
        {
                if(Pawn?.health?.hediffSet?.HasHediff(HediffDefOf.Pregnant) == true){

                    Messages.Message("VRE_AlreadyPregnant".Translate(), parent.pawn, MessageTypeDefOf.NegativeEvent, true);


                }
                else
                {
                    Hediff_Pregnant hediff_Pregnant = (Hediff_Pregnant)HediffMaker.MakeHediff(HediffDefOf.Pregnant, parent.pawn);
                    GeneSet inheritedGeneSet = PregnancyUtility.GetInheritedGeneSet(parent.pawn, parent.pawn);
                    hediff_Pregnant.SetParents(parent.pawn, null, inheritedGeneSet);
                    parent.pawn.health.AddHediff(hediff_Pregnant);
                    Messages.Message("VRE_GotPregnant".Translate(parent.pawn), parent.pawn, MessageTypeDefOf.PositiveEvent, true);
                }
                

            


        }


    }
}


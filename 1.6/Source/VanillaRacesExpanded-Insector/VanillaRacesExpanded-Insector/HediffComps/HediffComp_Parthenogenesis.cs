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
            if (this.parent.pawn.gender != Gender.Female)
            {
                command_Action.Disable("VRE_PawnIsMale".Translate());
            }
            if (this.parent.pawn.ageTracker.AgeBiologicalYears < 14)
            {

                command_Action.Disable("VRE_PawnIsTooYoung".Translate(parent.pawn));
            }

            yield return command_Action;



        }

        public void TryParthenogenesis()
        {
            if (parent.pawn.health.hediffSet == null)
                return;

            if (Pawn.health.hediffSet.HasPregnancyHediff())
            {

                Messages.Message("VRE_AlreadyPregnant".Translate(), parent.pawn, MessageTypeDefOf.NegativeEvent, true);


            }
            else if (Pawn.health.hediffSet.HasHediff(InternalDefOf.VREInsector_TempSterile))
            {

                Messages.Message("VRE_TempSterile".Translate(parent.pawn), parent.pawn, MessageTypeDefOf.NegativeEvent, true);

            }
            else
            {
                Hediff_Pregnant hediff_Pregnant;
                if (!Pawn.HasActiveGene(InternalDefOf.VRE_ChestburstPregnancy))
                    Messages.Message("VRE_GotPregnant".Translate(parent.pawn), parent.pawn, MessageTypeDefOf.PositiveEvent, true);

                hediff_Pregnant = (Hediff_Pregnant)HediffMaker.MakeHediff(HediffDefOf.PregnantHuman, parent.pawn);

                GeneSet inheritedGeneSet = PregnancyUtility.GetInheritedGeneSet(parent.pawn, parent.pawn);
                hediff_Pregnant.SetParents(parent.pawn, null, inheritedGeneSet);
                parent.pawn.health.AddHediff(hediff_Pregnant);
                

            }





        }


    }
}


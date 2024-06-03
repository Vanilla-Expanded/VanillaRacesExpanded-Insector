using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using RimWorld;
using UnityEngine;

namespace VanillaRacesExpandedInsector
{
    public class JobDriver_ChestburstImplantation : JobDriver
    {




        private Pawn Targetpawn
        {
            get
            {
                return this.job.GetTarget(TargetIndex.A).Pawn;
            }
        }

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(Targetpawn, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {


            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
            Toil toil = Toils_General.Wait(100, TargetIndex.None);
            toil.WithProgressBarToilDelay(TargetIndex.A, false, -0.5f);
            toil.FailOnDespawnedOrNull(TargetIndex.A);
            toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            yield return toil;
            Toil chestburst = new Toil();
            chestburst.initAction = delegate ()
            {
                
                

                Hediff pregnancy = pawn.health?.hediffSet?.GetFirstHediffOfDef(InternalDefOf.VRE_ChestburstPregnancyHediff);
                if (pregnancy != null)
                {
                    HediffComp_ChestburstPregnancy comp = pregnancy.TryGetComp<HediffComp_ChestburstPregnancy>();
                    if (comp != null)
                    {
                        Utils.DamageUntilDowned(Targetpawn);
                       
                        FleckMaker.AttachedOverlay(Targetpawn, FleckDefOf.FlashHollow, new Vector3(0f, 0f, 0.26f));

                        Targetpawn.needs?.mood?.thoughts?.memories?.TryGainMemory((Thought_Memory)ThoughtMaker.MakeThought(InternalDefOf.VRE_Implanted), pawn);

                        Targetpawn.needs?.mood?.thoughts?.memories?.TryGainMemory((Thought_Memory)ThoughtMaker.MakeThought(InternalDefOf.VRE_Implanted_Social), pawn);

                        for (int i = 0; i < 20; i++)
                        {
                            IntVec3 c;
                            CellFinder.TryFindRandomReachableCellNearPosition(Targetpawn.Position, Targetpawn.Position, Targetpawn.Map, 2, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), null, null, out c);
                            FilthMaker.TryMakeFilth(c, Targetpawn.Map, ThingDefOf.Filth_Blood);

                        }
                        if (Targetpawn.Faction != null && Targetpawn.Faction != Faction.OfPlayer)
                        {
                            if (Targetpawn.Faction.HasGoodwill)
                            {
                                Faction.OfPlayer.TryAffectGoodwillWith(Targetpawn.Faction, -500, canSendMessage: true, false, HistoryEventDefOf.AttackedMember, Targetpawn);
                            }
                            else { Targetpawn.Faction.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile); }
                            
                        }
                        Targetpawn.health.AddHediff(InternalDefOf.VRE_ChestburstPregnancy_Victim);

                        Hediff victimPregnancy = Targetpawn.health?.hediffSet?.GetFirstHediffOfDef(InternalDefOf.VRE_ChestburstPregnancy_Victim);
                        HediffComp_ChestburstPregnancyVictim victimComp = victimPregnancy.TryGetComp<HediffComp_ChestburstPregnancyVictim>();
                        victimComp.genes = comp.genes;
                        victimComp.mother = comp.mother;
                        victimComp.father = comp.father;
                        comp.miscarriage = false;
                        pawn.health.RemoveHediff(pregnancy);
                    }

                }


            };
            yield return chestburst;
            yield break;
        }

    }
}

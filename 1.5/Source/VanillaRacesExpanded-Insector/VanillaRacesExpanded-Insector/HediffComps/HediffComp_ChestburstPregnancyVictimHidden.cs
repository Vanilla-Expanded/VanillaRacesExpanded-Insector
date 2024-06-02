using RimWorld;
using Verse;
using Verse.Sound;
using System;
using System.Collections.Generic;
using RimWorld.Planet;


namespace VanillaRacesExpandedInsector
{
    public class HediffComp_ChestburstPregnancyVictimHidden : HediffComp
    {

        private static List<string> tmpLastNames = new List<string>(3);


        public HediffCompProperties_ChestburstPregnancyVictimHidden Props
        {
            get
            {
                return (HediffCompProperties_ChestburstPregnancyVictimHidden)this.props;
            }
        }

   

        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {

            float severityToTurn = Props.severityToTurn;

            Map map = this.parent.pawn.Corpse.Map;
            if (map != null && this.parent.Severity > severityToTurn)
            {

                Hatch();

                for (int i = 0; i < 20; i++)
                {
                    IntVec3 c;
                    CellFinder.TryFindRandomReachableCellNearPosition(this.parent.pawn.Corpse.Position, this.parent.pawn.Corpse.Position, map, 2, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), null, null, out c);

                    FilthMaker.TryMakeFilth(c, this.parent.pawn.Corpse.Map, ThingDefOf.Filth_Blood);

                }


                InternalDefOf.Hive_Spawn.PlayOneShot(new TargetInfo(this.parent.pawn.Corpse.Position, map, false));


            }

        }

        public void Hatch()
        {
            //try            {
            PawnGenerationRequest request;

            //request = new PawnGenerationRequest(mother.kindDef, mother.Faction, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: false, allowDead: false, allowDowned: true, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false,  0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, forceNoIdeo: false, forceNoBackstory: false, forbidAnyTitle: false, forceDead: false, null, null, null, null, null, 0f, DevelopmentalStage.Newborn);
            request = new PawnGenerationRequest(this.parent.pawn.kindDef, Faction.OfPlayerSilentFail, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: false, allowDead: false, allowDowned: true, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, RandomLastName(this.parent.pawn, null), null, null, null, forceNoIdeo: true, forceNoBackstory: false, forbidAnyTitle: false, false, null, forcedXenotype: InternalDefOf.VRE_Insector, forcedCustomXenotype: null, allowedXenotypes: null, forceBaselinerChance: 0f, developmentalStages: DevelopmentalStage.Newborn);

            Pawn pawn = PawnGenerator.GeneratePawn(request);

            if (PawnUtility.TrySpawnHatchedOrBornPawn(pawn, this.parent.pawn.Corpse))
            {

                if (pawn != null)
                {
                    if (this.parent.pawn.Corpse != null)
                    {
                        if (pawn.playerSettings != null && this.parent.pawn.playerSettings != null)
                        {
                            pawn.playerSettings.AreaRestrictionInPawnCurrentMap = this.parent.pawn.playerSettings.AreaRestrictionInPawnCurrentMap;
                        }
                       
                       
                    }

                }


                Find.LetterStack.ReceiveLetter("VRE_ChestburstBirthHiddenLabel".Translate(pawn.NameShortColored), "VRE_ChestburstBirthHidden".Translate(this.parent.pawn.NameShortColored,pawn.NameShortColored), LetterDefOf.PositiveEvent, (TargetInfo)pawn);


            }
            else
            {
                Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
            }




            // }
            //catch (Exception) { Log.Message("something failed on the try catch"); }



        }

        private static string RandomLastName(Pawn geneticMother, Pawn father)
        {
            tmpLastNames.Clear();
            if (geneticMother != null)
            {
                tmpLastNames.Add(PawnNamingUtility.GetLastName(geneticMother));
            }
            if (father != null)
            {
                tmpLastNames.Add(PawnNamingUtility.GetLastName(father));
            }

            if (tmpLastNames.Count == 0)
            {
                return null;
            }
            return tmpLastNames.RandomElement();
        }

   

        public override IEnumerable<Gizmo> CompGetGizmos()
        {
            base.CompGetGizmos();
            if (!DebugSettings.ShowDevGizmos)
            {
                yield break;
            }

            yield return new Command_Action
            {
                defaultLabel = "DEV: Advance severity",
                defaultDesc = "haha fetus goes brrrr",
                action = delegate
                {
                    this.parent.Severity += 0.1f;
                }

            };



        }
    }
}



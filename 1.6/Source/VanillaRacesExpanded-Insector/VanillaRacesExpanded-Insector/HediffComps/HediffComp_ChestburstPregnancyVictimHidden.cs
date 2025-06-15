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

        public static List<string> tmpLastNames = new(3);

        private HediffCompProperties_ChestburstPregnancyVictimHidden Props => (HediffCompProperties_ChestburstPregnancyVictimHidden)this.props;

        private bool spawned = false;

        public override void CompPostTick(ref float severityAdjustment)
        {
            if (parent.CauseDeathNow())
            {
                SpawnNewBorn(Pawn.Map, Pawn.Position, Pawn, Props.severityToTurn);
            }
            if (spawned)
            {
                Pawn.health.RemoveHediff(parent);
            }
        }

        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            SpawnNewBorn(this.parent.pawn.Corpse.Map, this.parent.pawn.Corpse.Position, this.parent.pawn.Corpse, Props.severityToTurn);
            Pawn.health.RemoveHediff(parent);
        }

        public virtual void SpawnNewBorn(Map map, IntVec3 position, Thing motherOrEgg, float severityToTurn)
        {
            if (spawned)
            {
                Pawn.health.RemoveHediff(parent);
                return;
            }
            if (map != null && this.parent.Severity > severityToTurn)
            {
                Hatch(motherOrEgg);
                for (int i = 0; i < 20; i++)
                {
                    CellFinder.TryFindRandomReachableCellNearPosition(position, position, map, 2, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false), null, null, out IntVec3 c);
                    FilthMaker.TryMakeFilth(c, map, ThingDefOf.Filth_Blood);
                }
                InternalDefOf.Hive_Spawn.PlayOneShot(new TargetInfo(position, map, false));
            }
        }

        public virtual void Hatch(Thing motherOrEgg)
        {
            PawnGenerationRequest request = new(this.parent.pawn.kindDef, Faction.OfPlayerSilentFail, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: false, allowDead: false, allowDowned: true, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, RandomLastName(this.parent.pawn, null), null, null, null, forceNoIdeo: true, forceNoBackstory: false, forbidAnyTitle: false, false, null, forcedXenotype: InternalDefOf.VRE_Insector, forcedCustomXenotype: null, allowedXenotypes: null, forceBaselinerChance: 0f, developmentalStages: DevelopmentalStage.Newborn);
            Pawn pawn = PawnGenerator.GeneratePawn(request);
            if (PawnUtility.TrySpawnHatchedOrBornPawn(pawn, motherOrEgg))
            {
                if (pawn != null && motherOrEgg != null && pawn.playerSettings != null && this.parent.pawn.playerSettings != null)
                {
                    pawn.playerSettings.AreaRestrictionInPawnCurrentMap = this.parent.pawn.playerSettings.AreaRestrictionInPawnCurrentMap;
                }
                spawned = true;
                Find.LetterStack.ReceiveLetter("VRE_ChestburstBirthHiddenLabel".Translate(pawn.NameShortColored), "VRE_ChestburstBirthHidden".Translate(this.parent.pawn.NameShortColored,pawn.NameShortColored), LetterDefOf.PositiveEvent, (TargetInfo)pawn);
            }
            else
            {
                Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
            }
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


        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_Values.Look(ref spawned, "newBornSpawned");
        }

    }
}



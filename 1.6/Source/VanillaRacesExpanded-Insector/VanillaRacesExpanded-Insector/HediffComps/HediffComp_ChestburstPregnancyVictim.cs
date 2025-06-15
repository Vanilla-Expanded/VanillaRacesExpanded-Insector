using RimWorld;
using Verse;
using Verse.Sound;
using System;
using System.Collections.Generic;
using RimWorld.Planet;


namespace VanillaRacesExpandedInsector
{
    public class HediffComp_ChestburstPregnancyVictim : HediffComp_ChestburstPregnancyVictimHidden
    {
        public GeneSet genes;
        public Pawn mother;
        public Pawn father;
        //private static List<string> tmpLastNames = new List<string>(3);


        private HediffCompProperties_ChestburstPregnancyVictim Props => (HediffCompProperties_ChestburstPregnancyVictim)this.props;

        public override void CompExposeData()
        {
            base.CompExposeData();
            Scribe_References.Look(ref father, "father", saveDestroyedThings: true);
            Scribe_References.Look(ref mother, "mother", saveDestroyedThings: true);
            Scribe_Deep.Look(ref genes, "genes");
        }

        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            SpawnNewBorn(this.parent.pawn.Corpse.Map, this.parent.pawn.Corpse.Position, this.parent.pawn.Corpse, Props.severityToTurn);
            Pawn.health.RemoveHediff(parent);
        }

        public override void Hatch(Thing motherOrEgg)
        {
            PawnGenerationRequest request = new(mother.kindDef, mother.Faction, PawnGenerationContext.NonPlayer, -1, forceGenerateNewPawn: false, allowDead: false, allowDowned: true, canGeneratePawnRelations: true, mustBeCapableOfViolence: false, 1f, forceAddFreeWarmLayerIfNeeded: false, allowGay: true, allowPregnant: false, allowFood: true, allowAddictions: true, inhabitant: false, certainlyBeenInCryptosleep: false, forceRedressWorldPawnIfFormerColonist: false, worldPawnFactionDoesntMatter: false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, RandomLastName(mother, father), null, null, null, forceNoIdeo: true, forceNoBackstory: false, forbidAnyTitle: false, false, null, forcedXenotype: XenotypeDefOf.Baseliner, forcedEndogenes: (genes != null) ? genes.GenesListForReading : PregnancyUtility.GetInheritedGenes(father, mother), forcedCustomXenotype: null, allowedXenotypes: null, forceBaselinerChance: 0f, developmentalStages: DevelopmentalStage.Newborn);
            Pawn pawn = PawnGenerator.GeneratePawn(request);
            if (PawnUtility.TrySpawnHatchedOrBornPawn(pawn, motherOrEgg))
            {
                if (pawn != null && mother != null)
                {
                    if (pawn.playerSettings != null && mother.playerSettings != null)
                    {
                        pawn.playerSettings.AreaRestrictionInPawnCurrentMap = mother.playerSettings.AreaRestrictionInPawnCurrentMap;
                    }
                    if (pawn.RaceProps.IsFlesh)
                    {
                        pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, mother);
                        if (father != null)
                        {
                            pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, father);
                        }
                    }
                    if (GeneUtility.SameHeritableXenotype(mother, father) && mother.genes.UniqueXenotype)
                    {
                        pawn.genes.xenotypeName = mother.genes.xenotypeName;
                        pawn.genes.iconDef = mother.genes.iconDef;
                    }
                    if (TryGetInheritedXenotype(mother, father, out var xenotype))
                    {
                        pawn.genes?.SetXenotypeDirect(xenotype);
                    }
                    else if (ShouldByHybrid(mother, father))
                    {
                        pawn.genes.hybrid = true;
                        pawn.genes.xenotypeName = "Hybrid".Translate();
                    }
                }
                Find.LetterStack.ReceiveLetter("VRE_ChestburstBirthLabel".Translate(pawn.NameShortColored), "VRE_ChestburstBirth".Translate(pawn.NameShortColored), LetterDefOf.PositiveEvent, (TargetInfo)pawn);
            }
            else
            {
                Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
            }
        }

        private static string RandomLastName(Pawn geneticMother,  Pawn father)
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

        private static bool TryGetInheritedXenotype(Pawn mother, Pawn father, out XenotypeDef xenotype)
        {
            bool flag = mother?.genes != null;
            bool flag2 = father?.genes != null;
            if (flag && flag2 && mother.genes.Xenotype.inheritable && father.genes.Xenotype.inheritable && mother.genes.Xenotype == father.genes.Xenotype)
            {
                xenotype = mother.genes.Xenotype;
                return true;
            }
            if (flag && !flag2 && mother.genes.Xenotype.inheritable)
            {
                xenotype = mother.genes.Xenotype;
                return true;
            }
            if (flag2 && !flag && father.genes.Xenotype.inheritable)
            {
                xenotype = father.genes.Xenotype;
                return true;
            }
            xenotype = null;
            return false;
        }

        private static bool ShouldByHybrid(Pawn mother, Pawn father)
        {
            bool flag = mother?.genes != null;
            bool flag2 = father?.genes != null;
            if (flag && flag2)
            {
                if (mother.genes.hybrid && father.genes.hybrid)
                {
                    return true;
                }
                if (mother.genes.Xenotype.inheritable && father.genes.Xenotype.inheritable)
                {
                    return true;
                }
                bool num = flag && (mother.genes.Xenotype.inheritable || mother.genes.hybrid);
                bool flag3 = flag2 && (father.genes.Xenotype.inheritable || father.genes.hybrid);
                if (num || flag3)
                {
                    return true;
                }
            }
            if ((flag && !flag2 && mother.genes.hybrid) || (flag2 && !flag && father.genes.hybrid))
            {
                return true;
            }
            return false;
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



using RimWorld;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace VanillaRacesExpandedInsector
{
    public static class Utils
    {
        public static List<GenelineGeneDef> cachedGeneDefsInOrder = null;

        public static List<GenelineGeneDef> GenelineGenesInOrder
        {
            get
            {
                if (cachedGeneDefsInOrder == null)
                {
                    cachedGeneDefsInOrder = new List<GenelineGeneDef>();

                    GameComponent_UnlockedGenes comp = GameComponent_UnlockedGenes.Instance;

                    List<GenelineGeneDef> genelinegenes = DefDatabase<GenelineGeneDef>.AllDefs.Where(x => !x.unlockable || 
                    (x.unlockable && (comp.SorneGeneUnlocked(x)|| comp.ChelisGeneUnlocked(x)|| comp.NuchadusGeneUnlocked(x)||comp.KemiaGeneUnlocked(x) || comp.XanidesGeneUnlocked(x)) ) ).ToList();


                    foreach (var allDef in genelinegenes)
                    {
                        cachedGeneDefsInOrder.Add(allDef);
                    }
                    cachedGeneDefsInOrder.SortBy((GenelineGeneDef x) => 0f - x.displayCategory.displayPriorityInXenotype, (GenelineGeneDef x) => x.displayCategory.label, (GenelineGeneDef x) => x.displayOrderInCategory);
                }
                return cachedGeneDefsInOrder;
            }
        }

        public static void SortGeneDefs(this List<GenelineGeneDef> geneDefs)
        {
            geneDefs.SortBy((GenelineGeneDef x) => 0f - x.displayCategory.displayPriorityInXenotype, (GenelineGeneDef x) => x.displayOrderInCategory, (GenelineGeneDef y) => y.label);
        }

        public static bool HasActiveGene(this Pawn pawn, GeneDef geneDef)
        {
            if (pawn?.genes is null) return false;
            return pawn.genes.GetGene(geneDef)?.Active ?? false;
        }

        public static Metapod MakeMetapod(this Pawn pawn)
        {
            var metapod = ThingMaker.MakeThing(InternalDefOf.VRE_Metapod) as Metapod;
            if (pawn.Spawned)
            {
                pawn.DeSpawn();
            }
            metapod.SetFaction(pawn.Faction);
            metapod.innerContainer.TryAddOrTransfer(pawn, false);
            return metapod;
        }



        public static void DamageUntilDowned(Pawn p, bool allowBleedingWounds = true, DamageDef damage = null, ThingDef sourceDef = null, BodyPartGroupDef bodyGroupDef = null)
        {
            if (p.Downed)
            {
                return;
            }
            HediffSet hediffSet = p.health.hediffSet;
            p.health.forceDowned = true;
            IEnumerable<BodyPartRecord> source = from x in HittablePartsViolence(hediffSet)
                                                 where !p.health.hediffSet.hediffs.Any((Hediff y) => y.Part == x && y.CurStage != null && y.CurStage.partEfficiencyOffset < 0f)
                                                 select x;
            int num = 0;
            while (num < 300 && !p.Downed && source.Any())
            {
                num++;
                BodyPartRecord bodyPartRecord = source.RandomElementByWeight((BodyPartRecord x) => x.coverageAbs);
                int num2 = Mathf.RoundToInt(hediffSet.GetPartHealth(bodyPartRecord));
                float statValue = p.GetStatValue(StatDefOf.IncomingDamageFactor);
                if (statValue > 0f)
                {
                    num2 = (int)((float)num2 / statValue);
                }
                num2 -= 3;
                if (num2 > 0 && (num2 >= 8 || num >= 250))
                {
                    if (num > 275)
                    {
                        num2 = Rand.Range(1, 8);
                    }
                    DamageDef damageDef = (damage != null) ? damage : ((bodyPartRecord.depth != BodyPartDepth.Outside) ? DamageDefOf.Blunt : ((allowBleedingWounds || !(bodyPartRecord.def.bleedRate > 0f)) ? RandomViolenceDamageType() : DamageDefOf.Blunt));
                    int num3 = Rand.RangeInclusive(Mathf.RoundToInt((float)num2 * 0.65f), num2);
                    HediffDef hediffDefFromDamage = HealthUtility.GetHediffDefFromDamage(damageDef, p, bodyPartRecord);
                    if (!p.health.WouldDieAfterAddingHediff(hediffDefFromDamage, bodyPartRecord, (float)num3 * p.GetStatValue(StatDefOf.IncomingDamageFactor)))
                    {
                        DamageInfo dinfo = new DamageInfo(damageDef, num3, 999f, -1f, null, bodyPartRecord, null, DamageInfo.SourceCategory.ThingOrUnknown, null, instigatorGuilty: true, spawnFilth: true, QualityCategory.Normal, checkForJobOverride: false);
                        dinfo.SetAllowDamagePropagation(val: false);
                        foreach (Hediff hediff in p.TakeDamage(dinfo).hediffs)
                        {
                            if (sourceDef != null)
                            {
                                hediff.sourceDef = sourceDef;
                            }
                            if (bodyGroupDef != null)
                            {
                                hediff.sourceBodyPartGroup = bodyGroupDef;
                            }
                        }
                    }
                }
            }
            if (p.Dead && !p.kindDef.forceDeathOnDowned)
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.AppendLine(p + " died during GiveInjuriesToForceDowned");
                for (int i = 0; i < p.health.hediffSet.hediffs.Count; i++)
                {
                    stringBuilder.AppendLine("   -" + p.health.hediffSet.hediffs[i]);
                }
                Log.Error(stringBuilder.ToString());
            }
            p.health.forceDowned = false;
        }

        private static IEnumerable<BodyPartRecord> HittablePartsViolence(HediffSet bodyModel)
        {
            return from x in bodyModel.GetNotMissingParts()
                   where x.depth == BodyPartDepth.Outside || (x.depth == BodyPartDepth.Inside && x.def.IsSolid(x, bodyModel.hediffs))
                   select x;
        }

        public static DamageDef RandomViolenceDamageType()
        {
            switch (Rand.RangeInclusive(0, 3))
            {
                
                case 0:
                    return DamageDefOf.Blunt;
                case 1:
                    return DamageDefOf.Stab;
                case 2:
                    return DamageDefOf.Scratch;
                case 3:
                    return DamageDefOf.Cut;
                default:
                    return null;
            }
        }

    }
}
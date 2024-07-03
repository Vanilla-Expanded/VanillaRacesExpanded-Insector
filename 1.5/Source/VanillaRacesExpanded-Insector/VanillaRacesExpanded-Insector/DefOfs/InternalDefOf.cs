using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using RimWorld;

namespace VanillaRacesExpandedInsector
{

    [DefOf]
    public static class InternalDefOf
    {
        static InternalDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
        }

        public static GeneDef VRE_GenelineEvolution;
        public static GeneDef VRE_InsectFlesh;
        public static GeneDef VRE_InsectPheromones;
        public static GeneDef VRE_HypothermicSlowdown;
        public static GeneDef VRE_Heatshock;
        public static GeneDef VRE_HypothermicHibernation;
        public static GeneDef VRE_Heatstress;
        public static GeneDef VRE_InflexibleJoints;
        public static GeneDef VRE_ChestburstPregnancy;
        public static GeneDef VRE_Hiveglands;
        public static GeneDef VRE_SpawningSack;
        public static GeneDef VRE_Parthenogenesis;
        public static GeneDef VRE_LowOctopamine;
        public static GeneDef VRE_PollutionDependency;
        public static GeneDef VRE_InsectJellyDependency;
        [MayRequire("OskarPotocki.VFE.Insectoid2")]
        public static GeneDef VRE_SwarmSynapse;

        public static ThingDef Meat_Megaspider;
        public static ThingDef VRE_Filth_BugFilth;
        public static ThingDef VRE_InfestedShipPart;
        public static ThingDef VRE_InfestedShipPart_Spawned;
        public static ThingDef VRE_ActiveDropPod;

        public static ThoughtDef AteHumanlikeMeatDirectCannibal;
        public static ThoughtDef VRE_Implanted;
        public static ThoughtDef VRE_Implanted_Social;
        [MayRequire("OskarPotocki.VFE.Insectoid2")]
        public static ThoughtDef VFEI2_AteRoyalJelly;

        public static HediffDef HypothermicSlowdown;
        public static HediffDef VRE_HeatshockHediff;
        public static HediffDef VRE_HypothermicHibernationHediff;
        public static HediffDef VRE_HeatstressHediff;
        public static HediffDef Hypothermia;
        public static HediffDef Heatstroke;
        public static HediffDef VREInsector_TempSterile;
        public static HediffDef VRE_ChestburstPregnancyHediff;
        public static HediffDef VRE_ChestburstPregnancy_Victim;
        public static HediffDef VRE_ChestburstPregnancy_Victim_Hidden;
        public static HediffDef VRE_PollutionDependencyHediff;
        public static HediffDef VRE_InsectJellyDependencyHediff;
        [MayRequire("OskarPotocki.VFE.Insectoid2")]
        public static HediffDef VFEI2_RoyalJellyHigh;
        [MayRequire("OskarPotocki.VFE.Insectoid2")]
        public static HediffDef VFEI2_EmpressSpawn;
        [MayRequire("OskarPotocki.VFE.Insectoid2")]
        public static HediffDef VFEI2_GigamiteSpawn;
        [MayRequire("OskarPotocki.VFE.Insectoid2")]
        public static HediffDef VFEI2_SilverfishSpawn;
        [MayRequire("OskarPotocki.VFE.Insectoid2")]
        public static HediffDef VFEI2_TitantickSpawn;
        [MayRequire("OskarPotocki.VFE.Insectoid2")]
        public static HediffDef VFEI2_TeramantisSpawn;

        public static XenotypeDef VRE_Insector;

        public static SoundDef Hive_Spawn;
        public static SoundDef Meal_Eat;

        [MayRequireIdeology]
        public static PreceptDef Cannibalism_Acceptable;
        [MayRequireIdeology]
        public static PreceptDef Cannibalism_Preferred;
        [MayRequireIdeology]
        public static PreceptDef Cannibalism_RequiredStrong;
        [MayRequireIdeology]
        public static PreceptDef Cannibalism_RequiredRavenous;

        public static ThingDef VRE_Metapod;
        public static HediffDef VRE_MetapodHediff, VRE_MetapodSickness;
        public static EffecterDef CocoonWakingUp;
        public static EffecterDef EatVegetarian;

        public static JobDef VRE_ChestburstImplantationJob;
        public static JobDef VRE_ConsumeInsectJelly;
        [MayRequire("OskarPotocki.VFE.Insectoid2")]
        public static JobDef VRE_IngestPherocore;

    }
}
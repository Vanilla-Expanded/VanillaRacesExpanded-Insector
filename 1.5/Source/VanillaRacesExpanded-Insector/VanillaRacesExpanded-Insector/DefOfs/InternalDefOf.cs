﻿using System;
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

        public static GeneDef VRE_InsectFlesh;
        public static GeneDef VRE_InsectPheromones;
        public static GeneDef VRE_HypothermicSlowdown;
        public static GeneDef VRE_Heatshock;

        public static ThingDef Meat_Megaspider;

        public static ThoughtDef AteHumanlikeMeatDirectCannibal;

        public static HediffDef HypothermicSlowdown;
        public static HediffDef VRE_HeatshockHediff;

        [MayRequireIdeology]
        public static PreceptDef Cannibalism_Acceptable;
        [MayRequireIdeology]
        public static PreceptDef Cannibalism_Preferred;
        [MayRequireIdeology]
        public static PreceptDef Cannibalism_RequiredStrong;
        [MayRequireIdeology]
        public static PreceptDef Cannibalism_RequiredRavenous;


    }
}
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

        public static GeneDef VRE_InsectFlesh;
        public static GeneDef VRE_InsectPheromones;

        public static ThingDef Meat_Megaspider;    
        

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
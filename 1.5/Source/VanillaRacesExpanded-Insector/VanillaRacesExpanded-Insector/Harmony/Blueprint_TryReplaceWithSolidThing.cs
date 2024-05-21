using HarmonyLib;
using RimWorld;
using Verse;
using System.Collections.Generic;
using UnityEngine;
using Verse.Noise;

namespace VanillaRacesExpandedInsector
{

    [HarmonyPatch(typeof(Blueprint))]
    [HarmonyPatch("TryReplaceWithSolidThing")]
    public static class VanillaRacesExpandedInsector_Blueprint_TryReplaceWithSolidThing_Patch
    {
        private static readonly IntRange BloodFilth = new IntRange(2, 4);

        [HarmonyPostfix]
        public static void CreateFilth(Pawn workerPawn, ref bool __result, ref Thing createdThing )
        {
            if (__result && workerPawn.HasActiveGene(InternalDefOf.VRE_Hiveglands))
            {
                if (createdThing?.Map != null && createdThing?.Position != null)
                {
                    for (int i = 0; i < BloodFilth.RandomInRange; i++)
                    {

                        IntVec3 randomCell;
                        CellFinder.TryRandomClosewalkCellNear(createdThing.Position, createdThing.Map, 1, out randomCell);
                        if (randomCell.InBounds(createdThing.Map))
                        {
                            FilthMaker.TryMakeFilth(randomCell, createdThing.Map, InternalDefOf.VRE_Filth_BugFilth);
                        }

                    }

                    
                }
            }
        }


    }













}


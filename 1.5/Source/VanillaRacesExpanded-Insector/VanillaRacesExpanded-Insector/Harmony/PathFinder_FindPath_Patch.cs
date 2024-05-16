using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Verse;
using Verse.AI;

namespace VanillaRacesExpandedInsector
{
    [HarmonyPatch(typeof(PathFinder), nameof(PathFinder.FindPath), new Type[] { typeof(IntVec3), typeof(LocalTargetInfo), typeof(TraverseParms), typeof(PathEndMode), typeof(PathFinderCostTuning) })]
    public static class PathFinder_FindPath_Patch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = instructions.ToList();
            var found = false;
            for (int i = 0; i < codes.Count; i++)
            {
                yield return codes[i];
                if (!found && codes[i].opcode == OpCodes.Stloc_S && codes[i].operand is LocalBuilder lb && lb.LocalIndex == 53)
                {
                    found = true;
                    yield return new CodeInstruction(OpCodes.Ldloc_0);
                    yield return new CodeInstruction(OpCodes.Ldloc_S, 41);
                    yield return new CodeInstruction(OpCodes.Ldloc_S, 42);
                    yield return new CodeInstruction(OpCodes.Ldloc_S, 46);
                    yield return new CodeInstruction(OpCodes.Call, typeof(PathFinder_FindPath_Patch).GetMethod(nameof(PathFinder_FindPath_Patch.ChangePathCostIfNeeded)));
                    yield return new CodeInstruction(OpCodes.Stloc_S, 46);
                }
            }
            if (!found)
            {
                Log.Error("[VRE Insectoids] PathFinder.FindPath Transpiler failed. The code won't work.");
            }
        }

        static public float ChangePathCostIfNeeded(Pawn pawn, int xCell, int zCell, float cost)
        {
            if (pawn.HasActiveGene(InternalDefOf.VRE_InflexibleJoints))
            {
                var cell = new IntVec3(xCell, 0, zCell);
                var terrain = cell.GetTerrain(pawn.Map);
                if (terrain.pathCost > 4)
                {
                    return cost + terrain.pathCost;
                }
            }
            return cost;
        }
    }
}


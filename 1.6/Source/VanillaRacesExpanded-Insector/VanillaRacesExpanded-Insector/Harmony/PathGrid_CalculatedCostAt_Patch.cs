using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;
using Verse.AI;

namespace VanillaRacesExpandedInsector;

[HarmonyPatch(typeof(PathGrid), nameof(PathGrid.CalculatedCostAt))]
public static class PathGrid_CalculatedCostAt_Patch
{
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase original)
    {
        var targetField = typeof(BuildableDef).DeclaredField(nameof(BuildableDef.pathCost));
        var instrList = instructions.ToList();
        var patched = 0;

        for (var i = 0; i < instrList.Count; i++)
        {
            var instr = instrList[i];

            // num = terrainDef.pathCost
            // Insert after "terrainDef.pathCost" is pushed to the stack, but before the local value is set using it
            if (i > 2 && instr.opcode == OpCodes.Stloc_0 && instrList[i - 2].opcode == OpCodes.Ldloc_2 && instrList[i - 1].LoadsField(targetField))
            {
                yield return CodeInstruction.Call(typeof(PathGrid_CalculatedCostAt_Patch), nameof(ReplaceValue));
                patched++;
            }

            yield return instr;
        }

        const int expected = 1;
        if (patched != expected)
        {
            var name = (original.DeclaringType?.Namespace).NullOrEmpty() ? original.Name : $"{original.DeclaringType!.Name}:{original.Name}";
            Log.Warning($"Patched incorrect number of {nameof(PathGrid.CalculatedCostAt)} calls (patched {patched}, expected {expected}) for method {name}");
        }
    }

    public static int ReplaceValue(int terrainCost)
    {
        if (Pawn_PathFollower_CostToMoveIntoCell_Patch.activePawn != null && terrainCost > 4)
            terrainCost *= 2;
        return terrainCost;
    }
}
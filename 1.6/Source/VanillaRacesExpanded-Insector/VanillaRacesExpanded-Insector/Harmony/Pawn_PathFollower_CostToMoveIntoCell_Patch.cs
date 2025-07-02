using HarmonyLib;
using System;
using Verse;
using Verse.AI;

namespace VanillaRacesExpandedInsector;

[HarmonyPatch(typeof(Pawn_PathFollower), "CostToMoveIntoCell", new Type[] { typeof(Pawn), typeof(IntVec3) })]
public static class Pawn_PathFollower_CostToMoveIntoCell_Patch
{
    // Rather than using this prefix/finalizer combo with PathGrid.CalculatedCostAt transpiler, we could just
    // increase the path cost by the terrain cost. However, this would cause several unintended consequences:
    // - It would increase pathing cost for flying
    // - It would ignore terrain cost overrides for that pawn
    // - It would raise cost in situations where terrain cost doesn't apply, like buildings having higher path cost
    // - Ignore speed boosts from PawnKindDef.moveSpeedFactorByTerrainTag
    // - Ignore speed changes caused from the job's locomotion urgency
    // - Possibly raise the path cost over the maximum allowed of 450 (or 900/1350 on lower locomotion urgency)

    public static Pawn activePawn;

    public static void Prefix(Pawn pawn)
    {
        if (pawn.Map != null && pawn.HasActiveGene(InternalDefOf.VRE_InflexibleJoints))
            activePawn = pawn;
    }

    public static void Finalizer()
    {
        activePawn = null;
    }
}
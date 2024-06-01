using RimWorld;
using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace VanillaRacesExpandedInsector
{
    public class ScenPart_LowOrbitCrash : ScenPart
    {
        public override void GenerateIntoMap(Map map)
        {
            if (Find.GameInitData == null)
                return;

           
            this.DropThingGroupsNear(MapGenerator.PlayerStartSpot, map, CreateThingGroups());

            /*for (int i = 0; i < 3; i++)
            {*/
                TryFindShipChunkDropCell(InternalDefOf.VRE_InfestedShipPart, MapGenerator.PlayerStartSpot, map, out IntVec3 spawnPos);
                Thing inner = ThingMaker.MakeThing(InternalDefOf.VRE_InfestedShipPart_Spawned);
                inner.SetFactionDirect(Faction.OfPlayer);


                SkyfallerMaker.SpawnSkyfaller(InternalDefOf.VRE_InfestedShipPart, inner, spawnPos, map);

            /*}*/
        }


        private bool TryFindShipChunkDropCell(ThingDef chunk, IntVec3 nearLoc, Map map, out IntVec3 pos) => CellFinderLoose.TryFindSkyfallerCell(chunk, map, out pos, nearLoc: nearLoc, nearLocMaxDist: 15);

        // Pod utils
        private List<List<Thing>> CreateThingGroups()
        {
            List<List<Thing>> thingsGroups = new();
            foreach (Pawn pawn in Find.GameInitData.startingAndOptionalPawns)
                thingsGroups.Add(new List<Thing>() { pawn });

            List<Thing> thingList = new();
            foreach (ScenPart allPart in Find.Scenario.AllParts)
                thingList.AddRange(allPart.PlayerStartingThings());

            int index = 0;
            foreach (Thing thing in thingList)
            {
                if (thing.def.CanHaveFaction) thing.SetFactionDirect(Faction.OfPlayer);

                thingsGroups[index].Add(thing);

                ++index;
                if (index >= thingsGroups.Count) index = 0;
            }

            return thingsGroups;
        }

        private void DropThingGroupsNear(IntVec3 dropCenter, Map map, List<List<Thing>> thingsGroups, int openDelay = 110)
        {
            foreach (List<Thing> thingsGroup in thingsGroups)
            {
                if (!DropCellFinder.TryFindDropSpotNear(dropCenter, map, out IntVec3 result, false, false) && (false || !DropCellFinder.TryFindDropSpotNear(dropCenter, map, out result, false, true)))
                {
                    Log.Warning("DropThingsNear failed to find a place to drop " + thingsGroup.FirstOrDefault() + " near " + dropCenter + ". Dropping on random square instead.");
                    result = CellFinderLoose.RandomCellWith(c => c.Walkable(map), map);
                }

                for (int index = 0; index < thingsGroup.Count; ++index)
                    thingsGroup[index].SetForbidden(true, false);

                ActiveDropPodInfo info = new();
                foreach (Thing thing in thingsGroup)
                    info.innerContainer.TryAdd(thing);
                info.openDelay = openDelay;
                info.leaveSlag = true;
                this.MakeDropPodAt(result, map, info);
            }
        }

        private void MakeDropPodAt(IntVec3 c, Map map, ActiveDropPodInfo info)
        {
            ActiveDropPod activeDropPod = (ActiveDropPod)ThingMaker.MakeThing(ThingDefOf.ActiveDropPod);
            activeDropPod.Contents = info;
            SkyfallerMaker.SpawnSkyfaller(InternalDefOf.VRE_InfestedShipPart, activeDropPod, c, map);
            foreach (Thing thing in activeDropPod.Contents.innerContainer)
            {
                if (thing is Pawn p1 && p1.IsWorldPawn())
                {
                    Find.WorldPawns.RemovePawn(p1);
                    p1.psychicEntropy?.SetInitialPsyfocusLevel();
                }
            }
        }

        public override void PostMapGenerate(Map map)
        {
            if (Find.GameInitData == null)
                return;
            PawnUtility.GiveAllStartingPlayerPawnsThought(ThoughtDefOf.CrashedTogether);
        }
    }
}

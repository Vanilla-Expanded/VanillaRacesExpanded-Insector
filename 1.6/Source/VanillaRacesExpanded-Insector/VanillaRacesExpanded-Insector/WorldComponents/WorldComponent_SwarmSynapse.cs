using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
using RimWorld.Planet;

namespace VanillaRacesExpandedInsector
{
    public class WorldComponent_SwarmSynapse : WorldComponent
    {
        public static WorldComponent_SwarmSynapse Instance;
      
        public WorldComponent_SwarmSynapse(World world) : base(world)
        {
          
            Instance = this;
        }

        public int tickCounter = 0;
        public int tickInterval = 10000;
        public int pawnsWithSwarmSynapse = 0;

        public override void ExposeData()
        {
            Instance = this;
            base.ExposeData();
            Scribe_Values.Look<int>(ref this.tickCounter, "tickCounterSwarmSynapse", 0, true);
            Scribe_Values.Look<int>(ref this.pawnsWithSwarmSynapse, "pawnsWithSwarmSynapse", 0, true);

        }


        public override void WorldComponentTick()
        {


            tickCounter++;
            if ((tickCounter > tickInterval))
            {
                pawnsWithSwarmSynapse = 0;
                List<Pawn> colonists = PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_FreeColonists;
                foreach (Pawn pawn in colonists)
                {
                    if (pawn.genes?.HasActiveGene(InternalDefOf.VRE_SwarmSynapse) == true)
                    {
                        pawnsWithSwarmSynapse++;
                    }

                }


                tickCounter = 0;
            }



        }

    }
}
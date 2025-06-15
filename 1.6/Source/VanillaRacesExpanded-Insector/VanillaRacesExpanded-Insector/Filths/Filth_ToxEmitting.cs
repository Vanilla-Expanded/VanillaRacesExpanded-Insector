using Verse;
using RimWorld;
using Verse.Sound;
using Verse.Noise;
using System.Collections.Generic;
using System;

namespace VanillaRacesExpandedInsector
{
    class Filth_ToxEmitting : Filth
    {

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            GasUtility.AddGas(this.PositionHeld, this.Map, GasType.ToxGas, 2000);
        }

    }
}
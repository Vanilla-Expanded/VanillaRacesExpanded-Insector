using System;
using Verse;
using System.Collections.Generic;

namespace VanillaRacesExpandedInsector
{
    public class HediffCompProperties_ChestburstPregnancyVictim : HediffCompProperties
    {


        public float severityToTurn = 0.9f;

        public HediffCompProperties_ChestburstPregnancyVictim()
        {
            this.compClass = typeof(HediffComp_ChestburstPregnancyVictim);
        }
    }
}


using Verse;
using System;
using RimWorld;
using System.Collections.Generic;
using System.Linq;
using VEF.CacheClearing;


namespace VanillaRacesExpandedInsector
{

    public static class StaticCollectionsClass
    {

        static StaticCollectionsClass()
        {
            ClearCaches.clearCacheTypes.Add(typeof(StaticCollectionsClass));

        }


        // A list of colonists needing chestburst implantation
        public static HashSet<Thing> chestburst_implantation_needed = new HashSet<Thing>();

     

        public static void AddColonistToChestburstImplantationAlert(Thing thing)
        {

            if (!chestburst_implantation_needed.Contains(thing))
            {
                chestburst_implantation_needed.Add(thing);
            }
        }

        public static void RemoveColonistFromChestburstImplantationAlert(Thing thing)
        {
            if (chestburst_implantation_needed.Contains(thing))
            {
                chestburst_implantation_needed.Remove(thing);
            }

        }

      


    }
}

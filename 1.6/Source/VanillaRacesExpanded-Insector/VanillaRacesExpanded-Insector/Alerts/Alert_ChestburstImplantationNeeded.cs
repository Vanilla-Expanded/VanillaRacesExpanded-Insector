using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;

namespace VanillaRacesExpandedInsector
{
    public class Alert_ChestburstImplantationNeeded : Alert
    {
        public Alert_ChestburstImplantationNeeded()
        {
            defaultPriority = AlertPriority.High;
            defaultLabel = "VRE_ChestburstImplantationNeeded".Translate();

        }

        public override AlertReport GetReport()
        {

            var map = Find.CurrentMap;
            if (map == null)
            {
                return AlertReport.Inactive;
            }

            return AlertReport.CulpritsAre(StaticCollectionsClass.chestburst_implantation_needed.ToList());
        }

        public override TaggedString GetExplanation()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (Thing thing in StaticCollectionsClass.chestburst_implantation_needed)
            {
                Pawn pawn = thing as Pawn;
                stringBuilder.AppendLine("  - " + pawn.NameShortColored.Resolve());
            }
            return "VRE_ChestburstImplantationNeededDesc".Translate(stringBuilder.ToString());
        }
    }
}

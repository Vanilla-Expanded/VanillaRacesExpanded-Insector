using RimWorld;
using System.Collections.Generic;
using System.Linq;
using Verse;
namespace VanillaRacesExpandedInsector
{

    public class Gene_GenelineEvolution : Gene
    {
        public Geneline geneline;

        public override IEnumerable<Gizmo> GetGizmos()
        {
            if (pawn.IsColonistPlayerControlled)
            {
                yield return new Command_Action
                {
                    defaultLabel = geneline != null ? geneline.name : "VRE_ManageGenelines".Translate(),
                    defaultDesc = "",
                    icon = def.Icon,
                    action = () =>
                    {
                        if (geneline is null)
                        {

                        }
                        else
                        {
                            var options = new List<FloatMenuOption>();
                            foreach (var allGeneline in GameComponent_Genelines.Instance.genelines.Where(x => x != geneline))
                            {
                                options.Add(new FloatMenuOption(allGeneline.name, delegate
                                {
                                    geneline?.RemovePawn(pawn);
                                    allGeneline.AddPawn(pawn);
                                    geneline = allGeneline;
                                }));
                            }
                            options.Add(new FloatMenuOption("VRE_ManageGenelines".Translate(), delegate
                            {

                            }));
                            Find.WindowStack.Add(new FloatMenu(options));
                        }
                    }
                };
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref geneline, "geneline");
        }
    }
}
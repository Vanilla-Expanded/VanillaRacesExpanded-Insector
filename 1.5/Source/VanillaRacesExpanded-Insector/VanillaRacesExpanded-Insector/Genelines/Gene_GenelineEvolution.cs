using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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
                var manageGenelines = new Command_Action
                {
                    defaultLabel = geneline != null ? geneline.name : "VRE_ManageGenelines".Translate(),
                    defaultDesc = "VRE_ManageGenelinesDesc".Translate(),
                    icon = ContentFinder<Texture2D>.Get("UI/Abilities/GenelinePanel"),
                    action = () =>
                    {
                        var options = new List<FloatMenuOption>();
                        foreach (var allGeneline in GameComponent_Genelines.Instance.genelines.Where(x => x != geneline))
                        {
                            options.Add(new FloatMenuOption(geneline != null ? "VRE_ChangeTo".Translate(allGeneline.name) : allGeneline.name, delegate
                            {
                                allGeneline.AddPawnWithMetapod(pawn);
                            }));
                        }
                        options.Add(new FloatMenuOption("VRE_ManageGenelines".Translate() + "...", delegate
                        {
                            Find.WindowStack.Add(new Window_ManageGenelines());
                        }));
                        Find.WindowStack.Add(new FloatMenu(options));
                    }
                };
                if (pawn.health.hediffSet.GetFirstHediffOfDef(InternalDefOf.VRE_MetapodSickness) != null)
                {
                    manageGenelines.Disable("VRE_MetapodSicknessCannotChangeGeneline".Translate());
                }
                yield return manageGenelines;
            }
        }

        public override void PostRemove()
        {
            base.PostRemove();
            geneline?.RemovePawn(pawn);
        }

        public override void Notify_PawnDied(DamageInfo? dinfo, Hediff culprit = null)
        {
            base.Notify_PawnDied(dinfo, culprit);
            geneline?.RemovePawn(pawn);
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref geneline, "geneline");
        }
    }
}
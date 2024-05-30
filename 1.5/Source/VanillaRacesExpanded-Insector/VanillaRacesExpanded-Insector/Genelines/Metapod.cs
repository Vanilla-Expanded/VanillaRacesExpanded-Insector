using RimWorld;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
namespace VanillaRacesExpandedInsector
{
    [HotSwappable]
    public class Metapod : Building, IThingHolder
    {
        public ThingOwner innerContainer;
        public Pawn Pawn => (Pawn)innerContainer.FirstOrDefault();
        protected Effecter progressBarEffecter;
        private Effecter wakeUpEffect;
        public CompRefuelable compRefuelable;

        public Metapod()
        {
            innerContainer = new ThingOwner<Thing>(this, oneStackOnly: false);
        }

        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, respawningAfterLoad);
            compRefuelable = GetComp<CompRefuelable>();
        }

        public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
        {
            base.DeSpawn(mode);
            progressBarEffecter?.Cleanup();
        }
        public override string Label => base.Label + ": " + Pawn?.LabelShort;

        public override void Tick()
        {
            base.Tick();
            var pawn = Pawn;
            if (pawn != null)
            {
                if (Spawned)
                {
                    var metapodHediff = pawn.health.hediffSet.GetFirstHediff<MetapodHediff>();
                    metapodHediff.jellyFueled = compRefuelable.HasFuel;
                    if ((5 * 60) >= metapodHediff.TicksEmergesIn)
                    {
                        if (wakeUpEffect == null)
                        {
                            wakeUpEffect = InternalDefOf.CocoonWakingUp.SpawnAttached(this, this.Map);
                        }
                        wakeUpEffect.EffectTick(this, this);
                    }
                    DoProgressBar(metapodHediff.GetProgress());
                    metapodHediff.Tick();
                    if (metapodHediff.growthPercent >= 1f)
                    {
                        Complete(pawn);
                    }
                }
            }
            else if (Spawned)
            {
                innerContainer.TryDropAll(Position, Map, ThingPlaceMode.Near);
            }
        }

        private void Complete(Pawn pawn)
        {
            wakeUpEffect?.Cleanup();
            innerContainer.TryDrop(pawn, ThingPlaceMode.Direct, out _);
            SpawnInsectStuff(pawn, pawn.Position, pawn.Map);
            Destroy();
        }

        public override IEnumerable<Gizmo> GetGizmos()
        {
            foreach (var g in base.GetGizmos())
            {
                yield return g;
            }
            if (DebugSettings.ShowDevGizmos)
            {
                yield return new Command_Action
                {
                    defaultLabel = "DEV: Near Complete",
                    action = () =>
                    {
                        var pawn = Pawn;
                        var metapodHediff = pawn.health.hediffSet.GetFirstHediff<MetapodHediff>();
                        metapodHediff.growthPercent = 0.99f;
                    }
                };
            }
        }

        private static readonly IntRange BloodFilth = new IntRange(6, 9);

        public void SpawnInsectStuff(Pawn pawn, IntVec3 pos, Map map)
        {
            CellRect cellRect = new CellRect(pos.x, pos.z, 3, 3).ClipInsideMap(map);
            for (int i = 0; i < BloodFilth.RandomInRange; i++)
            {
                IntVec3 randomCell = cellRect.RandomCell;
                if (randomCell.InBounds(map) && GenSight.LineOfSight(randomCell, pos, map))
                {
                    FilthMaker.TryMakeFilth(randomCell, map, ThingDefOf.Filth_BloodInsect);
                }
            }
        }


        private void DoProgressBar(float progress)
        {
            if (progressBarEffecter == null)
            {
                progressBarEffecter = EffecterDefOf.ProgressBar.Spawn();
            }
            MoteProgressBar mote = ((SubEffecter_ProgressBar)progressBarEffecter.children[0]).mote;
            if (mote.DestroyedOrNull())
            {
                progressBarEffecter = EffecterDefOf.ProgressBar.Spawn();
                progressBarEffecter.EffectTick(this, TargetInfo.Invalid);
                mote = ((SubEffecter_ProgressBar)progressBarEffecter.children[0]).mote;
            }
            else
            {
                progressBarEffecter.EffectTick(this, TargetInfo.Invalid);
            }
            mote.yOffset += 1;
            mote.progress = progress;
        }

        public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
        {
            base.Destroy(mode);
            innerContainer.ClearAndDestroyContents();
        }

        public ThingOwner GetDirectlyHeldThings()
        {
            return innerContainer;
        }

        public void GetChildHolders(List<IThingHolder> outChildren)
        {
            ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, GetDirectlyHeldThings());
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Deep.Look(ref innerContainer, "innerContainer", this);
        }

        public override string GetInspectStringLowPriority()
        {
            string text = base.GetInspectStringLowPriority();
            if (!text.NullOrEmpty())
            {
                text += "\n";
            }
            var pawn = Pawn;
            if (pawn != null)
            {
                var hediff = pawn.health.hediffSet.GetFirstHediff<MetapodHediff>();
                var period = hediff.TicksEmergesIn.ToStringTicksToPeriod();
                text += "VRE_MetapodTimer".Translate(pawn.Named("PAWN"), period);
            }
            return text;
        }
    }
}
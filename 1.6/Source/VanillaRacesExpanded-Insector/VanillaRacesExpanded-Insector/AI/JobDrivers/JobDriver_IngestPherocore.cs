
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;
using Verse.AI;

namespace VanillaRacesExpandedInsector
{
    public class JobDriver_IngestPherocore : JobDriver
    {
        private int useDuration = 100;

      

        public override bool TryMakePreToilReservations(bool errorOnFailed)
        {
            return pawn.Reserve(job.targetA, job, 1, -1, null, errorOnFailed);
        }

        protected override IEnumerable<Toil> MakeNewToils()
        {
            job.count = 1;
            this.FailOnIncapable(PawnCapacityDefOf.Manipulation);
            yield return Toils_Goto.GotoThing(TargetIndex.A, PathEndMode.Touch).FailOnDespawnedOrNull(TargetIndex.A);
            

            Toil toil = Toils_General.Wait(useDuration);
            toil.WithProgressBarToilDelay(TargetIndex.A);
            toil.FailOnDespawnedNullOrForbidden(TargetIndex.A);
            toil.FailOnCannotTouch(TargetIndex.A, PathEndMode.Touch);
            if (job.targetA.IsValid)
            {
                toil.FailOnDespawnedOrNull(TargetIndex.A);

            }
            yield return toil;
            Toil use = new Toil();

            use.initAction = delegate
            {
                Pawn actor = use.actor;
                

                ThingDef pherocore = job.targetA.Thing.def;

                if(pherocore == InternalDefOf.VFEI2_PherocoreSorne)
                {
                    Dictionary<GeneDef, bool> sorneGenes = WorldComponent_UnlockedGenes.Instance.sorne_pherocore_genes;
                    if(sorneGenes.Values.Any(x => x== false))
                    {
                        GeneDef gene = sorneGenes.Keys.Where(x => sorneGenes[x] == false).RandomElement();
                        sorneGenes[gene] = true;
                        GenelineGeneDef genelinegene = gene as GenelineGeneDef;
                        Messages.Message("VRE_PherocoreConsumed".Translate(pherocore.LabelCap,gene.LabelCap,IsEvolutionOrMutation(genelinegene)), pawn, MessageTypeDefOf.PositiveEvent, true);
                        DecreaseOrDestroy(TargetA.Thing);
                        Utils.cachedGeneDefsInOrder = null;
                        if (!sorneGenes.Values.Any(x => x == false))
                        {
                            WorldComponent_UnlockedGenes.Instance.allSorneGenesUnlocked = true;
                        }
                    } else {
                        Messages.Message("VRE_NoUnlockableGenes".Translate(pherocore.LabelCap), pawn, MessageTypeDefOf.NegativeEvent, true);
                    }
                }
                else
                if (pherocore == InternalDefOf.VFEI2_PherocoreNuchadus)
                {
                    Dictionary<GeneDef, bool> nuchadusGenes = WorldComponent_UnlockedGenes.Instance.nuchadus_pherocore_genes;
                    if (nuchadusGenes.Values.Any(x => x == false))
                    {
                        GeneDef gene = nuchadusGenes.Keys.Where(x => nuchadusGenes[x] == false).RandomElement();
                        nuchadusGenes[gene] = true;
                        GenelineGeneDef genelinegene = gene as GenelineGeneDef;
                        Messages.Message("VRE_PherocoreConsumed".Translate(pherocore.LabelCap, gene.LabelCap, IsEvolutionOrMutation(genelinegene)), pawn, MessageTypeDefOf.PositiveEvent, true);
                        DecreaseOrDestroy(TargetA.Thing);
                        Utils.cachedGeneDefsInOrder = null;
                        if (!nuchadusGenes.Values.Any(x => x == false))
                        {
                            WorldComponent_UnlockedGenes.Instance.allNuchadusGenesUnlocked = true;
                        }
                    }
                    else
                    {
                        Messages.Message("VRE_NoUnlockableGenes".Translate(pherocore.LabelCap), pawn, MessageTypeDefOf.NegativeEvent, true);
                    }
                }
                else
                if (pherocore == InternalDefOf.VFEI2_PherocoreChelis)
                {
                    Dictionary<GeneDef, bool> chelisGenes = WorldComponent_UnlockedGenes.Instance.chelis_pherocore_genes;
                    if (chelisGenes.Values.Any(x => x == false))
                    {
                        GeneDef gene = chelisGenes.Keys.Where(x => chelisGenes[x] == false).RandomElement();
                        chelisGenes[gene] = true;
                        GenelineGeneDef genelinegene = gene as GenelineGeneDef;
                        Messages.Message("VRE_PherocoreConsumed".Translate(pherocore.LabelCap, gene.LabelCap, IsEvolutionOrMutation(genelinegene)), pawn, MessageTypeDefOf.PositiveEvent, true);
                        DecreaseOrDestroy(TargetA.Thing);
                        Utils.cachedGeneDefsInOrder = null;
                        if (!chelisGenes.Values.Any(x => x == false))
                        {
                            WorldComponent_UnlockedGenes.Instance.allChelisGenesUnlocked = true;
                        }
                    }
                    else
                    {
                        Messages.Message("VRE_NoUnlockableGenes".Translate(pherocore.LabelCap), pawn, MessageTypeDefOf.NegativeEvent, true);
                    }
                }
                else
                if (pherocore == InternalDefOf.VFEI2_PherocoreKemian)
                {
                    Dictionary<GeneDef, bool> kemiaGenes = WorldComponent_UnlockedGenes.Instance.kemia_pherocore_genes;
                    if (kemiaGenes.Values.Any(x => x == false))
                    {
                        GeneDef gene = kemiaGenes.Keys.Where(x => kemiaGenes[x] == false).RandomElement();
                        kemiaGenes[gene] = true;
                        GenelineGeneDef genelinegene = gene as GenelineGeneDef;
                        Messages.Message("VRE_PherocoreConsumed".Translate(pherocore.LabelCap, gene.LabelCap, IsEvolutionOrMutation(genelinegene)), pawn, MessageTypeDefOf.PositiveEvent, true);
                        DecreaseOrDestroy(TargetA.Thing);
                        Utils.cachedGeneDefsInOrder = null;
                        if (!kemiaGenes.Values.Any(x => x == false))
                        {
                            WorldComponent_UnlockedGenes.Instance.allKemiaGenesUnlocked = true;
                        }
                    }
                    else
                    {
                        Messages.Message("VRE_NoUnlockableGenes".Translate(pherocore.LabelCap), pawn, MessageTypeDefOf.NegativeEvent, true);
                    }
                }
                else
                if (pherocore == InternalDefOf.VFEI2_PherocoreXanides)
                {
                    Dictionary<GeneDef, bool> xanidesGenes = WorldComponent_UnlockedGenes.Instance.xanides_pherocore_genes;
                    if (xanidesGenes.Values.Any(x => x == false))
                    {
                        GeneDef gene = xanidesGenes.Keys.Where(x => xanidesGenes[x] == false).RandomElement();
                        xanidesGenes[gene] = true;
                        GenelineGeneDef genelinegene = gene as GenelineGeneDef;
                        Messages.Message("VRE_PherocoreConsumed".Translate(pherocore.LabelCap, gene.LabelCap, IsEvolutionOrMutation(genelinegene)), pawn, MessageTypeDefOf.PositiveEvent, true);
                        DecreaseOrDestroy(TargetA.Thing);
                        Utils.cachedGeneDefsInOrder = null;
                        if (!xanidesGenes.Values.Any(x => x == false))
                        {
                            WorldComponent_UnlockedGenes.Instance.allXanidesGenesUnlocked = true;
                        }
                    }
                    else
                    {
                        Messages.Message("VRE_NoUnlockableGenes".Translate(pherocore.LabelCap), pawn, MessageTypeDefOf.NegativeEvent, true);
                    }
                }
                else 
                if(pherocore == DefDatabase<ThingDef>.GetNamedSilentFail("VFEI2_PherocoreBlack"))
                {
                    Dictionary<GeneDef, bool> blackHiveGenes = WorldComponent_UnlockedGenes.Instance.black_pherocore_genes;
                    if (blackHiveGenes.Values.Any(x => x == false))
                    {
                        GeneDef gene = blackHiveGenes.Keys.Where(x => blackHiveGenes[x] == false).RandomElement();
                        blackHiveGenes[gene] = true;
                        GenelineGeneDef genelinegene = gene as GenelineGeneDef;
                        Messages.Message("VRE_PherocoreConsumed".Translate(pherocore.LabelCap, gene.LabelCap, IsEvolutionOrMutation(genelinegene)), pawn, MessageTypeDefOf.PositiveEvent, true);
                        DecreaseOrDestroy(TargetA.Thing);
                        Utils.cachedGeneDefsInOrder = null;
                        if (!blackHiveGenes.Values.Any(x => x == false))
                        {
                            WorldComponent_UnlockedGenes.Instance.allBlackGenesUnlocked = true;
                        }
                    }
                    else
                    {
                        Messages.Message("VRE_NoUnlockableGenes".Translate(pherocore.LabelCap), pawn, MessageTypeDefOf.NegativeEvent, true);
                    }
                }



            };
            use.defaultCompleteMode = ToilCompleteMode.Instant;
            yield return use;
            yield break;

        }

        public static string IsEvolutionOrMutation(GenelineGeneDef gene)
        {
            if (gene.IsMutation) { return "mutation"; } else return "evolution";

        }

        public void DecreaseOrDestroy(Thing thing)
        {
            thing.stackCount--;
            if(thing.stackCount == 0) { thing.Destroy(); }
        }
    }
}

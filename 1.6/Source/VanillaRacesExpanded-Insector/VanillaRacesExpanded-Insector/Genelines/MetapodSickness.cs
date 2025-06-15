using Verse;
namespace VanillaRacesExpandedInsector
{
    public class MetapodSickness : HediffWithComps
    {
        public Geneline genelineToSwap;
        public bool replaceGeneline;

        public override void PostRemoved()
        {
            base.PostRemoved();
            if (replaceGeneline)
            {
                var geneline = pawn.genes.GetFirstGeneOfType<Gene_GenelineEvolution>();
                if (geneline != null)
                {
                    geneline.geneline.SetToMetapodStage(pawn, genelineToSwap);
                }
            }
        }

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_References.Look(ref genelineToSwap, "genelineToSwap");
            Scribe_Values.Look(ref replaceGeneline, "replaceGeneline");
        }
    }
}
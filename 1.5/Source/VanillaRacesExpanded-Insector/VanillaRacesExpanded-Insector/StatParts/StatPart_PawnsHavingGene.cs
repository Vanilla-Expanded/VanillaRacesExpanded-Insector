using RimWorld;
using UnityEngine;
using VanillaRacesExpandedInsector;
using Verse;

namespace VanillaRacesExpandedInsector
{

    public class StatPart_PawnsHavingGene : StatPart
    {
        public override void TransformValue(StatRequest req, ref float val)
        {
            float offset = 0f;
            if (HasRequiredGene(req.Thing))
            {
                offset = OffSetByNumberOfPawns(req.Thing);
            }
            if (offset > 1) { offset=1; }
            val += offset;
        }

        public bool HasRequiredGene(Thing thing)
        {
            Pawn pawn = thing as Pawn;
            if (pawn?.genes?.HasActiveGene(InternalDefOf.VRE_SwarmSynapse) == true)
            {
                return true;
            }
            return false;
        }

        public float OffSetByNumberOfPawns(Thing thing)
        {

            return GameComponent_SwarmSynapse.Instance.pawnsWithSwarmSynapse * 0.05f;

        }

        public override string ExplanationPart(StatRequest req)
        {
            if (req.HasThing && HasRequiredGene(req.Thing))
            {
                return "VRE_SwarmSynapseOffset".Translate() + (": +" + OffSetByNumberOfPawns(req.Thing));
            }
            return null;
        }


    }
}

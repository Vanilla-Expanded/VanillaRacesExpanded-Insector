using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace VanillaRacesExpandedInsector
{
    public class GeneGizmo_Resource_InsectJelly : GeneGizmo_Resource
    {
        public GeneGizmo_Resource_InsectJelly(Gene_Resource_InsectJelly gene, List<IGeneResourceDrain> drainGenes, Color barColor, Color barhighlightColor) : base(gene, drainGenes, barColor, barhighlightColor)
        {

        }

        private static bool draggingBar;

        protected override bool DraggingBar
        {
            get
            {
                return draggingBar;
            }
            set
            {
                draggingBar = value;
            }
        }
        public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
        {
            GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth, parms);
            return result;
        }
        protected override string GetTooltip()
        {
            tmpDrainGenes.Clear();
            string text = string.Format("{0}: {1} / {2}\n", gene.ResourceLabel.CapitalizeFirst().Colorize(ColoredText.TipSectionTitleColor), gene.ValueForDisplay, gene.MaxForDisplay);
            if (!this.drainGenes.NullOrEmpty<IGeneResourceDrain>())
            {
                float num = 0f;
                foreach (IGeneResourceDrain geneResourceDrain in this.drainGenes)
                {
                    if (geneResourceDrain.CanOffset)
                    {
                        this.tmpDrainGenes.Add(new Pair<IGeneResourceDrain, float>(geneResourceDrain, geneResourceDrain.ResourceLossPerDay));
                        num += geneResourceDrain.ResourceLossPerDay;
                    }
                }
                if (num != 0f)
                {
                    string text2 = (num < 0f) ? "RegenerationRate".Translate() : "DrainRate".Translate();
                    text = string.Concat(new string[]
                    {
                        text,
                        "\n\n",
                        text2,
                        ": ",
                        "PerDay".Translate(Mathf.Abs(this.gene.PostProcessValue(num))).Resolve()
                    });
                    foreach (Pair<IGeneResourceDrain, float> pair in this.tmpDrainGenes)
                    {
                        text = string.Concat(new string[]
                        {
                            text,
                            "\n  - ",
                            pair.First.DisplayLabel.CapitalizeFirst(),
                            ": ",
                            "PerDay".Translate(this.gene.PostProcessValue(-pair.Second).ToStringWithSign()).Resolve()
                        });
                    }
                }
            }
            if (!gene.def.resourceDescription.NullOrEmpty())
            {
                text = text + "\n\n" + this.gene.def.resourceDescription.Formatted(this.gene.pawn.Named("PAWN")).Resolve();
            }
            return text;
        }
        private List<Pair<IGeneResourceDrain, float>> tmpDrainGenes = new List<Pair<IGeneResourceDrain, float>>();

    }
}

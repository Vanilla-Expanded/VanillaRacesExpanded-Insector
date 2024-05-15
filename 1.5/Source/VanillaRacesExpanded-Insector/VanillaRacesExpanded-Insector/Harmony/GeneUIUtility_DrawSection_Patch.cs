using HarmonyLib;
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

namespace VanillaRacesExpandedInsector
{
    [HarmonyPatch(typeof(GeneUIUtility), "DrawSection")]
    public static class GeneUIUtility_DrawSection_Patch
    {
        public static List<Gene> insectGenes = new List<Gene>();
        public static void Prefix(List<Gene> ___xenogenes, bool xeno, ref int count)
        {
            if (xeno)
            {
                insectGenes = ___xenogenes.Where(x => x.def is GenelineGeneDef).ToList();
                ___xenogenes.RemoveAll(x => x.def is GenelineGeneDef);
                count = ___xenogenes.Count;
            }
        }

        public static void Postfix(Rect rect, bool xeno, ref float curY, Rect containingRect, ref float ___xenogenesHeight)
        {
            if (xeno && insectGenes.Any())
            {
                DrawSection(rect, insectGenes.Count, ref curY, ref ___xenogenesHeight, delegate (int i, Rect r)
                {
                    GeneUIUtility.DrawGene(insectGenes[i], r, GeneType.Xenogene);
                }, containingRect);
            }
        }


        private static void DrawSection(Rect rect, int count, ref float curY, ref float sectionHeight,
            Action<int, Rect> drawer, Rect containingRect)
        {
            Widgets.Label(10f, ref curY, rect.width, "VRE_Genelines".Translate(), "VRE_GenelinesDesc".Translate());
            float num = curY;
            Rect rect2 = new Rect(rect.x, curY, rect.width, sectionHeight);
            Widgets.DrawMenuSection(rect2);
            float num2 = (rect.width - 12f - 630f - 36f) / 2f;
            curY += num2;
            int num3 = 0;
            int num4 = 0;
            for (int i = 0; i < count; i++)
            {
                if (num4 >= 6)
                {
                    num4 = 0;
                    num3++;
                }
                else if (i > 0)
                {
                    num4++;
                }
                Rect rect3 = new Rect(num2 + (float)num4 * 90f + (float)num4 * 6f, curY + (float)num3 * 90f + (float)num3 * 6f, 90f, 90f);
                if (containingRect.Overlaps(rect3))
                {
                    drawer(i, rect3);
                }
            }
            curY += (float)(num3 + 1) * 90f + (float)num3 * 6f + num2;
            if (Event.current.type == EventType.Layout)
            {
                sectionHeight = curY - num;
            }
        }
    }
}


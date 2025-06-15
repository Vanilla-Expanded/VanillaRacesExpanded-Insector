using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
namespace VanillaRacesExpandedInsector
{
    [HotSwappable]
    [StaticConstructorOnStartup]
    public static class GenelineStatsTable
    {
        public static readonly IntRange GenelineStatRange = new IntRange(-20, 5);
        private struct GenelineStatData
        {
            public string labelKey;

            public string descKey;

            public Texture2D icon;

            public GenelineStatData(string labelKey, string descKey, Texture2D icon)
            {
                this.labelKey = labelKey;
                this.descKey = descKey;
                this.icon = icon;
            }
        }

        private static float cachedWidth;

        public static readonly CachedTexture EvolutionTex = new CachedTexture("UI/Icons/Biostats/Biostat_GenelineEvolution");
        public static readonly CachedTexture MutationTex = new CachedTexture("UI/Icons/Biostats/Biostat_GenelineMutation");
        public static readonly CachedTexture CosmeticTex = new CachedTexture("UI/Icons/Biostats/Biostat_GenelineCosmetic");

        private static readonly GenelineStatData[] GenelineStats = new GenelineStatData[2]
        {
            new GenelineStatData("VRE_Evolutions", "VRE_EvolutionsDesc", EvolutionTex.Texture),
            new GenelineStatData("VRE_Degrades", "VRE_DegradesDesc", MutationTex.Texture),
        };

        private static Dictionary<string, string> truncateCache = new Dictionary<string, string>();
        private static float MaxLabelWidth()
        {
            float num = 0f;
            int num2 = GenelineStats.Length;
            for (int i = 0; i < num2; i++)
            {
                num = Mathf.Max(num, Text.CalcSize(GenelineStats[i].labelKey.Translate()).x);
            }
            return num;
        }

        public static float HeightForBiostats()
        {
            float num = Text.LineHeight * 2f;
            return num;
        }

        public static void Draw(Rect rect, int evolution, int mutation)
        {
            int num = GenelineStats.Length;
            float num2 = MaxLabelWidth();
            float num3 = rect.height / (float)num;
            GUI.BeginGroup(rect);
            for (int i = 0; i < num; i++)
            {
                Rect position = new Rect(0f, (float)i * num3 + (num3 - 22f) / 2f, 22f, 22f);
                Rect rect2 = new Rect(position.xMax + 4f, (float)i * num3, num2, num3);
                Rect rect3 = new Rect(0f, rect2.y, rect.width, rect2.height);
                if (i % 2 == 1)
                {
                    Widgets.DrawLightHighlight(rect3);
                }
                Widgets.DrawHighlightIfMouseover(rect3);
                rect3.xMax = rect2.xMax + 4f + 90f;
                TaggedString taggedString = GenelineStats[i].descKey.Translate();
                TooltipHandler.TipRegion(rect3, taggedString);
                GUI.DrawTexture(position, GenelineStats[i].icon);
                Text.Anchor = TextAnchor.MiddleLeft;
                Widgets.Label(rect2, GenelineStats[i].labelKey.Translate());
                Text.Anchor = TextAnchor.UpperLeft;
            }
            float num4 = num2 + 4f + 22f + 4f;
            string text = evolution.ToString();
            string text2 = mutation.ToString();
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(new Rect(num4, 0f, 90f, num3), text);
            Widgets.Label(new Rect(num4, num3, 90f, num3), text2);
            Text.Anchor = TextAnchor.MiddleLeft;
            float width = rect.width - num2 - 90f - 22f - 4f;
            Rect rect4 = new Rect(num4 + 90f + 4f, num3, width, num3);
            if (rect4.width != cachedWidth)
            {
                cachedWidth = rect4.width;
                truncateCache.Clear();
            }
            Text.Anchor = TextAnchor.UpperLeft;
            GUI.EndGroup();
        }

        public static readonly SimpleCurve PowerEfficiencyToPowerDrainFactorCurve = new SimpleCurve
        {
            new CurvePoint(-20f, 6.0f),
            new CurvePoint(0f, 1f),
            new CurvePoint(5f, 0.5f)
        };

    }
}
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.Sound;
namespace VanillaRacesExpandedInsector
{
    [HotSwappable]
    public class Window_EditGeneline : Window
    {
        public Geneline geneline;
        public string genelineName;
        public Window_EditGeneline(Geneline geneline, Action callback)
        {
            this.geneline = geneline;
            this.callback = callback;
            genelineName = geneline.name.NullOrEmpty() ? GetGenelineName() : geneline.name;
            selectedGenes = geneline.genes.ToList();
            forcePause = true;
            absorbInputAroundWindow = true;
            foreach (GeneCategoryDef allDef in DefDatabase<GeneCategoryDef>.AllDefs)
            {
                collapsedCategories.Add(allDef, value: false);
            }
            OnGenesChanged();
        }

        protected float scrollHeight;

        protected Vector2 scrollPosition;

        protected float selectedHeight;

        protected float unselectedHeight;

        protected Action callback;

        public static readonly Vector2 GeneSize = new Vector2(87f, 68f);

        protected List<GenelineGeneDef> selectedGenes = new List<GenelineGeneDef>();

        protected bool? selectedCollapsed = false;

        protected HashSet<GeneCategoryDef> matchingCategories = new HashSet<GeneCategoryDef>();

        protected Dictionary<GeneCategoryDef, bool> collapsedCategories = new Dictionary<GeneCategoryDef, bool>();

        protected bool hoveredAnyGene;

        protected GenelineGeneDef hoveredGene;

        public override Vector2 InitialSize => new Vector2(750, 800);
        public List<GenelineGeneDef> SelectedGenes => selectedGenes;
        protected static readonly Vector2 ButSize = new Vector2(150f, 38f);

        public int TotalMutation => selectedGenes.Sum(x => x.mutation);
        public int TotalEvolution => selectedGenes.Sum(x => x.evolution);

        public override void DoWindowContents(Rect rect)
        {
            Rect rect2 = rect;
            rect2.yMax -= ButSize.y + 4f;
            Rect rect3 = new Rect(rect2.x, rect2.y, rect2.width, 35f);
            Text.Font = GameFont.Medium;
            Text.Anchor = TextAnchor.UpperLeft;
            Widgets.Label(rect3, "VRE_EditGeneline".Translate());
            Text.Font = GameFont.Small;
            float num3 = Text.LineHeight * 2f;
            rect2.y += 10;
            Rect geneRect = new Rect(rect2.x, rect2.y, rect2.width, rect2.height - num3 - 8f - 50);
            DrawGenes(geneRect);
            float num4 = geneRect.yMax + 4f;
            Rect bioStatsRect = new Rect(rect2.x + 4f, num4, rect.width - 4, num3);
            bioStatsRect.yMax = geneRect.yMax + num3 + 4f;
            GenelineStatsTable.Draw(bioStatsRect, TotalEvolution, TotalMutation);
            var bottomAreaRect = new Rect(bioStatsRect.x, bioStatsRect.yMax + 10, bioStatsRect.width, 75);
            DoBottomButtons(bottomAreaRect);
        }

        private string GetGenelineName()
        {
            var letter = GetLetter(geneline.id);
            return "VRE_GenelineName".Translate(letter);
        }

        static string GetLetter(int size)
        {
            string result = string.Empty;
            while (--size >= 0)
            {
                result = (char)('A' + size % 26) + result;
                size /= 26;
            }
            return result;
        }

        public void Accept()
        {
            geneline.EditGeneline(SelectedGenes, genelineName);
            callback?.Invoke();
            Close();
        }

        public void DrawGenes(Rect rect)
        {
            hoveredAnyGene = false;
            GUI.BeginGroup(rect);
            float curY = 15f;
            float num = curY;
            curY += 15f;
            float num2 = curY;
            Rect rect2 = new Rect(0f, curY, rect.width - 16f, scrollHeight);
            var outerRect = new Rect(0f, curY, rect.width, rect.height - curY);
            Widgets.BeginScrollView(outerRect, ref scrollPosition, rect2);
            bool? collapsed = null;
            DrawSection(rect, Utils.GenelineGenesInOrder.ToList(), null, ref curY, ref unselectedHeight, adding: true, ref collapsed);
            var rect3 = new Rect(0f, 0f, rect.width, selectedHeight);
            DrawSection(rect3, selectedGenes, "SelectedGenes".Translate(), ref curY, ref selectedHeight, adding: false, ref selectedCollapsed);
            if (Event.current.type == EventType.Layout)
            {
                scrollHeight = curY - num2;
            }
            Widgets.EndScrollView();
            GUI.EndGroup();
            if (!hoveredAnyGene)
            {
                hoveredGene = null;
            }
        }

        private void DrawSection(Rect rect, List<GenelineGeneDef> genes, string label, ref float curY, 
            ref float sectionHeight, bool adding, ref bool? collapsed)
        {
            float curX = 4f;
            if (!label.NullOrEmpty())
            {
                var offset = 4f;
                Rect rect2 = new Rect(offset, curY, rect.width - 4, Text.LineHeight);
                rect2.xMax -= offset;
                if (collapsed.HasValue)
                {
                    Rect position = new Rect(rect2.x, rect2.y + (rect2.height - 18f) / 2f, 18f, 18f);
                    GUI.DrawTexture(position, collapsed.Value ? TexButton.Reveal : TexButton.Collapse);
                    if (Widgets.ButtonInvisible(rect2))
                    {
                        collapsed = !collapsed;
                        if (collapsed.Value)
                        {
                            SoundDefOf.TabClose.PlayOneShotOnCamera();
                        }
                        else
                        {
                            SoundDefOf.TabOpen.PlayOneShotOnCamera();
                        }
                    }
                    if (Mouse.IsOver(rect2))
                    {
                        Widgets.DrawHighlight(rect2);
                    }
                    rect2.xMin += position.width;
                }
                Widgets.Label(rect2, label);
                curY += Text.LineHeight + 3f;
            }
            if (collapsed == true)
            {
                if (Event.current.type == EventType.Layout)
                {
                    sectionHeight = 0f;
                }
                return;
            }
            float num = curY;
            bool flag = false;
            float num2 = 34f + GeneSize.x + 8f;
            float num3 = rect.width - 16f;
            float num4 = num2 + 4f;
            float b = (num3 - num4 * Mathf.Floor(num3 / num4)) / 2f;
            Rect rect3 = new Rect(4f, curY, rect.width - 10, sectionHeight);
            if (!adding)
            {
                Widgets.DrawRectFast(rect3, Widgets.MenuSectionBGFillColor);
            }
            curY += 4f;
            if (!genes.Any())
            {
                Text.Anchor = TextAnchor.MiddleCenter;
                GUI.color = ColoredText.SubtleGrayColor;
                Widgets.Label(rect3, "(" + "NoneLower".Translate() + ")");
                GUI.color = Color.white;
                Text.Anchor = TextAnchor.UpperLeft;
            }
            else
            {
                GeneCategoryDef geneCategoryDef = null;
                int num5 = 0;
                for (int i = 0; i < genes.Count; i++)
                {
                    var geneDef = genes[i];
                    bool flag2 = false;
                    if (curX + num2 > num3)
                    {
                        curX = 4f;
                        curY += GeneSize.y + 8f + 4f;
                        flag2 = true;
                    }
                    bool flag4 = collapsedCategories[geneDef.displayCategory];
                    if (adding && geneCategoryDef != geneDef.displayCategory)
                    {
                        if (!flag2 && flag)
                        {
                            curX = 4f;
                            curY += GeneSize.y + 8f + 4f;
                        }
                        geneCategoryDef = geneDef.displayCategory;
                        Rect rect4 = new Rect(curX, curY, rect.width - 8f, Text.LineHeight);

                        Rect position2 = new Rect(rect4.x, rect4.y + (rect4.height - 18f) / 2f, 18f, 18f);
                        GUI.DrawTexture(position2, flag4 ? TexButton.Reveal : TexButton.Collapse);
                        if (Widgets.ButtonInvisible(rect4))
                        {
                            collapsedCategories[geneDef.displayCategory] = !collapsedCategories[geneDef.displayCategory];
                            if (collapsedCategories[geneDef.displayCategory])
                            {
                                SoundDefOf.TabClose.PlayOneShotOnCamera();
                            }
                            else
                            {
                                SoundDefOf.TabOpen.PlayOneShotOnCamera();
                            }
                        }
                        if (num5 % 2 == 1)
                        {
                            Widgets.DrawLightHighlight(rect4);
                        }
                        if (Mouse.IsOver(rect4))
                        {
                            Widgets.DrawHighlight(rect4);
                        }
                        rect4.xMin += position2.width;

                        Widgets.Label(rect4, geneCategoryDef.LabelCap);
                        curY += rect4.height;
                        if (!flag4)
                        {
                            GUI.color = Color.grey;
                            Widgets.DrawLineHorizontal(curX, curY, rect.width - 8f);
                            GUI.color = Color.white;
                            curY += 10f;
                        }
                        num5++;
                    }
                    if (adding && flag4)
                    {
                        flag = false;
                        if (Event.current.type == EventType.Layout)
                        {
                            sectionHeight = curY - num;
                        }
                        continue;
                    }
                    curX = Mathf.Max(curX, b);
                    flag = true;
                    if (DrawGene(geneDef, !adding, ref curX, curY, num2, false))
                    {
                        if (selectedGenes.Contains(geneDef))
                        {
                            SoundDefOf.Tick_Low.PlayOneShotOnCamera();
                            selectedGenes.Remove(geneDef);
                        }
                        else
                        {
                            SoundDefOf.Tick_High.PlayOneShotOnCamera();
                            selectedGenes.Add(geneDef);
                        }
                        OnGenesChanged();
                        break;
                    }
                }
            }
            if (!adding || flag)
            {
                curY += GeneSize.y + 12f;
            }
            if (Event.current.type == EventType.Layout)
            {
                sectionHeight = curY - num;
            }
        }

        protected List<GeneLeftChosenGroup> leftChosenGroups = new List<GeneLeftChosenGroup>();

        private bool DrawGene(GenelineGeneDef geneDef, bool selectedSection, ref float curX, float curY, float packWidth, bool isMatch)
        {
            bool result = false;
            Rect rect = new Rect(curX, curY, packWidth, GeneSize.y + 8f);
            bool selected = !selectedSection && selectedGenes.Contains(geneDef);
            bool overridden = leftChosenGroups.Any((GeneLeftChosenGroup x) => x.overriddenGenes.Contains(geneDef));
            Widgets.DrawOptionBackground(rect, selected);
            curX += 4f;
            DrawBiostats(geneDef.evolution, geneDef.mutation, ref curX, curY, 4f);
            Rect rect2 = new Rect(curX, curY + 4f, GeneSize.x, GeneSize.y);
            if (isMatch)
            {
                Widgets.DrawStrongHighlight(rect2.ExpandedBy(6f));
            }
            GeneUIUtility.DrawGeneDef(geneDef, rect2, GeneType.Xenogene, () => GeneTip(geneDef, selectedSection), doBackground: false, clickable: false, overridden);
            curX += GeneSize.x + 4f;
            if (Mouse.IsOver(rect))
            {
                hoveredGene = geneDef;
                hoveredAnyGene = true;
            }
            else if (hoveredGene != null && geneDef.ConflictsWith(hoveredGene))
            {
                Widgets.DrawLightHighlight(rect);
            }
            if (Widgets.ButtonInvisible(rect))
            {
                result = true;
            }
            curX = Mathf.Max(curX, rect.xMax + 4f);
            return result;
        }

        private static void DrawStat(Rect iconRect, CachedTexture icon, string stat, float iconWidth)
        {
            GUI.DrawTexture(iconRect, icon.Texture);
            Text.Anchor = TextAnchor.MiddleRight;
            Widgets.LabelFit(new Rect(iconRect.xMax, iconRect.y, 38f - iconWidth, iconWidth), stat);
            Text.Anchor = TextAnchor.UpperLeft;
        }

        public static void DrawBiostats(int evolution, int mutation, ref float curX, float curY, float margin = 6f)
        {
            float num2 = 0f;
            float num3 = Text.LineHeightOf(GameFont.Small);
            Rect iconRect = new Rect(curX, curY + margin + num2, num3, num3);
            if (evolution > 0)
            {
                DrawStat(iconRect, GenelineStatsTable.EvolutionTex, evolution.ToString(), num3);
            }
            else
            {
                DrawStat(iconRect, GenelineStatsTable.MutationTex, mutation.ToString(), num3);
            }
            curX += 34f;
        }
        protected List<GenelineGeneDef> cachedOverriddenGenes = new List<GenelineGeneDef>();

        private string GeneTip(GenelineGeneDef geneDef, bool selectedSection)
        {
            string text = null;
            if (selectedSection)
            {
                if (leftChosenGroups.Any((GeneLeftChosenGroup x) => x.leftChosen == geneDef))
                {
                    text = GroupInfo(leftChosenGroups.FirstOrDefault((GeneLeftChosenGroup x) => x.leftChosen == geneDef));
                }
                else if (cachedOverriddenGenes.Contains(geneDef))
                {
                    text = GroupInfo(leftChosenGroups.FirstOrDefault((GeneLeftChosenGroup x) => x.overriddenGenes.Contains(geneDef)));
                }
            }
            if (selectedGenes.Contains(geneDef) && geneDef.prerequisite != null && !selectedGenes.Contains(geneDef.prerequisite))
            {
                if (!text.NullOrEmpty())
                {
                    text += "\n\n";
                }
                text += ("MessageGeneMissingPrerequisite".Translate(geneDef.label).CapitalizeFirst() + ": " + geneDef.prerequisite.LabelCap).Colorize(ColorLibrary.RedReadable);
            }
            if (!text.NullOrEmpty())
            {
                text += "\n\n";
            }
            return text + (selectedGenes.Contains(geneDef) ? "ClickToRemove" : "ClickToAdd").Translate().Colorize(ColoredText.SubtleGrayColor);
            static string GroupInfo(GeneLeftChosenGroup group)
            {
                if (group == null)
                {
                    return null;
                }
                return ("GeneLeftmostActive".Translate() + ":\n  - " + group.leftChosen.LabelCap + " (" + "Active".Translate() + ")" + "\n" + group.overriddenGenes.Select((GeneDef x) => (x.label + " (" + "Suppressed".Translate() + ")").Colorize(ColorLibrary.RedReadable)).ToLineList("  - ", capitalizeItems: true)).Colorize(ColoredText.TipSectionTitleColor);
            }
        }

        public void OnGenesChanged()
        {
            selectedGenes.SortGeneDefs();
            leftChosenGroups.Clear();
            cachedOverriddenGenes.Clear();
            for (int k = 0; k < selectedGenes.Count; k++)
            {
                for (int l = k + 1; l < selectedGenes.Count; l++)
                {
                    if (!selectedGenes[k].ConflictsWith(selectedGenes[l]))
                    {
                        continue;
                    }
                    int num = GeneUtility.GenesInOrder.IndexOf(selectedGenes[k]);
                    int num2 = GeneUtility.GenesInOrder.IndexOf(selectedGenes[l]);
                    GeneDef leftMost = ((num < num2) ? selectedGenes[k] : selectedGenes[l]);
                    GeneDef rightMost = ((num >= num2) ? selectedGenes[k] : selectedGenes[l]);
                    GeneLeftChosenGroup geneLeftChosenGroup = leftChosenGroups.FirstOrDefault((GeneLeftChosenGroup x) => x.leftChosen == leftMost);
                    GeneLeftChosenGroup geneLeftChosenGroup2 = leftChosenGroups.FirstOrDefault((GeneLeftChosenGroup x) => x.leftChosen == rightMost);
                    if (geneLeftChosenGroup == null)
                    {
                        geneLeftChosenGroup = new GeneLeftChosenGroup(leftMost);
                        leftChosenGroups.Add(geneLeftChosenGroup);
                    }
                    if (geneLeftChosenGroup2 != null)
                    {
                        foreach (GeneDef overriddenGene in geneLeftChosenGroup2.overriddenGenes)
                        {
                            if (!geneLeftChosenGroup.overriddenGenes.Contains(overriddenGene))
                            {
                                geneLeftChosenGroup.overriddenGenes.Add(overriddenGene);
                            }
                            if (!cachedOverriddenGenes.Contains(overriddenGene))
                            {
                                cachedOverriddenGenes.Add(overriddenGene as GenelineGeneDef);
                            }
                        }
                        leftChosenGroups.Remove(geneLeftChosenGroup2);
                    }
                    if (!geneLeftChosenGroup.overriddenGenes.Contains(rightMost))
                    {
                        geneLeftChosenGroup.overriddenGenes.Add(rightMost);
                    }
                    if (!cachedOverriddenGenes.Contains(rightMost))
                    {
                        cachedOverriddenGenes.Add(rightMost as GenelineGeneDef);
                    }
                }
            }
            foreach (GeneLeftChosenGroup leftChosenGroup in leftChosenGroups)
            {
                leftChosenGroup.overriddenGenes.SortBy((GeneDef x) => selectedGenes.IndexOf(x as GenelineGeneDef));
            }
        }

        public void DoBottomButtons(Rect rect)
        {
            var textInputRect = new Rect(rect.x, rect.y, 250, 32);
            genelineName = Widgets.TextField(textInputRect, genelineName);
            var saveGenelineRect = new Rect(textInputRect.xMax + 10, textInputRect.y, 150, 32);
            var cannotReasonRect = new Rect(saveGenelineRect.xMax + 10, rect.y,
                rect.width - (textInputRect.width + saveGenelineRect.width + 20), 50);
            if (Widgets.ButtonText(saveGenelineRect, "VRE_SaveGeneline".Translate()) && CanAccept())
            {
                Accept();
                return;
            }

            if (leftChosenGroups.Any())
            {
                int num = leftChosenGroups.Sum((GeneLeftChosenGroup x) => x.overriddenGenes.Count);
                GeneLeftChosenGroup geneLeftChosenGroup = leftChosenGroups[0];
                string text = "GenesConflict".Translate() + ": " + "GenesConflictDesc".Translate(geneLeftChosenGroup.leftChosen.Named("FIRST"), geneLeftChosenGroup.overriddenGenes[0].Named("SECOND")).CapitalizeFirst() + ((num > 1) ? (" +" + (num - 1)) : string.Empty);
                DrawCannotReason(cannotReasonRect, text);
            }
            else if (TotalEvolution != TotalMutation)
            {
                DrawCannotReason(cannotReasonRect, "VRE_NeedEqualAmount".Translate());
            }

            Text.Anchor = TextAnchor.UpperLeft;
            Text.Font = GameFont.Tiny;
            var explanationRect = new Rect(rect.x, saveGenelineRect.yMax + 15, rect.width, 50);
            GUI.color = Color.grey;
            Widgets.Label(explanationRect, "VRE_ChangingGenelineExplanation".Translate());
            GUI.color = Color.white;
            Text.Font = GameFont.Small;
        }

        private static void DrawCannotReason(Rect rect, string text)
        {
            text = "VRE_CannotSave".Translate(text);
            GUI.color = ColorLibrary.RedReadable;
            var labelRect = new Rect(rect.x, rect.y, rect.width, rect.height);
            Widgets.Label(labelRect, text);
            GUI.color = Color.white;
        }

        public bool CanAccept()
        {
            if (leftChosenGroups.Any())
            {
                Messages.Message("VRE_MessageConflictingGenesPresent".Translate(), null, MessageTypeDefOf.RejectInput, historical: false);
                return false;
            }
            string text = geneline.name;
            if (text != null && text.Trim().Length == 0)
            {
                Messages.Message("VRE_GenelineNameCannotBeEmpty".Translate(), MessageTypeDefOf.RejectInput, historical: false);
                return false;
            }

            if (TotalEvolution != TotalMutation)
            {
                Messages.Message("VRE_NeedEqualAmount".Translate(), MessageTypeDefOf.RejectInput, historical: false);
                return false;
            }
            List<GenelineGeneDef> selectedGenes = SelectedGenes;
            foreach (var selectedGene in SelectedGenes)
            {
                if (selectedGene.prerequisite != null && !selectedGenes.Contains(selectedGene.prerequisite))
                {
                    Messages.Message("MessageGeneMissingPrerequisite".Translate(selectedGene.label).CapitalizeFirst() + ": " + selectedGene.prerequisite.LabelCap, null, MessageTypeDefOf.RejectInput, historical: false);
                    return false;
                }
            }
            return true;
        }
    }
}
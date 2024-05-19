using RimWorld;
using System;
using System.Linq;
using UnityEngine;
using Verse;
namespace VanillaRacesExpandedInsector
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct)]
    public class HotSwappableAttribute : Attribute
    {
    }
    [HotSwappable]
    public class Window_ManageGenelines : Window
    {
        protected float bottomAreaHeight => 55;

        protected Vector2 scrollPosition = Vector2.zero;

        protected string deleteTipKey = "VRE_DeleteThisGeneline";

        protected const float EntryHeight = 40f;

        protected const float FileNameLeftMargin = 8f;

        protected const float FileNameRightMargin = 4f;

        protected const float FileInfoWidth = 94f;

        protected const float InteractButWidth = 100f;

        protected const float InteractButHeight = 36f;

        protected const float DeleteButSize = 36f;

        private static readonly Color DefaultFileTextColor = new Color(1f, 1f, 0.6f);

        protected const float NameTextFieldWidth = 400f;

        protected const float NameTextFieldHeight = 35f;

        protected const float NameTextFieldButtonSpace = 20f;

        public override Vector2 InitialSize => new Vector2(620f, 700f);

        public Window_ManageGenelines()
        {
            doCloseButton = false;
            doCloseX = true;
            forcePause = true;
            absorbInputAroundWindow = true;
            closeOnAccept = false;
            closeOnCancel = true;
        }

        public override void DoWindowContents(Rect inRect)
        {
            Vector2 vector = new Vector2(inRect.width - 16f, 40f);
            float y = vector.y;
            float height = (float)GameComponent_Genelines.Instance.genelines.Count * y;
            Rect viewRect = new Rect(0f, 0f, inRect.width - 16f, height);
            float num = inRect.height - Window.CloseButSize.y - bottomAreaHeight - 18f;
            Rect outRect = inRect.TopPartPixels(num);
            Widgets.BeginScrollView(outRect, ref scrollPosition, viewRect);
            float num2 = 0f;
            int num3 = 0;
            foreach (var geneline in GameComponent_Genelines.Instance.genelines.ToList())
            {
                if (num2 + vector.y >= scrollPosition.y && num2 <= scrollPosition.y + outRect.height)
                {
                    Rect rect = new Rect(0f, num2, vector.x, vector.y);
                    if (num3 % 2 == 0)
                    {
                        Widgets.DrawAltRect(rect);
                    }
                    Widgets.BeginGroup(rect);
                    Rect rect2 = new Rect(rect.width - 36f, (rect.height - 36f) / 2f, 36f, 36f);
                    if (Widgets.ButtonImage(rect2, TexButton.Delete, Color.white, GenUI.SubtleMouseoverColor))
                    {
                        var additionalText = "";
                        if (geneline.pawns.Any(x => x.IsColonist))
                        {
                            var colonists = geneline.pawns.Where(x => x.IsColonist).ToList();
                            var pawnList = GenLabel.ThingsLabel(colonists);
                            additionalText = "VRE_DeleteGenelineConfirmation".Translate(pawnList) + "\n";
                        }
                        Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(additionalText + "ConfirmDelete".Translate(geneline.name), delegate
                        {
                            GameComponent_Genelines.Instance.DeleteGeneline(geneline);
                        }, destructive: true));
                    }
                    TooltipHandler.TipRegionByKey(rect2, deleteTipKey);
                    Text.Font = GameFont.Small;
                    Rect rect3 = new Rect(rect2.x - 100f, (rect.height - 36f) / 2f, 100f, 36f);
                    if (Widgets.ButtonText(rect3, "VRE_EditGeneline".Translate()))
                    {
                        EditGeneline(geneline);
                    }
                    Rect rect4 = new Rect(rect3.x - 94f, 0f, 94f, rect.height);
                    GUI.color = Color.white;
                    Text.Anchor = TextAnchor.UpperLeft;
                    Rect rect5 = new Rect(8f, 0f, rect4.x - 8f - 4f, rect.height);
                    Text.Anchor = TextAnchor.MiddleLeft;
                    Text.Font = GameFont.Small;
                    string fileNameWithoutExtension = geneline.name;
                    Widgets.Label(rect5, fileNameWithoutExtension.Truncate(rect5.width * 1.8f));
                    Text.Anchor = TextAnchor.UpperLeft;
                    Widgets.EndGroup();
                }
                num2 += vector.y;
                num3++;
            }
            Widgets.EndScrollView();
            var createGenelineRect = new Rect(outRect.x + 175, outRect.yMax + 15, outRect.width - 350, 32);
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleCenter;
            if (Widgets.ButtonText(createGenelineRect, "VRE_CreateGeneline".Translate()))
            {
                var newGeneline = GameComponent_Genelines.Instance.CreateGeneline();
                Find.WindowStack.Add(new Window_EditGeneline(newGeneline, delegate
                {
                    GameComponent_Genelines.Instance.AddGeneline(newGeneline);
                }));
            }
            Text.Anchor = TextAnchor.UpperCenter;
            Text.Font = GameFont.Tiny;
            var explanationRect = new Rect(outRect.x + 70, createGenelineRect.yMax + 15, outRect.width - 140, 50);
            GUI.color = Color.grey;
            Widgets.Label(explanationRect, "VRE_CreateGenelineExplanation".Translate());
            GUI.color = Color.white;
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.UpperLeft;
        }

        protected void EditGeneline(Geneline geneline)
        {
            Find.WindowStack.Add(new Window_EditGeneline(geneline, null));
        }
    }
}
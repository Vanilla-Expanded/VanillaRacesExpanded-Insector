using HarmonyLib;
using RimWorld;
using Verse;
using System.Collections.Generic;
using System.Linq;

namespace VanillaRacesExpandedInsector
{
    [HarmonyPatch(typeof(GeneUtility), "GenesInOrder", MethodType.Getter)]
    public static class GeneUtility_GenesInOrder_Patch
    {
        [HarmonyPriority(int.MinValue)]
        public static void Postfix(ref List<GeneDef> __result)
        {
            var window = Find.WindowStack.WindowOfType<GeneCreationDialogBase>();
            if (window != null)
            {
                //if (window is Window_CreateAndroidBase)
                //{
                //    __result = Utils.GenelineGenesInOrder;
                //}
                //else
                {
                    __result = __result.Where(x => x is not GenelineGeneDef).ToList();
                }
            }
        }
    }













}


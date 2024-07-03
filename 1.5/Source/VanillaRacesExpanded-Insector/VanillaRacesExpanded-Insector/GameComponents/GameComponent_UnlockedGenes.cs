using RimWorld;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Noise;
namespace VanillaRacesExpandedInsector
{
    public class GameComponent_UnlockedGenes : GameComponent
    {
        public static GameComponent_UnlockedGenes Instance;

        public Dictionary<GeneDef, bool> sorne_pherocore_genes = new Dictionary<GeneDef, bool>();
        public bool allSorneGenesUnlocked = false;

        List<GeneDef> list;
        List<bool> list2;

        public GameComponent_UnlockedGenes(Game game)
        {
            Instance = this;
        }

        public override void FinalizeInit()
        {
            base.FinalizeInit();

            if (sorne_pherocore_genes.NullOrEmpty() && InternalDefOf.VRE_SwarmSynapse != null)
            {
                sorne_pherocore_genes = new Dictionary<GeneDef, bool>() { { InternalDefOf.VRE_SwarmSynapse, false },
                { InternalDefOf.VRE_RoyalJellyInjector, false },{ InternalDefOf.VRE_Microsized, false },{ InternalDefOf.VRE_Colossal, false }};

            }

        }

        public override void ExposeData()
        {
            Instance = this;
            base.ExposeData();
            Scribe_Collections.Look<GeneDef, bool>(ref sorne_pherocore_genes, "sorne_pherocore_genes", LookMode.Def, LookMode.Value, ref list, ref list2);
            Scribe_Values.Look<bool>(ref this.allSorneGenesUnlocked, "allSorneGenesUnlocked", false, true);

        }

        public bool SorneGeneUnlocked(GeneDef gene)
        {
            if (sorne_pherocore_genes.ContainsKey(gene) && sorne_pherocore_genes[gene]) return true;
            return false;
        }




    }
}
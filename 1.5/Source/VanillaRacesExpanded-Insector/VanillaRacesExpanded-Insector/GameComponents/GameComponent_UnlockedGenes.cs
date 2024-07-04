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
        List<GeneDef> sorneList;
        List<bool> sorneList2;

        public Dictionary<GeneDef, bool> nuchadus_pherocore_genes = new Dictionary<GeneDef, bool>();
        public bool allNuchadusGenesUnlocked = false;
        List<GeneDef> nuchadusList;
        List<bool> nuchadusList2;

        public Dictionary<GeneDef, bool> chelis_pherocore_genes = new Dictionary<GeneDef, bool>();
        public bool allChelisGenesUnlocked = false;
        List<GeneDef> chelisList;
        List<bool> chelisList2;

        public Dictionary<GeneDef, bool> kemia_pherocore_genes = new Dictionary<GeneDef, bool>();
        public bool allKemiaGenesUnlocked = false;
        List<GeneDef> kemiaList;
        List<bool> kemiaList2;

        public Dictionary<GeneDef, bool> xanides_pherocore_genes = new Dictionary<GeneDef, bool>();
        public bool allXanidesGenesUnlocked = false;
        List<GeneDef> xanidesList;
        List<bool> xanidesList2;

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

            if (nuchadus_pherocore_genes.NullOrEmpty() && InternalDefOf.VRE_PyroResistantChitin != null)
            {
                nuchadus_pherocore_genes = new Dictionary<GeneDef, bool>() { { InternalDefOf.VRE_PyroResistantChitin, false },
                { InternalDefOf.VRE_FlameGlands, false },{ InternalDefOf.VRE_ChemfuelSacks, false },{ InternalDefOf.VRE_Pyrophiliac, false }};

            }

            if (chelis_pherocore_genes.NullOrEmpty() && InternalDefOf.VRE_LocustWings != null)
            {
                chelis_pherocore_genes = new Dictionary<GeneDef, bool>() { { InternalDefOf.VRE_LocustWings, false },
                { InternalDefOf.VRE_InsectRostrum, false },{ InternalDefOf.VRE_InsectVolatile, false },{ InternalDefOf.VRE_EcdysoneOverdrive, false }};

            }
            if (kemia_pherocore_genes.NullOrEmpty() && InternalDefOf.VRE_AcidGlands != null)
            {
                kemia_pherocore_genes = new Dictionary<GeneDef, bool>() { { InternalDefOf.VRE_AcidGlands, false },
                { InternalDefOf.VRE_InfraredSensors, false },{ InternalDefOf.VRE_AcidBurstSack, false },{ InternalDefOf.VRE_SolidGreyMatter, false }};

            }

            if (xanides_pherocore_genes.NullOrEmpty() && InternalDefOf.VRE_MineralRichInsectskin != null)
            {
                xanides_pherocore_genes = new Dictionary<GeneDef, bool>() { { InternalDefOf.VRE_MineralRichInsectskin, false },
                { InternalDefOf.VRE_ChargerClaws, false },{ InternalDefOf.VRE_HardLockedJoints, false },{ InternalDefOf.VRE_PassiveInsect, false }};

            }

        }

        public override void ExposeData()
        {
            Instance = this;
            base.ExposeData();
            Scribe_Collections.Look<GeneDef, bool>(ref sorne_pherocore_genes, "sorne_pherocore_genes", LookMode.Def, LookMode.Value, ref sorneList, ref sorneList2);
            Scribe_Values.Look<bool>(ref this.allSorneGenesUnlocked, "allSorneGenesUnlocked", false, true);

            Scribe_Collections.Look<GeneDef, bool>(ref nuchadus_pherocore_genes, "nuchadus_pherocore_genes", LookMode.Def, LookMode.Value, ref nuchadusList, ref nuchadusList2);
            Scribe_Values.Look<bool>(ref this.allNuchadusGenesUnlocked, "allNuchadusGenesUnlocked", false, true);

            Scribe_Collections.Look<GeneDef, bool>(ref chelis_pherocore_genes, "chelis_pherocore_genes", LookMode.Def, LookMode.Value, ref chelisList, ref chelisList2);
            Scribe_Values.Look<bool>(ref this.allChelisGenesUnlocked, "allChelisGenesUnlocked", false, true);

            Scribe_Collections.Look<GeneDef, bool>(ref kemia_pherocore_genes, "kemia_pherocore_genes", LookMode.Def, LookMode.Value, ref kemiaList, ref kemiaList2);
            Scribe_Values.Look<bool>(ref this.allKemiaGenesUnlocked, "allKemiaGenesUnlocked", false, true);

            Scribe_Collections.Look<GeneDef, bool>(ref xanides_pherocore_genes, "xanides_pherocore_genes", LookMode.Def, LookMode.Value, ref xanidesList, ref xanidesList2);
            Scribe_Values.Look<bool>(ref this.allXanidesGenesUnlocked, "allXanidesGenesUnlocked", false, true);

        }

        public bool SorneGeneUnlocked(GeneDef gene)
        {
            if (sorne_pherocore_genes.ContainsKey(gene) && sorne_pherocore_genes[gene]) return true;
            return false;
        }

        public bool NuchadusGeneUnlocked(GeneDef gene)
        {
            if (nuchadus_pherocore_genes.ContainsKey(gene) && nuchadus_pherocore_genes[gene]) return true;
            return false;
        }

        public bool ChelisGeneUnlocked(GeneDef gene)
        {
            if (chelis_pherocore_genes.ContainsKey(gene) && chelis_pherocore_genes[gene]) return true;
            return false;
        }

        public bool KemiaGeneUnlocked(GeneDef gene)
        {
            if (kemia_pherocore_genes.ContainsKey(gene) && kemia_pherocore_genes[gene]) return true;
            return false;
        }

        public bool XanidesGeneUnlocked(GeneDef gene)
        {
            if (xanides_pherocore_genes.ContainsKey(gene) && xanides_pherocore_genes[gene]) return true;
            return false;
        }




    }
}
using Verse;

namespace VanillaRacesExpandedInsector
{
    public class GenelineGeneDef : GeneDef
    {
        public int mutation;
        public int evolution;
        public int cosmetic;
        public bool unlockable = false;
        public bool IsMutation => mutation > 0;
        public bool IsEvolution => evolution > 0;
        public bool IsCosmetic => cosmetic > 0;

    }
}
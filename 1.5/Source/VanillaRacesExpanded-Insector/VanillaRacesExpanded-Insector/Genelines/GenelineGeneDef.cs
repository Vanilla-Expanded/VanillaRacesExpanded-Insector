using Verse;

namespace VanillaRacesExpandedInsector
{
    public class GenelineGeneDef : GeneDef
    {
        public int mutation;
        public int evolution;
        public bool unlockable = false;
        public bool IsMutation => mutation > 0;
        public bool IsEvolution => evolution > 0;

       
    }
}
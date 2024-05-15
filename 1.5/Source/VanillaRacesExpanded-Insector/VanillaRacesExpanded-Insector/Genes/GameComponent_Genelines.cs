using System.Collections.Generic;
using Verse;
namespace VanillaRacesExpandedInsector
{
    public class GameComponent_Genelines : GameComponent
    {
        public static GameComponent_Genelines Instance;
        public List<Geneline> genelines = new();
        public GameComponent_Genelines(Game game)
        {
            Instance = this;
        }

        public override void ExposeData()
        {
            Instance = this;
            base.ExposeData();
            Scribe_Collections.Look(ref genelines, "genelines", LookMode.Deep);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                genelines ??= new List<Geneline>();
            }
        }
    }
}
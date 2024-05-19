using System.Collections.Generic;
using System.Linq;
using Verse;
namespace VanillaRacesExpandedInsector
{
    public class GameComponent_Genelines : GameComponent
    {
        public static GameComponent_Genelines Instance;
        public List<Geneline> genelines = new();
        public int curID;
        public GameComponent_Genelines(Game game)
        {
            Instance = this;
        }
        public Geneline CreateGeneline()
        {
            var geneline = new Geneline();
            geneline.id = curID + 1;
            return geneline;
        }

        public void AddGeneline(Geneline geneline)
        {
            genelines.Add(geneline);
            curID = geneline.id;
        }

        public void DeleteGeneline(Geneline geneline)
        {
            foreach (var pawn in geneline.pawns.ToList())
            {
                geneline.RemovePawn(pawn);
            }
            genelines.Remove(geneline);
        }

        public override void ExposeData()
        {
            Instance = this;
            base.ExposeData();
            Scribe_Values.Look(ref curID, "curID");
            Scribe_Collections.Look(ref genelines, "genelines", LookMode.Deep);
            if (Scribe.mode == LoadSaveMode.PostLoadInit)
            {
                genelines ??= new List<Geneline>();
            }
        }
    }
}
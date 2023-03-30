using System.Windows.Forms;

namespace Digger
{
    public static class Game
    {
        private const string mapWithPlayerTerrain = @"
TTT T
TTP T
T T T
TT TT";

//        private const string mapWithPlayerTerrainSackGold = @"
//PTTGTT TS
//TST  TSTT
//TTTTTTSTT
//T TSTS TT
//T TTTG ST
//TSTSTT TT";
        private const string mapWithPlayerTerrainSackGold = @"
 S
PT
 T
 T
 T";

//        private const string mapWithPlayerTerrainSackGoldMonster = @"
//T  PTTGTT TST
//T  TST  TSTTM
//T  TTT TTSTTT
//T  T TSTS TTT
//T  T TTTGMSTS
//T  T TMT M TS
//T  TSTSTTMTTT
//T  S TTST  TG
//T   TGST MTTT
//T   T  TMTTTT";
        private const string mapWithPlayerTerrainSackGoldMonster = @"
M 
 M
  ";

        public static ICreature[,] Map;
        public static int Scores;
        public static bool IsOver;

        public static Keys KeyPressed;
        public static int MapWidth => Map.GetLength(0);
        public static int MapHeight => Map.GetLength(1);

        public static void CreateMap()
        {
            Map = CreatureMapCreator.CreateMap(mapWithPlayerTerrainSackGoldMonster);
        }
    }
}
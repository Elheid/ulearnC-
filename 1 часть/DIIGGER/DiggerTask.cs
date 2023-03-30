using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace Digger
{
    public class Move
    {
        public static CreatureCommand MoveObject(int x, int y)
        {
            return new CreatureCommand()
            { DeltaX = x, DeltaY = y };
        }

        public static CreatureCommand MoveObjectAndTransformToGold(int x, int y, Gold gold)
        {
            return new CreatureCommand()
            { DeltaX = x, DeltaY = y, TransformTo = gold };
        }

        public static CreatureCommand PlayerMove(bool canMove, int x, int y)
        {
            var stayOn = Move.MoveObject(0, 0);
            if (canMove)
                return Move.MoveObject(x, y);
            return stayOn;
        }
    }

    public class Player : ICreature
    {
        public static int XDigger = 0;
        public static int YDigger = 0;

        public static bool IsPlayerExist()
        {
            for (var x = 0; x < Game.MapWidth; x++)
            {
                for (var y = 0; y < Game.MapHeight; y++)
                {
                    if (Game.Map[x, y] != null && Game.Map[x, y] is Player)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        //монстр всегда действует первым, если записывать координаты
        // игрока в действии игрока, то они могут быть
        //не теми, какими являются, т.к ещё не записаны
        public static int[] FindPlayerStartPosition()
        {
            for (var x = 0; x < Game.MapWidth; x++)
            {
                for (var y = 0; y < Game.MapHeight; y++)
                {
                    if (Game.Map[x, y] != null && Game.Map[x, y] is Player)
                    {
                        return new int[] { x, y };
                    }
                }
            }
            return new int[] { };
        }

        public CreatureCommand Act(int x, int y)
        {
            XDigger = x;
            YDigger = y;
            var diffCoordinatAndCells = 1;
            var oneStep = 1;
            var mapWidth = Game.MapWidth;
            var mapHeight = Game.MapHeight;
            var leftBorder = x != 0;
            var upBorder = y != 0;
            var rightBorder = mapWidth - diffCoordinatAndCells != x;
            var downBorder = mapHeight - diffCoordinatAndCells != y;
            var moveDown = WayIsFree(x, y + oneStep, downBorder);
            var moveUp = WayIsFree(x, y - oneStep, upBorder);
            var moveLeft = WayIsFree(x - oneStep, y, leftBorder);
            var moveRight = WayIsFree(x + oneStep, y, rightBorder);
            var stayOn = Move.MoveObject(0, 0);

            switch (Game.KeyPressed)
            {
                case Keys.Down:
                    return Move.PlayerMove(moveDown, 0, oneStep);
                case Keys.Up:
                    return Move.PlayerMove(moveUp, 0, -oneStep);
                case Keys.Left:
                    return Move.PlayerMove(moveLeft, -oneStep, 0);
                case Keys.Right:
                    return Move.PlayerMove(moveRight, oneStep, 0);
                default:
                    return stayOn;
            }
        }

        public bool WayIsFree(int x, int y, bool notOnMapBorder)
        {
            if (notOnMapBorder)
                return Game.Map[x, y] == null || !(Game.Map[x, y] is Sack);
            return false;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Sack || conflictedObject is Monster;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public string GetImageFileName()
        {
            return "Digger.png";
        }
    }

    public class Terrain : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return Move.MoveObject(0, 0);
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return true;
        }

        public int GetDrawingPriority()
        {
            return 1;
        }

        public string GetImageFileName()
        {
            return "Terrain.png";
        }
    }

    public class Sack : ICreature
    {
        public int FallCount = 0;
        public bool CheckIsEmptyOrDiggerInDown(int x, int y)
        {
            var objectInDown = Game.Map[x, y + 1];
            return objectInDown == null || (objectInDown is Player || objectInDown is Monster) && FallCount > 0;
        }

        public CreatureCommand Act(int x, int y)
        {
            var mapHeight = Game.MapHeight;
            var diffCoordinatAndCells = 1;
            var downBorder = mapHeight - diffCoordinatAndCells != y;
            var sackFall = Move.MoveObject(0, 1);
            var sackStayInPeace = Move.MoveObject(0, 0);
            var sackTransform = Move.MoveObjectAndTransformToGold(0, 0, new Gold());
            while (downBorder)
            {
                if (CheckIsEmptyOrDiggerInDown(x, y))
                {
                    FallCount++;
                    return sackFall;
                }
                if (FallCount > 1)
                    return sackTransform;
                FallCount = 0;
                return sackStayInPeace;
            }
            return FallCount > 1 ? sackTransform : sackStayInPeace;
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return false;
        }

        public int GetDrawingPriority()
        {
            return 2;
        }

        public string GetImageFileName()
        {
            return "Sack.png";
        }
    }

    public class Gold : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            return Move.MoveObject(0, 0);
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            if (conflictedObject is Player)
                Game.Scores += 10;
            return true;
        }

        public int GetDrawingPriority()
        {
            return 3;
        }

        public string GetImageFileName()
        {
            return "Gold.png";
        }
    }

    public class Monster : ICreature
    {
        public CreatureCommand Act(int x, int y)
        {
            var monsterStepX = 0;
            var monsterStepY = 0;
            monsterStepX = MonsterChaseCoordinate(x, y)[0];
            monsterStepY = MonsterChaseCoordinate(x, y)[1];
            var monsterMoveX = x + monsterStepX;
            var monsterMoveY = y + monsterStepY;
            var monsterLocation = Game.Map[monsterMoveX, monsterMoveY];
            if (!(monsterMoveX >= 0 && monsterMoveX < Game.MapWidth
                && monsterMoveY >= 0 && monsterMoveY < Game.MapHeight))
                return Move.MoveObject(0, 0);
            if (monsterLocation != null && (monsterLocation is Terrain || monsterLocation is Sack || monsterLocation is Monster))
                return Move.MoveObject(0, 0);
            return Move.MoveObject(monsterStepX, monsterStepY);
        }

        public int[] MonsterChaseCoordinate(int x, int y)
        {
            var monsterStepX = 0;
            var monsterStepY = 0;
            if (Player.IsPlayerExist())
            {
                var xDigger = Player.XDigger == 0 ? Player.FindPlayerStartPosition()[0] : Player.XDigger;
                var yDigger = Player.YDigger == 0 ? Player.FindPlayerStartPosition()[1] : Player.YDigger;
                if (xDigger == x)
                {
                    if (yDigger < y) monsterStepY = -1;
                    else if (yDigger > y) monsterStepY = 1;
                }
                else if (yDigger == y)
                {
                    if (xDigger < x) monsterStepX = -1;
                    else if (xDigger > x) monsterStepX = 1;
                }
                else
                {
                    if (xDigger < x) monsterStepX = -1;
                    else if (xDigger > x) monsterStepX = 1;
                }
            }
            return new int[] { monsterStepX, monsterStepY };
        }

        public bool DeadInConflict(ICreature conflictedObject)
        {
            return conflictedObject is Sack || conflictedObject is Monster;
        }

        public int GetDrawingPriority()
        {
            return 0;
        }

        public string GetImageFileName()
        {
            return "Monster.png";
        }
    }
}
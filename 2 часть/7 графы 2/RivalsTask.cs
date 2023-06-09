using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Rivals;

public class RivalsTask
{
    public static IEnumerable<OwnedLocation> AssignOwners(Map map)
    {
        var queue = new Queue<OwnedLocation>();
        var visitedPoints = new HashSet<Point>();
        AddAllPlayers(map, queue);
        while (queue.Count > 0)
        {
            var playerTerritory = queue.Dequeue();
            var location = playerTerritory.Location;
            if (!map.InBounds(location) || map.Maze[location.X, location.Y] == MapCell.Wall) continue;
            if (visitedPoints.Contains(location)) continue;
            visitedPoints.Add(location);
            yield return playerTerritory;
            SearchInPossibleDirections(queue, playerTerritory);
        }
    }

    public static void AddAllPlayers(Map map, Queue<OwnedLocation> queue)
    {
        var playersLocation = map.Players;
        var playerIndex = 0;
        foreach (var player in playersLocation)
            queue.Enqueue(new OwnedLocation(playerIndex++, player, 0));
    }

    public static void SearchInPossibleDirections(Queue<OwnedLocation> queue, OwnedLocation territory)
    {
        for (var dy = -1; dy <= 1; dy++)
            for (var dx = -1; dx <= 1; dx++)
                if (dy != 0 && dx != 0) continue;
                else
                {
                    var nextPoint = new Point(territory.Location.X + dx, territory.Location.Y + dy);
                    var newTerritiory = new OwnedLocation(territory.Owner, nextPoint, territory.Distance + 1);
                    queue.Enqueue(newTerritiory);
                }
    }
}
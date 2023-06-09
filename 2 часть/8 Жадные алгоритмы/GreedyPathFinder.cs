using System.Collections.Generic;
using System.Linq;
using Greedy.Architecture;

namespace Greedy;

public class GreedyPathFinder : IPathFinder
{
	public List<Point> FindPathToCompleteGoal(State state)
	{
		var pathFinder = new DijkstraPathFinder();
		var chestSet = new HashSet<Point>();
		var availableEnergy = state.InitialEnergy;
        foreach (var target in state.Chests)
        {
            chestSet.Add(target);
        }
        var result = new List<Point>();
        if (state.Goal > state.Chests.Count)
            return new List<Point>();
        while (state.Scores < state.Goal)
		{
			var path = pathFinder.GetPathsByDijkstra(state, state.Position, chestSet).FirstOrDefault();
            if (Equals(path, null))
                return new List<Point>();
            availableEnergy -= path.Cost;
            if (availableEnergy < 0)
                return new List<Point>();
            foreach (var point in path.Path.Skip(1))
				result.Add(point);
            ChangeState(state, path.Path, chestSet);
        }
		return result;	
	}
    public void ChangeState(State state, List<Point> path, HashSet<Point> chests)
    {
        state.Position = path.Last();
        chests.Remove(state.Position);
        state.Scores++;
    }
}
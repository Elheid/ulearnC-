using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace rocket_bot;

public partial class Bot
{
    private List<(Turn Turn, double Score)> discisions = new List<(Turn Turn, double Score)>();
    public Rocket GetNextMove(Rocket rocket)
    {
        var tasks = CreateTasks(rocket);
        foreach (var task in tasks)
        {
            lock (discisions)
                discisions.Add(task.Result);
        }
        Task.WaitAll();
        var (turn, score) = tasks.Max()
            .GetAwaiter()
            .GetResult();
        return rocket.Move(turn, level);
    }


    public List<Task<(Turn Turn, double Score)>> CreateTasks(Rocket rocket)
    {
        return new() { Task.Run(() => SearchBestMove(rocket, new Random(random.Next()), iterationsCount/threadsCount)) };
    }
}

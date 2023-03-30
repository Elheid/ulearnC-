using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

namespace StructBenchmarking
{
    public class Experiments
    {
        public static ChartData BuildChartDataForArrayCreation(
            IBenchmark benchmark, int repetitionsCount)
        {
            var arrayCreationTask = new ResultsFactoryArrayCreation();
            return FillExperimentsResults(benchmark, repetitionsCount,
                arrayCreationTask, arrayCreationTask.TitleForGraph);
        }

        public static ChartData BuildChartDataForMethodCall(
            IBenchmark benchmark, int repetitionsCount)
        {
            var methodCallTask = new ResultsFactoryMethodCall();
            return FillExperimentsResults(benchmark, repetitionsCount,
                methodCallTask, methodCallTask.TitleForGraph);
        }

        public static ChartData FillExperimentsResults(IBenchmark benchmark,
                int repetitionsCount, ITaskFactory taskFactory, string title)
        {
            var classesTimes = new List<ExperimentResult>();
            var structuresTimes = new List<ExperimentResult>();
            foreach (var fieldSize in Constants.FieldCounts)
            {
                var classTasks = taskFactory.CreateTaskForClasses(fieldSize);
                var structureTasks = taskFactory.CreateTaskForStructures(fieldSize);
                GiveTimeResultAndAddToList(benchmark, classTasks,
                    classesTimes, repetitionsCount, fieldSize);
                GiveTimeResultAndAddToList(benchmark, structureTasks,
                    structuresTimes, repetitionsCount, fieldSize);
            }

            return new ChartData
            {
                Title = title,
                ClassPoints = classesTimes,
                StructPoints = structuresTimes,
            };
        }

        public static void GiveTimeResultAndAddToList(IBenchmark benchmark
                , ITask someTask, List<ExperimentResult> listWithResults
                 , int repetitionsCount, int size)
        {
            var taskResult = benchmark.MeasureDurationInMs(someTask, repetitionsCount);
            listWithResults.Add(new ExperimentResult(size, taskResult));
        }

        public interface ITaskFactory
        {
            ITask CreateTaskForClasses(int size);
            ITask CreateTaskForStructures(int size);
        }

        public class ResultsFactoryArrayCreation : ITaskFactory
        {
            public ResultsFactoryArrayCreation()
            {
                TitleForGraph = "Create array";
            }

            public string TitleForGraph;

            public ITask CreateTaskForClasses(int size)
            {
                return new ClassArrayCreationTask(size);
            }

            public ITask CreateTaskForStructures(int size)
            {
                return new StructArrayCreationTask(size);
            }
        }

        public class ResultsFactoryMethodCall : ITaskFactory
        {
            public ResultsFactoryMethodCall()
            {
                TitleForGraph = "Call method with argument";
            }

            public string TitleForGraph;

            public ITask CreateTaskForClasses(int size)
            {
                return new MethodCallWithClassArgumentTask(size);
            }

            public ITask CreateTaskForStructures(int size)
            {
                return new MethodCallWithStructArgumentTask(size);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NUnit.Framework;

namespace StructBenchmarking
{
    public class Benchmark : IBenchmark
	{
        public double MeasureDurationInMs(ITask task, int repetitionCount)
        {
            task.Run(); 
            GC.Collect();      
            GC.WaitForPendingFinalizers();
            var stopWatch = Stopwatch.StartNew();           
            for (var i = 0; i < repetitionCount; i++)
                task.Run();
            stopWatch.Stop();

			return stopWatch.Elapsed.TotalMilliseconds / repetitionCount;
		}
	}

    [TestFixture]
    public class RealBenchmarkUsageSample
    {
        [Test]
        public void StringConstructorFasterThanStringBuilder()
        {
            var builderTest = new BuilderString(1000);
            var stringTest = new DesignerString(1000);
            var benchmark = new Benchmark();

            var testWithDesigner = benchmark.MeasureDurationInMs
                (builderTest, BuilderString.RepetitionCount);
            var testWithBuilder = benchmark.MeasureDurationInMs
                (stringTest, DesignerString.RepetitionCount);
            Assert.Less(testWithBuilder, testWithDesigner);
            return;
        }
    }
    public class BuilderString : ITask
    {
        public static int RepetitionCount;
        private StringBuilder bigStringBuilder;
        public BuilderString(int repetitionCount)
        {
            RepetitionCount = repetitionCount;
        }
        public void Run()
        {
            bigStringBuilder = new StringBuilder();
            for (var i = 0; i < 10000; i++)
                bigStringBuilder.Append('a');
            bigStringBuilder.ToString();
            return;
        }

    }
    public class DesignerString : ITask
    {
        public static int RepetitionCount = 1000;
        private string bigString;
        public DesignerString(int repetitionCount)
        {
            RepetitionCount = repetitionCount;
        }
        public void Run()
        {
            bigString = new string('a', 1000);
            return;
        }
    }
}
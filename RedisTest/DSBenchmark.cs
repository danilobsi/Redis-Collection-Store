using System.Diagnostics;
using System.Text;

namespace RedisTest
{
    public static class DSBenchmark
    {
        public static void Print<T>(Func<T> functionToMeasure, string? methodName = null, int warmUpExecutions = 0, int benchMarkExecutions = 1)
        {
            var text = new StringBuilder();

            //Warm up
            for (var i = 0; i < warmUpExecutions; i++)
                functionToMeasure();

            double average = 0;

            //Measure
            for (var i = 0; i < benchMarkExecutions; i++)
            {
                var watch = new Stopwatch();

                watch.Start();
                functionToMeasure();
                watch.Stop();

                average = ((average * i) + watch.ElapsedMilliseconds) / (i + 1);
            }
            methodName ??= functionToMeasure.Method.Name;
            text.AppendLine($"{methodName}: avg: {average} ms");

            Console.Write(text.ToString());
        }
    }
}

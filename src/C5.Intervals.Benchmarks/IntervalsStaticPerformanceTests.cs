namespace C5.Intervals.Benchmarks
{
    using System;

    using NUnit.Framework;

    [TestFixture]
    public class IntervalsStaticPerformanceTests
    {
        [TestFixtureTearDown]
        public void PlotResults()
        {
            Benchmarker.PlotResults();
        }

        [Test, TestCaseSource(typeof(TestFactory), "TestCasesWithQueryRange")]
        public void FindOverlapsByInterval(TestConfigurationWithQueryRange config)
        {
            var action = new Action(() => config.IntervalCollection.FindOverlaps(config.QueryRangeInterval).Enumerate());

            var testName = string.Format("{0}_{1}_{2}", "FindOverlapsByInterval", config.DataSetName, config.QueryRangeName);

            action.Benchmark(config.Reference, testName, config.NumberOfIntervals);
        }

        [Test, TestCaseSource(typeof(TestFactory), "TestCasesWithQueryRange")]
        public void FindOverlapsByValue(TestConfigurationWithQueryRange config)
        {
            // TODO: We need test cases to check the performance at the start and end as well.
            var median = config.IntervalCollection.Span.Middle();
            var action = new Action(() => config.IntervalCollection.FindOverlaps(median).Enumerate());

            var testName = string.Format("{0}_{1}_Center", "FindOverlapsByValue", config.DataSetName);

            action.Benchmark(config.Reference, testName, config.NumberOfIntervals);
        }

        [Test, TestCaseSource(typeof(TestFactory), "TestCases")]
        public void Gaps(TestConfiguration config)
        {
            var action = new Action(() => config.IntervalCollection.Gaps.Enumerate());

            var testName = string.Format("{0}_{1}", "Gaps", config.DataSetName);

            action.Benchmark(config.Reference, testName, config.NumberOfIntervals);
        }

        [Test, TestCaseSource(typeof(TestFactory), "TestCasesWithQueryRange")]
        public void FindGaps(TestConfigurationWithQueryRange config)
        {
            var action = new Action(() => config.IntervalCollection.FindGaps(config.QueryRangeInterval).Enumerate());

            var testName = string.Format("{0}_{1}_{2}", "FindGaps", config.DataSetName, config.QueryRangeName);

            action.Benchmark(config.Reference, testName, config.NumberOfIntervals);
        }

        [Test, TestCaseSource(typeof(TestFactory), "TestCases")]
        public void GetEnumerator(TestConfiguration config)
        {
            var action = new Action(() => config.IntervalCollection.Enumerate());

            var testName = string.Format("{0}_{1}", "GetEnumerator", config.DataSetName);

            action.Benchmark(config.Reference, testName, config.NumberOfIntervals);

            // TODO: Need to assert  the collection that comes back from the enumerator is sorted.
        }
    }
}

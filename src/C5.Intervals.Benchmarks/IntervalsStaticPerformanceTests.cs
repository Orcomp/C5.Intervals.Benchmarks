using System.Collections.Generic;

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

        [Test, TestCaseSource(typeof(CreateIntervalCollectionTestFactory), "TestCases")]
        public void Constructor(CreateIntervalCollectionTestConfiguration config)
        {
            var testName = string.Format("{0}_{1}", "Constructor", config.DataSetName);

            config.CreateCollection.Benchmark(config.CollectionName, testName, config.NumberOfIntervals);
        }

        [Test, TestCaseSource(typeof(TestFactory), "TestCasesWithQueryRange")]
        public void FindOverlapsByInterval(IntervalCollectionTestConfigurationWithQueryRange config)
        {
            var action = new Action(() => config.IntervalCollection.FindOverlaps(config.QueryRange.Interval).Enumerate());

            var testName = string.Format("{0}_{1}_{2}",
                "FindOverlapsByInterval", config.DataSetName, config.QueryRange.Name);

            action.Benchmark(config.Reference, testName, config.NumberOfIntervals);
        }

        [Test, TestCaseSource(typeof(TestFactory), "TestCasesWithQueryRange")]
        public void FindOverlapsByValue(IntervalCollectionTestConfigurationWithQueryRange config)
        {
            // TODO: We need test cases to check the performance at the start and end as well.
            var median = config.IntervalCollection.Span.Middle();
            var action = new Action(() => config.IntervalCollection.FindOverlaps(median).Enumerate());

            var testName = string.Format("{0}_{1}_Center",
                "FindOverlapsByValue", config.DataSetName);

            action.Benchmark(config.Reference, testName, config.NumberOfIntervals);
        }

        [Test, TestCaseSource(typeof(TestFactory), "TestCases")]
        public void Gaps(IntervalCollectionTestConfiguration config)
        {
            var action = new Action(() => config.IntervalCollection.Gaps.Enumerate());

            var testName = string.Format("{0}_{1}", "Gaps", config.DataSetName);

            action.Benchmark(config.Reference, testName, config.NumberOfIntervals);
        }

        [Test, TestCaseSource(typeof(TestFactory), "TestCases")]
        public void LowestIntervals(IntervalCollectionTestConfiguration config)
        {
            var action = new Action(() => config.IntervalCollection.LowestIntervals.Enumerate());

            var testName = string.Format("{0}_{1}", "LowestIntervals", config.DataSetName);

            action.Benchmark(config.Reference, testName, config.NumberOfIntervals);
        }

        [Test, TestCaseSource(typeof(TestFactory), "TestCases")]
        public void HighestIntervals(IntervalCollectionTestConfiguration config)
        {
            var action = new Action(() => config.IntervalCollection.HighestIntervals.Enumerate());

            var testName = string.Format("{0}_{1}", "HighestIntervals", config.DataSetName);

            action.Benchmark(config.Reference, testName, config.NumberOfIntervals);
        }

        [Test, TestCaseSource(typeof(TestFactory), "TestCases")]
        public void Sorted(IntervalCollectionTestConfiguration config)
        {
            var action = new Action(() => config.IntervalCollection.Sorted.Enumerate());

            var testName = string.Format("{0}_{1}", "Sorted", config.DataSetName);

            action.Benchmark(config.Reference, testName, config.NumberOfIntervals);
        }

        [Test, TestCaseSource(typeof(TestFactory), "TestCasesWithQueryRange")]
        public void FindGaps(IntervalCollectionTestConfigurationWithQueryRange config)
        {
            var action = new Action(() => config.IntervalCollection.FindGaps(config.QueryRange.Interval).Enumerate());

            var testName = string.Format("{0}_{1}_{2}",
                "FindGaps", config.DataSetName, config.QueryRange.Name);

            action.Benchmark(config.Reference, testName, config.NumberOfIntervals);
        }

        [Test, TestCaseSource(typeof(TestFactory), "TestCases")]
        public void GetEnumerator(IntervalCollectionTestConfiguration config)
        {
            AssertIntervalCollectionIsSorted(config.IntervalCollection);
            var action = new Action(() => config.IntervalCollection.Enumerate());

            var testName = string.Format("{0}_{1}", "GetEnumerator", config.DataSetName);

            action.Benchmark(config.Reference, testName, config.NumberOfIntervals);
        }

        private void AssertIntervalCollectionIsSorted(IIntervalCollection<IInterval<int>, int> intervalCollection)
        {
            IComparer<IInterval<int>> comparer = ComparerFactory<IInterval<int>>
                .CreateComparer((x, y) => x.CompareLow(y) != 0 ? x.CompareLow(y) : x.CompareHigh(y));
            Assert.True(intervalCollection.IsSorted(comparer));
        }
    }
}

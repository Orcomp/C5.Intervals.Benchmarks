namespace C5.Intervals.Benchmarks
{
	#region using...

	using Entities;
	using Extensions;
	using Factories;
	using global::NUnitBenchmarker;
	using NUnit.Framework;

	#endregion

	[TestFixture]
	public class IntervalsStaticPerformanceTests
	{
		[TestFixtureSetUp]
		public void TestFixture()
		{
			Benchmarker.Init();
		}

		[Test, TestCaseSource(typeof (CreateIntervalCollectionTestFactory), "TestCases")]
		public void Constructor(CreateIntervalCollectionTestConfiguration config)
		{
			var testName = string.Format("{0}_{1}", "Constructor", config.DataSetName);
			config.IsReusable = true;
			config.Benchmark(testName, config.NumberOfIntervals, 5);
		}

		[Test, TestCaseSource(typeof (TestFactory), "TestCasesWithQueryRange")]
		public void FindGaps(TestConfigurationWithQueryRange config)
		{
			var testName = string.Format("{0}_{1}_{2}", "FindGaps", config.DataSetName, config.QueryRangeName);
			// Action does not modify the preparation data: 
			config.IsReusable = true;
			config.Run = (i =>
			{
				var c = ((TestConfigurationWithQueryRange) i);
				// TODO: We need test cases to check the performance at the start and end as well.
				c.IntervalCollection.FindGaps(c.QueryRange.Interval).Enumerate();
			});
			config.Benchmark(testName, config.NumberOfIntervals, 10);
		}

		[Test, TestCaseSource(typeof (TestFactory), "TestCasesWithQueryRange")]
		public void FindOverlapsByInterval(TestConfigurationWithQueryRange config)
		{
			var testName = string.Format("{0}_{1}_{2}", "FindOverlapsByInterval", config.DataSetName, config.QueryRangeName);
			// Action does not modify the preparation data: 
			config.IsReusable = true;
			config.Run = (i =>
			{
				var c = ((TestConfigurationWithQueryRange) i);
				// TODO: We need test cases to check the performance at the start and end as well.
				c.IntervalCollection.FindOverlaps(c.QueryRange.Interval).Enumerate();
			});
			config.Benchmark(testName, config.NumberOfIntervals, 10);
		}


		[Test, TestCaseSource(typeof (TestFactory), "TestCasesWithQueryRange")]
		public void FindOverlapsByValue(TestConfigurationWithQueryRange config)
		{
			var testName = string.Format("{0}_{1}_Center", "FindOverlapsByValue", config.DataSetName);

			// Action does not modify the preparation data: 
			config.IsReusable = true;
			config.Run = (i =>
			{
				var c = ((TestConfigurationWithQueryRange) i);
				// TODO: We need test cases to check the performance at the start and end as well.
				c.IntervalCollection.FindOverlaps(c.Median).Enumerate();
			});
			config.Benchmark(testName, config.NumberOfIntervals, 10);
		}

		[Test, TestCaseSource(typeof (TestFactory), "TestCases")]
		public void Gaps(TestConfiguration config)
		{
			var testName = string.Format("{0}_{1}", "Gaps", config.DataSetName);

			// Action does not modify the preparation data: 
			config.IsReusable = true;
			config.Run = (c => ((TestConfiguration) c).IntervalCollection.Gaps.Enumerate());
			config.Benchmark(testName, config.NumberOfIntervals, 5);
		}

		[Test, TestCaseSource(typeof (TestFactory), "TestCases")]
		public void GetEnumerator(TestConfiguration config)
		{
			var testName = string.Format("{0}_{1}", "GetEnumerator", config.DataSetName);

			// Action does not modify the preparation data: 
			config.IsReusable = true;
			config.Run = (c => ((TestConfiguration) c).IntervalCollection.Enumerate());
			config.Benchmark(testName, config.NumberOfIntervals, 5);
		}


		[Test, TestCaseSource(typeof (TestFactory), "TestCases")]
		public void HighestIntervals(TestConfiguration config)
		{
			var testName = string.Format("{0}_{1}", "HighestIntervals", config.DataSetName);

			// Action does not modify the preparation data: 
			config.IsReusable = true;
			config.Run = (c => ((TestConfiguration) c).IntervalCollection.HighestIntervals.Enumerate());
			config.Benchmark(testName, config.NumberOfIntervals, 5);
		}

		[Test, TestCaseSource(typeof (TestFactory), "TestCases")]
		public void LowestIntervals(TestConfiguration config)
		{
			var testName = string.Format("{0}_{1}", "LowestIntervals", config.DataSetName);
			// Action does not modify the preparation data: 
			config.IsReusable = true;
			config.Run = (c => ((TestConfiguration) c).IntervalCollection.LowestIntervals.Enumerate());
			config.Benchmark(testName, config.NumberOfIntervals, 5);
		}


		[Test, TestCaseSource(typeof (TestFactory), "TestCases")]
		public void Sorted(TestConfiguration config)
		{
			var testName = string.Format("{0}_{1}", "Sorted", config.DataSetName);
			// Action does not modify the preparation data: 
			config.IsReusable = true;
			config.Run = (c => ((TestConfiguration) c).IntervalCollection.Sorted.Enumerate());
			config.Benchmark(testName, config.NumberOfIntervals, 5);
		}
	}
}
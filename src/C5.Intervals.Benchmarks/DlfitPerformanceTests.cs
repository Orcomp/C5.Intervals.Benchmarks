#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="DlfitPerformanceTests.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace C5.Intervals.Benchmarks
{
	#region using...
	using Entities;
	using Factories;
	using NUnit.Framework;
	using NUnitBenchmarker;

	#endregion

	[TestFixture]
	public class PerformanceTest
	{
		[TestFixtureSetUp]
		public void TestFixture()
		{
			Benchmarker.Init(); 
		}

		[Test, TestCaseSource(typeof (DlfitTestFactory), "AddTestCases")]
		public void Add(DlfitTestConfiguration config)
		{
			// About count = 1: There is no need to execute the tests multiple times
			// the test itself repeats the Add operation 'size' times. 
			// (the displayed time is notmalized for _one_ operation)
			config.Benchmark(config.TestName, config.Size, 3);
		}

		[Test, TestCaseSource(typeof (DlfitTestFactory), "RemoveTestCases")]
		public void Remove(DlfitTestConfiguration config)
		{
			// About count = 1: There is no need to execute the tests multiple times
			// the test itself repeats the Remove operation 'size' times. 
			// (the displayed time is for _one_ operation)
			config.Benchmark(config.TestName, config.Size, 3);
		}

		[Test, TestCaseSource(typeof(DlfitTestFactory), "GetNextFindFirstTestCases")]
		public void FindFirst(DlfitTestConfiguration config)
		{
			config.Benchmark(config.TestName, config.Size, 3);
		}

		[Test, TestCaseSource(typeof(DlfitTestFactory), "GetNextEnumerateAllTestCases")]
		public void EnumerateAll(DlfitTestConfiguration config) 
		{
			// About count = 1: There is no need to execute the tests multiple times
			// the test itself repeats the Search operation 'size' times. 
			// (the displayed time is for _one_ operation)
			config.Benchmark(config.TestName, config.Size, 1);
		}
	}
}
namespace C5.Intervals.Benchmarks.Factories
{
	#region using...

	using System;
	using System.Collections.Generic;
	using Entities;
	using Extensions;
	using global::NUnitBenchmarker.Configuration;

	#endregion

	public class TestFactory : TestFactoryBase
	{
		public IEnumerable<TestConfigurationWithQueryRange> TestCasesWithQueryRange()
		{
			var sizes = TestFactoryHelper.Sizes;
			var dataSets = TestFactoryHelper.DataSets;

			foreach (var implementationType in ImplementationTypes)
			{
				foreach (var dataset in dataSets)
				{
					if (TestFactoryHelper.IsFinite(implementationType.Name) && dataset.HasOverlaps)
					{
						continue;
					}

					// Get an instance with minimal overload impact to get identifier:
					var minimalImpact = CreateIntervalCollection(implementationType, dataset, 1);

					foreach (var size in sizes)
					{
						foreach (var queryRangeName in CreateQueryRangesNames())
						{
							var identifier = string.Format("{0}_{1}", GetIdentifier(minimalImpact, implementationType), queryRangeName);
							var prepare = new Action<IPerformanceTestCaseConfiguration>(i =>
							{
								var config = (TestConfigurationWithQueryRange) i;
								var data = CreateIntervalCollection(implementationType, dataset, size);
								config.IntervalCollection = CreateIntervalCollection(implementationType, dataset, size);
								config.QueryRange = CreateQueryRange(queryRangeName, data);
								config.Median = config.IntervalCollection.Span.Middle();
							});


							yield return
								new TestConfigurationWithQueryRange
								{
									TargetImplementationType = implementationType,
									Identifier = identifier,
									NumberOfIntervals = size,
									DataSetName = dataset.Name,
									QueryRangeName = queryRangeName,
									Prepare = prepare,
								};
						}
					}
				}
			}
		}

		public IEnumerable<TestConfiguration> TestCases()
		{
			var sizes = TestFactoryHelper.Sizes;
			var dataSets = TestFactoryHelper.DataSets;

			foreach (var implementationType in ImplementationTypes)
			{
				foreach (var dataset in dataSets)
				{
					if (TestFactoryHelper.IsFinite(implementationType.Name) && dataset.HasOverlaps)
					{
						continue;
					}

					// Get an instance with minimal overload impact to get identifier:
					var minimalImpact = CreateIntervalCollection(implementationType, dataset, 1);
					var identifier = GetIdentifier(minimalImpact, implementationType);

					foreach (var size in sizes)
					{
						var prepare = new Action<IPerformanceTestCaseConfiguration>(i =>
						{
							var config = (TestConfiguration) i;
							config.IntervalCollection = CreateIntervalCollection(implementationType, dataset, size);
						});

						yield return
							new TestConfiguration
							{
								TargetImplementationType = implementationType,
								Identifier = identifier,
								NumberOfIntervals = size,
								DataSetName = dataset.Name,
								Prepare = prepare,
							};
					}
				}
			}
		}
	}
}
namespace C5.Intervals.Benchmarks.Factories
{
	#region using...

	using System;
	using System.Collections.Generic;
	using Entities;
	using Fasterflect;
	using global::NUnitBenchmarker.Configuration;

	#endregion

	public class CreateIntervalCollectionTestFactory : TestFactoryBase
	{
		public IEnumerable<CreateIntervalCollectionTestConfiguration> TestCases()
		{
			var dataSets = TestFactoryHelper.DataSets;
			var sizes = TestFactoryHelper.Sizes;

			foreach (var implementation in ImplementationTypes)
			{
				foreach (var dataset in dataSets)
				{
					foreach (var size in sizes)
					{
						if (TestFactoryHelper.IsFinite(implementation.Name) && dataset.HasOverlaps)
						{
							continue;
						}

						Action<IPerformanceTestCaseConfiguration> prepare;
						Action<IPerformanceTestCaseConfiguration> run;
						// Get an instance with minimal overload impact to get identifier:
						var minimalImpact = CreateIntervalCollection(implementation, dataset, 1);
						var identifier = GetIdentifier(minimalImpact, implementation);

						if (HasParameterlessConstructor(implementation))
						{
							prepare = i =>
							{
								var config = (CreateIntervalCollectionTestConfiguration) i;
								config.Data = dataset.IntervalsFactory(size);
								config.GenericCreatableType = GetGenericCreatableType(implementation);
							};

							run = i =>
							{
								var config = (CreateIntervalCollectionTestConfiguration) i;
								config.IntervalCollection = config.GenericCreatableType
									.TryCreateInstanceWithValues() as IIntervalCollection<IInterval<int>, int>;
								config.IntervalCollection.AddAll(config.Data);
							};
						}
						else
						{
							prepare = i =>
							{
								var config = (CreateIntervalCollectionTestConfiguration) i;
								config.Data = dataset.IntervalsFactory(size);
								config.GenericCreatableType = GetGenericCreatableType(implementation);
								config.Parameters = new Dictionary<string, object> {{"intervals", config.Data}};
							};

							run = i =>
							{
								var config = (CreateIntervalCollectionTestConfiguration) i;
								config.GenericCreatableType.TryCreateInstance(config.Parameters);
							};
						}

						yield return new CreateIntervalCollectionTestConfiguration
						{
							TargetImplementationType = implementation,
							Identifier = identifier,
							NumberOfIntervals = size,
							DataSetName = dataset.Name,
							Prepare = prepare,
							Run = run
						};
					}
				}
			}
		}
	}
}
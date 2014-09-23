namespace C5.Intervals.Benchmarks.Factories
{
	#region using...

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Entities;
	using Fasterflect;
	using global::NUnitBenchmarker;

	#endregion

	public abstract class TestFactoryBase
	{
		protected TestFactoryBase()
		{
			// See app.config for filters
			ImplementationTypes = Benchmarker.GetImplementations(typeof (IIntervalCollection<,>), true).ToList();
		}

		protected List<Type> ImplementationTypes { get; set; }

		protected static IIntervalCollection<IInterval<int>, int> CreateIntervalCollection(
			Type implementation,
			TestFactoryHelper.DataSet dataset = null,
			int size = 0)
		{
			var data = dataset.IntervalsFactory(size);

			// We need some logic to deal with static interval trees since they do not have parameterless constructors.
			var hasParameterlessConstructor = implementation.Constructor(Type.EmptyTypes) != null;

			IIntervalCollection<IInterval<int>, int> dataStructure = null;

			if (hasParameterlessConstructor)
			{
				dataStructure = CreateIntervalCollectionInstance(implementation);
				if (data != null && size != 0)
				{
					dataStructure.AddAll(data);
				}
			}
			else
			{
				dataStructure = CreateIntervalCollectionInstanceWithData(implementation, data);
			}

			return dataStructure;
		}

		protected static IIntervalCollection<IInterval<int>, int> CreateIntervalCollectionInstance(Type implementation)
		{
			return
				implementation.MakeGenericType(new[] {typeof (IInterval<int>), typeof (int)})
					.TryCreateInstanceWithValues() as IIntervalCollection<IInterval<int>, int>;
		}

		protected static IIntervalCollection<IInterval<int>, int> CreateIntervalCollectionInstanceWithData(
			Type implementation,
			IEnumerable<IInterval<int>> data)
		{
			var parameters = new Dictionary<string, object>();
			parameters.Add("intervals", data);
			return
				implementation.MakeGenericType(new[] {typeof (IInterval<int>), typeof (int)})
					.TryCreateInstance(parameters) as IIntervalCollection<IInterval<int>, int>;
		}

		protected string[] CreateQueryRangesNames()
		{
			return new[]
			{
				"FirstHalf",
				"FirstTenth",
				"MiddleTenth",
				"LastTenth"
			};
		}

		protected QueryRange CreateQueryRange(string name, IIntervalCollection<IInterval<int>, int> dataStructure)
		{
			var span = dataStructure.Span.High - dataStructure.Span.Low;

			switch (name)
			{
				case "FirstHalf":
					return new QueryRange(name, new IntervalBase<int>(dataStructure.Span.Low, span/2));
				case "FirstTenth":
					return new QueryRange(name, new IntervalBase<int>(dataStructure.Span.Low, span/10));
				case "MiddleTenth":
					return new QueryRange(name, new IntervalBase<int>((span/2) - span/20, (span/2) + span/20));
				case "LastTenth":
					return new QueryRange(name,
						new IntervalBase<int>(dataStructure.Span.High - span/10, dataStructure.Span.High));
			}
			throw new ArgumentException("name");
		}

		protected static string GetIdentifier(IIntervalCollection<IInterval<int>, int> intervals, Type implementation)
		{
			var dynamicOrStatic = intervals.IsReadOnly ? "Static" : "Dynamic";
			var overlapOrNoOverlaps = intervals.AllowsOverlaps ? "Overlaps" : "NoOverlaps";
			var identifier = string.Format("{0}:({1}-{2})", implementation.GetFriendlyName(), dynamicOrStatic,
				overlapOrNoOverlaps);
			return identifier;
		}


		protected static bool HasParameterlessConstructor(Type implementation)
		{
			return implementation.Constructor(Type.EmptyTypes) != null;
		}

		protected Type GetGenericCreatableType(Type implementation)
		{
			return implementation.MakeGenericType(new[] {typeof (IInterval<int>), typeof (int)});
		}
	}
}
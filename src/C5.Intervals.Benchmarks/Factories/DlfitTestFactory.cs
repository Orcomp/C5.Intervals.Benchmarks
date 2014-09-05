#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="DlfitTestFactory.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace C5.Intervals.Benchmarks.Factories
{
	#region using...
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Entities;
	using NUnitBenchmarker;
	using NUnitBenchmarker.Configuration;

	#endregion

	public class DlfitTestFactory
	{
		#region Fields
		private readonly long _offset = new DateTime(2000, 1, 1).Ticks;
		private readonly Random _random = new Random(0);
		#endregion

		#region Properties
		private IEnumerable<Type> ImplementationTypes
		{
			get
			{
				return new[]
				{
					typeof (DoublyLinkedFiniteIntervalTree<IntervalBase<DateTime>, DateTime>)
				};
			}
		}

		private static IEnumerable<int> Sizes
		{
			get { return new[] {1000, 10000, 100000, 1000000}; }
			//get { return new[] {1000}; }
		}
		#endregion

		#region Methods
		private IEnumerable<DlfitTestConfiguration> AddTestCases()
		{
			return from implementationType in ImplementationTypes
			       from size in Sizes
			       let prepare = new Action<IPerformanceTestCaseConfiguration>(c =>
			       {
				       var config = (DlfitTestConfiguration) c;
				       PrepareAdd(size, implementationType, config);
			       })
			       let run = new Action<IPerformanceTestCaseConfiguration>(c =>
			       {
				       var config = (DlfitTestConfiguration) c;
				       config.Target = CreateTarget<DateTime>(implementationType);
				       for (var i = 0; i < size; i++)
				       {
					       config.Target.Add(config.RandomDateTimeIntervals[i]);
				       }
			       })
			       select new DlfitTestConfiguration
			       {
				       TestName = "Add",
				       TargetImplementationType = implementationType,
				       Identifier = string.Format("{0}", implementationType.GetFriendlyName()),
				       Size = size,
				       Prepare = prepare,
				       Run = run,
				       IsReusable = true,
				       Divider = size
			       };
		}

		private IEnumerable<DlfitTestConfiguration> RemoveTestCases()
		{
			return from implementationType in ImplementationTypes
			       from size in Sizes
			       let prepare = new Action<IPerformanceTestCaseConfiguration>(c =>
			       {
				       var config = (DlfitTestConfiguration) c;
				       PrepareRemove(size, implementationType, config);
			       })
			       let run = new Action<IPerformanceTestCaseConfiguration>(c =>
			       {
				       var config = (DlfitTestConfiguration) c;
				       for (var i = 0; i < size; i++)
				       {
						   config.Target.Remove(config.RandomDateTimeIntervals[i]);
				       }
			       })
			       select new DlfitTestConfiguration
			       {
				       TestName = "Remove",
				       TargetImplementationType = implementationType,
				       Identifier = string.Format("{0}", implementationType.GetFriendlyName()),
				       Size = size,
				       Prepare = prepare,
				       Run = run,
				       IsReusable = false,
				       Divider = size
			       };
		}

		private IEnumerable<DlfitTestConfiguration> GetNextFindFirstTestCases()
		{
			return from implementationType in ImplementationTypes
				   from size in Sizes
				   let prepare = new Action<IPerformanceTestCaseConfiguration>(c =>
				   {
					   var config = (DlfitTestConfiguration)c;
					   PrepareSearch(size, implementationType, config);
				   })
				   let run = new Action<IPerformanceTestCaseConfiguration>(c =>
				   {
					   var config = (DlfitTestConfiguration)c;
					   for (var i = 0; i < size; i++)
					   {
						   var result = config.Target.GetNext(config.RandomDateTimeIntervals[i]).FirstOrDefault();
					   }
				   })
				   select new DlfitTestConfiguration
				   {
					   TestName = "GetNext().FirstOrDefault Average",
					   TargetImplementationType = implementationType,
					   Identifier = string.Format("{0}", implementationType.GetFriendlyName()),
					   Size = size,
					   Prepare = prepare,
					   Run = run,
					   IsReusable = true,
					   Divider = size
				   };
		}

		private IEnumerable<DlfitTestConfiguration> GetNextEnumerateAllTestCases()
		{
			return from implementationType in ImplementationTypes
				   from size in Sizes
				   let prepare = new Action<IPerformanceTestCaseConfiguration>(c =>
				   {
					   var config = (DlfitTestConfiguration)c;
					   PrepareSearch(size, implementationType, config);
				   })
				   let run = new Action<IPerformanceTestCaseConfiguration>(c =>
				   {
					   var config = (DlfitTestConfiguration)c;
					   var increment = size / 100;
					   for (var i = 0; i < size; i += increment)
					   {
						   foreach (var intervalBase in config.Target.GetNext(config.RandomDateTimeIntervals[i]))
						   {
							   ;
						   }

					   }
				   })
				   select new DlfitTestConfiguration
				   {
					   TestName = "GetNext() enumerate all",
					   TargetImplementationType = implementationType,
					   Identifier = string.Format("{0}", implementationType.GetFriendlyName()),
					   Size = size,
					   Prepare = prepare,
					   Run = run,
					   IsReusable = true,
					   Divider = 100
				   };
		}

		private void PrepareAdd(int size, Type type, DlfitTestConfiguration config)
		{
			config.RandomDateTimeIntervals = new IntervalBase<DateTime>[size];
			var permutation = Randomize(GetZeroToN(size)).ToArray();

			for (var i = 0; i < size; i++)
			{
				var low = new DateTime(_offset + permutation[i]*10);
				config.RandomDateTimeIntervals[i] = new IntervalBase<DateTime>(low, low.AddTicks(5));
			}

			config.Target = CreateTarget<DateTime>(type);
		}

		private void PrepareRemove(int size, Type type, DlfitTestConfiguration config)
		{
			// Do not generate in random order to speed up test preparation
			PrepareSearch(size, type, config);

			// Regenerate a random order for removal:
			config.RandomDateTimeIntervals = new IntervalBase<DateTime>[size]; 
			var permutation = Randomize(GetZeroToN(size)).ToArray();
			for (var i = 0; i < size; i++)
			{
				var low = new DateTime(_offset + permutation[i] * 10);
				config.RandomDateTimeIntervals[i] = new IntervalBase<DateTime>(low, low.AddTicks(5));
			}
		}

		private void PrepareSearch(int size, Type type, DlfitTestConfiguration config)
		{
			// Do not generate in random order to speed up test preparation
			config.RandomDateTimeIntervals = new IntervalBase<DateTime>[size];
			config.Target = CreateTarget<DateTime>(type);
			
			for (var i = 0; i < size; i++)
			{
				var low = new DateTime(_offset + i * 10);
				config.RandomDateTimeIntervals[i] = new IntervalBase<DateTime>(low, low.AddTicks(5));
				config.Target.Add(config.RandomDateTimeIntervals[i]);
			}
		}

		private IEnumerable<int> GetZeroToN(int n)
		{
			return Enumerable.Range(0, n);
		}

		private IEnumerable<T> Randomize<T>(IEnumerable<T> longs)
		{
			return (from kvp in longs.Select(value => new KeyValuePair<int, T>(_random.Next(), value)).ToList()
			        orderby kvp.Key
			        select kvp.Value).ToArray();
		}

		private IIntervalCollection<IntervalBase<T>, T> CreateTarget<T>(Type type) where T : IComparable<T>
		{
			return (IIntervalCollection<IntervalBase<T>, T>)type
				.GetConstructors().First(ci => !ci.GetParameters().Any()).Invoke(new object[0]);
		}
		#endregion
	}
}
namespace C5.Intervals.Benchmarks.Factories
{
	#region using...

	using System;

	#endregion

	public static class TestFactoryHelper
	{
		public static DataSet[] DataSets
		{
			get
			{
				return new[]
				{
					new DataSet("NoOverlaps", false, IntervalsFactory.NoOverlaps),
					//  new DataSet("Meets", false, IntervalsFactory.Meets),
					new DataSet("Overlaps", true, IntervalsFactory.Overlaps)
					// new DataSet("PineTreeForest", true, IntervalsFactory.PineTreeForest)
				};
			}
		}

		public static int[] Sizes
		{
			get { return new[] {10, 10000, 100000}; }
		}

		public static bool IsFinite(string name)
		{
			return name.Contains("Finite");
		}

		public class DataSet
		{
			public DataSet(string name, bool hasOverlaps, Func<int, IInterval<int>[]> intervalsFactory)
			{
				Name = name;
				HasOverlaps = hasOverlaps;
				IntervalsFactory = intervalsFactory;
			}

			public string Name { get; set; }

			public bool HasOverlaps { get; set; }

			public Func<int, IInterval<int>[]> IntervalsFactory { get; set; }
		}
	}
}
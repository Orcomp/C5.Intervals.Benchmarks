using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using C5.Intervals;
using C5.Intervals.Benchmarks;
using Fasterflect;

static internal class TestFactoryHelper
{
    public static DataSet[] DataSets
    {
        get
        {
            return new[]
            {
                new DataSet("NoOverlaps", false, IntervalsFactory.NoOverlaps),
                new DataSet("Meets", false, IntervalsFactory.Meets),
                new DataSet("Overlaps", true, IntervalsFactory.Overlaps),
                new DataSet("PineTreeForest", true, IntervalsFactory.PineTreeForest)
            };
        }
    }

    public static int[] CollectionSizes
    {
        get
        {
            return new[] { 100, 1000, 10000, };
        }
    }

    public class DataSet
    {
        public string Name { get; set; }

        public bool HasOverlaps { get; set; }

        public Func<int, IInterval<int>[]> IntervalsFactory { get; set; }

        public DataSet(string name, bool hasOverlaps, Func<int, IInterval<int>[]> intervalsFactory)
        {
            Name = name;
            HasOverlaps = hasOverlaps;
            IntervalsFactory = intervalsFactory;
        }
    }

    public static List<Type> GetImplementations()
    {
        // Instead of Assembly.Load we could look at the current directory and LoadFrom() all the assemblies we find in it.
        return Assembly.Load("C5.Intervals")
            .Types()
            .Where(x => x.Implements(typeof(IIntervalCollection<,>)))
            .Where(x => !x.Name.Contains("Old") && !x.Name.Contains("List2"))
            .Select(x => x)
            .ToList();
    }

    public static bool IsFinite(string name)
    {
        return name.Contains("Finite");
    }
}
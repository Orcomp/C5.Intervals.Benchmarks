namespace C5.Intervals.Benchmarks
{ 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using C5.Intervals;
using Fasterflect;

    public class TestFactory
    {
        private List<Type> Implementations { get; set; }

        public TestFactory()
        {
            // FindImplementations classes that implement IGraph
            this.FindImplementations();
        }

        private void FindImplementations()
        {
            // Instead of Assembly.Load we could look at the current directory and LoadFrom() all the assemblies we find in it.
            Implementations =
                Assembly.Load("C5.Intervals")
                    .Types()
                    .Where(x => x.Implements(typeof(IIntervalCollection<,>)))
                    .Where(x => !x.Name.Contains("Old") && !x.Name.Contains("List2"))
                    .Select(x => x)
                    .ToList();
        }

        public IEnumerable<TestConfigurationWithQueryRange> TestCasesWithQueryRange()
        {
            var collectionSizes = CollectionSizes;
            var dataSets = DataSets;

            foreach (var implementation in Implementations)
            {
                foreach (var dataset in dataSets)
                {
                    if (this.IsFinite(implementation.Name) && dataset.HasOverlaps)
                    {
                        continue;
                    }

                    foreach (var size in collectionSizes)
                    {
                        foreach (var queryRange in CreateQueryRanges(size))
                        {
                            var dataStructure = CreateIntervalCollection(implementation, dataset, size);

                            yield return
                                new TestConfigurationWithQueryRange
                                    {
                                        IntervalCollection = dataStructure,
                                        NumberOfIntervals = size,
                                        DataSetName = dataset.Name,
                                        QueryRange = queryRange,
                                    };
                        }
                    }
                }
            }
        }

        public IEnumerable<TestConfiguration> TestCases()
        {
            var collectionSizes = CollectionSizes;
            var dataSets = DataSets;

            foreach (var implementation in Implementations)
            {
                foreach (var dataset in dataSets)
                {
                    if (this.IsFinite(implementation.Name) && dataset.HasOverlaps)
                    {
                        continue;
                    }

                    foreach (var size in collectionSizes)
                    {
                        var dataStructure = CreateIntervalCollection(implementation, dataset, size);

                        yield return
                            new TestConfiguration
                                {
                                    IntervalCollection = dataStructure,
                                    NumberOfIntervals = size,
                                    DataSetName = dataset.Name,
                                };
                    }
                }
            }
        }


        private static IIntervalCollection<IInterval<int>, int> CreateIntervalCollection(
            Type implementation,
            DataSet dataset,
            int size)
        {
            var data = dataset.IntervalsFactory(size);

            // We need some logic to deal with static interval trees since they do not have parameterless constructors.
            var hasParameterlessConstructor = implementation.Constructor(Type.EmptyTypes) != null;

            IIntervalCollection<IInterval<int>, int> dataStructure = null;

            if (hasParameterlessConstructor)
            {
                dataStructure = CreateIntervalCollectionInstance(implementation);
                dataStructure.AddAll(data);
            }
            else
            {
                dataStructure = CreateIntervalCollectionInstanceWithData(implementation, data);
            }

            return dataStructure;
        }

        private static IIntervalCollection<IInterval<int>, int> CreateIntervalCollectionInstance(Type implementation)
        {
            return
                implementation.MakeGenericType(new[] { typeof(IInterval<int>), typeof(int) })
                    .TryCreateInstanceWithValues() as IIntervalCollection<IInterval<int>, int>;
        }

        private static IIntervalCollection<IInterval<int>, int> CreateIntervalCollectionInstanceWithData(
            Type implementation,
            IEnumerable<IInterval<int>> data)
        {
            var parameters = new Dictionary<string, object>();
            parameters.Add("intervals", data);
            return
                implementation.MakeGenericType(new[] { typeof(IInterval<int>), typeof(int) })
                    .TryCreateInstance(parameters) as IIntervalCollection<IInterval<int>, int>;
        }

        private static DataSet[] DataSets
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

        private static int[] CollectionSizes
        {
            get
            {
                return new[] { 100, 1000, 10000, };
            }
        }

        public QueryRange[] CreateQueryRanges(int size)
        {
            return new[]
            {
                new QueryRange("FirstHalf", new IntervalBase<int>(1, size / 2)),
                new QueryRange("FirstTenth", new IntervalBase<int>(1, 10)),
                new QueryRange("FirstTenth", new IntervalBase<int>((size / 2) - 5, (size / 2) + 5)),
                new QueryRange("LastTenth", new IntervalBase<int>(size - 10, size)),
            };
        }

        private bool IsFinite(string name)
        {
            return name.Contains("Finite");
        }

        private class DataSet
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
    }
}
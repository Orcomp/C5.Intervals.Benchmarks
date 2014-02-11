namespace C5.Intervals.Benchmarks
{ 
using System;
using System.Collections.Generic;
using C5.Intervals;
using Fasterflect;

    public class CreateIntervalCollectionTestFactory
    {
        public IEnumerable<CreateIntervalCollectionTestConfiguration> TestCases()
        {
            var implementations = TestFactoryHelper.GetImplementations();
            var dataSets = TestFactoryHelper.DataSets;
            var collectionSizes = TestFactoryHelper.CollectionSizes;

            foreach (var implementation in implementations)
            {
                foreach (var dataset in dataSets)
                {
                    foreach (var size in collectionSizes)
                    {
                        var data = dataset.IntervalsFactory(size);
                        var implementationType = CreateTestType(implementation);
                        
                        Action action = null;
                        if (HasParameterlessConstructor(implementation))
                        {
                            action = () =>
                            {
                                var dataStructure = implementationType
                                    .TryCreateInstanceWithValues() as IIntervalCollection<IInterval<int>, int>;
                                dataStructure.AddAll(data);
                            };
                        }
                        else
                        {
                            var parameters = new Dictionary<string, object>();
                            parameters.Add("intervals", data);

                            action = () =>
                            {
                                implementationType.TryCreateInstanceWithValues(parameters);
                            };
                        }
                        yield return new CreateIntervalCollectionTestConfiguration
                        {
                            CollectionName = implementation.Name,
                            DataSetName = dataset.Name,
                            NumberOfIntervals = size,
                            CreateCollection = action
                        };
                    }
                }
            }
        }

        private static bool HasParameterlessConstructor(Type implementation)
        {
            return implementation.Constructor(Type.EmptyTypes) != null;
        }

        private Type CreateTestType(Type implementation)
        {
            return implementation.MakeGenericType(new[] {typeof (IInterval<int>), typeof (int)});
        }
    }
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
            Implementations = TestFactoryHelper.GetImplementations();
        }

        public IEnumerable<IntervalCollectionTestConfigurationWithQueryRange> TestCasesWithQueryRange()
        {
            var collectionSizes = TestFactoryHelper.CollectionSizes;
            var dataSets = TestFactoryHelper.DataSets;

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

                        foreach (var queryRange in CreateQueryRanges(dataStructure))
                        {
                            yield return
                                new IntervalCollectionTestConfigurationWithQueryRange
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

        public IEnumerable<IntervalCollectionTestConfiguration> TestCases()
        {
            var collectionSizes = TestFactoryHelper.CollectionSizes;
            var dataSets = TestFactoryHelper.DataSets;

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
                            new IntervalCollectionTestConfiguration
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
            TestFactoryHelper.DataSet dataset,
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

        public QueryRange[] CreateQueryRanges(IIntervalCollection<IInterval<int>, int> dataStructure)
        {
            var span = dataStructure.Span.High - dataStructure.Span.Low;

            return new[]
            {
                new QueryRange("FirstHalf", new IntervalBase<int>(dataStructure.Span.Low, span / 2)),
                new QueryRange("FirstTenth", new IntervalBase<int>(dataStructure.Span.Low, span / 10)),
                new QueryRange("MiddleTenth", new IntervalBase<int>((span / 2) - span / 20, (span / 2) + span / 20)),
                new QueryRange("LastTenth", new IntervalBase<int>(dataStructure.Span.High - span / 10, dataStructure.Span.High)),
            };
        }

        private bool IsFinite(string name)
        {
            return name.Contains("Finite");
        }
    }
}
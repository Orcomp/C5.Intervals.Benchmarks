using System;

namespace C5.Intervals.Benchmarks
{
    using System.Collections.Generic;

    public abstract class TestConfiguration
    {
        public string DataSetName { get; set; }
        public int NumberOfIntervals { get; set; }
    }
    public class CreateIntervalCollectionTestConfiguration : TestConfiguration
    {
        public Action CreateCollection { get; set; }
        public string CollectionName { get; set; }
    }
    public class IntervalCollectionTestConfiguration : TestConfiguration
    {
        public IIntervalCollection<IInterval<int>, int> IntervalCollection { get; set; }
        public IEnumerable<IInterval<int>> Data { get; set; } 

        public string Reference
        {
            get
            {
                var dynamicOrStatic = this.IntervalCollection.IsReadOnly ? "Static" : "Dynamic";
                var overlapOrNoOverlaps = this.IntervalCollection.AllowsOverlaps ? "Overlaps" : "NoOverlaps";

                return this.IntervalCollection.GetType().Name + " - " + dynamicOrStatic  + " - " + overlapOrNoOverlaps;
            }
        }

        public override string ToString()
        {
            return string.Format("Type: {0}, DataSet: {1}, Count: {2}", this.IntervalCollection.GetType().Name, this.DataSetName, this.NumberOfIntervals);
        }
    }

    public class IntervalCollectionTestConfigurationWithQueryRange : IntervalCollectionTestConfiguration
    {
        public QueryRange QueryRange { get; set; }

        public override string ToString()
        {
            return string.Format("Type: {0}, DataSet: {1}, Count: {2}, QueryRange: {3}", this.IntervalCollection.GetType().Name, this.DataSetName, this.NumberOfIntervals, this.QueryRange.Name);
        }
    }
}
namespace C5.Intervals.Benchmarks
{
    using System.Collections.Generic;

    public class TestConfiguration
    {
        public IIntervalCollection<IInterval<int>, int> IntervalCollection { get; set; }
        public IEnumerable<IInterval<int>> Data { get; set; } 
        public int NumberOfIntervals { get; set; }
        public string DataSetName { get; set; }

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
    public class TestConfigurationWithQueryRange : TestConfiguration
    {
        public string QueryRangeName { get; set; }
        public IInterval<int> QueryRangeInterval { get; set; }

        public override string ToString()
        {
            return string.Format("Type: {0}, DataSet: {1}, Count: {2}, QueryRange: {3}", this.IntervalCollection.GetType().Name, this.DataSetName, this.NumberOfIntervals, this.QueryRangeName);
        }
    }
}
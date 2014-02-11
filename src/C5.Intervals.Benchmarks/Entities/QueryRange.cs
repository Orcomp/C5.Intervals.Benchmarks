namespace C5.Intervals.Benchmarks
{
    public class QueryRange
    {
        public string Name { get; set; }
        public IInterval<int> Interval { get; set; }

        public QueryRange(string name, IInterval<int> interval)
        {
            Name = name;
            Interval = interval;
        }
    }
}
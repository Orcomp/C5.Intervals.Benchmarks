namespace C5.Intervals.Benchmarks.Entities
{
	public class QueryRange
	{
		public QueryRange(string name, IInterval<int> interval)
		{
			Name = name;
			Interval = interval;
		}

		public string Name { get; set; }
		public IInterval<int> Interval { get; set; }
	}
}
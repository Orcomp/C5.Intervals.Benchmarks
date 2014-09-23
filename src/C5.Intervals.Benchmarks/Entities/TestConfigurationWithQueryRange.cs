namespace C5.Intervals.Benchmarks.Entities
{
	public class TestConfigurationWithQueryRange : TestConfiguration
	{
		public QueryRange QueryRange { get; set; }
		public int Median { get; set; }
		public string QueryRangeName { get; set; }
	}
}
namespace C5.Intervals.Benchmarks.Entities
{
	#region using...

	using global::NUnitBenchmarker.Configuration;

	#endregion

	public class TestConfiguration : PerformanceTestCaseConfigurationBase
	{
		public string DataSetName { get; set; }
		public int NumberOfIntervals { get; set; }
		public IIntervalCollection<IInterval<int>, int> IntervalCollection { get; set; }

		public override string ToString()
		{
			return Identifier;
		}
	}
}
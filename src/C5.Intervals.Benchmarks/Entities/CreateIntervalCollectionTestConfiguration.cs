namespace C5.Intervals.Benchmarks.Entities
{
	#region using...

	using System;
	using System.Collections.Generic;

	#endregion

	public class CreateIntervalCollectionTestConfiguration : TestConfiguration
	{
		public Type GenericCreatableType { get; set; }
		public Dictionary<string, object> Parameters { get; set; }
		public IEnumerable<IInterval<int>> Data { get; set; }
	}
}
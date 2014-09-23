#region Copyright (c) 2014 Orcomp development team.
// -------------------------------------------------------------------------------------------------------------------
// <copyright file="DlfitTestConfiguration.cs" company="Orcomp development team">
//   Copyright (c) 2014 Orcomp development team. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------
#endregion

namespace C5.Intervals.Benchmarks.Entities
{
	#region using...
	using System;
	using NUnitBenchmarker.Configuration;

	#endregion

	public class DlfitTestConfiguration : PerformanceTestCaseConfigurationBase
	{
		#region Properties
		public int Size { get; set; }
		public string TestName { get; set; }
		public IIntervalCollection<IntervalBase<DateTime>, DateTime> Target { get; set; }
		public IntervalBase<DateTime>[] RandomDateTimeIntervals { get; set; }
		#endregion

		#region Methods
		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>A <see cref="System.String" /> that represents this instance.</returns>
		public override string ToString()
		{
			return TestName;
		}
		#endregion
	}
}
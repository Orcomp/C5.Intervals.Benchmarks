namespace C5.Intervals.Benchmarks.Extensions
{
	#region using...

	using System.Collections.Generic;

	#endregion

	internal static class EnumerableExtension
	{
		public static void Enumerate<T>(this IEnumerable<T> enumerable)
		{
			foreach (var item in enumerable)
			{
				;
			}
		}
	}
}
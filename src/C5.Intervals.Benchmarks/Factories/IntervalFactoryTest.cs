namespace C5.Intervals.Benchmarks.Factories
{
	#region using...

	using System.Linq;
	using NUnit.Framework;

	#endregion

	[TestFixture, Ignore]
	public class IntervalFactoryTest
	{
		[Test]
		public void ConstantLengthMeetsTest()
		{
			var intervals = IntervalsFactory.ConstantLengthMeets(10, 1, 0).ToList();

			Assert.IsTrue(intervals.IsContiguous());
		}

		[Test]
		public void ContainsAscendingTest()
		{
			var intervals = IntervalsFactory.ContainsAscending(3, 0).ToList();

			Assert.IsTrue(intervals[0].Low == 0);
			Assert.IsTrue(intervals[0].High == 1);

			Assert.IsTrue(intervals[1].Low == -1);
			Assert.IsTrue(intervals[1].High == 2);

			Assert.IsTrue(intervals[2].Low == -2);
			Assert.IsTrue(intervals[2].High == 3);
		}

		[Test]
		public void ContainsDescendingTest()
		{
			var intervals = IntervalsFactory.ContainsDescending(3, 0).ToList();

			Assert.IsTrue(intervals[0].Low == 0);
			Assert.IsTrue(intervals[0].High == 5);

			Assert.IsTrue(intervals[1].Low == 1);
			Assert.IsTrue(intervals[1].High == 4);

			Assert.IsTrue(intervals[2].Low == 2);
			Assert.IsTrue(intervals[2].High == 3);
		}

		[Test]
		public void NoOverlapsConstantLenghtConstantGapTest()
		{
			var intervals = IntervalsFactory.NoOverlapsConstantLenghtConstantGap(3, 1, 1, 0).ToList();

			Assert.IsTrue(intervals[0].Low == 0);
			Assert.IsTrue(intervals[0].High == 1);

			Assert.IsTrue(intervals[2].Low == 4);
			Assert.IsTrue(intervals[2].High == 5);
		}

		[Test]
		public void NoOverlapsConstantLenghtGrowingGapTest()
		{
			var intervals = IntervalsFactory.NoOverlapsConstantLenghtGrowingGap(3, 1, 0).ToList();

			Assert.IsTrue(intervals[0].Low == 0);
			Assert.IsTrue(intervals[0].High == 1);

			Assert.IsTrue(intervals[2].Low == 5);
			Assert.IsTrue(intervals[2].High == 6);
		}

		[Test]
		public void OverlapsConstantLengthTest()
		{
			var intervals = IntervalsFactory.OverlapsConstantLength(10, 0).ToList();

			Assert.IsTrue(intervals[9].High == 19);
		}
	}
}
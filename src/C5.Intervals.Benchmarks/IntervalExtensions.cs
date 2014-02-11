namespace C5.Intervals.Benchmarks
{
    public static class IntervalExtensions
    {
        public static int Middle(this IInterval<int> interval)
        {
            return (interval.High - interval.Low) / 2;
        }
    }
}
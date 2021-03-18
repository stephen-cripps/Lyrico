using System.Collections.Generic;
using System.Linq;

namespace Lyrico.Application.Extensions
{
    public static class EnumerableExtensions
    {
        public static double? Median(this IEnumerable<uint> enumerable)
        {
            var cloned = new List<uint>(enumerable)
                .OrderByDescending(i => i)
                .ToList();

            if (!cloned.Any())
                return null;

            var mid = (cloned.Count() - 1) / 2.0;
            var t1 = cloned[(int)(mid)];
            var t2 = cloned[(int)(mid + 0.5)];


            return (cloned[(int)(mid)] + cloned[(int)(mid + 0.5)]) / 2.0;
        }

        public static double? PopulationVariance(this IEnumerable<uint> enumerable)
        {
            var list = enumerable.ToList();
            return list.Count == 0 ? null : PopulationVariance(list, list.Average(i => i));
        }

        public static double? PopulationVariance(this IEnumerable<uint> enumerable, double mean)
        {
            var list = enumerable.ToList();

            if (!list.Any())
                return null;

            var sum = list
                .Select(i => (i - mean) * (i - mean))
                .Sum();

            return sum / list.Count();
        }

    }
}

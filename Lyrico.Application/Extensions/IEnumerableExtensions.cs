using System.Collections.Generic;
using System.Linq;

namespace Lyrico.Application.Extensions
{
    /// <summary>
    /// A set of methods for mathematical analysis ofg an iEnumerable
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Finds the median number of an enumerable
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Finds the population variance of a given enumerable
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        public static double? PopulationVariance(this IEnumerable<uint> enumerable)
        {
            var list = enumerable.ToList();
            return list.Count == 0 ? null : PopulationVariance(list, list.Average(i => i));
        }

        /// <summary>
        /// Finds the population variance of a given enumerable. To be used if the mean has already been calculated 
        /// </summary>
        /// <param name="enumerable"></param>
        /// <param name="mean"></param>
        /// <returns></returns>
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

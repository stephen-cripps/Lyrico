using System.Collections.Generic;
using System.Linq;
using Lyrico.Application.Extensions;
using Xunit;

namespace Lyrico.Testing.UnitTests
{
    public class EnumerableExtensionTests
    {
        [Theory]
        [InlineData(new uint[] { 1, 2, 6, 5, 8, 7, 5, 9, 6, 3, 2, 4, 5, 5, 5, 5, 5, 5, 5, 5 }, 5)]
        [InlineData(new uint[] { 56, 87, 45, 95, 326, 512, 8456, 888, 888, 888, 88, 8888, 888, 888, 12, 531, 54 }, 512)]
        [InlineData(new uint[] { 1, 2, 3, 4, 5, 6 }, 3.5)]
        public void Median_ValidList_ReturnsMedian(IEnumerable<uint> enumerable, double expectedMedian)
        {
            var median = enumerable.Median();

            Assert.Equal(expectedMedian, (median));
        }

        [Fact]
        public void median_EmptyList_ReturnsNull()
        {
            var enumerable = new List<uint>();
            var median = enumerable.Median();

            Assert.Null(median);
        }

        [Theory]
        [InlineData(new uint[] { 1, 2, 6, 5, 8, 7, 5, 9, 6, 3, 2, 4, 5, 5, 5, 5, 5, 5, 5, 5 }, 3.49)]
        [InlineData(new uint[] { 56, 87, 45, 95, 326, 512, 8456, 888, 888, 888, 88, 8888, 888, 888, 12, 531, 54 }, 7199502.7)]
        [InlineData(new uint[] { 1, 2, 3, 4, 5, 6 }, 2.92)]
        public void variance_WithoutMean_ReturnsVariance(IEnumerable<uint> enumerable, double expectedVariance)
        {
            var variance = enumerable.PopulationVariance();

            Assert.NotNull(variance);
            Assert.Equal(expectedVariance, (double)variance, 2);
        }

        [Theory]
        [InlineData(new uint[] { 1, 2, 6, 5, 8, 7, 5, 9, 6, 3, 2, 4, 5, 5, 5, 5, 5, 5, 5, 5 }, 3.49)]
        [InlineData(new uint[] { 56, 87, 45, 95, 326, 512, 8456, 888, 888, 888, 88, 8888, 888, 888, 12, 531, 54 }, 7199502.7)]
        [InlineData(new uint[] { 1, 2, 3, 4, 5, 6 }, 2.92)]
        public void variance_WithMean_ReturnsVariance(IEnumerable<uint> enumerable, double expectedVariance)
        {
            var list = enumerable.ToList();

            var mean = list.Average(i => i);
            var variance = list.PopulationVariance(mean);

            Assert.NotNull(variance);
            Assert.Equal(expectedVariance, (double)variance, 2);
        }

        [Fact]
        public void variance_EmptyList_ReturnsNull()
        {
            var enumerable = new List<uint>();
            var variance = enumerable.PopulationVariance();

            Assert.Null(variance);
        }

        [Fact]
        public void variance_EmptyListWithMean_ReturnsNull()
        {
            var enumerable = new List<uint>();
            var variance = enumerable.PopulationVariance(53);

            Assert.Null(variance);
        }
    }
}

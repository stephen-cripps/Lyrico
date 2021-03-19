using System;
using System.Collections.Generic;
using System.Linq;
using Lyrico.Domain;
using Xunit;

namespace Lyrico.Testing.UnitTests
{
    /// <summary>
    /// These unit tests ensures the rich domain is validating inputs as expected
    /// </summary>
    public class ArtistTests
    {

        /// <summary>
        /// Tests successful instantiation of a the artist aggregate release
        /// </summary>
        [Fact]
        public void ArtistCtor_ValidInputs_CreatesArtist()
        {
            //Arrange
            const string artistName = "Artist Name";
            const string releaseName = "Release Name";
            const string trackName = "Track Name";

            //Test
            var tracks = new List<Track>() { new Track(trackName) };
            var releases = new List<Release>() { new Release(releaseName, tracks) };
            var artist = new Artist(artistName, releases);

            //Assert
            Assert.Equal(artistName, artist.Name);
            var artistReleases = artist.Releases.Single();
            Assert.Equal(releaseName, artistReleases.Name);
            var artistTracks = artistReleases.TrackList.Single();
            Assert.Equal(trackName, artistTracks.Name);
        }

        /// <summary>
        /// Checks validation of track name
        /// </summary>
        /// <param name="name"></param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void TrackCtor_EmptyName_ThrowsArgumentException(string name)
        {
            //Test
            var exception = Assert.Throws<ArgumentException>(() => new Track(name));

            //Assert
            Assert.Equal("Value cannot be null or whitespace. (Parameter 'name')", exception.Message);
        }

        /// <summary>
        /// Checks validation of Release name
        /// </summary>
        /// <param name="name"></param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ReleaseCtor_EmptyName_ThrowsArgumentException(string name)
        {
            //Test
            var exception = Assert.Throws<ArgumentException>(() => new Release(name, new List<Track>()));

            //Assert
            Assert.Equal("Value cannot be null or whitespace. (Parameter 'name')", exception.Message);
        }

        /// <summary>
        /// Checks validation of Artist name
        /// </summary>
        /// <param name="name"></param>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ArtistCtor_EmptyName_ThrowsArgumentException(string name)
        {
            //Test
            var exception = Assert.Throws<ArgumentException>(() => new Artist(name, new List<Release>()));

            //Assert
            Assert.Equal("Value cannot be null or whitespace. (Parameter 'name')", exception.Message);
        }
    }
}

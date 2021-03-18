using System;
using System.Collections.Generic;
using Lyrico.Domain;
using Xunit;

namespace Lyrico.Testing.UnitTests
{
    /// <summary>
    /// These unit tests ensures the rich domain is validating inputs as expected
    /// </summary>
    public class ArtistTests
    {
        [Fact]
        public void TrackCtor_ValidInputs_CreatesTrack()
        {
            //Arrange
            const string name = "Test Name";

            //Test
            var track = new Track(name);

            //Assert
            Assert.Equal(name, track.Name);
        }

        [Fact]
        public void ReleaseCtor_ValidInputs_CreatesRelease()
        {
            //Arrange
            const string name = "Test Name";
            var tracks = new List<Track>();
            const int year = 2021;

            //Test
            var release = new Release(name, tracks);

            //Assert
            Assert.Equal(release.Name, name);
        }

        [Fact]
        public void ArtistCtor_ValidInputs_CreatesArtist()
        {
            //Arrange
            const string name = "Test Name";
            var releases = new List<Release>();

            //Test
            var artist = new Artist(name, releases);

            //Assert
            Assert.Equal(artist.Name, name);
        }

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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ReleaseCtor_EmptyName_ThrowsArgumentException(string name)
        {
            //Test
            var exception = Assert.Throws<ArgumentException>(() => new Release(name, new List<Track>() ));

            //Assert
            Assert.Equal("Value cannot be null or whitespace. (Parameter 'name')", exception.Message);
        }

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

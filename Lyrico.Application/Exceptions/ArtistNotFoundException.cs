using System;

namespace Lyrico.Application.Exceptions
{
    public class ArtistNotFoundException : ApplicationException
    {
        /// <summary>
        /// Exception to be used when an artist cannot be found in the artist service
        /// </summary>
        /// <param name="artistName"></param>
        public ArtistNotFoundException(string artistName) : base($"'{artistName}' could not be found")
        { }
    }
}

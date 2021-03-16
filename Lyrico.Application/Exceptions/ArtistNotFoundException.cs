using System;

namespace Lyrico.Application.Exceptions
{
    public class ArtistNotFoundException : ApplicationException
    {
        public ArtistNotFoundException(string artistName) : base($"'{artistName}' could not be found")
        { }
    }
}

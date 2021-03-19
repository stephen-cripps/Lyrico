namespace Lyrico.MusicBrainz.DependencyInjection
{
    /// <summary>
    /// Used to inject options into the service
    /// </summary>
    public class Options
    {
        public string UserAgent { get; set; }
        public string BaseUrl { get; set; }
    }
}

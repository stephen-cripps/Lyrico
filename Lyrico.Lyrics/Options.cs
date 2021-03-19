namespace Lyrico.Lyricsovh
{
    /// <summary>
    /// Used to inject options into the service 
    /// </summary>
    public class Options
    { 
        public string BaseUrl { get; set; }

        //Defines when the request to the lyric api should cancel
        public int Timeout { get; set; } = 60;
    }
}

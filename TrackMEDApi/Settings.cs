using Microsoft.Extensions.Configuration;

namespace TrackMEDApi
{
    public class Settings
    {
        public string Database { get; set; }
        public string MongoConnection { get; set; }

    }
}
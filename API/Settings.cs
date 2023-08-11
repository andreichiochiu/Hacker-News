using Settings;
using System.Linq;
using System.Text.Json.Serialization;

namespace API
{
    public class Settings : ISettings
    {
        [JsonPropertyName("domain")]
        public string FirebaseDomain { get; set; }

        [JsonPropertyName("version")]
        public string ServiceVersion { get; set; }

        [JsonPropertyName("stories_path")]
        public string GetStoriesRelativePath { get; set; }

        [JsonPropertyName("story_path")]
        public string GetStoryRelativePath { get; set; }

        public bool HasEmptyProperties() => GetType().GetProperties().Any(prop => prop.PropertyType == typeof(string) && string.IsNullOrEmpty((string)prop.GetValue(this)));
    }
}

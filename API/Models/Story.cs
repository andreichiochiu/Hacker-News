using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace API.Models
{
    public class Story
    {

        [JsonPropertyName("by")]
        public string Author { get; set; }

        [JsonPropertyName("descendants")]
        public long Descendants { get; set; }

        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("kids")]
        public List<long> Kids { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("time")]
        public long TimeInTicks { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("url")]
        public string URL { get; set; }

    }
}
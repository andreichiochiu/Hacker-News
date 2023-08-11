using API.Models;
using Microsoft.Extensions.Caching.Memory;
using Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace API.Services
{
    public class FirebaseService : IFirebaseService
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly IMemoryCache memoryCache;
        private readonly Settings localSettings;

        public FirebaseService(IHttpClientFactory clientFactory, IMemoryCache inMemoryCache, SettingsProvider<Settings> settingsProvider)
        {
            httpClientFactory = clientFactory;
            localSettings = settingsProvider.Settings;
            memoryCache = inMemoryCache;
        }

        public async Task<IList<API.Response.Story>> GetStoriesOrderedByScoreAsync(int n)
        {
            List<API.Response.Story> response = new();
            List<Story> stories = new();

            var httpClient = httpClientFactory.CreateClient();

            var ids = await GetBestStoriesIdsAsync(httpClient);

            if (ids.Count <= 0)
            { 
                return response;
            }

            foreach (var id in ids)
            {
                if (memoryCache.TryGetValue(id, out Story existingStory))
                {
                    stories.Add(existingStory);
                }
                else
                {
                    var story = await GetStoryByIdAsync(httpClient, id);
                    memoryCache.Set(id, story, GetMemoryCacheEntryOptions());
                    stories.Add(story);
                }
            }

            var orderedByScore = stories.OrderByDescending(x => x.Score).Take(n);
            foreach (var orderedScore in orderedByScore)
            {
                var resourceScore = new API.Response.Story {
                    Score = orderedScore.Score,
                    Title = orderedScore.Title,
                    URI = orderedScore.URL,
                    PostedBy = orderedScore.Author,
                    CommentCount = orderedScore.Kids.Count
                };
                response.Add(resourceScore);
            }

            return response;
        }

        private async Task<IList<int>> GetBestStoriesIdsAsync(HttpClient httpClient)
        {
            string getBestStoriesUrl = $"https://{localSettings.FirebaseDomain}/{localSettings.ServiceVersion}/{localSettings.GetStoriesRelativePath}";

            var httpResponseMessage = await httpClient.GetAsync(getBestStoriesUrl);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                using var contentStream = await httpResponseMessage.Content.ReadAsStreamAsync();
                IList<int> storiesIds = await JsonSerializer.DeserializeAsync<List<int>>(contentStream);

                return storiesIds;
            }

            return null;
        }

        private async Task<Story> GetStoryByIdAsync(HttpClient httpClient, int id)
        {
            string getStoryUrl = $"https://{localSettings.FirebaseDomain}/{localSettings.ServiceVersion}/{localSettings.GetStoryRelativePath}".Replace("{id}", id.ToString());

            var httpResponseMessage = await httpClient.GetAsync(getStoryUrl);

            if (httpResponseMessage.IsSuccessStatusCode)
            {
                var contentString = await httpResponseMessage.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Story>(contentString);
            }

            return null;
        }

        private static MemoryCacheEntryOptions GetMemoryCacheEntryOptions()
        {
            return new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(300))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                    .SetPriority(CacheItemPriority.Normal);
        }
    }
}
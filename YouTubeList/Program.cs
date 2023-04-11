using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;


namespace YouTubeAPI
{
    class Program
    {
        static readonly HttpClient httpClient = new HttpClient();

        static async Task Main(string[] args)
        {
            string apiKey = "AIzaSyAArbWyATROGH5QLaE0KmTumaYqM2h0Ga8";

            string playlistEndpoint = $"https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&maxResults=50&playlistId=PLSN6qXliOioz5lnckfofNcLJ3CnZJvEJO&key={apiKey}";

            PlaylistResponse playlistResponse = await GetJsonAsync<PlaylistResponse>(playlistEndpoint);

            List<string> videoIds = new List<string>();
            Console.WriteLine("List of videos: ");
            int videoQuantity = 0;
            foreach (var item in playlistResponse.items)
            {
                videoIds.Add(item.snippet.resourceId.videoId);
                Console.WriteLine(item.snippet.title);
                videoQuantity++;
            }
            Console.WriteLine("Total number of videos: {0}", videoQuantity);
            string videoEndpoint = $"https://www.googleapis.com/youtube/v3/videos?part=statistics&id={string.Join(",", videoIds)}&key={apiKey}";

            VideoResponse videoResponse = await GetJsonAsync<VideoResponse>(videoEndpoint);

            int totalViews = 0;
            int totalLikes = 0;

            foreach (var item in videoResponse.items)
            {
                int viewCount = int.Parse(item.statistics.viewCount);
                int likeCount = int.Parse(item.statistics.likeCount);

                totalViews += viewCount;
                totalLikes += likeCount;
            }

            Console.WriteLine($"Total views: {totalViews}");
            Console.WriteLine($"Total likes: {totalLikes}");

            Console.ReadKey();
        }

        static async Task<T> GetJsonAsync<T>(string endpoint)
        {
            HttpResponseMessage response = await httpClient.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(json);
        }
    }

    class PlaylistResponse
    {
        public List<PlaylistItem> items { get; set; }
    }

    class PlaylistItem
    {
        public Snippet snippet { get; set; }
    }

    class Snippet
    {
        public ResourceId resourceId { get; set; }
        public string title { get; set; }

    }

    class ResourceId
    {
        public string videoId { get; set; }
    }

    class VideoResponse
    {
        public List<VideoItem> items { get; set; }
    }

    class VideoItem
    {
        public Statistics statistics { get; set; }
    }

    class Statistics
    {
        public string viewCount { get; set; }
        public string likeCount { get; set; }
    }
}
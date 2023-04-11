using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace YouTubeAPIExample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string playlistUrl = "https://www.googleapis.com/youtube/v3/playlistItems?part=snippet&playlistId=PLSN6qXliOioz5lnckfofNcLJ3CnZJvEJO&key=AIzaSyAArbWyATROGH5QLaE0KmTumaYqM2h0Ga8";
            string videoUrl = "https://www.googleapis.com/youtube/v3/videos?part=statistics&id={0}&key=AIzaSyAArbWyATROGH5QLaE0KmTumaYqM2h0Ga8";

            int totalViews = 0;
            int totalLikes = 0;

            using (HttpClient client = new HttpClient())
            {
                HttpResponseMessage response = await client.GetAsync(playlistUrl);
                string responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    JsonDocument data = JsonDocument.Parse(responseString);

                    foreach (JsonElement item in data.RootElement.GetProperty("items").EnumerateArray())
                    {
                        string videoId = item.GetProperty("snippet").GetProperty("resourceId").GetProperty("videoId").GetString();
                        response = await client.GetAsync(string.Format(videoUrl, videoId));
                        responseString = await response.Content.ReadAsStringAsync();

                        if (response.IsSuccessStatusCode)
                        {
                            data = JsonDocument.Parse(responseString);
                            JsonElement statistics = data.RootElement.GetProperty("items")[0].GetProperty("statistics");
                            int viewCount = statistics.GetProperty("viewCount").GetInt32();
                            int likeCount = statistics.GetProperty("likeCount").GetInt32();
                            totalViews += viewCount;
                            totalLikes += likeCount;
                        }
                    }

                    Console.WriteLine("Total views: " + totalViews);
                    Console.WriteLine("Total likes: " + totalLikes);
                }
            }
        }
    }
}

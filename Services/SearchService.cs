using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;
using Smokeball_SEORank_Api.Models;

namespace Smokeball_SEORank_Api.Services
{
    public class SeoRankService : ISeoRankService
    {
        private readonly HttpClient _httpClient;
        private readonly SearchEngineSettings _searchEngineSettings;

        public SeoRankService(HttpClient httpClient, IOptions<SearchEngineSettings> searchEngineSettings)
        {
            _httpClient = httpClient;
            _searchEngineSettings = searchEngineSettings.Value;
        }

        public async Task<string> PerformSeoRankSearch(string keywords, string url)
        {
            var positions = new List<int>();
            int maxResults = 100;
            int totalLinksChecked = 0;
            int start = 0;

            while (totalLinksChecked < maxResults)
            {
                var searchUrl = $"{_searchEngineSettings.GoogleBaseUrl}?q={Uri.EscapeDataString(keywords)}&start={start}";
                Console.WriteLine($"Searching URL: {searchUrl}");

                var response = await _httpClient.GetStringAsync(searchUrl);
                Console.WriteLine($"Response: {response}");

                var regex = new Regex(@"<a href=""/url\?q=([^&""]+)", RegexOptions.IgnoreCase);
                var matches = regex.Matches(response);

                for (int i = 0; i < matches.Count; i++)
                {
                    var matchUrl = matches[i].Groups[1].Value;
                    if (!matchUrl.Contains("google.com", StringComparison.OrdinalIgnoreCase)){
                        totalLinksChecked++;
                        Console.WriteLine($"Checking link {totalLinksChecked}: {matchUrl}");

                        // Cheking if the URL match
                        if (matchUrl.Contains(url, StringComparison.OrdinalIgnoreCase))
                        {
                            positions.Add(totalLinksChecked);
                        }
                    }
                }

                start += 10;
            }

            return positions.Count > 0 ? string.Join(", ", positions) : "0";
        }
    }
}
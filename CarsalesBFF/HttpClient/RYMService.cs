using CarsalesBFF.HttpClient.Episode;
using Newtonsoft.Json;
using System.Web;
using System;
using System.Text.RegularExpressions;

namespace CarsalesBFF.HttpClient
{
    public class RymService
    {
        public System.Net.Http.HttpClient Client { get; }

        public RymService(System.Net.Http.HttpClient client)
        {
            client.BaseAddress = new Uri("https://rickandmortyapi.com/api/");

            Client = client;
        }

        public string[] GetIdsFromUrl(string[] urls) => urls.Select(url => new Uri(url).Segments.Last()).ToArray();

        public async Task<Episode.Episode> GetEpisode(int page = 1)
        {
            var response = await Client.GetAsync($"episode?page={page}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Episode.Episode>(content);
        }

        public async Task<IEnumerable<EpisodeResponse>> GetAllEpisodes()
        {
            var episodeResults = new List<Result>();
            var episodeResponse = await GetEpisode(1);
            episodeResults.AddRange(episodeResponse.results);

            while (!string.IsNullOrEmpty(episodeResponse.info.next))
            {
                var url = new Uri(episodeResponse.info.next);
                var nextPage = HttpUtility.ParseQueryString(url.Query).Get("page");
                episodeResponse = await GetEpisode(Convert.ToInt32(nextPage));
                episodeResults.AddRange(episodeResponse.results);
            }


            var episodeList = episodeResults.Select(episode =>
            {
                var regex = new Regex("([0-9][0-9])");
                var matchs = regex.Matches(episode.episode);
                var seasonNumber = matchs[0];
                var episodeNumber = matchs[1];

                return new EpisodeResponse
                {
                    Id = episode.id,
                    Name = episode.name,
                    AirDate = episode.air_date,
                    Season = Convert.ToInt32(seasonNumber.Value),
                    Episode = Convert.ToInt32(episodeNumber.Value),
                    Characters = GetIdsFromUrl(episode.characters)
                };
            });


            return episodeList;
        }

        public async Task<Character.Character> GetCharacter(string id)
        {
            var response = await Client.GetAsync($"character/{id}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Character.Character>(content);
        }

        public async Task<Character.Character[]> GetCharacters(string[] ids)
        {
            var queryIds = string.Join(",", ids);
            var response = await Client.GetAsync($"character/{queryIds}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Character.Character[]>(content);
        }
    }
}

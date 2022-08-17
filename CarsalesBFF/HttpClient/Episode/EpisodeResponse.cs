namespace CarsalesBFF.HttpClient.Episode
{
    public class EpisodeResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string AirDate { get; set; }
        public int Episode { get; set; }
        public int Season { get; set; }
        public string[] Characters { get; set; }
    }
}

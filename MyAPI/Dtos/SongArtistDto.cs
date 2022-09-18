using System.Text.Json.Serialization;

namespace MyAPI.Dtos
{
    public class SongArtistDto
    {
        [JsonPropertyName("artist")]
        public string Artist { get; set; }

        public SongArtistDto(string artist)
        {
            Artist = artist;
        }
    }
}

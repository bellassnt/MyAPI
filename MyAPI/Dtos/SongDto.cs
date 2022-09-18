using System.Text.Json.Serialization;

namespace MyAPI.Dtos
{
    public class SongDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("artist")]
        public string Artist { get; set; }

        [JsonPropertyName("album")]
        public string Album { get; set; }

        [JsonPropertyName("year")]
        public string Year { get; set; }

        public SongDto(string title, string artist, string album, string year)
        {         
            Title = title;
            Artist = artist;
            Album = album;
            Year = year;
        }
    }
}
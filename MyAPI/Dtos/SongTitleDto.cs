using System.Text.Json.Serialization;

namespace MyAPI.Dtos
{
    public class SongTitleDto
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        public SongTitleDto(string title)
        {
            Title = title;
        }
    }
}

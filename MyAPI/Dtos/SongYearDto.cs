using System.Text.Json.Serialization;

namespace MyAPI.Dtos
{
    public class SongYearDto
    {
        [JsonPropertyName("year")]
        public string Year { get; set; }

        public SongYearDto(string year)
        {
            Year = year;
        }
    }
}

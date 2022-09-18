using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MyAPI.Models
{
    public class Song
    {
        [Key]
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("artist")]
        public string? Artist { get; set; }

        [JsonPropertyName("album")]
        public string? Album { get; set; }

        [JsonPropertyName("year")]
        public string? Year { get; set; }

        public Song(int id, string title, string artist, string album, string year)
        {
            Id = id;
            Title = title;
            Artist = artist;
            Album = album;
            Year = year;
        }

        public Song Clone()
        {
            return (Song)this.MemberwiseClone(); // Shallow clone
        }
    }
}


using System.Text.Json.Serialization;

namespace MyAPI.Dtos
{
    public class SongAlbumDto
    {
        [JsonPropertyName("album")]
        public string Album { get; set; }

        public SongAlbumDto(string album)
        {
            Album = album;
        }
    }
}

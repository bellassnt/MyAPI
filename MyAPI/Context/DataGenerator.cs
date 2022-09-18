using MyAPI.Models;
using System.Text.Json;

namespace MyAPI.Context
{
    public class DataGenerator
    {
        private readonly InMemoryContext _context;

        public DataGenerator(InMemoryContext context)
        {
            _context = context;
        }

        public void Generate()
        {
            if (!_context.Song.Any())
            {
                List<Song> songs;

                using (StreamReader reader = new("songs.json"))
                {
                    string json = reader.ReadToEnd();
                    songs = JsonSerializer.Deserialize<List<Song>>(json)!;
                }

                _context.Song.AddRange(songs);
                _context.SaveChanges();
            }

            if (!_context.User.Any())
            {
                var users = UserFaker.Create(10);
                
                _context.User.AddRange(users);
                _context.SaveChanges();
            }
        }
    }
}


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAPI.Dtos;
using MyAPI.Filters;
using MyAPI.Interfaces;
using MyAPI.Models;
using System.Net.Mime;
using System.Text.Json;
//using MyAPI.Loggers;

namespace MyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SongController : ControllerBase, ISongController<Song, SongDto, SongTitleDto, SongArtistDto, SongAlbumDto, SongYearDto>
    {
        private readonly IRepository<Song> _repository;

        public SongController(IRepository<Song> repository)
        {
            _repository = repository;
        }

        private static Song UpdateSong(Song newSongData, SongDto entity)
        {
            newSongData.Title = entity.Title;
            newSongData.Album = entity.Artist;
            newSongData.Album = entity.Album;
            newSongData.Year = entity.Year;

            return newSongData;
        }

        #region Gets
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] int page, int maxResultsPerPage)
        {
            var songs = await _repository.Get(page, maxResultsPerPage);

            return Ok(songs);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var song = await _repository.GetByKey(id);

            if (song == null)
                throw new NullReferenceException();

            return Ok(song);
        }
        #endregion

        #region Posts
        [HttpPost]
        [Authorize(Roles = "Manager, Developer")]
        public async Task<IActionResult> Post([FromBody] SongDto entity)
        {
            var newSong = new Song(id: 0,
                    entity.Title,
                    entity.Artist,
                    entity.Album,
                    entity.Year); // DAO

            var insertedSong = await _repository.Insert(newSong);

            return Created(string.Empty, insertedSong);
        }

        [HttpPost("year")]
        public async Task<IActionResult> GetByDecade([FromQuery] int page, int maxResultsPerPage, [FromBody] int decade)
        {
            var songs = await _repository.Get(page, maxResultsPerPage);

            var songsByYear = songs.Where(x => int.Parse(x.Year!) >= decade && int.Parse(x.Year!) < decade + 10);                

            return Ok(songsByYear);
        }

        [HttpPost("artist")]
        public async Task<IActionResult> GetByArtist([FromQuery] int page, int maxResultsPerPage, [FromBody] string artist)
        {
            var songs = await _repository.Get(page, maxResultsPerPage);

            var songsByArtist = songs.Where(x => x.Artist!.Contains(artist, StringComparison.InvariantCultureIgnoreCase));

            return Ok(songsByArtist);
        }
        #endregion

        #region Put
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager, Developer")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] SongDto entity)
        {
            var song = await _repository.GetByKey(id);

            if (song == null)
            {
                var newSong = new Song(id: 0, 
                    entity.Title, 
                    entity.Artist, 
                    entity.Album, 
                    entity.Year); // DAO

                var insertedSong = await _repository.Insert(newSong);

                return Created(string.Empty, insertedSong);
            }

            song = UpdateSong(song, entity);

            var updatedSong = await _repository.Update(id, song);

            return Ok(updatedSong);
        }
        #endregion

        #region Patches
        [HttpPatch("title/{id}")]
        [Authorize(Roles = "Manager, Developer")]
        public async Task<IActionResult> TitlePatch([FromRoute] int id, [FromBody] SongTitleDto entity)
        {
            var song = await _repository.GetByKey(id);

            if (song == null)
                throw new NullReferenceException();

            song.Title = entity.Title;

            var updatedSong = await _repository.Update(id, song);
            
            return Ok(updatedSong);
        }

        [HttpPatch("artist/{id}")]
        [Authorize(Roles = "Manager, Developer")]
        public async Task<IActionResult> ArtistPatch([FromRoute] int id, [FromBody] SongArtistDto entity)
        {
            var song = await _repository.GetByKey(id);

            if (song == null)
                throw new NullReferenceException();

            song.Artist = entity.Artist;

            var updatedSong = await _repository.Update(id, song);

            return Ok(updatedSong);
        }

        [HttpPatch("album/{id}")]
        [Authorize(Roles = "Manager, Developer")]
        public async Task<IActionResult> AlbumPatch([FromRoute] int id, [FromBody] SongAlbumDto entity)
        {
            var song = await _repository.GetByKey(id);

            if (song == null)
                throw new NullReferenceException();

            song.Album = entity.Album;

            var updatedSong = await _repository.Update(id, song);

            return Ok(updatedSong);
        }

        [HttpPatch("year/{id}")]
        [Authorize(Roles = "Manager, Developer")]
        public async Task<IActionResult> YearPatch([FromRoute] int id, [FromBody] SongYearDto entity)
        {
            var song = await _repository.GetByKey(id);

            if (song == null)
                throw new NullReferenceException();

            song.Year = entity.Year;

            var updatedSong = await _repository.Update(id, song);

            return Ok(updatedSong);
        }
        #endregion

        #region Delete
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var song = await _repository.GetByKey(id);

            if (song == null)
                throw new NullReferenceException();

            var deteledSong = await _repository.Delete(id);

            return Ok(deteledSong);
        }
        #endregion
    }
}
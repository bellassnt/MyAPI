using Microsoft.AspNetCore.Mvc;

namespace MyAPI.Interfaces
{
    public interface ISongController<T, P, Ti, Ar, Al, Y>
    {
        Task<IActionResult> Get(int page, int resultsPerPage);

        Task<IActionResult> Get(int key);

        Task<IActionResult> Post(P entity);

        Task<IActionResult> GetByDecade(int page, int maxResultsPerPage, int decade);

        Task<IActionResult> GetByArtist(int page, int maxResultsPerPage, string artist);

        Task<IActionResult> Put(int key, P entity);

        Task<IActionResult> TitlePatch(int key, Ti entity);

        Task<IActionResult> ArtistPatch(int key, Ar entity);

        Task<IActionResult> AlbumPatch(int key, Al entity);

        Task<IActionResult> YearPatch(int key, Y entity);

        Task<IActionResult> Delete(int key);
    }
}

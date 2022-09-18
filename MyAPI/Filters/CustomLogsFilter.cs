using Microsoft.AspNetCore.Mvc.Filters;
using MyAPI.Interfaces;
using MyAPI.Logs;
using MyAPI.Models;

namespace MyAPI.Filters
{
    public class CustomLogsFilter : IResultFilter, IActionFilter
    {
        private readonly List<int> _successStatusCodes;
        private readonly IRepository<Song> _repository;
        private readonly Dictionary<int, Song> _context;

        public CustomLogsFilter(IRepository<Song> repository)
        {
            _repository = repository;
            _context = new Dictionary<int, Song>();
            _successStatusCodes = new List<int>() { StatusCodes.Status200OK, StatusCodes.Status201Created }; // Status de sucesso
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (string.Equals(context.ActionDescriptor.RouteValues["controller"], "Song", StringComparison.InvariantCultureIgnoreCase))
            {                
                if (context.ActionArguments.ContainsKey("id") && int.TryParse(context.ActionArguments["id"].ToString(), out int id))
                {
                    if (context.HttpContext.Request.Method.Equals(("PUT"), StringComparison.InvariantCultureIgnoreCase) 
                        || context.HttpContext.Request.Method.Equals(("PATCH"), StringComparison.InvariantCultureIgnoreCase)
                        || context.HttpContext.Request.Method.Equals(("DELETE"), StringComparison.InvariantCultureIgnoreCase))
                    {
                        var song = _repository.GetByKey(id).Result;

                        if (song != null)
                        {
                            var songClone = song.Clone(); // Shallow clone
                            _context.Add(id, songClone);
                        }
                    }
                }
            }
        }

        public void OnResultExecuted(ResultExecutedContext context) // Resolver para não "quebrar" em nenhum dos métodos. Dica:
                                                                    // Dica: a verificação do método vem antes do "Parse"
        {
            if (context.HttpContext.Request.Path.Value.StartsWith("/api/Song", StringComparison.InvariantCultureIgnoreCase))
            {
                if (_successStatusCodes.Contains(context.HttpContext.Response.StatusCode))
                {
                    if (int.TryParse(context.HttpContext.Request.Path.ToString().Split("/").Last(), out int id))
                    {
                        if (context.HttpContext.Request.Method.Equals(("PUT"), StringComparison.InvariantCultureIgnoreCase)
                        || context.HttpContext.Request.Method.Equals(("PATCH"), StringComparison.InvariantCultureIgnoreCase))
                        {
                            var afterUpdate = _repository.GetByKey(id).Result;

                            if (afterUpdate != null)
                            {
                                if (_context.TryGetValue(id, out Song beforeUpdate))
                                {
                                    CustomLogs.SaveLog(afterUpdate.Id,
                                        "Song",
                                        afterUpdate.Title,
                                        afterUpdate.Artist,
                                        context.HttpContext.Request.Method,
                                        beforeUpdate,
                                        afterUpdate);
                                }
                            }
                        }
                        else if (context.HttpContext.Request.Method.Equals(("DELETE"), StringComparison.InvariantCultureIgnoreCase))
                        {
                            if (_context.TryGetValue(id, out Song beforeUpdate))
                            {
                                CustomLogs.SaveLog(beforeUpdate.Id,
                                    "Song",
                                    beforeUpdate.Title,
                                    beforeUpdate.Artist,
                                    context.HttpContext.Request.Method);

                                _context.Remove(id);
                            }
                        }
                    }                    
                }
            }
        }

        #region Métodos não utilizados
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnResultExecuting(ResultExecutingContext context)
        {

        }
        #endregion
    }
}

using Microsoft.AspNetCore.Mvc;
using OptixTechnicalTest.DataLayer.Interfaces;
using OptixTechnicalTest.Dto;

namespace OptixTechnicalTest.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieSearchController(ILogger<MovieSearchController> logger, IDbConnectionApi db) : ControllerBase
{
	[HttpPost(Name = "SearchMovies")]
	public async Task<MovieSearchResults> Post(string movieNameSubstring, string? orderByField, bool? orderAscending, string? genreIdFilter, string? actorIdFilter, int? pageLength, int? pageNumber)
	{
		if ((movieNameSubstring?.Length ?? 0) == 0)
		{
			var errorText = "search phrase is missing";
			logger.LogError(errorText);
			return new MovieSearchResults();
		}

		logger.LogInformation($"searching for {movieNameSubstring} page");

		var orderBy = new MovieOrderBy
		{
			OrderByField = orderByField ?? string.Empty,
			Ascending = orderAscending ?? true
		};
		var filterBy = new MovieFilters
		{
			GenreList = genreIdFilter ?? string.Empty,
			ActorList = actorIdFilter ?? string.Empty
		};

		return await db.SearchMovies(movieNameSubstring!, orderBy, filterBy, pageLength, pageNumber);
	}
}

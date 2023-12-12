using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using OptixTechnicalTest.DataLayer.Interfaces;
using OptixTechnicalTest.Dto;

namespace OptixTechnicalTest.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieSearchController(ILogger<MovieSearchController> logger, IDbConnectionApi db) : ControllerBase
{
	static readonly string[] OrderByFields = ["", "Name", "ReleaseDate", "Score"];

	[HttpPost(Name = "SearchMovies")]
	public async Task<MovieSearchResults> Post(string movieNameSubstring, string? orderByField, bool? orderAscending, string? genreIdFilter, string? actorIdFilter, int? pageLength, int? pageNumber)
	{
		if ((movieNameSubstring?.Length ?? 0) < 2)
		{
			return new MovieSearchResults { ErrorMessage= "search phrase needs to be at least 2 characters" };
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

		if (!OrderByFields.Contains(orderBy.OrderByField))
		{
			return new MovieSearchResults { ErrorMessage = $"'orderByField' should be one of '{string.Join("', '", OrderByFields)}', but is provided as '{orderBy.OrderByField}'" };
		}

		return await db.SearchMovies(movieNameSubstring!, orderBy, filterBy, pageLength, pageNumber);
	}
}

using Dapper;
using Microsoft.Extensions.Configuration;
using OptixTechnicalTest.DataLayer.Interfaces;
using OptixTechnicalTest.Dto;

namespace OptixTechnicalTest.DataLayer.Classes;

public class DbConnectionApi(IConfiguration configuration) : DbConnection(configuration), IDbConnectionApi
{
	private const int DefaultPageLength = 10;
	private const int DefaultPageNumber = 0;

	public async Task<MovieSearchResults> SearchMovies(string searchSubstring, MovieOrderBy orderBy, MovieFilters filters, int? pageLength, int? pageNumber)
	{
		CheckConnection();
		var results = new MovieSearchResults();

		var ob = orderBy.OrderByField.Length>0
			? orderBy
			: new MovieOrderBy { OrderByField="Name" };
		var f = filters ?? new MovieFilters();
		var pl = pageLength ?? DefaultPageLength;
		var pn = pageNumber ?? DefaultPageNumber;

		var sql = $"EXEC dbo.SearchMovies '{searchSubstring}', '{f.GenreList}', '{f.ActorList}', {pn}, {pl}, '{ob.OrderByField} {(ob.Ascending ? "asc" : "desc")}'";
//throw new Exception(sql);

		var resultTables = await _connection!.QueryMultipleAsync(sql);
		results.Genres = resultTables.Read<Genre>().ToList();
		results.Actors = resultTables.Read<Actor>().ToList();
		results.Movies = resultTables.Read<Movie>().ToList();
		results.TotalResults = resultTables.Read<int>()?.ToList()[0] ?? 0;

		return results;
	}
}

using OptixTechnicalTest.Dto;

namespace OptixTechnicalTest.DataLayer.Interfaces;

public interface IDbConnectionApi
{
	Task<MovieSearchResults> SearchMovies(string searchSubstring, MovieOrderBy orderBy, MovieFilters filters, int? pageLength, int? pageNumber);
}
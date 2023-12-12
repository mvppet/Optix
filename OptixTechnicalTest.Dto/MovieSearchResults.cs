namespace OptixTechnicalTest.Dto;

public class MovieSearchResults
{
	public List<Movie> Movies { get; set; } = [];
	public List<Genre> Genres { get; set; } = [];
	public List<Actor> Actors { get; set; } = [];
	public int TotalResults { get; set; } = 0;
	public string? ErrorMessage { get; set; }

}

namespace OptixTechnicalTest.Dto;

public class Movie
{
	public required string Name { get; set; }

	public DateTime ReleaseDate { get; set; }

	public decimal Score { get; set; }

	public List<Genre> Genres { get; set; } = [];

	public required string Overview { get; set; }

	public List<CastMember> Cast { get; set; } = [];

	public required string OriginalTitle { get; set; }

	public required string Status { get; set; }

	public required string Language { get; set; }

	public required decimal Budget { get; set; }

	public required decimal Revenue { get; set; }

	public required string Country { get; set; }

}

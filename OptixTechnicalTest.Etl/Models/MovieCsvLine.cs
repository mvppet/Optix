using CsvHelper.Configuration.Attributes;

namespace OptixTechnicalTest.Etl.Models;

internal class MovieCsvLine: BaseMovieData
{
	[Name("genre")]
	public string Genres { get; set; } = string.Empty;

	[Name("crew")]
	public string CastMembers { get; set; } = string.Empty;
}

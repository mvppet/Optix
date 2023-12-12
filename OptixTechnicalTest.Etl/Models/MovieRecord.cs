namespace OptixTechnicalTest.Etl.Models;

internal class MovieRecord: BaseMovieData
{
	public List<string> Genres { get; set; } = [];

	public List<(string Actor, string Character, bool voiceOnly)> CastMembers { get; set; } = [];

	public MovieRecord(MovieCsvLine movieRecord)
	{
        Title = movieRecord.Title.Trim();
        Released = movieRecord.Released;
        Score = movieRecord.Score;
        Overview = movieRecord.Overview.Trim();
        OriginalTitle = movieRecord.OriginalTitle.Trim();
        Budget = movieRecord.Budget;
        Revenue = movieRecord.Revenue;
        Language = movieRecord.Language.Trim();
        Country = movieRecord.Country.Trim();
        Status = movieRecord.Status.Trim();
	}

}

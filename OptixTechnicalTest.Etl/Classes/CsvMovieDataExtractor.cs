namespace OptixTechnicalTest.Etl.Classes;

internal class CsvMovieDataExtractor() : ICsvMovieDataExtractor
{
	public List<MovieRecord> ExtractRawMovieRecordsFromCsvData(List<MovieCsvLine> rawMovieData)
	{
		List<MovieRecord> movies = [];
		rawMovieData.ForEach(r => movies.Add(ProcessRecord(r)));
		return movies;
	}

	private static MovieRecord ProcessRecord(MovieCsvLine movieRecord)
	{
		var movie = new MovieRecord(movieRecord)
		{
			Genres = ParseStringList(movieRecord.Genres),
			CastMembers = ParseCastList(movieRecord.CastMembers)
		};

		return movie;
	}

	private static List<string> ParseStringList(string items)
	{
		var trimmedList = new List<string>();
		items
			.Split(',')
			.ToList()
			.ForEach(i => trimmedList.Add(i.Trim()));
		return trimmedList;
	}

	private static List<(string, string, bool)> ParseCastList(string castList)
	{
		var castMembers = new List<(string, string, bool)>();

		if (castList.Length > 0)
		{
			var tupleMembers = ParseStringList(castList);
			const string VoiceString = "(voice)";

			// make sure we have the correct number of (actor,character) items
			if (tupleMembers.Count % 2 != 0)
			{
				throw new CastMembersMalformedException();
			}

			for (int tupleNum = 0; tupleNum < tupleMembers.Count; ++tupleNum)
			{
				var actor = tupleMembers[tupleNum];
				var character = tupleMembers[++tupleNum].Replace('"', '\'');
				var voiceOnly = character.EndsWith(VoiceString);
				if (voiceOnly)
				{
					character = character.Replace(VoiceString, string.Empty).Trim();
					actor = actor.Replace(VoiceString, string.Empty).Trim();
				}
				castMembers.Add((actor, character, voiceOnly));
			}
		}

		return castMembers;
	}
}

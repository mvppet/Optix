using OptixTechnicalTest.DataLayer.Interfaces;
using OptixTechnicalTest.Dto;

namespace OptixTechnicalTest.Etl.Classes;

internal class DataMigration(ICsvMovieDataParser parser, ICsvMovieDataExtractor extractor, IDbConnectionEtl dbConnection)
{
	Dictionary<string, RelationTableCache> _lookupTables = new Dictionary<string, RelationTableCache>();

	internal void ProcessCsvFile(string filename)
	{
		// parse the CSV file and split the text lists into separate entities
		var rawData = parser.ReadCsvFile(filename);
		var movies = extractor.ExtractRawMovieRecordsFromCsvData(rawData);

		// Add the relations to the database
		var genres = AddRelations<Genre>(movies.SelectMany(m => m.Genres));
		var actors = AddRelations<Actor>(movies.SelectMany(m => m.CastMembers).Select(cm => cm.Actor));
		var countries = AddRelations<Country>(movies.Select(m => m.Country));
		var languages = AddRelations<Language>(movies.Select(m => m.Language));
		var statuses = AddRelations<Status>(movies.Select(m => m.Status));

		// now the main movie table
		Console.WriteLine($"Adding Movies");
		movies.ForEach(m => AddMovie(m, genres, actors, countries, languages, statuses));

		// and the relational links
	}

	private RelationTableCache AddRelations<T>(IEnumerable<string> items) where T : BaseRelationTable
	{
		// tidy the list
		var itemDistinct = items.Distinct().Order().ToList();

		// load the existing values
		var tableName = typeof(T).Name;
		var rtc = new RelationTableCache(tableName, dbConnection);
		Console.WriteLine($"Adding {tableName} Records.");
		_lookupTables.Add(tableName, rtc);
		rtc.GetCurrrentValues();

		// add them if they aren't there
		itemDistinct.ForEach(rtc.PossiblyAddRelation);

		return rtc;
	}


	private void AddMovie(MovieRecord movie, RelationTableCache genres, RelationTableCache actors, RelationTableCache countries, RelationTableCache languages, RelationTableCache statuses)
	{

		// first, the main movie record
		var movieId = dbConnection.InsertBasicMovie(
							movie.Title,
							movie.OriginalTitle,
							movie.Released.ToDateTime(TimeOnly.MinValue),
							movie.Score,
							movie.Overview,
							movie.Budget,
							movie.Revenue,
							languages.GetRelationId(movie.Language),
							countries.GetRelationId(movie.Country),
							statuses.GetRelationId(movie.Status)
							
						);

		// now add some relations
		movie.Genres.ForEach(genre => dbConnection.AddMovieRelation(nameof(Genre), movieId, genres.GetRelationId(genre)));
		movie.CastMembers.ForEach(cm => dbConnection.AddCastMember(movieId, actors.GetRelationId(cm.Actor), cm.Character, cm.voiceOnly));
						
	}

}

using Microsoft.Extensions.Configuration;
using OptixTechnicalTest.DataLayer.Classes;

try
{
	string filename = args.Length > 0
					? args[0]
					: "imdb_movies.csv";

	IConfiguration config = new ConfigurationBuilder()
			.AddJsonFile("appsettings.json", optional: false)
			.Build();

	using var db = new DbConnectionEtl(config);
	
	// read the CSV file
	var parser = new CsvMovieDataParser();
	var extractor = new CsvMovieDataExtractor();

	// using Poor Man's DI
	new DataMigration(
		parser,
		extractor,
		db
	).ProcessCsvFile(filename);

}
catch (Exception ex)
{
	Console.Write($"Error: {ex.Message}");
}


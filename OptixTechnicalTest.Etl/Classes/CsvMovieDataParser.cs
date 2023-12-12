using System.Globalization;
using System.Runtime.CompilerServices;
using CsvHelper;
using CsvHelper.Configuration;

[assembly: InternalsVisibleTo("OptixTechnicalTest.Etl.Tests")]
namespace OptixTechnicalTest.Etl.Classes;

internal class CsvMovieDataParser: ICsvMovieDataParser
{
	public List<MovieCsvLine> ReadCsvFile(string filename)
	{
		if (!File.Exists(filename))
		{
			throw new CsvFileNotFoundException(filename);
		}

		using var reader = new StreamReader(filename);
		return ReadCsvStream(reader);
	}

	public List<MovieCsvLine> ReadCsvStream(StreamReader reader)
	{
		var config = new CsvConfiguration(new CultureInfo("en-US")) // dates in this file are US format mm/dd/yyyy
		{
			TrimOptions = TrimOptions.Trim // some fields have leading/trailing spaces
		};
		using var csv = new CsvReader(reader, config); 
		var movieList = csv.GetRecords<MovieCsvLine>()?.ToList();

		return movieList != null && movieList.Count > 0
			? movieList
			: throw new CsvFileEmptyException();
	}

}

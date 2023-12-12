namespace OptixTechnicalTest.Etl.Interfaces;
internal interface ICsvMovieDataParser
{
	List<MovieCsvLine> ReadCsvFile(string filename);
	List<MovieCsvLine> ReadCsvStream(StreamReader reader);
}

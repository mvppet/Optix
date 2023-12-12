namespace OptixTechnicalTest.Etl.Interfaces;

internal interface ICsvMovieDataExtractor
{
    List<MovieRecord> ExtractRawMovieRecordsFromCsvData(List<MovieCsvLine> rawMovieData);
}
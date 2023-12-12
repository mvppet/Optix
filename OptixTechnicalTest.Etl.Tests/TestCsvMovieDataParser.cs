using System.Text;
using FluentAssertions;

namespace OptixTechnicalTest.Etl.Tests;

public class TestCsvMovieDataParser
{
	const string TestData = "names,date_x,score,genre,overview,crew,orig_title,status,orig_lang,budget_x,revenue,country\r\n"
					+ "Name 1,05/23/1996 ,1,\"Drama, Crime\",\"Overview 1.\",\"Actor Number One, Character \"\"Number\"\" 1, Actor 2, Character 2\",  Original Name 1  , Released, English,11.0,111.0,AU\r\n"
					+ "Name 2,01/02/1996 ,2,\"Drama, Comedy\",\"Overview 2.\",\"Actor Three, Character \"\"Number\"\" 3, Actor 2, Character 4\",Original Name 2, Not Released , Spanish,22.0,222.0,GB\r\n"
					;

	[Fact]
	public void Given_NonExistentFile_When_FileIsOpened_Then_ExceptionThrown()
	{
		// Arrange
		var filename = Guid.NewGuid().ToString();
		var parser = new CsvMovieDataParser();

		// Act
		Action act = () => parser.ReadCsvFile(filename);

		// Assert
		act.Should().Throw<CsvFileNotFoundException>();
	}

	[Fact]
	public void Given_OpenStreamWithValidData_WhenStrewamIsRead_Then_ExpectedResults()
	{
		// arrange
		var movieStream = new MemoryStream(Encoding.UTF8.GetBytes(TestData));
		var movieStreamReader = new StreamReader(movieStream);
		var parser = new CsvMovieDataParser();

		// act
		var movies = parser.ReadCsvStream(movieStreamReader);

		// assert
		movies.Count.Should().Be(2);

		// "Name 1,05/23/1996 ,1,\"Drama, Crime\",\"Overview 1.\",\"Actor Number One, Character \"\"Number\"\" 1, Actor 2, Character 2\",  Original Name 1  , Released, English,11.0,111.0,AU\r\n"
		movies[0].Title.Should().Be("Name 1");
		movies[0].Released.Year.Should().Be(1996);
		movies[0].Released.Month.Should().Be(5);
		movies[0].Released.Day.Should().Be(23);
		movies[0].Score.Should().Be(1);
		movies[0].Genres.Should().Be("Drama, Crime");
		movies[0].Overview.Should().Be("Overview 1.");
		movies[0].CastMembers.Should().Be("Actor Number One, Character \"Number\" 1, Actor 2, Character 2");
		movies[0].OriginalTitle.Should().Be("Original Name 1");
		movies[0].Status.Should().Be("Released");
		movies[0].Language.Should().Be("English");
		movies[0].Budget.Should().Be(11);
		movies[0].Revenue.Should().Be(111);
		movies[0].Country.Should().Be("AU");

		// "Name 2,01/02/1996 ,2,\"Drama, Comedy\",\"Overview 2.\",\"Actor Three, Character \"\"Number\"\" 3, Actor 2, Character 4\",Original Name 2, Not Released , Spanish,22.0,222.0,GB\r\n"
		movies[1].Title.Should().Be("Name 2");
		movies[1].Released.Year.Should().Be(1996);
		movies[1].Released.Month.Should().Be(1);
		movies[1].Released.Day.Should().Be(2);
		movies[1].Score.Should().Be(2);
		movies[1].Genres.Should().Be("Drama, Comedy");
		movies[1].Overview.Should().Be("Overview 2.");
		movies[1].CastMembers.Should().Be("Actor Three, Character \"Number\" 3, Actor 2, Character 4");
		movies[1].OriginalTitle.Should().Be("Original Name 2");
		movies[1].Status.Should().Be("Not Released");
		movies[1].Language.Should().Be("Spanish");
		movies[1].Budget.Should().Be(22);
		movies[1].Revenue.Should().Be(222);
		movies[1].Country.Should().Be("GB");

	}

}

using Dapper;
using Microsoft.Extensions.Configuration;
using OptixTechnicalTest.DataLayer.Interfaces;
using OptixTechnicalTest.Dto;

namespace OptixTechnicalTest.DataLayer.Classes;

public class DbConnectionEtl(IConfiguration configuration) : DbConnection(configuration), IDbConnectionEtl
{

	public int InsertRelation(string tableName, string textValue)
	{
		CheckConnection();

		string sql = @$"insert into [{tableName}](Name) values(@name)";
		_connection!.Execute(sql, new { name = textValue });


		sql = $@"select IDENT_CURRENT('{tableName}')";
		return _connection!.QueryFirst<int>(sql, new { name = textValue });
	}

	public List<BaseRelationTable> GetRelations(string tableName)
	{
		CheckConnection();

		string sql = @$"select Id, lower(Name) Name from [{tableName}]";

		var results = _connection!.Query<BaseRelationTable>(sql);

		return results.ToList();
	}

	public int InsertBasicMovie(string title, string originalTitle, DateTime releaseDate, decimal score, string overview, decimal budget, decimal revenue, int languageId, int countryId, int statusId)
	{

		string sql = @$"insert into [Movie](Name, OriginalName, ReleaseDate, Score, Overview, Budget, Revenue, LanguageId, CountryId, StatusId)
						values(@title, @originalTitle, @releaseDate, @score, @overview, @budget, @revenue, @languageId, @countryId, @statusId)";
		_connection!.Execute(sql, new
		{
			title,
			originalTitle,
			releaseDate,
			score,
			overview,
			budget,
			revenue,
			languageId,
			countryId,
			statusId
		});

		sql = "select IDENT_CURRENT('Movie')";
		return _connection!.QueryFirst<int>(sql);
	}

	public void AddMovieRelation(string tableName, int movieId, int relationId)
	{
		string sql = @$"insert into [Movie{tableName}](MovieId, {tableName}Id) values(@movieId, @relationId)";
		_connection!.Execute(sql, new { movieId, relationId });
	}

	public void AddCastMember(int movieId, int actorId, string characterName, bool voiceOnly)
	{
		string sql = @$"insert into [MovieCastMember](MovieId, ActorId, CharacterName, VoiceOnly) values(@movieId, @actorId, @characterName, @voiceOnly)";
		_connection!.Execute(sql, new { movieId, actorId, characterName, voiceOnly });
	}



}

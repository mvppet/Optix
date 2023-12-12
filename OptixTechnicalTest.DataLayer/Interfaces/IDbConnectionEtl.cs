using OptixTechnicalTest.Dto;

namespace OptixTechnicalTest.DataLayer.Interfaces;

public interface IDbConnectionEtl
    {
	int InsertRelation(string tableName, string textValue);
	List<BaseRelationTable> GetRelations(string tableName);
	int InsertBasicMovie(string title, string originalTitle, DateTime releaseDate, decimal score, string overview, decimal budget, decimal revenue, int languageId, int countryId, int statusId);
	void AddMovieRelation(string tableName, int movieId, int relationId);
	void AddCastMember(int movieId, int actorId, string character, bool voiceOnly);
}
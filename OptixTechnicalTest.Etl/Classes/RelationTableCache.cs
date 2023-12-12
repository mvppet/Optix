using OptixTechnicalTest.DataLayer.Interfaces;

namespace OptixTechnicalTest.Etl.Classes;

internal class RelationTableCache(string tableName, IDbConnectionEtl db)
{
	Dictionary<string, int> _tableCache = new Dictionary<string, int>();

	public void GetCurrrentValues()
	{
		var items = db.GetRelations(tableName);
		items.ForEach(item => _tableCache.Add(item.Name, item.Id));
	}

	public void PossiblyAddRelation(string textValue)
	{
		string textLower = textValue.ToLower();
		if (!_tableCache.ContainsKey(textLower))
		{
			// doesn't exist in cache. Insert it
			var id = db.InsertRelation(tableName, textValue);
			_tableCache.Add(textLower, id);
		}
	}

	public int GetRelationId(string textValue) => _tableCache[textValue.ToLower()];

}

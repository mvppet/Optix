using CsvHelper.Configuration.Attributes;

namespace OptixTechnicalTest.Etl.Models;

internal class BaseMovieData
{
	[Name("names")]
	public string Title { get; set; } = string.Empty;
	
	[Name("date_x")]
	public DateOnly Released { get; set; }
	
	[Name("score")]
	public decimal Score { get; set; }
	
	[Name("overview")]
	public string Overview { get; set; } = string.Empty;
	
	[Name("orig_title")]
	public string OriginalTitle { get; set; } = string.Empty;
	
	[Name("status")]
	public string Status { get; set; } = string.Empty;
	
	[Name("orig_lang")]
	public string Language { get; set; } = string.Empty;
	
	[Name("budget_x")]
	public decimal Budget { get; set; }
	
	[Name("revenue")]
	public decimal Revenue { get; set; }
	
	[Name("country")]
	public string Country { get; set; } = string.Empty;
}

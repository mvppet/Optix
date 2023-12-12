namespace OptixTechnicalTest.Etl.Classes;

internal class CsvFileNotFoundException(string filename) : FileNotFoundException(filename) { }

internal class CsvFileEmptyException : Exception
{
	internal CsvFileEmptyException() : base("No Data Read From CSV File") { }
}

internal class CastMembersMalformedException: Exception
{
	internal CastMembersMalformedException() : base("Cast Member list should have an even numberr of items as the list will be parsed as tuples of (actor,character)") { }
}


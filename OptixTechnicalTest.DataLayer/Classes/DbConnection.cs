using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using OptixTechnicalTest.DataLayer.Interfaces;
using static OptixTechnicalTest.DataLayer.Classes.Exceptions;

namespace OptixTechnicalTest.DataLayer.Classes;

public class DbConnection : IDisposable
{
	protected SqlConnection? _connection;
	private readonly string _connectionString;

	public DbConnection(IConfiguration configuration)
	{
		_connectionString = configuration.GetConnectionString("Optix")
							?? throw new NullConnectionStringException()
							;

	}

	public void Dispose()
	{
		if (_connection != null)
		{
			_connection.Close();
			_connection.Dispose();
		}
	}

	protected void CheckConnection()
	{
		if (_connection == null)
		{
			_connection = new SqlConnection(_connectionString);
		}
		if (_connection?.State != System.Data.ConnectionState.Open)
		{
			_connection!.Open();
		}
	}

}

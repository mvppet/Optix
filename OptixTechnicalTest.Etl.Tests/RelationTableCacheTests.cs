using Moq;
using FluentAssertions;
using OptixTechnicalTest.DataLayer.Interfaces;

namespace OptixTechnicalTest.Etl.Tests;

public class RelationTableCacheTests
{

	[Fact]
	public void Given_DbConnection_WhenAddingToEmptyCache_Then_DatabaseCalled()
	{
		const string val1 = "val1";
		const string val2 = "val2";

		// Arrange
		var mockDbConnection = new Mock<IDbConnectionEtl>(MockBehavior.Strict);
		mockDbConnection.Setup(mdbc => mdbc.InsertRelation(It.IsAny<string>(), val1)).Returns(1);
		mockDbConnection.Setup(mdbc => mdbc.InsertRelation(It.IsAny<string>(), val2)).Returns(2);

		var rtc = new RelationTableCache(It.IsAny<string>(), mockDbConnection.Object);

		// Act
		rtc.PossiblyAddRelation(val1);
		rtc.PossiblyAddRelation(val2);

		// Assert
		rtc.GetRelationId(val1).Should().Be(1);
		rtc.GetRelationId(val2).Should().Be(2);

	}

	[Fact]
	public void Given_DbConnection_WhenAddingToExistingCache_Then_DatabaseNotCalled()
	{
		const string val1 = "val1";

		// Arrange
		var mockDbConnection = new Mock<IDbConnectionEtl>(MockBehavior.Strict);
		mockDbConnection.Setup(db => db.InsertRelation(It.IsAny<string>(), It.IsAny<string>())).Returns(1);

		var rtc = new RelationTableCache(string.Empty, mockDbConnection.Object);

		// Act
		rtc.PossiblyAddRelation(val1);
		rtc.PossiblyAddRelation(val1);

		// Assert
		mockDbConnection.Verify(
			db => db.InsertRelation(It.IsAny<string>(), It.IsAny<string>()),
			Times.Exactly(1)
		);

	}

}

# Optix Technical Test - Dave Woodhead

## DataSet
As the dataset in the API spec doesn't contain actors, this system uses one from the same site. That file had problems in the cast members list, so a fixed version is included in the OptixTechnicalTest.Etl project.

## Database
This project is developed using SQL Server 2022.
The DB creation script "DbSetup.sql" uses an "IF EXISTS" clause, so will need at least SQL Server 2016. This script can be found in the solution root, and also in the Documentation folder in the solution itself.
It is recommended to run the database creation script on an empty database. The script can be run many times to clear and recreate the database.

## Caveats
This project is missing some tests, but tests for a couple of the Etl classes can be found in the OptixTechnicalTest.Etl.Tests project.
Given more time, I would have added more Etl tests, and tests for the move searching.

## Usage
For the Genre and Actor filters, the text field on the API query expect a comma-separated list of IDs of those items. For instance "1730,11065" for Actors Amy Shumer and Ewen Bremner, or "5,6" for Comedy and Crime movies.
For the "orderByField" filters, the valid values are Id, Name, OriginalName, ReleaseDate, Score, Language, Country, Status, Budget, Revenue, Overview. "orderByField" should be true or null for ascending searches, and false for descending.
For sanity, the search phrase must be at least two charaters.

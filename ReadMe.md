# Optix Teachnical Test - Dave Woodhead

## DataSet
As the dataset in the API spec doesn't contain actors, this system uses one from the same site. That file had problems in the cast members list, so a fixed version is included in the OptixTechnicalTest.Etl project.

## Database
This project is developed using SQL Server 2022 running in a Docker container.
The DB creation script "DbSetup.sql" uses an "IF EXISTS" clause, so will need at least SQL Server 2016. This script can be found in the solution root, and also in the Documentation folder in the solution itself.


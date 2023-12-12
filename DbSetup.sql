-- CSV Header: names,date_x,score,genre,overview,crew,orig_title,status,orig_lang,budget_x,revenue,country

drop view if exists [vMovieCastMembers];
go
drop view if exists [vMovieGenres]
go
drop view if exists [vMovies];
go
drop table IF EXISTS [MovieCastMember];
go
drop table IF EXISTS [MovieGenre];
go
drop table IF EXISTS [Movie]
go
drop table IF EXISTS [Status]
go
drop table IF EXISTS [Country]
go
drop table IF EXISTS [Language]
go
drop table IF EXISTS [Actor]
go
drop table IF EXISTS [Genre]
go

create table [Genre]
(
	[Id]			int not null identity primary key,
	[Name]			nvarchar(256) not null unique
)
go

create table [Actor]
(
	[Id]			int not null identity primary key,
	[Name]			nvarchar(256) not null unique
)
go

create table [Language]
(
	[Id]			int not null identity primary key,
	[Name]			nvarchar(256) not null unique
)
go

create table [Country]
(
	[Id]			int not null identity primary key,
	[Name]			nvarchar(256) not null unique
)
go

create table [Status]
(
	[Id]			int not null identity primary key,
	[Name]			nvarchar(256) not null unique
)
go

create table [Movie]
(
	[Id]			int not null identity primary key,
	[Name]			nvarchar(1024) not null,
	OriginalName	nvarchar(1024) null,
	ReleaseDate		date null,
	Score			int null,
	Overview		nvarchar(max) null,
	Budget			bigint null,
	Revenue			bigint null,
	LanguageId		int null references [Language],
	CountryId		int null references [Country],
	StatusId		int null references [Status]

)
go

create table [MovieGenre]
(
	MovieId			int not null references [Movie],
	GenreId			int not null references [Genre]
)
go

create table [MovieCastMember]
(
	MovieId			int not null references [Movie],
	ActorId			int not null references [Actor],
	VoiceOnly		bit not null,
	CharacterName	nvarchar(max) null
)
go

USE [Optix]
GO

/****** Object:  View [dbo].[vMovie]    Script Date: 12/12/2023 11:07:19 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE VIEW [vMovies]
AS
	SELECT m.Id, m.Name, m.OriginalName, m.ReleaseDate, m.Score, l.Id AS LanguageId, l.Name AS Language, c.Id AS CountryId, c.Name AS Country, s.Id AS StatusId, s.Name AS Status, m.Budget, m.Revenue, m.Overview
		FROM		dbo.Movie AS m
		INNER JOIN	dbo.Status AS s ON m.StatusId = s.Id
		INNER JOIN	dbo.Language AS l ON m.LanguageId = l.Id
		INNER JOIN	dbo.Country AS c ON m.CountryId = c.Id
GO

CREATE VIEW [vMovieCastMembers]
AS
	SELECT cm.MovieId, a.Id, a.Name, cm.VoiceOnly, cm.CharacterName
		FROM		dbo.MovieCastMember AS cm
		INNER JOIN	dbo.Actor AS a ON cm.ActorId = a.Id
GO

CREATE VIEW [vMovieGenres]
AS
	SELECT mg.MovieId, g.Id, g.Name
		FROM		dbo.MovieGenre AS mg
		INNER JOIN	dbo.Genre AS g ON mg.GenreId = g.Id
GO

CREATE OR ALTER PROCEDURE dbo.SearchMovies
(
	@nameSubstring	varchar(1024),
	@genres			varchar(1024),
	@actors			varchar(1024),
	@pageNum		int,
	@pageLength		int,
	@orderBy		varchar(1024)
)
AS
BEGIN
	set nocount on;

	select *
		into #AllMatchingMovies 
		from vMovies
		where lower([Name]) like '%' + lower(@nameSubstring) + '%';
		;

	-- get all the genres and send to output
	select distinct mg.Id, mg.Name from vMovieGenres mg
		inner join #AllMatchingMovies m on mg.MovieId = m.Id
		;

	-- same with actors
	select distinct ma.Id, ma.Name from vMovieCastMembers ma
		inner join #AllMatchingMovies m on ma.MovieId = m.Id
		;

	-- can we filter any results out?
	declare @sql nvarchar(1024);
	if Len(@genres) > 0
	begin
		set @sql = 'delete from #AllMatchingMovies where Id not in (select MovieId from MovieGenre where GenreId in (' + @genres + '))';
		EXEC sp_executesql @sql;
	end;
	if Len(@actors) > 0
	begin
		set @sql = 'delete from #AllMatchingMovies where Id not in (select MovieId from MovieCastMember where ActorId in (' + @actors + '))';
		EXEC sp_executesql @sql;
	end;

	-- now the paged results
	set @sql = 'select * from #AllMatchingMovies
		ORDER BY ' + @orderBy + '
		OFFSET ' + convert(varchar, @pageNum * @pageLength) + ' ROWS
		FETCH NEXT ' + convert(varchar, @pageLength) + ' ROWS ONLY';
	EXEC sp_executesql @sql;

	select count(*) as ResultCount from #AllMatchingMovies;

END;
go



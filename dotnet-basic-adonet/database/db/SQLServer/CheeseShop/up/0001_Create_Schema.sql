CREATE SCHEMA [security]

GO

CREATE TABLE [security].[Member]
(
	Id			int				NOT NULL	IDENTITY(1,1),
	Email		varchar(100)	NOT NULL,
	Forename	varchar(100)	NOT NULL,
	Surname		varchar(100)	NOT NULL,
	Version		int				NOT NULL,
	Parameter	int				NULL,
	Salt		varbinary(32)	NOT NULL,
	Password	varbinary(32)	NOT NULL,
	CONSTRAINT PK_Member PRIMARY KEY(Id)
)


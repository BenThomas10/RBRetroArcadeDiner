# RBRetroArcadeDiner
Angular .Net Core Coding Challenge

This solution was built on enitity framework data-first and a local sql express server

# SQL setup
In SSMS run the following query to build the database (ArcadeDiner), table(ReservationInfo), and seed a little data for this application.


IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'ArcadeDiner')
  BEGIN
    CREATE DATABASE [ArcadeDiner]
    END
    GO

USE [ArcadeDiner]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

IF OBJECT_ID(N'dbo.ReservationInfo', N'U') IS NOT NULL  
   DROP TABLE [dbo].[ReservationInfo];   

CREATE TABLE [dbo].[ReservationInfo](
	[ReservationId] [int] identity NOT NULL primary key,
	[PartyName] [nvarchar](50) NOT NULL,
	[NumberInParty] [int] NOT NULL,
	[ReservationDate] [date] NOT NULL,
	[ReservationTime] [time] NOT NULL,
	[ReservationNumber] [int] NOT NULL,
	[SubmissionDateTime] [datetime] NOT NULL,
	[Email] [nvarchar] (100) NOT NULL,
	[IsFulfilled] [bit] DEFAULT 0 NOT NULL,
	[LastUpdateDate] [datetime] DEFAULT GETDATE() NOT NULL
) ON [PRIMARY]
GO

INSERT INTO [dbo].[ReservationInfo] values 
('Thomas', 3, '12/09/2022', '6:00 PM', 1001, GETDATE(), 'thomasemail@yahoo.com',0, GETDATE()),
('Smith', 2, '12/05/2022', '8:00 PM', 1002, GETDATE(), 'smithemail@email.com', 0, GETDATE()),
('DeLeon', 5, '12/05/2022', '5:30 PM', 1003, GETDATE(), 'deleonemail@email.com', 0, GETDATE()),
('Armstrong', 7, '12/15/2022', '7:30 PM', 1004, GETDATE(), 'amstrong@email.com', 0,GETDATE())


# Sln Connection String
~/Data/ArcadeDinerContext.cs line 25 will need updating to point to your local sql instance replace YOUR-HOSTNAME with your local sql host name:
"Server={YOUR-HOSTNAME}\\SQLEXPRESS;Database=ArcadeDiner;TrustServerCertificate=True;Trusted_Connection=True;Encrypt=False"

It may be possible that you have to change your SQL server to mixed security (SQL Server and Windows Authentication Mode) if errors occur.
The Sln will run when opened and debugged in Visual Studio (I used Visual Studio 2022 and VS Code during the challenge).

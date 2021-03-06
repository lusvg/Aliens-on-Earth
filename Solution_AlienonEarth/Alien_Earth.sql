/****** Object:  Table [dbo].[Alien_Details]    Script Date: 07/29/2014 16:57:06 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Alien_Details]') AND type in (N'U'))
DROP TABLE [dbo].[Alien_Details]
GO
/****** Object:  Table [dbo].[Alien_Details]    Script Date: 07/29/2014 16:57:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Alien_Details]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Alien_Details](
	[S_No] [int] IDENTITY(1,1) NOT NULL,
	[Code_Name] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Blood_Color] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
	[Antenna] [int] NOT NULL,
	[No_Legs] [int] NOT NULL,
	[Home_Planet] [varchar](max) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL,
 CONSTRAINT [PK_Alien_Details] PRIMARY KEY CLUSTERED 
(
	[S_No] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON)
)
END
GO

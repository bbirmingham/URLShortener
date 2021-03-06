USE [TestProject]
GO
/****** Object:  Table [dbo].[URLs]    Script Date: 17/06/2021 21:50:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[URLs](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[URL] [nvarchar](max) NOT NULL,
	[URLShortened] [nchar](10) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  StoredProcedure [dbo].[AddURL]    Script Date: 17/06/2021 21:50:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Create the stored procedure in the specified schema
CREATE PROCEDURE [dbo].[AddURL]
    @url VARCHAR(MAX),
    @urlShortened VARCHAR(10)
-- add more stored procedure parameters here
AS
BEGIN
    -- Insert rows into table 'URLs' in schema '[dbo]'
    INSERT INTO [dbo].[URLs]
    ( -- Columns to insert data into
        URL, URLShortened
    )
    VALUES
    ( -- First row: values for the columns in the list above
        @url,
        @urlShortened
    )
END
GO
/****** Object:  StoredProcedure [dbo].[GetURL]    Script Date: 17/06/2021 21:50:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- Create the stored procedure in the specified schema
CREATE PROCEDURE [dbo].[GetURL]
    @urlShortened VARCHAR(10)
-- add more stored procedure parameters here
AS
BEGIN
    Select * From URLs
    Where URLShortened = @urlShortened
END
GO

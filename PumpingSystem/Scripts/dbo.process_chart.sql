CREATE TABLE [dbo].[process_chart]
(
	[process_chart_id] INT NOT NULL IDENTITY(1,1) PRIMARY KEY, 
    [creation_date] DATETIME NOT NULL, 
    [last_modified_date] DATETIME NOT NULL,
    [process_chart] VARBINARY(MAX) NOT NULL
)

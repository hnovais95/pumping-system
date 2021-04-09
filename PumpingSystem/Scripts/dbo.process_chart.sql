CREATE TABLE [dbo].[process_chart] (
    [process_chart_id]   BIGINT          NOT NULL	IDENTITY (1, 1) ,
    [creation_date]      DATETIME        NOT NULL,
    [process_chart]      VARBINARY (MAX) NOT NULL,
    PRIMARY KEY CLUSTERED ([process_chart_id] ASC)
);
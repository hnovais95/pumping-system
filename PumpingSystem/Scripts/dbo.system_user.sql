CREATE TABLE [dbo].[system_user] (
    [system_user_id] BIGINT        IDENTITY (1, 1) NOT NULL,
    [creation_date]  DATETIME      NOT NULL,
    [user_name]      NVARCHAR (20) NOT NULL,
    [password]       NVARCHAR (20) NOT NULL,
    PRIMARY KEY CLUSTERED ([system_user_id] ASC),
    UNIQUE NONCLUSTERED ([user_name] ASC)
);


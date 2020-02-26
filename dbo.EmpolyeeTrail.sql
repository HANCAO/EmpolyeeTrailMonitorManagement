CREATE TABLE [dbo].[EmpolyeeTrail] (
    [Id]             BIGINT            IDENTITY (1, 1) NOT NULL,
    [GPSX]           FLOAT           NULL,
    [GPSY]           FLOAT           NULL,
    [BmapLap]        FLOAT           NULL,
    [BmapLng]        FLOAT           NULL,
    [CreateTime]     DATETIME  NULL DEFAULT (getdate()),
    [CreateUser]     VARCHAR(20) NULL,
    [IsCar]          BIT            NULL DEFAULT ((0)),
    [Distance]       INT            NULL,
    [DistanceSecond] INT            NULL,
    CONSTRAINT [PK_EmpolyeeTrail] PRIMARY KEY CLUSTERED ([Id] ASC)
);


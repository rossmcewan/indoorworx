
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK7FDA1AFC888C8F93]') AND parent_object_id = OBJECT_ID('[Catalog]'))
alter table [Catalog]  drop constraint FK7FDA1AFC888C8F93


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK218B12D2D7098544]') AND parent_object_id = OBJECT_ID('[VideoReview]'))
alter table [VideoReview]  drop constraint FK218B12D2D7098544


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK218B12D2E9F4749E]') AND parent_object_id = OBJECT_ID('[VideoReview]'))
alter table [VideoReview]  drop constraint FK218B12D2E9F4749E


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK30300B57A4ECB12B]') AND parent_object_id = OBJECT_ID('[Video]'))
alter table [Video]  drop constraint FK30300B57A4ECB12B


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK9449533D1422C52E]') AND parent_object_id = OBJECT_ID('[TrainingSet]'))
alter table [TrainingSet]  drop constraint FK9449533D1422C52E


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK9449533D6669A625]') AND parent_object_id = OBJECT_ID('[TrainingSet]'))
alter table [TrainingSet]  drop constraint FK9449533D6669A625


    if exists (select * from dbo.sysobjects where id = object_id(N'[Catalog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Catalog]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Category]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Category]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Review]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Review]

    if exists (select * from dbo.sysobjects where id = object_id(N'[VideoReview]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [VideoReview]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Video]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Video]

    if exists (select * from dbo.sysobjects where id = object_id(N'[TrainingSet]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [TrainingSet]

    create table [Catalog] (
        Id UNIQUEIDENTIFIER not null,
       ImageUri NVARCHAR(255) null,
       Title NVARCHAR(255) not null,
       Description NVARCHAR(255) null,
       Sequence INT null,
       Category UNIQUEIDENTIFIER null,
       primary key (Id)
    )

    create table [Category] (
        Id UNIQUEIDENTIFIER not null,
       Description NVARCHAR(255) null,
       Title NVARCHAR(255) not null,
       Sequence INT null,
       primary key (Id)
    )

    create table [Review] (
        Id UNIQUEIDENTIFIER not null,
       Modified DATETIME null,
       ModifiedBy NVARCHAR(255) null,
       Created DATETIME null,
       CreatedBy NVARCHAR(255) null,
       Comment NVARCHAR(255) null,
       Title NVARCHAR(255) null,
       Rating INT null,
       primary key (Id)
    )

    create table [VideoReview] (
        Id UNIQUEIDENTIFIER not null,
       Video UNIQUEIDENTIFIER null,
       primary key (Id)
    )

    create table [Video] (
        Id UNIQUEIDENTIFIER not null,
       Title NVARCHAR(255) not null,
       Description NVARCHAR(255) null,
       Created DATETIME null,
       CreatedBy NVARCHAR(255) null,
       Modified DATETIME null,
       ModifiedBy NVARCHAR(255) null,
       Sequence INT null,
       ImageUri NVARCHAR(255) null,
       StreamUri NVARCHAR(255) not null,
       Duration BIGINT null,
       Catalog UNIQUEIDENTIFIER null,
       primary key (Id)
    )

    create table [TrainingSet] (
        Id UNIQUEIDENTIFIER not null,
       IntensityFactor DOUBLE PRECISION null,
       NormalizedPower DOUBLE PRECISION null,
       VariabilityIndex DOUBLE PRECISION null,
       AveragePower DOUBLE PRECISION null,
       TelemetryUri NVARCHAR(255) null,
       RecordingInterval DOUBLE PRECISION null,
       Parent UNIQUEIDENTIFIER null,
       primary key (Id)
    )

    alter table [Catalog] 
        add constraint FK7FDA1AFC888C8F93 
        foreign key (Category) 
        references [Category]

    alter table [VideoReview] 
        add constraint FK218B12D2D7098544 
        foreign key (Id) 
        references [Review]

    alter table [VideoReview] 
        add constraint FK218B12D2E9F4749E 
        foreign key (Video) 
        references [Video]

    alter table [Video] 
        add constraint FK30300B57A4ECB12B 
        foreign key (Catalog) 
        references [Catalog]

    alter table [TrainingSet] 
        add constraint FK9449533D1422C52E 
        foreign key (Id) 
        references [Video]

    alter table [TrainingSet] 
        add constraint FK9449533D6669A625 
        foreign key (Parent) 
        references [Video]

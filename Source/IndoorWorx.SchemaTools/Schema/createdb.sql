
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKAA25469B8F0EA360]') AND parent_object_id = OBJECT_ID('[Activity]'))
alter table [Activity]  drop constraint FKAA25469B8F0EA360


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKAA25469B7680667F]') AND parent_object_id = OBJECT_ID('[Activity]'))
alter table [Activity]  drop constraint FKAA25469B7680667F


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKAA25469B946CA9AD]') AND parent_object_id = OBJECT_ID('[Activity]'))
alter table [Activity]  drop constraint FKAA25469B946CA9AD


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKC29188BE7680667F]') AND parent_object_id = OBJECT_ID('ActivityTypesToEquipment'))
alter table ActivityTypesToEquipment  drop constraint FKC29188BE7680667F


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKC29188BE8F0EA360]') AND parent_object_id = OBJECT_ID('ActivityTypesToEquipment'))
alter table ActivityTypesToEquipment  drop constraint FKC29188BE8F0EA360


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK7FDA1AFC888C8F93]') AND parent_object_id = OBJECT_ID('[Catalog]'))
alter table [Catalog]  drop constraint FK7FDA1AFC888C8F93


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK58954903C74A7030]') AND parent_object_id = OBJECT_ID('[Equipment]'))
alter table [Equipment]  drop constraint FK58954903C74A7030


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKDA8429D43925FF00]') AND parent_object_id = OBJECT_ID('EquipmentToEquipmentFeatures'))
alter table EquipmentToEquipmentFeatures  drop constraint FKDA8429D43925FF00


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKDA8429D47680667F]') AND parent_object_id = OBJECT_ID('EquipmentToEquipmentFeatures'))
alter table EquipmentToEquipmentFeatures  drop constraint FKDA8429D47680667F


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK66ADF8866462D24C]') AND parent_object_id = OBJECT_ID('[Measurement]'))
alter table [Measurement]  drop constraint FK66ADF8866462D24C


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK66ADF8865428AC01]') AND parent_object_id = OBJECT_ID('[Measurement]'))
alter table [Measurement]  drop constraint FK66ADF8865428AC01


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK218B12D2D7098544]') AND parent_object_id = OBJECT_ID('[VideoReview]'))
alter table [VideoReview]  drop constraint FK218B12D2D7098544


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK218B12D2E9F4749E]') AND parent_object_id = OBJECT_ID('[VideoReview]'))
alter table [VideoReview]  drop constraint FK218B12D2E9F4749E


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK644A96ED670AA5CD]') AND parent_object_id = OBJECT_ID('[SocialMediaNotification]'))
alter table [SocialMediaNotification]  drop constraint FK644A96ED670AA5CD


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKF4EDF6D2F3F41700]') AND parent_object_id = OBJECT_ID('[SocialMediaProfile]'))
alter table [SocialMediaProfile]  drop constraint FKF4EDF6D2F3F41700


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKF4EDF6D2946CA9AD]') AND parent_object_id = OBJECT_ID('[SocialMediaProfile]'))
alter table [SocialMediaProfile]  drop constraint FKF4EDF6D2946CA9AD


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKC33B97A41906BB0A]') AND parent_object_id = OBJECT_ID('[TrainingZone]'))
alter table [TrainingZone]  drop constraint FKC33B97A41906BB0A


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKC33B97A44584847D]') AND parent_object_id = OBJECT_ID('[TrainingZone]'))
alter table [TrainingZone]  drop constraint FKC33B97A44584847D


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK30300B57A4ECB12B]') AND parent_object_id = OBJECT_ID('[Video]'))
alter table [Video]  drop constraint FK30300B57A4ECB12B


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK9449533D1422C52E]') AND parent_object_id = OBJECT_ID('[TrainingSet]'))
alter table [TrainingSet]  drop constraint FK9449533D1422C52E


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK9449533D6669A625]') AND parent_object_id = OBJECT_ID('[TrainingSet]'))
alter table [TrainingSet]  drop constraint FK9449533D6669A625


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKE82DEBED74BBB917]') AND parent_object_id = OBJECT_ID('[VideoText]'))
alter table [VideoText]  drop constraint FKE82DEBED74BBB917


    if exists (select * from dbo.sysobjects where id = object_id(N'[Activity]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Activity]

    if exists (select * from dbo.sysobjects where id = object_id(N'[ActivityType]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [ActivityType]

    if exists (select * from dbo.sysobjects where id = object_id(N'ActivityTypesToEquipment') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ActivityTypesToEquipment

    if exists (select * from dbo.sysobjects where id = object_id(N'[ApplicationUser]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [ApplicationUser]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Catalog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Catalog]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Category]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Category]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Colour]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Colour]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Equipment]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Equipment]

    if exists (select * from dbo.sysobjects where id = object_id(N'EquipmentToEquipmentFeatures') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table EquipmentToEquipmentFeatures

    if exists (select * from dbo.sysobjects where id = object_id(N'[EquipmentFeatures]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [EquipmentFeatures]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Manufacturer]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Manufacturer]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Measurement]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Measurement]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Review]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Review]

    if exists (select * from dbo.sysobjects where id = object_id(N'[VideoReview]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [VideoReview]

    if exists (select * from dbo.sysobjects where id = object_id(N'[SocialMediaNotification]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [SocialMediaNotification]

    if exists (select * from dbo.sysobjects where id = object_id(N'[SocialMediaProfile]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [SocialMediaProfile]

    if exists (select * from dbo.sysobjects where id = object_id(N'[SocialMediaType]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [SocialMediaType]

    if exists (select * from dbo.sysobjects where id = object_id(N'[TrainingZone]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [TrainingZone]

    if exists (select * from dbo.sysobjects where id = object_id(N'[UnitOfMeasure]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [UnitOfMeasure]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Video]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Video]

    if exists (select * from dbo.sysobjects where id = object_id(N'[TrainingSet]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [TrainingSet]

    if exists (select * from dbo.sysobjects where id = object_id(N'[VideoText]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [VideoText]

    create table [Activity] (
        Id UNIQUEIDENTIFIER not null,
       ActivityType_id UNIQUEIDENTIFIER not null,
       Equipment_id UNIQUEIDENTIFIER null,
       ApplicationUser_id NVARCHAR(255) null,
       primary key (Id)
    )

    create table [ActivityType] (
        Id UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) not null unique,
       primary key (Id)
    )

    create table ActivityTypesToEquipment (
        ActivityType_id UNIQUEIDENTIFIER not null,
       Equipment_id UNIQUEIDENTIFIER not null
    )

    create table [ApplicationUser] (
        Username NVARCHAR(255) not null,
       Firstname NVARCHAR(255) not null,
       Lastname NVARCHAR(255) not null,
       Gender NVARCHAR(255) not null,
       Email NVARCHAR(255) not null,
       Country NVARCHAR(255) not null,
       primary key (Username)
    )

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

    create table [Colour] (
        Id UNIQUEIDENTIFIER not null,
       Alpha TINYINT null,
       Red TINYINT null,
       Green TINYINT null,
       Blue TINYINT null,
       primary key (Id)
    )

    create table [Equipment] (
        Id UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) not null,
       Manufacturer_id UNIQUEIDENTIFIER null,
       primary key (Id)
    )

    create table EquipmentToEquipmentFeatures (
        Equipment_id UNIQUEIDENTIFIER not null,
       EquipmentFeatures_id UNIQUEIDENTIFIER not null
    )

    create table [EquipmentFeatures] (
        Id UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) not null unique,
       primary key (Id)
    )

    create table [Manufacturer] (
        Id UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) not null unique,
       primary key (Id)
    )

    create table [Measurement] (
        Id UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) not null unique,
       Value DOUBLE PRECISION not null unique,
       UnitOfMeasure_id UNIQUEIDENTIFIER not null,
       Activity_id UNIQUEIDENTIFIER null,
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

    create table [SocialMediaNotification] (
        Id UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) not null unique,
       SocialMediaProfile_id UNIQUEIDENTIFIER null,
       primary key (Id)
    )

    create table [SocialMediaProfile] (
        Id UNIQUEIDENTIFIER not null,
       Username NVARCHAR(255) null,
       Password NVARCHAR(255) null,
       SocialMediaType_id UNIQUEIDENTIFIER not null,
       ApplicationUser_id NVARCHAR(255) null,
       primary key (Id)
    )

    create table [SocialMediaType] (
        Id UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) not null unique,
       primary key (Id)
    )

    create table [TrainingZone] (
        Id UNIQUEIDENTIFIER not null,
       UpperValue DOUBLE PRECISION null,
       LowerValue DOUBLE PRECISION null,
       Name NVARCHAR(255) null,
       ColorRepresentation_id UNIQUEIDENTIFIER null,
       Measurement_id UNIQUEIDENTIFIER null,
       primary key (Id)
    )

    create table [UnitOfMeasure] (
        Id UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) not null unique,
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

    create table [VideoText] (
        Id UNIQUEIDENTIFIER not null,
       Animation NVARCHAR(255) null,
       Duration BIGINT null,
       MainText NVARCHAR(255) null,
       StartTime BIGINT null,
       SubText NVARCHAR(255) null,
       TrainingSet UNIQUEIDENTIFIER null,
       primary key (Id)
    )

    alter table [Activity] 
        add constraint FKAA25469B8F0EA360 
        foreign key (ActivityType_id) 
        references [ActivityType]

    alter table [Activity] 
        add constraint FKAA25469B7680667F 
        foreign key (Equipment_id) 
        references [Equipment]

    alter table [Activity] 
        add constraint FKAA25469B946CA9AD 
        foreign key (ApplicationUser_id) 
        references [ApplicationUser]

    alter table ActivityTypesToEquipment 
        add constraint FKC29188BE7680667F 
        foreign key (Equipment_id) 
        references [Equipment]

    alter table ActivityTypesToEquipment 
        add constraint FKC29188BE8F0EA360 
        foreign key (ActivityType_id) 
        references [ActivityType]

    alter table [Catalog] 
        add constraint FK7FDA1AFC888C8F93 
        foreign key (Category) 
        references [Category]

    alter table [Equipment] 
        add constraint FK58954903C74A7030 
        foreign key (Manufacturer_id) 
        references [Manufacturer]

    alter table EquipmentToEquipmentFeatures 
        add constraint FKDA8429D43925FF00 
        foreign key (EquipmentFeatures_id) 
        references [EquipmentFeatures]

    alter table EquipmentToEquipmentFeatures 
        add constraint FKDA8429D47680667F 
        foreign key (Equipment_id) 
        references [Equipment]

    alter table [Measurement] 
        add constraint FK66ADF8866462D24C 
        foreign key (UnitOfMeasure_id) 
        references [UnitOfMeasure]

    alter table [Measurement] 
        add constraint FK66ADF8865428AC01 
        foreign key (Activity_id) 
        references [Activity]

    alter table [VideoReview] 
        add constraint FK218B12D2D7098544 
        foreign key (Id) 
        references [Review]

    alter table [VideoReview] 
        add constraint FK218B12D2E9F4749E 
        foreign key (Video) 
        references [Video]

    alter table [SocialMediaNotification] 
        add constraint FK644A96ED670AA5CD 
        foreign key (SocialMediaProfile_id) 
        references [SocialMediaProfile]

    alter table [SocialMediaProfile] 
        add constraint FKF4EDF6D2F3F41700 
        foreign key (SocialMediaType_id) 
        references [SocialMediaType]

    alter table [SocialMediaProfile] 
        add constraint FKF4EDF6D2946CA9AD 
        foreign key (ApplicationUser_id) 
        references [ApplicationUser]

    alter table [TrainingZone] 
        add constraint FKC33B97A41906BB0A 
        foreign key (ColorRepresentation_id) 
        references [Colour]

    alter table [TrainingZone] 
        add constraint FKC33B97A44584847D 
        foreign key (Measurement_id) 
        references [Measurement]

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

    alter table [VideoText] 
        add constraint FKE82DEBED74BBB917 
        foreign key (TrainingSet) 
        references [TrainingSet]

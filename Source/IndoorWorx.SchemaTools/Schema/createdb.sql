
    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKAA25469B1EFA0600]') AND parent_object_id = OBJECT_ID('[Activity]'))
alter table [Activity]  drop constraint FKAA25469B1EFA0600


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKAA25469B52BE496C]') AND parent_object_id = OBJECT_ID('[Activity]'))
alter table [Activity]  drop constraint FKAA25469B52BE496C


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKE824B6D532B0BDDD]') AND parent_object_id = OBJECT_ID('ActivityType_Equipment'))
alter table ActivityType_Equipment  drop constraint FKE824B6D532B0BDDD


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKE824B6D51EFA0600]') AND parent_object_id = OBJECT_ID('ActivityType_Equipment'))
alter table ActivityType_Equipment  drop constraint FKE824B6D51EFA0600


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK4376B14876FFFECB]') AND parent_object_id = OBJECT_ID('[ApplicationUser]'))
alter table [ApplicationUser]  drop constraint FK4376B14876FFFECB


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK4376B1485124BE25]') AND parent_object_id = OBJECT_ID('[ApplicationUser]'))
alter table [ApplicationUser]  drop constraint FK4376B1485124BE25


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK4376B14820A42DDD]') AND parent_object_id = OBJECT_ID('[ApplicationUser]'))
alter table [ApplicationUser]  drop constraint FK4376B14820A42DDD


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKAD938CC889F7ABF4]') AND parent_object_id = OBJECT_ID('WidgetToApplicationUser'))
alter table WidgetToApplicationUser  drop constraint FKAD938CC889F7ABF4


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKAD938CC8BCD0E4BF]') AND parent_object_id = OBJECT_ID('WidgetToApplicationUser'))
alter table WidgetToApplicationUser  drop constraint FKAD938CC8BCD0E4BF


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK7FDA1AFC888C8F93]') AND parent_object_id = OBJECT_ID('[Catalog]'))
alter table [Catalog]  drop constraint FK7FDA1AFC888C8F93


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK58954903EE4EEE93]') AND parent_object_id = OBJECT_ID('[Equipment]'))
alter table [Equipment]  drop constraint FK58954903EE4EEE93


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK589549032FF0048C]') AND parent_object_id = OBJECT_ID('[Equipment]'))
alter table [Equipment]  drop constraint FK589549032FF0048C


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK76FBBBA858C3A259]') AND parent_object_id = OBJECT_ID('Equipment_EquipmentFeatures'))
alter table Equipment_EquipmentFeatures  drop constraint FK76FBBBA858C3A259


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK76FBBBA832B0BDDD]') AND parent_object_id = OBJECT_ID('Equipment_EquipmentFeatures'))
alter table Equipment_EquipmentFeatures  drop constraint FK76FBBBA832B0BDDD


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK66ADF886BFB37A41]') AND parent_object_id = OBJECT_ID('[Measurement]'))
alter table [Measurement]  drop constraint FK66ADF886BFB37A41


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK66ADF88611C9D409]') AND parent_object_id = OBJECT_ID('[Measurement]'))
alter table [Measurement]  drop constraint FK66ADF88611C9D409


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK218B12D2D7098544]') AND parent_object_id = OBJECT_ID('[VideoReview]'))
alter table [VideoReview]  drop constraint FK218B12D2D7098544


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK218B12D2E9F4749E]') AND parent_object_id = OBJECT_ID('[VideoReview]'))
alter table [VideoReview]  drop constraint FK218B12D2E9F4749E


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKF4EDF6D218E054F6]') AND parent_object_id = OBJECT_ID('[SocialMediaProfile]'))
alter table [SocialMediaProfile]  drop constraint FKF4EDF6D218E054F6


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKF4EDF6D2912B3D27]') AND parent_object_id = OBJECT_ID('[SocialMediaProfile]'))
alter table [SocialMediaProfile]  drop constraint FKF4EDF6D2912B3D27


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK1C5DD9686EDD5235]') AND parent_object_id = OBJECT_ID('SocialMediaProfile_SocialMediaNotifications'))
alter table SocialMediaProfile_SocialMediaNotifications  drop constraint FK1C5DD9686EDD5235


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK1C5DD9688F98D97D]') AND parent_object_id = OBJECT_ID('SocialMediaProfile_SocialMediaNotifications'))
alter table SocialMediaProfile_SocialMediaNotifications  drop constraint FK1C5DD9688F98D97D


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKE20BB28CD299260D]') AND parent_object_id = OBJECT_ID('[SportingHabits]'))
alter table [SportingHabits]  drop constraint FKE20BB28CD299260D


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKE20BB28C25951F24]') AND parent_object_id = OBJECT_ID('[SportingHabits]'))
alter table [SportingHabits]  drop constraint FKE20BB28C25951F24


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKE20BB28CAE3CB57]') AND parent_object_id = OBJECT_ID('[SportingHabits]'))
alter table [SportingHabits]  drop constraint FKE20BB28CAE3CB57


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK7141095EFA202CA7]') AND parent_object_id = OBJECT_ID('SportingHabits_Sport'))
alter table SportingHabits_Sport  drop constraint FK7141095EFA202CA7


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FK7141095E9EFA6CD7]') AND parent_object_id = OBJECT_ID('SportingHabits_Sport'))
alter table SportingHabits_Sport  drop constraint FK7141095E9EFA6CD7


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKC33B97A4263AB471]') AND parent_object_id = OBJECT_ID('[TrainingZone]'))
alter table [TrainingZone]  drop constraint FKC33B97A4263AB471


    if exists (select 1 from sys.objects where object_id = OBJECT_ID(N'[FKC33B97A49B4FD2BF]') AND parent_object_id = OBJECT_ID('[TrainingZone]'))
alter table [TrainingZone]  drop constraint FKC33B97A49B4FD2BF


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

    if exists (select * from dbo.sysobjects where id = object_id(N'ActivityType_Equipment') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table ActivityType_Equipment

    if exists (select * from dbo.sysobjects where id = object_id(N'[ApplicationUser]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [ApplicationUser]

    if exists (select * from dbo.sysobjects where id = object_id(N'WidgetToApplicationUser') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table WidgetToApplicationUser

    if exists (select * from dbo.sysobjects where id = object_id(N'[Catalog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Catalog]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Category]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Category]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Colour]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Colour]

    if exists (select * from dbo.sysobjects where id = object_id(N'[CompetitiveLevel]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [CompetitiveLevel]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Equipment]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Equipment]

    if exists (select * from dbo.sysobjects where id = object_id(N'Equipment_EquipmentFeatures') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table Equipment_EquipmentFeatures

    if exists (select * from dbo.sysobjects where id = object_id(N'[EquipmentFeatures]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [EquipmentFeatures]

    if exists (select * from dbo.sysobjects where id = object_id(N'[IndoorTrainingFrequency]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [IndoorTrainingFrequency]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Manufacturer]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Manufacturer]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Measurement]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Measurement]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Occupation]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Occupation]

    if exists (select * from dbo.sysobjects where id = object_id(N'[ReferralSource]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [ReferralSource]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Review]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Review]

    if exists (select * from dbo.sysobjects where id = object_id(N'[VideoReview]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [VideoReview]

    if exists (select * from dbo.sysobjects where id = object_id(N'[SocialMediaNotification]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [SocialMediaNotification]

    if exists (select * from dbo.sysobjects where id = object_id(N'[SocialMediaProfile]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [SocialMediaProfile]

    if exists (select * from dbo.sysobjects where id = object_id(N'SocialMediaProfile_SocialMediaNotifications') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table SocialMediaProfile_SocialMediaNotifications

    if exists (select * from dbo.sysobjects where id = object_id(N'[SocialMediaType]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [SocialMediaType]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Sport]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Sport]

    if exists (select * from dbo.sysobjects where id = object_id(N'[SportingHabits]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [SportingHabits]

    if exists (select * from dbo.sysobjects where id = object_id(N'SportingHabits_Sport') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table SportingHabits_Sport

    if exists (select * from dbo.sysobjects where id = object_id(N'[TrainingVolume]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [TrainingVolume]

    if exists (select * from dbo.sysobjects where id = object_id(N'[TrainingZone]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [TrainingZone]

    if exists (select * from dbo.sysobjects where id = object_id(N'[UnitOfMeasure]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [UnitOfMeasure]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Video]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Video]

    if exists (select * from dbo.sysobjects where id = object_id(N'[TrainingSet]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [TrainingSet]

    if exists (select * from dbo.sysobjects where id = object_id(N'[VideoText]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [VideoText]

    if exists (select * from dbo.sysobjects where id = object_id(N'[Widget]') and OBJECTPROPERTY(id, N'IsUserTable') = 1) drop table [Widget]

    create table [Activity] (
        Id UNIQUEIDENTIFIER not null,
       ActivityType UNIQUEIDENTIFIER not null,
       Activity NVARCHAR(255) null,
       primary key (Id)
    )

    create table [ActivityType] (
        Id UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) not null unique,
       IsActive BIT null,
       primary key (Id)
    )

    create table ActivityType_Equipment (
        ActivityType UNIQUEIDENTIFIER not null,
       Equipment UNIQUEIDENTIFIER not null
    )

    create table [ApplicationUser] (
        Username NVARCHAR(255) not null,
       Firstname NVARCHAR(255) not null,
       Lastname NVARCHAR(255) not null,
       Gender NVARCHAR(255) not null,
       About NVARCHAR(255) null,
       Email NVARCHAR(255) not null,
       Country NVARCHAR(255) not null,
       Occupation_id UNIQUEIDENTIFIER null,
       ReferralSource_id UNIQUEIDENTIFIER null,
       SportingHabits_id UNIQUEIDENTIFIER null,
       primary key (Username)
    )

    create table WidgetToApplicationUser (
        ApplicationUser NVARCHAR(255) not null,
       Widget UNIQUEIDENTIFIER not null
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

    create table [CompetitiveLevel] (
        Id UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) not null,
       IsActive BIT not null,
       primary key (Id)
    )

    create table [Equipment] (
        Id UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) not null,
       IsActive BIT null,
       Manufacturer UNIQUEIDENTIFIER null,
       Equipment UNIQUEIDENTIFIER null,
       primary key (Id)
    )

    create table Equipment_EquipmentFeatures (
        Equipment UNIQUEIDENTIFIER not null,
       EquipmentFeatures UNIQUEIDENTIFIER not null
    )

    create table [EquipmentFeatures] (
        Id UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) not null unique,
       IsActive BIT null,
       primary key (Id)
    )

    create table [IndoorTrainingFrequency] (
        Id UNIQUEIDENTIFIER not null,
       Description NVARCHAR(255) not null,
       IsActive BIT not null,
       primary key (Id)
    )

    create table [Manufacturer] (
        Id UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) not null unique,
       IsActive BIT null,
       primary key (Id)
    )

    create table [Measurement] (
        Id UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) not null unique,
       Value DOUBLE PRECISION not null unique,
       IsActive BIT null,
       UnitOfMeasure UNIQUEIDENTIFIER not null,
       Measurement UNIQUEIDENTIFIER null,
       primary key (Id)
    )

    create table [Occupation] (
        Id UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) not null,
       IsActive BIT not null,
       primary key (Id)
    )

    create table [ReferralSource] (
        Id UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) not null,
       Description NVARCHAR(255) not null,
       IsActive BIT not null,
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
       IsActive BIT null,
       primary key (Id)
    )

    create table [SocialMediaProfile] (
        Id UNIQUEIDENTIFIER not null,
       Username NVARCHAR(255) null,
       Password NVARCHAR(255) null,
       SocialMediaType UNIQUEIDENTIFIER not null,
       SocialProfile NVARCHAR(255) null,
       primary key (Id)
    )

    create table SocialMediaProfile_SocialMediaNotifications (
        SocialMediaProfile UNIQUEIDENTIFIER not null,
       SocialMediaNotifications UNIQUEIDENTIFIER not null
    )

    create table [SocialMediaType] (
        Id UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) not null unique,
       IsActive BIT null,
       primary key (Id)
    )

    create table [Sport] (
        Id UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) not null,
       IsActive BIT not null,
       primary key (Id)
    )

    create table [SportingHabits] (
        Id UNIQUEIDENTIFIER not null,
       CompetitiveLevel_id UNIQUEIDENTIFIER null,
       TrainingVolume_id UNIQUEIDENTIFIER null,
       IndoorTrainingFrequency_id UNIQUEIDENTIFIER null,
       primary key (Id)
    )

    create table SportingHabits_Sport (
        SportingHabits UNIQUEIDENTIFIER not null,
       Sport UNIQUEIDENTIFIER not null
    )

    create table [TrainingVolume] (
        Id UNIQUEIDENTIFIER not null,
       Description NVARCHAR(255) not null,
       IsActive BIT not null,
       primary key (Id)
    )

    create table [TrainingZone] (
        Id UNIQUEIDENTIFIER not null,
       UpperValue DOUBLE PRECISION null,
       LowerValue DOUBLE PRECISION null,
       Name NVARCHAR(255) null,
       Color UNIQUEIDENTIFIER null,
       TrainingZone UNIQUEIDENTIFIER null,
       primary key (Id)
    )

    create table [UnitOfMeasure] (
        Id UNIQUEIDENTIFIER not null,
       Name NVARCHAR(255) not null unique,
       IsActive BIT null,
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

    create table [Widget] (
        Id UNIQUEIDENTIFIER not null,
       Image NVARCHAR(255) null,
       Title NVARCHAR(255) null,
       ContentRenderer NVARCHAR(255) null,
       primary key (Id)
    )

    alter table [Activity] 
        add constraint FKAA25469B1EFA0600 
        foreign key (ActivityType) 
        references [ActivityType]

    alter table [Activity] 
        add constraint FKAA25469B52BE496C 
        foreign key (Activity) 
        references [ApplicationUser]

    alter table ActivityType_Equipment 
        add constraint FKE824B6D532B0BDDD 
        foreign key (Equipment) 
        references [Equipment]

    alter table ActivityType_Equipment 
        add constraint FKE824B6D51EFA0600 
        foreign key (ActivityType) 
        references [ActivityType]

    alter table [ApplicationUser] 
        add constraint FK4376B14876FFFECB 
        foreign key (Occupation_id) 
        references [Occupation]

    alter table [ApplicationUser] 
        add constraint FK4376B1485124BE25 
        foreign key (ReferralSource_id) 
        references [ReferralSource]

    alter table [ApplicationUser] 
        add constraint FK4376B14820A42DDD 
        foreign key (SportingHabits_id) 
        references [SportingHabits]

    alter table WidgetToApplicationUser 
        add constraint FKAD938CC889F7ABF4 
        foreign key (Widget) 
        references [Widget]

    alter table WidgetToApplicationUser 
        add constraint FKAD938CC8BCD0E4BF 
        foreign key (ApplicationUser) 
        references [ApplicationUser]

    alter table [Catalog] 
        add constraint FK7FDA1AFC888C8F93 
        foreign key (Category) 
        references [Category]

    alter table [Equipment] 
        add constraint FK58954903EE4EEE93 
        foreign key (Manufacturer) 
        references [Manufacturer]

    alter table [Equipment] 
        add constraint FK589549032FF0048C 
        foreign key (Equipment) 
        references [Activity]

    alter table Equipment_EquipmentFeatures 
        add constraint FK76FBBBA858C3A259 
        foreign key (EquipmentFeatures) 
        references [EquipmentFeatures]

    alter table Equipment_EquipmentFeatures 
        add constraint FK76FBBBA832B0BDDD 
        foreign key (Equipment) 
        references [Equipment]

    alter table [Measurement] 
        add constraint FK66ADF886BFB37A41 
        foreign key (UnitOfMeasure) 
        references [UnitOfMeasure]

    alter table [Measurement] 
        add constraint FK66ADF88611C9D409 
        foreign key (Measurement) 
        references [Activity]

    alter table [VideoReview] 
        add constraint FK218B12D2D7098544 
        foreign key (Id) 
        references [Review]

    alter table [VideoReview] 
        add constraint FK218B12D2E9F4749E 
        foreign key (Video) 
        references [Video]

    alter table [SocialMediaProfile] 
        add constraint FKF4EDF6D218E054F6 
        foreign key (SocialMediaType) 
        references [SocialMediaType]

    alter table [SocialMediaProfile] 
        add constraint FKF4EDF6D2912B3D27 
        foreign key (SocialProfile) 
        references [ApplicationUser]

    alter table SocialMediaProfile_SocialMediaNotifications 
        add constraint FK1C5DD9686EDD5235 
        foreign key (SocialMediaNotifications) 
        references [SocialMediaNotification]

    alter table SocialMediaProfile_SocialMediaNotifications 
        add constraint FK1C5DD9688F98D97D 
        foreign key (SocialMediaProfile) 
        references [SocialMediaProfile]

    alter table [SportingHabits] 
        add constraint FKE20BB28CD299260D 
        foreign key (CompetitiveLevel_id) 
        references [CompetitiveLevel]

    alter table [SportingHabits] 
        add constraint FKE20BB28C25951F24 
        foreign key (TrainingVolume_id) 
        references [TrainingVolume]

    alter table [SportingHabits] 
        add constraint FKE20BB28CAE3CB57 
        foreign key (IndoorTrainingFrequency_id) 
        references [IndoorTrainingFrequency]

    alter table SportingHabits_Sport 
        add constraint FK7141095EFA202CA7 
        foreign key (Sport) 
        references [Sport]

    alter table SportingHabits_Sport 
        add constraint FK7141095E9EFA6CD7 
        foreign key (SportingHabits) 
        references [SportingHabits]

    alter table [TrainingZone] 
        add constraint FKC33B97A4263AB471 
        foreign key (Color) 
        references [Colour]

    alter table [TrainingZone] 
        add constraint FKC33B97A49B4FD2BF 
        foreign key (TrainingZone) 
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

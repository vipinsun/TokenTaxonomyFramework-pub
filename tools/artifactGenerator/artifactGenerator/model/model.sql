create table ArtifactContent
(
    Id          int          not null
        constraint ArtifactContent_pk
            primary key nonclustered,
    ContentType varchar(255) not null
)
go

create table ArtifactType
(
    Id   int          not null
        constraint ArtifactType_pk
            primary key nonclustered,
    Type varchar(255) not null
)
go

create table MappingType
(
    Id   int          not null
        constraint MappingType_pk
            primary key nonclustered,
    Type varchar(255) not null
)
go

create table TargetPlatform
(
    Id   int          not null
        constraint TargetPlatform_pk
            primary key nonclustered,
    Name varchar(255) not null
)
go

create table TokenFormula
(
    Id   int          not null
        constraint TokenFormula_pk
            primary key nonclustered,
    Type varchar(255) not null
)
go

create table TokenType
(
    Id   int          not null
        constraint TokenType_pk
            primary key nonclustered,
    Type varchar(255) not null
)
go

create table Artifacts
(
    Id           uniqueidentifier default newid() not null
        constraint Artifact_pk
            primary key nonclustered,
    ArtifactType int                              not null default 0
        constraint Artifact_ArtifactType_id_fk
            references ArtifactType,
    Name         varchar(255)                     not null,
    ControlUri      varchar(255)                not null      default ''
)
go

create table ArtifactAliases
(
    ArtifactId           uniqueidentifier       not null
        constraint ArtifactAliases_Artifact_id_fk
            references Artifacts,
    Name         varchar(255)                   not null
)
go

create table ArtifactSymbols
(
    ArtifactId           uniqueidentifier       not null
        constraint ArtifactSymbols_Artifact_id_fk
            references Artifacts,
    VisualSymbol         varchar(255)                   not null,
    ToolingSymbol         varchar(255)                   not null
)
go

create table ArtifactDefinitions
(
    ArtifactId           uniqueidentifier       not null
        constraint ArtifactDefinitions_Artifact_id_fk
            references Artifacts,
    BusinessDescription         varchar(MAX)                   not null,
    BusinessExample         varchar(MAX)                   not null default '',
)
go

create table ArtifactAnalogies
(
    ArtifactId           uniqueidentifier       not null
        constraint ArtifactAnalogies_Artifact_id_fk
            references Artifacts,
    Name         varchar(255)                   not null,
    Description  varchar(MAX)       not null
)
go

create table ArtifactFiles
(
    ArtifactId           uniqueidentifier       not null
        constraint ArtifactFiles_Artifact_id_fk
            references Artifacts,
    ArtifactContent int not null
        constraint ArtifactFiles_ArtifactContent_id_fk
            references ArtifactContent,
    FileName         varchar(255)                   not null,
    FileData  varbinary      default 0 not null,
)
go

create TABLE Maps
(
    Id  UNIQUEIDENTIFIER not null default newid()
        constraint Maps_pk
            primary key nonclustered,
    ArtifactId UNIQUEIDENTIFIER not null
        constraint Maps_Artifacts_Id_fk
            references Artifacts
)

create table MapReference
(
    MapId           uniqueidentifier       not null
        constraint MapReference_Maps_Id_fk
            references Maps,
    MappingType int not null default 0
        constraint MapReference_MappingType_Id_fk
            references MappingType,
    Name         varchar(255)                   not null,
    TargetPlatform  int       not null default 0
        constraint MapReference_TargetPlatform_Id_fk
            references TargetPlatform,
    ReferencePath   varchar(1024)   not null default ''
)
go

create table MapResourceReference
(
    MapId           uniqueidentifier       not null
        constraint MapResourceReference_Maps_Id_fk
            references Maps,
    MappingType int not null default 0
        constraint MapResourceReference_MappingType_Id_fk
            references MappingType,
    Name         varchar(255)                   not null,
    Description  varchar(MAX)       not null default '',
    ResourcePath   varchar(1024)   not null default ''
)
go


create table Bases
(
    Id           uniqueidentifier default newid() not null
        constraint Bases_pk
            primary key nonclustered,
    Artifact uniqueidentifier                              not null
        constraint Bases_Artifacts_Id_fk
            references Artifacts,
    TokenType int                              not null default 0
        constraint Bases_TokenType_Id_fk
            references TokenType,
    TokenFormula int                              not null default 0
        constraint Bases_TokenFormula_Id_fk
            references TokenFormula,
    Name         varchar(255)                     not null,
    Symbol      varchar(10)                not null      default '',
    Owner       varchar(255)            not null  default '',
    Quantity    varchar(255)            not null default '',
    Decimals    int         not null default 0,
    ConstructorName varchar(255)    not null default '',
    Constructor varbinary   not null default 0
)
go

create table BaseProperties
(
    BaseId           uniqueidentifier       not null
        constraint BaseProperties_Bases_id_fk
            references Bases,
    Name         varchar(255)                   not null,
    Value  varchar(255)      default '' not null,
)
go

create table Behaviors
(
    Id           uniqueidentifier default newid() not null
        constraint Behaviors_pk
            primary key nonclustered,
    Artifact uniqueidentifier                              not null
        constraint Behaviors_Artifacts_Id_fk
            references Artifacts,
    External         bit                   not null default 0,
    Value  varchar(255)      default '' not null,
)
go
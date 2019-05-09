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
    ArtifactId uniqueidentifier                              not null
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
    ArtifactId uniqueidentifier                              not null
        constraint Behaviors_Artifacts_Id_fk
            references Artifacts,
    IsExternal         bit                   not null default 0,
    Constructor  varbinary      default 0 not null,
)
go

create table BehaviorGroups
(
    Id           uniqueidentifier default newid() not null
        constraint BehaviorGroups_pk
            primary key nonclustered,
    ArtifactId uniqueidentifier                              not null
        constraint BehaviorGroups_Artifacts_Id_fk
            references Artifacts
)
go

create table BehaviorGroupBehaviors
(
    BehaviorGroupId           uniqueidentifier not null
        constraint BehaviorGroupBehaviors_BehaviorGroups_Id_fk
            references BehaviorGroups,
    BehaviorId uniqueidentifier                              not null
        constraint BehaviorGroupBehaviors_Behaviors_Id_fk
            references Behaviors
)
go

create table Invocations
(
    Id           uniqueidentifier default newid() not null
        constraint Invocations_pk
            primary key nonclustered,
    BehaviorId uniqueidentifier                              not null
        constraint Invocations_Behaviors_Id_fk
            references Behaviors,
    Name         varchar(255)                   not null,
    Description  varchar(1024)      default '' not null,
)
go

create table InvocationRequests
(
    Id           uniqueidentifier default newid() not null
        constraint InvocationRequests_pk
            primary key nonclustered,
    InvocationId           uniqueidentifier  not null
        constraint InvocationRequests_Invocations_Id_fk
            references Invocations,
    ControlMessageName         varchar(255)                   not null,
    Description  varchar(1024)      default '' not null
    
)
go

create table InvocationRequestParameters
(
    InvocationRequestId           uniqueidentifier  not null
        constraint InvocationRequestParameters_InvocationRequests_Id_fk
            references InvocationRequests,
    Name         varchar(255)                   not null,
    ValueDescription  varchar(1024)      default '' not null

)
go

create table InvocationResponses
(
    Id           uniqueidentifier default newid() not null
        constraint InvocationResponses_pk
            primary key nonclustered,
    InvocationId           uniqueidentifier  not null
        constraint InvocationResponses_Invocations_Id_fk
            references Invocations,
    ControlMessageName         varchar(255)                   not null,
    Description  varchar(1024)      default '' not null
)
go

create table InvocationResponsesParameters
(
    InvocationResponseId           uniqueidentifier  not null
        constraint InvocationResponsesParameters_InvocationResponses_Id_fk
            references InvocationResponses,
    Name         varchar(255)                   not null,
    ValueDescription  varchar(1024)      default '' not null

)
go

create table PropertySets
(
    Id           uniqueidentifier default newid() not null
        constraint PropertySets_pk
            primary key nonclustered,
    ArtifactId           uniqueidentifier  not null
        constraint PropertySets_Artifacts_Id_fk
            references Artifacts
)
go

create table Properties
(
    Id           uniqueidentifier default newid() not null
        constraint Properties_pk
            primary key nonclustered,
    PropertySetId           uniqueidentifier  not null
        constraint Properties_PropertySets_Id_fk
            references PropertySets,
    Name    varchar(255) not null,
    ValueDescription varchar(1024) not null default ''
)
go

create table PropertyInvocations
(
    Id           uniqueidentifier default newid() not null
        constraint PropertyInvocations_pk
            primary key nonclustered,
    PropertyId uniqueidentifier                              not null
        constraint PropertyInvocations_Properties_Id_fk
            references Properties,
    Name         varchar(255)                   not null,
    Description  varchar(1024)      default '' not null,
)
go

create table PropertyInvocationRequests
(
    Id           uniqueidentifier default newid() not null
        constraint PropertyInvocationRequests_pk
            primary key nonclustered,
    PropertyInvocationId           uniqueidentifier  not null
        constraint PropertyInvocationRequests_PropertyInvocations_Id_fk
            references PropertyInvocations,
    ControlMessageName         varchar(255)                   not null,
    Description  varchar(1024)      default '' not null

)
go

create table PropertyInvocationRequestParameters
(
    PropertyInvocationRequestId           uniqueidentifier  not null
        constraint PropertyInvocationRequestParameters_PropertyInvocationRequests_Id_fk
            references PropertyInvocationRequests,
    Name         varchar(255)                   not null,
    ValueDescription  varchar(1024)      default '' not null

)
go

create table PropertyInvocationResponses
(
    Id           uniqueidentifier default newid() not null
        constraint PropertyInvocationResponses_pk
            primary key nonclustered,
    PropertyInvocationId           uniqueidentifier  not null
        constraint PropertyInvocationResponses_PropertyInvocations_Id_fk
            references PropertyInvocations,
    ControlMessageName         varchar(255)                   not null,
    Description  varchar(1024)      default '' not null
)
go

create table PropertyInvocationResponsesParameters
(
    PropertyInvocationResponseId           uniqueidentifier  not null
        constraint PropertyInvocationResponsesParameters_PropertyInvocationResponses_Id_fk
            references PropertyInvocationResponses,
    Name         varchar(255)                   not null,
    ValueDescription  varchar(1024)      default '' not null

)
go

create table TokenTemplates
(
    Id           uniqueidentifier default newid() not null
        constraint TokenTemplates_pk
            primary key nonclustered,    
    ArtifactId uniqueidentifier                              not null
    constraint TokenTemplates_Artifacts_Id_fk
        references Artifacts,
    BaseId              uniqueidentifier    not null
        constraint TokenTemplates_Bases_Id_fk
            references Bases
)
go

create table TokenTemplateBehaviors
(
    TokenTemplateId           uniqueidentifier  not null
        constraint TokenTemplateBehaviors_TokenTemplates_Id_fk
            references TokenTemplates,
    BehaviorId           uniqueidentifier  not null
        constraint TokenTemplateBehaviors_Behaviors_Id_fk
            references Behaviors
)
go

create table TokenTemplateBehaviorGroups
(
    TokenTemplateId           uniqueidentifier  not null
        constraint TokenTemplateBehaviorGroups_TokenTemplates_Id_fk
            references TokenTemplates,
    BehaviorGroupId           uniqueidentifier  not null
        constraint TokenTemplateBehaviorGroups_BehaviorGroups_Id_fk
            references BehaviorGroups
)
go

create table TokenTemplatePropertySets
(
    TokenTemplateId           uniqueidentifier  not null
        constraint TokenTemplatePropertySets_TokenTemplates_Id_fk
            references TokenTemplates,
    PropertySetId           uniqueidentifier  not null
        constraint TokenTemplatePropertySets_PropertySets_Id_fk
            references PropertySets
)
go


create table SingleToken
(
    Id           uniqueidentifier default newid() not null
        constraint SingleToken_pk
            primary key nonclustered,
    GroupStart         varchar(2)                   default '(' not null,
    BaseId              uniqueidentifier    not null
        constraint SingleToken_Bases_Id_fk
            references Bases    
)
go

create table BehaviorLists
(
    SingleTokenId           uniqueidentifier  not null
        constraint BehaviorLists_SingleToken_Id_fk
            references SingleToken,
    BehaviorId           uniqueidentifier  not null
        constraint BehaviorLists_Behaviors_Id_fk
            references Behaviors
)
go

create table BehaviorGroupLists
(
    SingleTokenId           uniqueidentifier  not null
        constraint BehaviorGroupLists_SingleToken_Id_fk
            references SingleToken,
    BehaviorGroupId           uniqueidentifier  not null
        constraint BehaviorGroupLists_Behaviors_Id_fk
            references BehaviorGroups
)
go

create table PropertySetList
(
    Id           uniqueidentifier default newid() not null
        constraint PropertySetList_pk
            primary key nonclustered,
    SingleTokenId           uniqueidentifier  not null
        constraint PropertySetList_SingleToken_Id_fk
            references SingleToken
)
go

create table PropertySetItems
(
    PropertySetListId           uniqueidentifier  not null
        constraint PropertySetItems_PropertySetList_Id_fk
            references PropertySetList,
    ListStart         char(1)               default '+'   not null,
    PropertySetId   uniqueidentifier not null
        constraint PropertySetItems_PropertySet_Id_fk
            references PropertySets
)
go

create table HybridTokenFormula
(
    Id           uniqueidentifier default newid() not null
        constraint HybridTokenFormula_pk
            primary key nonclustered,
    ParentTokenId         uniqueidentifier                   not null
        constraint HybridTokenFormula_SingleToken_Id_fk
            references SingleToken,
    ChildrenStart              char(1)    not null default '(',
    ChildrenEnd              char(1)    not null default ')',
)

create table HybridTokenFormulaChildToken
(
    HybridTokenFormulaId           uniqueidentifier not null
        constraint HybridTokenFormulaChildToken_HybridTokenFormula_Id_fk
        references HybridTokenFormula,
    ChildTokenId           uniqueidentifier not null
        constraint HybridTokenFormulaChildToken_SingleToken_Id_fk
            references SingleToken,
)

create table HybridTokenWithHybridChildrenFormula
(
    Id           uniqueidentifier default newid() not null
        constraint HybridTokenWithHybridChildrenFormula_pk
            primary key nonclustered,
    ParentTokenId         uniqueidentifier                   not null
        constraint HybridTokenWithHybridChildrenFormula_SingleToken_Id_fk
            references SingleToken,
    HybridChildrenStart              char(1)    not null default '(',
    HybridChildrenEnd              char(1)    not null default ')',
)

create table HybridTokenFormulaHybridChildToken
(
    HybridTokenWithHybridChildrenFormulaId           uniqueidentifier not null
        constraint HybridTokenFormulaHybridChildToken_HybridTokenWithHybridChildrenFormula_Id_fk
            references HybridTokenWithHybridChildrenFormula,
    HybridTokenId           uniqueidentifier not null
        constraint HybridTokenFormulaHybridChildToken_HybridToken_Id_fk
            references HybridTokenFormula
)

create table FormulaGrammar
(
    Id           uniqueidentifier default newid() not null
        constraint FormulaGrammar_pk
            primary key nonclustered,
    SingleTokenId           uniqueidentifier  not null
    constraint FormulaGrammar_SingleToken_Id_fk
    references SingleToken,
    HybridTokenFormulaId           uniqueidentifier  not null
        constraint FormulaGrammar_HybridTokenFormula_Id_fk
            references HybridTokenFormula,
    HybridTokenWithHybridChildrenFormulaId           uniqueidentifier  not null
        constraint FormulaGrammar_HybridTokenWithHybridChildrenFormula_Id_fk
            references HybridTokenWithHybridChildrenFormula
)
go

create table TaxonomyVersion
(
    Id           uniqueidentifier default newid() not null
        constraint TaxonomyVersion_pk
            primary key nonclustered,
    Version         varchar(255)                   not null
)
go

create table TaxonomyBaseTokenTypes
(
    TaxonomyVersionId           uniqueidentifier not null
        constraint TaxonomyBaseTokenTypes_TaxonomyVersion_Id_fk
            references TaxonomyVersion,
    BaseId           uniqueidentifier not null
        constraint TaxonomyBaseTokenTypes_Bases_Id_fk
            references Bases
)
go

create table TaxonomyBehaviors
(
    TaxonomyVersionId           uniqueidentifier not null
        constraint TaxonomyBehaviors_TaxonomyVersion_Id_fk
            references TaxonomyVersion,
    BehaviorId           uniqueidentifier not null
        constraint TaxonomyBehaviors_Behaviors_Id_fk
            references Behaviors
)
go

create table TaxonomyBehaviorGroups
(
    TaxonomyVersionId           uniqueidentifier not null
        constraint TaxonomyBehaviorGroups_TaxonomyVersion_Id_fk
            references TaxonomyVersion,
    BehaviorId           uniqueidentifier not null
        constraint TaxonomyBehaviorGroups_BehaviorGroups_Id_fk
            references BehaviorGroups
)
go

create table TaxonomyPropertySets
(
    TaxonomyVersionId           uniqueidentifier not null
        constraint TaxonomyPropertySets_TaxonomyVersion_Id_fk
            references TaxonomyVersion,
    PropertySetId           uniqueidentifier not null
        constraint TaxonomyPropertySets_PropertySets_Id_fk
            references PropertySets
)
go

create table TaxonomyTokenTemplates
(
    TaxonomyVersionId           uniqueidentifier not null
        constraint TaxonomyTokenTemplates_TaxonomyVersion_Id_fk
            references TaxonomyVersion,
    TokenTemplateId           uniqueidentifier not null
        constraint TaxonomyTokenTemplates_TokenTemplates_Id_fk
            references TokenTemplates
)
go
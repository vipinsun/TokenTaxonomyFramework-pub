# TTF Services

The [TaxonomyService](../tools/TaxonomyService) provides access to the TOM, allowing Create, Read, Update and Delete (CRUD) functionality to the artifacts persisted in the [artifacts](../artifacts) folder. Artifacts are serialized and stored as json text files in the artifacts repo so that the GitHub infrastructure can provide change management services for local and global TTF maintenance.



# Taxonomy Services Documentation

## Table of Contents

- [printersvc.proto](#printersvc.proto)
    - [ArtifactToPrint](#taxonomy.ttfprinter.ArtifactToPrint)
    - [PrintResult](#taxonomy.ttfprinter.PrintResult)
    - [PrintTTFOptions](#taxonomy.ttfprinter.PrintTTFOptions)
  
    - [PrinterService](#taxonomy.ttfprinter.PrinterService)
  

- [service.proto](#service.proto)
  
    - [Service](#taxonomy.Service)
  

  
  
  

<a name="printersvc.proto"></a>
<p align="right"><a href="#top">Top</a></p>

## printersvc.proto



<a name="taxonomy.ttfprinter.ArtifactToPrint"></a>

### ArtifactToPrint



| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| type | [taxonomy.model.artifact.ArtifactType](#taxonomy.model.artifact.ArtifactType) |  | ArtifactType to print. |
| id | [string](#string) |  | Id of the artifact to print. |
| draft | [bool](#bool) |  | Should it include the Draft watermark? |






<a name="taxonomy.ttfprinter.PrintResult"></a>

### PrintResult
Expected Output from Print Request.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| open_xml_document | [string](#string) |  | May include a string containing openXML content. |






<a name="taxonomy.ttfprinter.PrintTTFOptions"></a>

### PrintTTFOptions
If Book, the all artifacts will print to a single file or book.  If not, each artifact will print into their respective folder.


| Field | Type | Label | Description |
| ----- | ---- | ----- | ----------- |
| book | [bool](#bool) |  | If true, print a single book file. |
| draft | [bool](#bool) |  | Should it include the Draft watermark? |





 

 

 


<a name="taxonomy.ttfprinter.PrinterService"></a>

### PrinterService
Service to Print Artifacts to OpenXML format.

| Method Name | Request Type | Response Type | Description |
| ----------- | ------------ | ------------- | ------------|
| PrintTTFArtifact | [ArtifactToPrint](#taxonomy.ttfprinter.ArtifactToPrint) | [PrintResult](#taxonomy.ttfprinter.PrintResult) |  |
| PrintTTF | [PrintTTFOptions](#taxonomy.ttfprinter.PrintTTFOptions) | [PrintResult](#taxonomy.ttfprinter.PrintResult) |  |

 



<a name="service.proto"></a>
<p align="right"><a href="#top">Top</a></p>

## service.proto


 

 

 


<a name="taxonomy.Service"></a>

### Service
Taxonomy Service - Create, Read, Update, Delete for the Taxonomy Object Model

| Method Name | Request Type | Response Type | Description |
| ----------- | ------------ | ------------- | ------------|
| GetFullTaxonomy | [model.TaxonomyVersion](#taxonomy.model.TaxonomyVersion) | [model.Taxonomy](#taxonomy.model.Taxonomy) |  |
| GetLiteTaxonomy | [model.TaxonomyVersion](#taxonomy.model.TaxonomyVersion) | [model.Taxonomy](#taxonomy.model.Taxonomy) |  |
| GetBaseArtifact | [model.artifact.ArtifactSymbol](#taxonomy.model.artifact.ArtifactSymbol) | [model.core.Base](#taxonomy.model.core.Base) |  |
| GetBehaviorArtifact | [model.artifact.ArtifactSymbol](#taxonomy.model.artifact.ArtifactSymbol) | [model.core.Behavior](#taxonomy.model.core.Behavior) |  |
| GetBehaviorGroupArtifact | [model.artifact.ArtifactSymbol](#taxonomy.model.artifact.ArtifactSymbol) | [model.core.BehaviorGroup](#taxonomy.model.core.BehaviorGroup) |  |
| GetPropertySetArtifact | [model.artifact.ArtifactSymbol](#taxonomy.model.artifact.ArtifactSymbol) | [model.core.PropertySet](#taxonomy.model.core.PropertySet) |  |
| GetTemplateFormulaArtifact | [model.artifact.ArtifactSymbol](#taxonomy.model.artifact.ArtifactSymbol) | [model.core.TemplateFormula](#taxonomy.model.core.TemplateFormula) |  |
| GetTemplateDefinitionArtifact | [model.artifact.ArtifactSymbol](#taxonomy.model.artifact.ArtifactSymbol) | [model.core.TemplateDefinition](#taxonomy.model.core.TemplateDefinition) |  |
| GetTokenTemplate | [model.artifact.TokenTemplateId](#taxonomy.model.artifact.TokenTemplateId) | [model.core.TokenTemplate](#taxonomy.model.core.TokenTemplate) |  |
| GetTokenSpecification | [model.artifact.TokenTemplateId](#taxonomy.model.artifact.TokenTemplateId) | [model.core.TokenSpecification](#taxonomy.model.core.TokenSpecification) |  |
| GetArtifactsOfType | [model.artifact.QueryOptions](#taxonomy.model.artifact.QueryOptions) | [model.artifact.QueryResult](#taxonomy.model.artifact.QueryResult) |  |
| InitializeNewArtifact | [model.artifact.InitializeNewArtifactRequest](#taxonomy.model.artifact.InitializeNewArtifactRequest) | [model.artifact.InitializeNewArtifactResponse](#taxonomy.model.artifact.InitializeNewArtifactResponse) |  |
| CreateArtifact | [model.artifact.NewArtifactRequest](#taxonomy.model.artifact.NewArtifactRequest) | [model.artifact.NewArtifactResponse](#taxonomy.model.artifact.NewArtifactResponse) |  |
| UpdateArtifact | [model.artifact.UpdateArtifactRequest](#taxonomy.model.artifact.UpdateArtifactRequest) | [model.artifact.UpdateArtifactResponse](#taxonomy.model.artifact.UpdateArtifactResponse) |  |
| DeleteArtifact | [model.artifact.DeleteArtifactRequest](#taxonomy.model.artifact.DeleteArtifactRequest) | [model.artifact.DeleteArtifactResponse](#taxonomy.model.artifact.DeleteArtifactResponse) |  |
| CreateTemplateDefinition | [model.artifact.NewTemplateDefinition](#taxonomy.model.artifact.NewTemplateDefinition) | [model.core.TemplateDefinition](#taxonomy.model.core.TemplateDefinition) |  |
| CommitLocalUpdates | [model.artifact.CommitUpdatesRequest](#taxonomy.model.artifact.CommitUpdatesRequest) | [model.artifact.CommitUpdatesResponse](#taxonomy.model.artifact.CommitUpdatesResponse) |  |
| PullRequest | [model.artifact.IssuePullRequest](#taxonomy.model.artifact.IssuePullRequest) | [model.artifact.IssuePullResponse](#taxonomy.model.artifact.IssuePullResponse) |  |
| GetConfig | [model.artifact.ConfigurationRequest](#taxonomy.model.artifact.ConfigurationRequest) | [model.artifact.ServiceConfiguration](#taxonomy.model.artifact.ServiceConfiguration) |  |

 


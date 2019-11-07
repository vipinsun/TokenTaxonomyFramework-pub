# Taxonomy Object Model (TOM) Guide

The TTF is comprised of artifacts that represent the components used to define a token template and specification. The TOM contains collections of TTF artifact types grouped together. There is a collection for Token Bases, Behaviors, Behavior Groups, Property Sets, Template Formulas and Template Definitions. The TOM also has a hierarchical layout of Token Templates and Specifications.

The TOM is persisted to the [artifacts](../artifacts) folder and is loaded, saved and provided by the [TaxonomyService](../tools/TaxonomyService) and should be the primary programmatic access to the TTF.

To understand how to obtain the TOM using a gRpc call and to create, update and delete artifacts, see [Taxonomy Services](taxomonyServices.md).

The TOM is defined in the [protos](protos) folder in a set of protocol buffer files that are compiled into libraries for popular platforms like .Net, Java, Go, TypeScript, etc. in the [TOM](../tools/TaxonomyObjectModel) folder. The `protoc` creates the libraries using the `build-model.sh` bash script.  Note, you may need to adjust the path to the `protoc` and `grpcPluggin` folders for your local install.

## Taxonomy

The Taxonomy Object is the parent or container object that holds all of the collections of artifacts. You can request from the Taxonomy Service to obtain the entire TOM or you can request specific artifacts using their unique identifiers or tooling symbols. Unless memory constrained or the TTF grows too large, it is recommended that the client request the full TOM and work with the the TOM locally sending individual artifacts to be updated back to the service to be persisted to the artifacts folder.

The Taxonomy is represented by a version, supporting potential side by side major versions of the TTF in the future.

## Taxonomy Object

The TOM root object is Taxonomy, which has a `TaxonomyVersion` property, key/value collections of artifacts by type, a Hierarchy of templates and a token formula grammar.

![Taxonomy Root](../images/tom.png)

Above are the collections, represented in the native language, in this case C# as a MapField, with the artifact Id as the key string and the object of the collection type as the value.

## Taxonomy Hierarchy

The TOM, when it is retrieved from the [artifacts](../artifacts) folder will build a default hierarchy of token templates by their classification. This is a dynamic hierarchy that is not persisted and can be changed at runtime.

This is useful for user interfaces that want to represent a visual hierarchy. The hierarchy can be adjusted and rebuilt based on a user's input without connecting with the Taxonomy Service.

![Hierarchy](../images/hierarchy-model.png)

## Artifact Type Collections

Each artifact type is place within a collection where the key is the artifact Id and the value is the object of that type.
const {Base, Behavior, BehaviorGroup, PropertySet, Property, TokenTemplate, Invocation, InvocationRequest, InvocationResponse, InvocationParameter} = require('./core_pb.js');
const {TokenType, ArtifactType, ArtifactContent, MappingType, TargetPlatform, Artifact, ArtifactSymbol, SymbolInfluence, ArtifactDefinition, ArtifactAnalogy, ArtifactFile, Maps, MapReference, MapResourceEntry, NewArtifactRequest, NewArtifactResponse, UpdateArtifactRequest, UpdateArtifactResponse, DeleteArtifactRequest, DeleteArtifactResponse} = require('./artifact_pb.js');
const {FormulaGrammar, HybridTokenFormula, HybridTokenWithHybridChildrenFormula, SingleToken, TokenBase, BehaviorList, PropertySetListItem} = require('./grammar_pb.js');
const {Taxonomy} = require('./taxonomy_pb.js');
const {TaxonomyClient} = require('./taxonomyservice_grpc_web_pb');

var taxonomyService = new TaxonomyServiceClient('http://localhost:8080');

var requestTaxonomy = new TaxonomyVersion();
request.setVersion('1.0');

taxonomyService.getFullTaxonomy(requestTaxonomy, {}, function(err, response) {
  // ...
});
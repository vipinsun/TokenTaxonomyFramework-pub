/**
 * @fileoverview gRPC-Web generated client stub for taxonomy.service
 * @enhanceable
 * @public
 */

// GENERATED CODE -- DO NOT EDIT!



const grpc = {};
grpc.web = require('grpc-web');


var taxonomy_pb = require('./taxonomy_pb.js')

var core_pb = require('./core_pb.js')

var artifact_pb = require('./artifact_pb.js')
const proto = {};
proto.taxonomy = {};
proto.taxonomy.service = require('./taxonomyservice_pb.js');

/**
 * @param {string} hostname
 * @param {?Object} credentials
 * @param {?Object} options
 * @constructor
 * @struct
 * @final
 */
proto.taxonomy.service.TaxonomyServiceClient =
    function(hostname, credentials, options) {
  if (!options) options = {};
  options['format'] = 'text';

  /**
   * @private @const {!grpc.web.GrpcWebClientBase} The client
   */
  this.client_ = new grpc.web.GrpcWebClientBase(options);

  /**
   * @private @const {string} The hostname
   */
  this.hostname_ = hostname;

  /**
   * @private @const {?Object} The credentials to be used to connect
   *    to the server
   */
  this.credentials_ = credentials;

  /**
   * @private @const {?Object} Options for the client
   */
  this.options_ = options;
};


/**
 * @param {string} hostname
 * @param {?Object} credentials
 * @param {?Object} options
 * @constructor
 * @struct
 * @final
 */
proto.taxonomy.service.TaxonomyServicePromiseClient =
    function(hostname, credentials, options) {
  if (!options) options = {};
  options['format'] = 'text';

  /**
   * @private @const {!grpc.web.GrpcWebClientBase} The client
   */
  this.client_ = new grpc.web.GrpcWebClientBase(options);

  /**
   * @private @const {string} The hostname
   */
  this.hostname_ = hostname;

  /**
   * @private @const {?Object} The credentials to be used to connect
   *    to the server
   */
  this.credentials_ = credentials;

  /**
   * @private @const {?Object} Options for the client
   */
  this.options_ = options;
};


/**
 * @const
 * @type {!grpc.web.AbstractClientBase.MethodInfo<
 *   !proto.taxonomy.model.TaxonomyVersion,
 *   !proto.taxonomy.model.Taxonomy>}
 */
const methodInfo_TaxonomyService_GetFullTaxonomy = new grpc.web.AbstractClientBase.MethodInfo(
  taxonomy_pb.Taxonomy,
  /** @param {!proto.taxonomy.model.TaxonomyVersion} request */
  function(request) {
    return request.serializeBinary();
  },
  taxonomy_pb.Taxonomy.deserializeBinary
);


/**
 * @param {!proto.taxonomy.model.TaxonomyVersion} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.Error, ?proto.taxonomy.model.Taxonomy)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.taxonomy.model.Taxonomy>|undefined}
 *     The XHR Node Readable Stream
 */
proto.taxonomy.service.TaxonomyServiceClient.prototype.getFullTaxonomy =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/taxonomy.service.TaxonomyService/GetFullTaxonomy',
      request,
      metadata || {},
      methodInfo_TaxonomyService_GetFullTaxonomy,
      callback);
};


/**
 * @param {!proto.taxonomy.model.TaxonomyVersion} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.taxonomy.model.Taxonomy>}
 *     A native promise that resolves to the response
 */
proto.taxonomy.service.TaxonomyServicePromiseClient.prototype.getFullTaxonomy =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/taxonomy.service.TaxonomyService/GetFullTaxonomy',
      request,
      metadata || {},
      methodInfo_TaxonomyService_GetFullTaxonomy);
};


/**
 * @const
 * @type {!grpc.web.AbstractClientBase.MethodInfo<
 *   !proto.taxonomy.model.Symbol,
 *   !proto.taxonomy.model.core.Base>}
 */
const methodInfo_TaxonomyService_GetBaseArtifact = new grpc.web.AbstractClientBase.MethodInfo(
  core_pb.Base,
  /** @param {!proto.taxonomy.model.Symbol} request */
  function(request) {
    return request.serializeBinary();
  },
  core_pb.Base.deserializeBinary
);


/**
 * @param {!proto.taxonomy.model.Symbol} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.Error, ?proto.taxonomy.model.core.Base)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.taxonomy.model.core.Base>|undefined}
 *     The XHR Node Readable Stream
 */
proto.taxonomy.service.TaxonomyServiceClient.prototype.getBaseArtifact =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/taxonomy.service.TaxonomyService/GetBaseArtifact',
      request,
      metadata || {},
      methodInfo_TaxonomyService_GetBaseArtifact,
      callback);
};


/**
 * @param {!proto.taxonomy.model.Symbol} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.taxonomy.model.core.Base>}
 *     A native promise that resolves to the response
 */
proto.taxonomy.service.TaxonomyServicePromiseClient.prototype.getBaseArtifact =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/taxonomy.service.TaxonomyService/GetBaseArtifact',
      request,
      metadata || {},
      methodInfo_TaxonomyService_GetBaseArtifact);
};


/**
 * @const
 * @type {!grpc.web.AbstractClientBase.MethodInfo<
 *   !proto.taxonomy.model.Symbol,
 *   !proto.taxonomy.model.core.Behavior>}
 */
const methodInfo_TaxonomyService_GetBehaviorArtifact = new grpc.web.AbstractClientBase.MethodInfo(
  core_pb.Behavior,
  /** @param {!proto.taxonomy.model.Symbol} request */
  function(request) {
    return request.serializeBinary();
  },
  core_pb.Behavior.deserializeBinary
);


/**
 * @param {!proto.taxonomy.model.Symbol} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.Error, ?proto.taxonomy.model.core.Behavior)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.taxonomy.model.core.Behavior>|undefined}
 *     The XHR Node Readable Stream
 */
proto.taxonomy.service.TaxonomyServiceClient.prototype.getBehaviorArtifact =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/taxonomy.service.TaxonomyService/GetBehaviorArtifact',
      request,
      metadata || {},
      methodInfo_TaxonomyService_GetBehaviorArtifact,
      callback);
};


/**
 * @param {!proto.taxonomy.model.Symbol} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.taxonomy.model.core.Behavior>}
 *     A native promise that resolves to the response
 */
proto.taxonomy.service.TaxonomyServicePromiseClient.prototype.getBehaviorArtifact =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/taxonomy.service.TaxonomyService/GetBehaviorArtifact',
      request,
      metadata || {},
      methodInfo_TaxonomyService_GetBehaviorArtifact);
};


/**
 * @const
 * @type {!grpc.web.AbstractClientBase.MethodInfo<
 *   !proto.taxonomy.model.Symbol,
 *   !proto.taxonomy.model.core.BehaviorGroup>}
 */
const methodInfo_TaxonomyService_GetBehaviorGroupArtifact = new grpc.web.AbstractClientBase.MethodInfo(
  core_pb.BehaviorGroup,
  /** @param {!proto.taxonomy.model.Symbol} request */
  function(request) {
    return request.serializeBinary();
  },
  core_pb.BehaviorGroup.deserializeBinary
);


/**
 * @param {!proto.taxonomy.model.Symbol} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.Error, ?proto.taxonomy.model.core.BehaviorGroup)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.taxonomy.model.core.BehaviorGroup>|undefined}
 *     The XHR Node Readable Stream
 */
proto.taxonomy.service.TaxonomyServiceClient.prototype.getBehaviorGroupArtifact =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/taxonomy.service.TaxonomyService/GetBehaviorGroupArtifact',
      request,
      metadata || {},
      methodInfo_TaxonomyService_GetBehaviorGroupArtifact,
      callback);
};


/**
 * @param {!proto.taxonomy.model.Symbol} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.taxonomy.model.core.BehaviorGroup>}
 *     A native promise that resolves to the response
 */
proto.taxonomy.service.TaxonomyServicePromiseClient.prototype.getBehaviorGroupArtifact =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/taxonomy.service.TaxonomyService/GetBehaviorGroupArtifact',
      request,
      metadata || {},
      methodInfo_TaxonomyService_GetBehaviorGroupArtifact);
};


/**
 * @const
 * @type {!grpc.web.AbstractClientBase.MethodInfo<
 *   !proto.taxonomy.model.Symbol,
 *   !proto.taxonomy.model.core.PropertySet>}
 */
const methodInfo_TaxonomyService_GetPropertySetArtifact = new grpc.web.AbstractClientBase.MethodInfo(
  core_pb.PropertySet,
  /** @param {!proto.taxonomy.model.Symbol} request */
  function(request) {
    return request.serializeBinary();
  },
  core_pb.PropertySet.deserializeBinary
);


/**
 * @param {!proto.taxonomy.model.Symbol} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.Error, ?proto.taxonomy.model.core.PropertySet)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.taxonomy.model.core.PropertySet>|undefined}
 *     The XHR Node Readable Stream
 */
proto.taxonomy.service.TaxonomyServiceClient.prototype.getPropertySetArtifact =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/taxonomy.service.TaxonomyService/GetPropertySetArtifact',
      request,
      metadata || {},
      methodInfo_TaxonomyService_GetPropertySetArtifact,
      callback);
};


/**
 * @param {!proto.taxonomy.model.Symbol} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.taxonomy.model.core.PropertySet>}
 *     A native promise that resolves to the response
 */
proto.taxonomy.service.TaxonomyServicePromiseClient.prototype.getPropertySetArtifact =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/taxonomy.service.TaxonomyService/GetPropertySetArtifact',
      request,
      metadata || {},
      methodInfo_TaxonomyService_GetPropertySetArtifact);
};


/**
 * @const
 * @type {!grpc.web.AbstractClientBase.MethodInfo<
 *   !proto.taxonomy.model.TaxonomyFormula,
 *   !proto.taxonomy.model.core.TokenTemplate>}
 */
const methodInfo_TaxonomyService_GetTokenTemplateArtifact = new grpc.web.AbstractClientBase.MethodInfo(
  core_pb.TokenTemplate,
  /** @param {!proto.taxonomy.model.TaxonomyFormula} request */
  function(request) {
    return request.serializeBinary();
  },
  core_pb.TokenTemplate.deserializeBinary
);


/**
 * @param {!proto.taxonomy.model.TaxonomyFormula} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.Error, ?proto.taxonomy.model.core.TokenTemplate)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.taxonomy.model.core.TokenTemplate>|undefined}
 *     The XHR Node Readable Stream
 */
proto.taxonomy.service.TaxonomyServiceClient.prototype.getTokenTemplateArtifact =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/taxonomy.service.TaxonomyService/GetTokenTemplateArtifact',
      request,
      metadata || {},
      methodInfo_TaxonomyService_GetTokenTemplateArtifact,
      callback);
};


/**
 * @param {!proto.taxonomy.model.TaxonomyFormula} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.taxonomy.model.core.TokenTemplate>}
 *     A native promise that resolves to the response
 */
proto.taxonomy.service.TaxonomyServicePromiseClient.prototype.getTokenTemplateArtifact =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/taxonomy.service.TaxonomyService/GetTokenTemplateArtifact',
      request,
      metadata || {},
      methodInfo_TaxonomyService_GetTokenTemplateArtifact);
};


/**
 * @const
 * @type {!grpc.web.AbstractClientBase.MethodInfo<
 *   !proto.taxonomy.model.artifact.NewArtifactRequest,
 *   !proto.taxonomy.model.artifact.NewArtifactResponse>}
 */
const methodInfo_TaxonomyService_CreateArtifact = new grpc.web.AbstractClientBase.MethodInfo(
  artifact_pb.NewArtifactResponse,
  /** @param {!proto.taxonomy.model.artifact.NewArtifactRequest} request */
  function(request) {
    return request.serializeBinary();
  },
  artifact_pb.NewArtifactResponse.deserializeBinary
);


/**
 * @param {!proto.taxonomy.model.artifact.NewArtifactRequest} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.Error, ?proto.taxonomy.model.artifact.NewArtifactResponse)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.taxonomy.model.artifact.NewArtifactResponse>|undefined}
 *     The XHR Node Readable Stream
 */
proto.taxonomy.service.TaxonomyServiceClient.prototype.createArtifact =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/taxonomy.service.TaxonomyService/CreateArtifact',
      request,
      metadata || {},
      methodInfo_TaxonomyService_CreateArtifact,
      callback);
};


/**
 * @param {!proto.taxonomy.model.artifact.NewArtifactRequest} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.taxonomy.model.artifact.NewArtifactResponse>}
 *     A native promise that resolves to the response
 */
proto.taxonomy.service.TaxonomyServicePromiseClient.prototype.createArtifact =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/taxonomy.service.TaxonomyService/CreateArtifact',
      request,
      metadata || {},
      methodInfo_TaxonomyService_CreateArtifact);
};


/**
 * @const
 * @type {!grpc.web.AbstractClientBase.MethodInfo<
 *   !proto.taxonomy.model.artifact.UpdateArtifactRequest,
 *   !proto.taxonomy.model.artifact.UpdateArtifactResponse>}
 */
const methodInfo_TaxonomyService_UpdateArtifact = new grpc.web.AbstractClientBase.MethodInfo(
  artifact_pb.UpdateArtifactResponse,
  /** @param {!proto.taxonomy.model.artifact.UpdateArtifactRequest} request */
  function(request) {
    return request.serializeBinary();
  },
  artifact_pb.UpdateArtifactResponse.deserializeBinary
);


/**
 * @param {!proto.taxonomy.model.artifact.UpdateArtifactRequest} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.Error, ?proto.taxonomy.model.artifact.UpdateArtifactResponse)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.taxonomy.model.artifact.UpdateArtifactResponse>|undefined}
 *     The XHR Node Readable Stream
 */
proto.taxonomy.service.TaxonomyServiceClient.prototype.updateArtifact =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/taxonomy.service.TaxonomyService/UpdateArtifact',
      request,
      metadata || {},
      methodInfo_TaxonomyService_UpdateArtifact,
      callback);
};


/**
 * @param {!proto.taxonomy.model.artifact.UpdateArtifactRequest} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.taxonomy.model.artifact.UpdateArtifactResponse>}
 *     A native promise that resolves to the response
 */
proto.taxonomy.service.TaxonomyServicePromiseClient.prototype.updateArtifact =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/taxonomy.service.TaxonomyService/UpdateArtifact',
      request,
      metadata || {},
      methodInfo_TaxonomyService_UpdateArtifact);
};


/**
 * @const
 * @type {!grpc.web.AbstractClientBase.MethodInfo<
 *   !proto.taxonomy.model.artifact.DeleteArtifactRequest,
 *   !proto.taxonomy.model.artifact.DeleteArtifactResponse>}
 */
const methodInfo_TaxonomyService_DeleteArtifact = new grpc.web.AbstractClientBase.MethodInfo(
  artifact_pb.DeleteArtifactResponse,
  /** @param {!proto.taxonomy.model.artifact.DeleteArtifactRequest} request */
  function(request) {
    return request.serializeBinary();
  },
  artifact_pb.DeleteArtifactResponse.deserializeBinary
);


/**
 * @param {!proto.taxonomy.model.artifact.DeleteArtifactRequest} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @param {function(?grpc.web.Error, ?proto.taxonomy.model.artifact.DeleteArtifactResponse)}
 *     callback The callback function(error, response)
 * @return {!grpc.web.ClientReadableStream<!proto.taxonomy.model.artifact.DeleteArtifactResponse>|undefined}
 *     The XHR Node Readable Stream
 */
proto.taxonomy.service.TaxonomyServiceClient.prototype.deleteArtifact =
    function(request, metadata, callback) {
  return this.client_.rpcCall(this.hostname_ +
      '/taxonomy.service.TaxonomyService/DeleteArtifact',
      request,
      metadata || {},
      methodInfo_TaxonomyService_DeleteArtifact,
      callback);
};


/**
 * @param {!proto.taxonomy.model.artifact.DeleteArtifactRequest} request The
 *     request proto
 * @param {?Object<string, string>} metadata User defined
 *     call metadata
 * @return {!Promise<!proto.taxonomy.model.artifact.DeleteArtifactResponse>}
 *     A native promise that resolves to the response
 */
proto.taxonomy.service.TaxonomyServicePromiseClient.prototype.deleteArtifact =
    function(request, metadata) {
  return this.client_.unaryCall(this.hostname_ +
      '/taxonomy.service.TaxonomyService/DeleteArtifact',
      request,
      metadata || {},
      methodInfo_TaxonomyService_DeleteArtifact);
};


module.exports = proto.taxonomy.service;


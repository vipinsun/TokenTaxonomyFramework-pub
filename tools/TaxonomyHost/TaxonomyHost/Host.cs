using System.Reflection;
using System.Threading.Tasks;
using Grpc.Core;
using log4net;
using TTI.TTF.Model.Artifact;
using TTI.TTF.Model.Core;
using TTI.TTF.Taxonomy;
using TTI.TTF.Taxonomy.Model;

namespace TaxonomyHost
{
    internal class Host : TTI.TTF.Taxonomy.TaxonomyService.TaxonomyServiceBase
    {
        private static ILog _log;

        public Host()
        {
            Utils.InitLog();
            _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        }

        public override Task<Taxonomy> GetFullTaxonomy(TaxonomyVersion version, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: GetFullTaxonomy");
            return Task.FromResult(ModelManager.GetFullTaxonomy(version));
        }

        public override Task<Base> GetBaseArtifact(Symbol symbol, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: GetBaseArtifact");
            return Task.FromResult(ModelManager.GetBaseArtifact(symbol));
        }
        
        public override Task<Behavior> GetBehaviorArtifact(Symbol symbol, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: GetBehaviorArtifact");
            return Task.FromResult(ModelManager.GetBehaviorArtifact(symbol));
        }
        
        public override Task<BehaviorGroup> GetBehaviorGroupArtifact(Symbol symbol, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: GetBehaviorGroupArtifact");
            return Task.FromResult(ModelManager.GetBehaviorGroupArtifact(symbol));
        }
        public override Task<PropertySet> GetPropertySetArtifact(Symbol symbol, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: GetPropertySetArtifact");
            return Task.FromResult(ModelManager.GetPropertySetArtifact(symbol));
        }
        public override Task<TokenTemplate> GetTokenTemplateArtifact(TaxonomyFormula formula, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: GetTokenTemplateArtifact");
            return Task.FromResult(ModelManager.GetTokenTemplateArtifact(formula));
        }
        public override Task<NewArtifactResponse> CreateArtifact(NewArtifactRequest artifactRequest, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: CreateArtifact");
            return Task.FromResult(ModelManager.CreateArtifact(artifactRequest));
        }
        public override Task<UpdateArtifactResponse> UpdateArtifact(UpdateArtifactRequest artifactRequest, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: UpdateArtifact");
            return Task.FromResult(ModelManager.UpdateArtifact(artifactRequest));
        }
        public override Task<DeleteArtifactResponse> DeleteArtifact(DeleteArtifactRequest artifactRequest, ServerCallContext ctx)
        {
            _log.Info("gRpc request for: DeleteArtifact");
            return Task.FromResult(ModelManager.DeleteArtifact(artifactRequest));
        }
    }
}

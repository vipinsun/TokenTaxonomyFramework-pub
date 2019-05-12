# Sample TTF Tools

Descriptions of the sample TTF tools

## dotnet core

- [Artifact Generator](artifactGenerator) is a simple artifact generator to create stubbed artifacts of a particular type.  This is a console based application that takes 3 arguments, a relative path to the TTF [artifacts](../artifacts) folder, an artifact name and artifact type.  Artifact types are: 0 = Base, 1 = Behavior, 2 = BehaviorGroup, 3 = PropertySet or 4 - TokenTemplate

```bash
dotnet factgen --p ../artifacts --n myArtifactName --type 1
```

The above creates a folder, if it doesn't already exist, in the artifacts folder for the type of artifact and the name of the artifact.  In this folder you will find a Json definition, proto control and md for diagrams.

- [Taxonomy Host](TaxonomyHost) is a gRpc service that allows you to request the artifact model, or just query for a symbol or receive a collection of artifact types as well as perform CRUD operations on the taxonomy.  There is also a sample client for the taxonomy.

 - From your cloned copy of the repo.
 
 ```bash
cd tools/TaxonomyHost
./docker-build.sh
docker compose service-compose.yaml up
docker compose service.client.yaml up
 ```
# TTF Tools

## Taxonomy Service

- [Taxonomy Service](TaxonomyService/TaxonomyService) is a gRpc service that provides CRUD (create, read, update and delete) capabilities for the repo.  You can run this service from your local clone or use the published service from the [TTI Site](http://tokentaxonomy.org) once it is published. When the service starts, it is passed the path to the artifacts folder and then reads the taxonomy folder structure and files into the [Taxonomy Model](./taxonomy-model.md).  

- [Taxonomy Client](TaxonomyService/TaxonomyClient) is a simple command line tool that demonstrates how to interact with the Taxonomy Service to fetch the model and then apply it to any type of application for display or input.

- From your cloned copy of the repo.

```bash
cd tools/TaxonomyHost
./docker-build.sh
 ```

This will build the Client and Service, starting the later awaiting client requests.  You can now start testing out the taxonomy.

### Test the Taxonomy

- To fetch the entire taxonomy model (using Docker to host the client):

`docker run -e gRpcHost=host.docker.internal txclient --f`

if using the dotnet client using the [dotnet core runtime](https://dotnet.microsoft.com/download):

`dotnet TaxonomyClient --f`

### Options for the Client

- `--f`: must be used as a single argument and fetches the entire Taxonomy Model and writes it to the console.
- `--ts`: option indicating the artifact symbol. Value should be the letter or acronym for the artifact.
- `--t`: option for the artifact type. Valid values are 0 = Base, 1 = Behavior, 2 = BehaviorGroup, 3 = PropertySet or 4 - TokenTemplate
- `--s`: option to save a queried artifact so you may edit it locally. No value is set after the option
- `--u`: option to update a saved local artifact to the taxonomy. Value is the name of the folder to be used for the update. This is the folder created by the --s option and should be a relative path from where you are executing the command.
- `--c`: option to create a new artifact. Value is a proposed symbol for the artifact.
- `--n`: option for the name of a new artifact, used with --c.  Value is the name of the artifact.
- `--d`: create a template definition from a template formula, requires `--n`

Examples:

- `--ts m --t 1` is a query for a behavior with the symbol `m`, or mintable.
- `--ts SC --t 2` is a query for a behavior-group with the symbol `SC`, or supply-control.
- `--ts r --t 1 --s` is a query for a behavior with the symbol `r` and save it locally.  This will fetch the roles behavior and save it in a folder `roles`.
- `--u roles --t 1` will update the artifact from the roles folder saved in the previous example.
- `--c phSKU --n sku --t 3` will create a new artifact, a behavior-set, called `sku` with a symbol phSKU.
- `--d 89ff775c-27f1-494e-b31c-f3fb3a9527ac --n InvoiceToken` will create a template definition called InvoiceToken from the template formula with the UUID/Guid after the `--d`.

## Artifact Generator

[Artifact Generator](artifactGenerator) is a simple artifact generator to create stubbed artifacts of a particular type.  This is a console based application that takes 3 arguments, a relative path to the TTF [artifacts](../artifacts) folder, an artifact name and artifact type.  Artifact types are: 0 = Base, 1 = Behavior, 2 = BehaviorGroup, 3 = PropertySet or 4 - TokenTemplate

```bash
dotnet factgen --p ../artifacts --n myArtifactName --type 1
```

The above creates a folder, if it doesn't already exist, in the artifacts folder for the type of artifact and the name of the artifact.  In this folder you will find a Json definition, proto control and md for diagrams.
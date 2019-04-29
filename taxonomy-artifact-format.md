# Token Taxonomy Framework Artifact File Format

Taxonomy artifacts and artifacts generated using the framework are in a common file format.  This format has metadata from or following the taxonomy and a basic structure for the creators of the artifact to follow.

This file format enables tooling to navigate these artifacts and for creators to generate artifacts that are common and easily understood by others using the framework.

## Artifact Structure

Artifacts should for a token definition, behavior or behavior group should be placed within a folder under the artifact type folder.  Within this folder there can be many files that are used to define the artifact, not all of them are required.

- Artifact Definition: this is a JSON formated file that is generated from the artifactGenerator and updated manually or through the Taxonomy Service. This file holds most of the metadata as well as the descriptions, analogies and aliases.
- Proto: this is a protocol buffer file that is used to describe the control messages used in an interaction with the artifact.
- Artifact Markdown:  this is used to host UML diagrams, primarily sequence diagrams and can use a recommended markdown plugin or custom implementation based on the requirements of the artifact owners. In the examples [plantUml](http://plantuml.com) is used for for [sequence diagrams](http://plantuml.com/sequence-diagram) because it has a GitHub supported rendering capability and a Visual Studio Code [plugin](https://marketplace.visualstudio.com/items?itemName=jebbs.plantuml#markdown-integrating).  However, other options like
[mermaid](https://marketplace.visualstudio.com/items?itemName=vstirbu.vscode-mermaid-preview) could be used.
- Custom: the artifact owners can also place an appropriate artifact files that are useful for the framework like PowerPoint slides, etc. It is recommend to use similar metadata tags and file naming to support consistency in the artifact's supporting documentation.

## Primary JSON Artifact File Format

An artifact can be generated using the artifactGenerator tool to create the artifact structure preparing for the artifact details to be populated.

If an artifact's definition is captured using a Word or other template, the artifact data can be imported manually via cut and paste and submitted to the Taxonomy Service to update the artifact in the taxonomy.  Other tools to allow for the artifact to be populated through a web interface or automated import can be developed in the future.

- Artifact summary: one paragraph description of artifact
- Optional list of artifact property-sets.
- Optional list of artifact behaviors: list of implemented behaviors as links to the associated behavior artifact.  A behavior would not have any of these, but a token definition or a behavior collection would.
- List of analogies and aliases
- Comments

## Proto File Format

Proto files are optional, but recommended for behaviors as they are useful in understanding how interaction with a behavior should work.  When used in addition with a supporting sequence diagram it can provide a deep definition of the artifact.

```protobuf
syntax = "proto3";

package transferable;

import "tokens.proto";
import "google/protobuf/any.proto";
import "google/protobuf/timestamp.proto";

option csharp_namespace = "TTF.Tokens.tokens";
option java_package = "org.tti.tokens";
option java_multiple_files = true;
```

[Artifacts](artifacts) are templates and samples to seed the initial set of artifacts.

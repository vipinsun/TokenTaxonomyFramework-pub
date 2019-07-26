# TTF Model

The Model is a prototype object model and simple api for creating and consuming the TTF metadata and content as well as building user interfaces for controlled user input to ensure valid content submission before pull requests to merge into the TTF after a workshop.

Additionally, the Model can be used to dynamically generate TTF maps to implementation source code, implementations or other frameworks. i.e. legal, regulatory.

This model could be used to represent an artifact's metadata and serialized and saved as json in the artifact folder instead of sprinkling metadata around in html tags or json within artifact documents.

Consuming an artifact folder to generate a artifact from the model would populate the object model with the metadata and the collection of files in the folder.

## Model of Artifacts

The model is used in conjunction with the artifact folders.

## Prerequisites

- install the protoc [gRpcWeb](https://github.com/grpc/grpc-web/releases) extension and [install](https://github.com/grpc/grpc-web).  OSX:

```bash
sudo mv ~/Downloads/protoc-gen-grpc-web-1.0.4-darwin-x86_64 \
  /usr/local/bin/protoc-gen-grpc-web
chmod +x /usr/local/bin/protoc-gen-grpc-web
```
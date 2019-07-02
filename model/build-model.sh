#!/bin/bash
#requires protoc be installed and in your path.  For Go, install the go plugin: https://github.com/golang/protobuf

mkdir -p ../tools/TaxonomyObjectModel/out/csharp
mkdir -p ../tools/TaxonomyObjectModel/out/java
mkdir -p ../tools/TaxonomyObjectModel/out/go
mkdir -p ../tools/TaxonomyObjectModel/out/js

#you will need to adjust the relative path to the protoc and grpc tools.
protoc --csharp_out=../tools/TaxonomyObjectModel/out/csharp --java_out=../tools/TaxonomyObjectModel/out/java --js_out=import_style=commonjs:../tools/TaxonomyObjectModel/out/js  --grpc-web_out=import_style=commonjs,mode=grpcwebtext:../tools/TaxonomyObjectModel/out/js --proto_path=./protos --proto_path=../../../../.nuget/packages/google.protobuf.tools/3.7.0/tools ./protos/artifact.proto --plugin=protoc-gen-web
protoc --csharp_out=../tools/TaxonomyObjectModel/out/csharp --java_out=../tools/TaxonomyObjectModel/out/java --js_out=import_style=commonjs:../tools/TaxonomyObjectModel/out/js  --grpc-web_out=import_style=commonjs,mode=grpcwebtext:../tools/TaxonomyObjectModel/out/js --proto_path=./protos --proto_path=../../../../.nuget/packages/google.protobuf.tools/3.7.0/tools ./protos/core.proto --plugin=protoc-gen-web
protoc --csharp_out=../tools/TaxonomyObjectModel/out/csharp --java_out=../tools/TaxonomyObjectModel/out/java --js_out=import_style=commonjs:../tools/TaxonomyObjectModel/out/js  --grpc-web_out=import_style=commonjs,mode=grpcwebtext:../tools/TaxonomyObjectModel/out/js --proto_path=./protos --proto_path=../../../../.nuget/packages/google.protobuf.tools/3.7.0/tools ./protos/taxonomy.proto --plugin=protoc-gen-web
protoc --csharp_out=../tools/TaxonomyObjectModel/out/csharp --java_out=../tools/TaxonomyObjectModel/out/java --js_out=import_style=commonjs:../tools/TaxonomyObjectModel/out/js  --grpc-web_out=import_style=commonjs,mode=grpcwebtext:../tools/TaxonomyObjectModel/out/js --proto_path=./protos --proto_path=../../../../.nuget/packages/google.protobuf.tools/3.7.0/tools  --grpc_out ../tools/TaxonomyService/TaxonomyService ./protos/taxonomyservice.proto --plugin=protoc-gen-grpc=../../../../.nuget/packages/grpc.tools/1.21.0/tools/macosx_x64/grpc_csharp_plugin --plugin=protoc-gen-web
protoc --proto_path=./protos --proto_path=../../../../.nuget/packages/google.protobuf.tools/3.7.0/tools  --grpc_out=no_server:../tools/TaxonomyService/TaxonomyClient ./protos/taxonomyservice.proto --plugin=protoc-gen-grpc=../../../../.nuget/packages/grpc.tools/1.21.0/tools/macosx_x64/grpc_csharp_plugin
protoc --proto_path=./protos --proto_path=../../../../.nuget/packages/google.protobuf.tools/3.7.0/tools  --grpc_out=no_server:../tools/TTF-Web-Explorer/Model ./protos/taxonomyservice.proto --plugin=protoc-gen-grpc=../../../../.nuget/packages/grpc.tools/1.21.0/tools/macosx_x64/grpc_csharp_plugin


cp ../tools/TaxonomyObjectModel/out/csharp/* ../tools/ArtifactGenerator/ArtifactGenerator/Model

cp ../tools/TaxonomyObjectModel/out/csharp/* ../tools/TaxonomyService/TaxonomyModel

cp ../tools/TaxonomyObjectModel/out/csharp/* ../tools/TTF-Web-Explorer/Model
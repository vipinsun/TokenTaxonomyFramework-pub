#!/bin/bash
#requires protoc be installed and in your path.  For Go, install the go plugin: https://github.com/golang/protobuf
mkdir -p out/csharp
mkdir -p out/java
mkdir -p out/go

#you will need to adjust the relative path to the protoc and grpc tools.
protoc --csharp_out=./out/csharp --java_out=./out/java  --proto_path=./model/protos --proto_path=../../../../.nuget/packages/google.protobuf.tools/3.7.0/tools ./model/protos/artifact.proto
protoc --csharp_out=./out/csharp --java_out=./out/java  --proto_path=./model/protos --proto_path=../../../../.nuget/packages/google.protobuf.tools/3.7.0/tools ./model/protos/core.proto
protoc --csharp_out=./out/csharp --java_out=./out/java  --proto_path=./model/protos --proto_path=../../../../.nuget/packages/google.protobuf.tools/3.7.0/tools ./model/protos/taxonomy.proto
protoc --csharp_out=./out/csharp --java_out=./out/java  --proto_path=./model/protos --proto_path=../../../../.nuget/packages/google.protobuf.tools/3.7.0/tools ./model/protos/grammar.proto
protoc --csharp_out=./out/csharp --java_out=./out/java  --proto_path=./model/protos --proto_path=../../../../.nuget/packages/google.protobuf.tools/3.7.0/tools  --grpc_out ../tools/TaxonomyHost/TaxonomyHost ./model/protos/taxonomyservice.proto --plugin=protoc-gen-grpc=../../../../.nuget/packages/grpc.tools/1.20.1/tools/macosx_x64/grpc_csharp_plugin

cp ./out/csharp/* ../tools/artifactGenerator/artifactGenerator/model

cp ./out/csharp/* ../tools/TaxonomyHost/TaxonomyHost/model
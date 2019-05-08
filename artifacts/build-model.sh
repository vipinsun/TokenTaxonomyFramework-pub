#!/bin/bash
#requires protoc be installed and in your path.  For Go, install the go plugin: https://github.com/golang/protobuf
mkdir -p out/csharp
mkdir -p out/java
mkdir -p out/go

#you will need to adjust the relative path to the protoc and grpc tools.
protoc --csharp_out=./out/csharp --java_out=./out/java --js_out=import_style=commonjs:./out/js  --grpc-web_out=import_style=commonjs,mode=grpcwebtext:./out/js --proto_path=./model/protos --proto_path=../../../../.nuget/packages/google.protobuf.tools/3.7.0/tools ./model/protos/artifact.proto --plugin=protoc-gen-web
protoc --csharp_out=./out/csharp --java_out=./out/java --js_out=import_style=commonjs:./out/js  --grpc-web_out=import_style=commonjs,mode=grpcwebtext:./out/js --proto_path=./model/protos --proto_path=../../../../.nuget/packages/google.protobuf.tools/3.7.0/tools ./model/protos/core.proto --plugin=protoc-gen-web
protoc --csharp_out=./out/csharp --java_out=./out/java --js_out=import_style=commonjs:./out/js  --grpc-web_out=import_style=commonjs,mode=grpcwebtext:./out/js --proto_path=./model/protos --proto_path=../../../../.nuget/packages/google.protobuf.tools/3.7.0/tools ./model/protos/taxonomy.proto --plugin=protoc-gen-web
protoc --csharp_out=./out/csharp --java_out=./out/java --js_out=import_style=commonjs:./out/js  --grpc-web_out=import_style=commonjs,mode=grpcwebtext:./out/js --proto_path=./model/protos --proto_path=../../../../.nuget/packages/google.protobuf.tools/3.7.0/tools ./model/protos/grammar.proto --plugin=protoc-gen-web
protoc --csharp_out=./out/csharp --java_out=./out/java --js_out=import_style=commonjs:./out/js  --grpc-web_out=import_style=commonjs,mode=grpcwebtext:./out/js --proto_path=./model/protos --proto_path=../../../../.nuget/packages/google.protobuf.tools/3.7.0/tools  --grpc_out ../tools/TaxonomyHost/TaxonomyHost ./model/protos/taxonomyservice.proto --plugin=protoc-gen-grpc=../../../../.nuget/packages/grpc.tools/1.20.1/tools/macosx_x64/grpc_csharp_plugin --plugin=protoc-gen-web

cp ./out/csharp/* ../tools/artifactGenerator/artifactGenerator/Model

cp ./out/csharp/* ../tools/TaxonomyHost/TaxonomyHost/Model

cp ./out/js/* ../tools/taxonomyWeb/src
import * as grpcWeb from 'grpc-web';
import {ServiceClient} from './model/ServiceServiceClientPb';
import {ConfigurationRequest} from "./model/artifact_pb";
import * as artifact_pb from "./model/artifact_pb";

let client = new ServiceClient("http://localhost:9080");

let call = client.getConfig(new ConfigurationRequest(), null, (err: grpcWeb.Error,
                                                               response: artifact_pb.ServiceConfiguration) => {
  console.log(err);
  console.log(response);
});

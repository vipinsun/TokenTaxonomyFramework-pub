import * as grpcWeb from 'grpc-web';
import {ServiceClient} from '../../TaxonomyObjectModel/out/ts/ServiceServiceClientPb';
import {ConfigurationRequest} from "../../TaxonomyObjectModel/out/ts/artifact_pb";
import * as artifact_pb from "../../TaxonomyObjectModel/out/ts/artifact_pb";

let client = new ServiceClient("http://localhost:8086");

let call = client.getConfig(new ConfigurationRequest(), null, (err: grpcWeb.Error,
                                                                             response: artifact_pb.ServiceConfiguration) => {
  console.log(err);
  console.log(response);
});

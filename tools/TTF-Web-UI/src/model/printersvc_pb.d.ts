/* eslint-disable */
import * as jspb from "google-protobuf"

import * as artifact_pb from './artifact_pb';

export class PrintArtifact extends jspb.Message {
  getType(): artifact_pb.ArtifactType;
  setType(value: artifact_pb.ArtifactType): void;

  getId(): string;
  setId(value: string): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): PrintArtifact.AsObject;
  static toObject(includeInstance: boolean, msg: PrintArtifact): PrintArtifact.AsObject;
  static serializeBinaryToWriter(message: PrintArtifact, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): PrintArtifact;
  static deserializeBinaryFromReader(message: PrintArtifact, reader: jspb.BinaryReader): PrintArtifact;
}

export namespace PrintArtifact {
  export type AsObject = {
    type: artifact_pb.ArtifactType,
    id: string,
  }
}

export class PrintTTFOptions extends jspb.Message {
  getBook(): boolean;
  setBook(value: boolean): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): PrintTTFOptions.AsObject;
  static toObject(includeInstance: boolean, msg: PrintTTFOptions): PrintTTFOptions.AsObject;
  static serializeBinaryToWriter(message: PrintTTFOptions, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): PrintTTFOptions;
  static deserializeBinaryFromReader(message: PrintTTFOptions, reader: jspb.BinaryReader): PrintTTFOptions;
}

export namespace PrintTTFOptions {
  export type AsObject = {
    book: boolean,
  }
}

export class PrintResult extends jspb.Message {
  getOpenXmlDocument(): string;
  setOpenXmlDocument(value: string): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): PrintResult.AsObject;
  static toObject(includeInstance: boolean, msg: PrintResult): PrintResult.AsObject;
  static serializeBinaryToWriter(message: PrintResult, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): PrintResult;
  static deserializeBinaryFromReader(message: PrintResult, reader: jspb.BinaryReader): PrintResult;
}

export namespace PrintResult {
  export type AsObject = {
    openXmlDocument: string,
  }
}


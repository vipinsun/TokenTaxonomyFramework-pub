/* eslint-disable */
import * as jspb from "google-protobuf"

import * as core_pb from './core_pb';
import * as artifact_pb from './artifact_pb';

export class Taxonomy extends jspb.Message {
  getVersion(): TaxonomyVersion | undefined;
  setVersion(value?: TaxonomyVersion): void;
  hasVersion(): boolean;
  clearVersion(): void;

  getBaseTokenTypesMap(): jspb.Map<string, core_pb.Base>;
  clearBaseTokenTypesMap(): void;

  getBehaviorsMap(): jspb.Map<string, core_pb.Behavior>;
  clearBehaviorsMap(): void;

  getBehaviorGroupsMap(): jspb.Map<string, core_pb.BehaviorGroup>;
  clearBehaviorGroupsMap(): void;

  getPropertySetsMap(): jspb.Map<string, core_pb.PropertySet>;
  clearPropertySetsMap(): void;

  getTemplateFormulasMap(): jspb.Map<string, core_pb.TemplateFormula>;
  clearTemplateFormulasMap(): void;

  getTemplateDefinitionsMap(): jspb.Map<string, core_pb.TemplateDefinition>;
  clearTemplateDefinitionsMap(): void;

  getTokenTemplateHierarchy(): Hierarchy | undefined;
  setTokenTemplateHierarchy(value?: Hierarchy): void;
  hasTokenTemplateHierarchy(): boolean;
  clearTokenTemplateHierarchy(): void;

  getFormulaGrammar(): artifact_pb.FormulaGrammar | undefined;
  setFormulaGrammar(value?: artifact_pb.FormulaGrammar): void;
  hasFormulaGrammar(): boolean;
  clearFormulaGrammar(): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): Taxonomy.AsObject;
  static toObject(includeInstance: boolean, msg: Taxonomy): Taxonomy.AsObject;
  static serializeBinaryToWriter(message: Taxonomy, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): Taxonomy;
  static deserializeBinaryFromReader(message: Taxonomy, reader: jspb.BinaryReader): Taxonomy;
}

export namespace Taxonomy {
  export type AsObject = {
    version?: TaxonomyVersion.AsObject,
    baseTokenTypesMap: Array<[string, core_pb.Base.AsObject]>,
    behaviorsMap: Array<[string, core_pb.Behavior.AsObject]>,
    behaviorGroupsMap: Array<[string, core_pb.BehaviorGroup.AsObject]>,
    propertySetsMap: Array<[string, core_pb.PropertySet.AsObject]>,
    templateFormulasMap: Array<[string, core_pb.TemplateFormula.AsObject]>,
    templateDefinitionsMap: Array<[string, core_pb.TemplateDefinition.AsObject]>,
    tokenTemplateHierarchy?: Hierarchy.AsObject,
    formulaGrammar?: artifact_pb.FormulaGrammar.AsObject,
  }
}

export class TaxonomyVersion extends jspb.Message {
  getId(): string;
  setId(value: string): void;

  getVersion(): string;
  setVersion(value: string): void;

  getStateHash(): string;
  setStateHash(value: string): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): TaxonomyVersion.AsObject;
  static toObject(includeInstance: boolean, msg: TaxonomyVersion): TaxonomyVersion.AsObject;
  static serializeBinaryToWriter(message: TaxonomyVersion, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): TaxonomyVersion;
  static deserializeBinaryFromReader(message: TaxonomyVersion, reader: jspb.BinaryReader): TaxonomyVersion;
}

export namespace TaxonomyVersion {
  export type AsObject = {
    id: string,
    version: string,
    stateHash: string,
  }
}

export class Hierarchy extends jspb.Message {
  getFungibles(): BranchRoot | undefined;
  setFungibles(value?: BranchRoot): void;
  hasFungibles(): boolean;
  clearFungibles(): void;

  getNonFungibles(): BranchRoot | undefined;
  setNonFungibles(value?: BranchRoot): void;
  hasNonFungibles(): boolean;
  clearNonFungibles(): void;

  getHybrids(): HybridBranchRoot | undefined;
  setHybrids(value?: HybridBranchRoot): void;
  hasHybrids(): boolean;
  clearHybrids(): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): Hierarchy.AsObject;
  static toObject(includeInstance: boolean, msg: Hierarchy): Hierarchy.AsObject;
  static serializeBinaryToWriter(message: Hierarchy, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): Hierarchy;
  static deserializeBinaryFromReader(message: Hierarchy, reader: jspb.BinaryReader): Hierarchy;
}

export namespace Hierarchy {
  export type AsObject = {
    fungibles?: BranchRoot.AsObject,
    nonFungibles?: BranchRoot.AsObject,
    hybrids?: HybridBranchRoot.AsObject,
  }
}

export class BranchIdentifier extends jspb.Message {
  getTokenType(): artifact_pb.TokenType;
  setTokenType(value: artifact_pb.TokenType): void;

  getBranch(): artifact_pb.ClassificationBranch;
  setBranch(value: artifact_pb.ClassificationBranch): void;

  getFormulaId(): string;
  setFormulaId(value: string): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): BranchIdentifier.AsObject;
  static toObject(includeInstance: boolean, msg: BranchIdentifier): BranchIdentifier.AsObject;
  static serializeBinaryToWriter(message: BranchIdentifier, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): BranchIdentifier;
  static deserializeBinaryFromReader(message: BranchIdentifier, reader: jspb.BinaryReader): BranchIdentifier;
}

export namespace BranchIdentifier {
  export type AsObject = {
    tokenType: artifact_pb.TokenType,
    branch: artifact_pb.ClassificationBranch,
    formulaId: string,
  }
}

export class BranchRoot extends jspb.Message {
  getBranchIdentifier(): BranchIdentifier | undefined;
  setBranchIdentifier(value?: BranchIdentifier): void;
  hasBranchIdentifier(): boolean;
  clearBranchIdentifier(): void;

  getParentFormulaId(): string;
  setParentFormulaId(value: string): void;

  getName(): string;
  setName(value: string): void;

  getBranchFormula(): core_pb.TemplateFormula | undefined;
  setBranchFormula(value?: core_pb.TemplateFormula): void;
  hasBranchFormula(): boolean;
  clearBranchFormula(): void;

  getTemplates(): core_pb.TokenTemplates | undefined;
  setTemplates(value?: core_pb.TokenTemplates): void;
  hasTemplates(): boolean;
  clearTemplates(): void;

  getBranchesList(): Array<BranchRoot>;
  setBranchesList(value: Array<BranchRoot>): void;
  clearBranchesList(): void;
  addBranches(value?: BranchRoot, index?: number): BranchRoot;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): BranchRoot.AsObject;
  static toObject(includeInstance: boolean, msg: BranchRoot): BranchRoot.AsObject;
  static serializeBinaryToWriter(message: BranchRoot, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): BranchRoot;
  static deserializeBinaryFromReader(message: BranchRoot, reader: jspb.BinaryReader): BranchRoot;
}

export namespace BranchRoot {
  export type AsObject = {
    branchIdentifier?: BranchIdentifier.AsObject,
    parentFormulaId: string,
    name: string,
    branchFormula?: core_pb.TemplateFormula.AsObject,
    templates?: core_pb.TokenTemplates.AsObject,
    branchesList: Array<BranchRoot.AsObject>,
  }
}

export class HybridBranchRoot extends jspb.Message {
  getFungibleParent(): BranchRoot | undefined;
  setFungibleParent(value?: BranchRoot): void;
  hasFungibleParent(): boolean;
  clearFungibleParent(): void;

  getNonFungibleParent(): BranchRoot | undefined;
  setNonFungibleParent(value?: BranchRoot): void;
  hasNonFungibleParent(): boolean;
  clearNonFungibleParent(): void;

  serializeBinary(): Uint8Array;
  toObject(includeInstance?: boolean): HybridBranchRoot.AsObject;
  static toObject(includeInstance: boolean, msg: HybridBranchRoot): HybridBranchRoot.AsObject;
  static serializeBinaryToWriter(message: HybridBranchRoot, writer: jspb.BinaryWriter): void;
  static deserializeBinary(bytes: Uint8Array): HybridBranchRoot;
  static deserializeBinaryFromReader(message: HybridBranchRoot, reader: jspb.BinaryReader): HybridBranchRoot;
}

export namespace HybridBranchRoot {
  export type AsObject = {
    fungibleParent?: BranchRoot.AsObject,
    nonFungibleParent?: BranchRoot.AsObject,
  }
}


// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: artifact.proto

package org.tti.tokens.model.artifact;

/**
 * Protobuf type {@code taxonomy.model.artifact.ArtifactSymbol}
 */
public  final class ArtifactSymbol extends
    com.google.protobuf.GeneratedMessageV3 implements
    // @@protoc_insertion_point(message_implements:taxonomy.model.artifact.ArtifactSymbol)
    ArtifactSymbolOrBuilder {
private static final long serialVersionUID = 0L;
  // Use ArtifactSymbol.newBuilder() to construct.
  private ArtifactSymbol(com.google.protobuf.GeneratedMessageV3.Builder<?> builder) {
    super(builder);
  }
  private ArtifactSymbol() {
    visualSymbol_ = "";
    toolingSymbol_ = "";
  }

  @java.lang.Override
  public final com.google.protobuf.UnknownFieldSet
  getUnknownFields() {
    return this.unknownFields;
  }
  private ArtifactSymbol(
      com.google.protobuf.CodedInputStream input,
      com.google.protobuf.ExtensionRegistryLite extensionRegistry)
      throws com.google.protobuf.InvalidProtocolBufferException {
    this();
    if (extensionRegistry == null) {
      throw new java.lang.NullPointerException();
    }
    int mutable_bitField0_ = 0;
    com.google.protobuf.UnknownFieldSet.Builder unknownFields =
        com.google.protobuf.UnknownFieldSet.newBuilder();
    try {
      boolean done = false;
      while (!done) {
        int tag = input.readTag();
        switch (tag) {
          case 0:
            done = true;
            break;
          case 34: {
            java.lang.String s = input.readStringRequireUtf8();

            visualSymbol_ = s;
            break;
          }
          case 42: {
            java.lang.String s = input.readStringRequireUtf8();

            toolingSymbol_ = s;
            break;
          }
          default: {
            if (!parseUnknownField(
                input, unknownFields, extensionRegistry, tag)) {
              done = true;
            }
            break;
          }
        }
      }
    } catch (com.google.protobuf.InvalidProtocolBufferException e) {
      throw e.setUnfinishedMessage(this);
    } catch (java.io.IOException e) {
      throw new com.google.protobuf.InvalidProtocolBufferException(
          e).setUnfinishedMessage(this);
    } finally {
      this.unknownFields = unknownFields.build();
      makeExtensionsImmutable();
    }
  }
  public static final com.google.protobuf.Descriptors.Descriptor
      getDescriptor() {
    return org.tti.tokens.model.artifact.ArtifactOuterClass.internal_static_taxonomy_model_artifact_ArtifactSymbol_descriptor;
  }

  @java.lang.Override
  protected com.google.protobuf.GeneratedMessageV3.FieldAccessorTable
      internalGetFieldAccessorTable() {
    return org.tti.tokens.model.artifact.ArtifactOuterClass.internal_static_taxonomy_model_artifact_ArtifactSymbol_fieldAccessorTable
        .ensureFieldAccessorsInitialized(
            org.tti.tokens.model.artifact.ArtifactSymbol.class, org.tti.tokens.model.artifact.ArtifactSymbol.Builder.class);
  }

  public static final int VISUAL_SYMBOL_FIELD_NUMBER = 4;
  private volatile java.lang.Object visualSymbol_;
  /**
   * <code>string visual_symbol = 4;</code>
   */
  public java.lang.String getVisualSymbol() {
    java.lang.Object ref = visualSymbol_;
    if (ref instanceof java.lang.String) {
      return (java.lang.String) ref;
    } else {
      com.google.protobuf.ByteString bs = 
          (com.google.protobuf.ByteString) ref;
      java.lang.String s = bs.toStringUtf8();
      visualSymbol_ = s;
      return s;
    }
  }
  /**
   * <code>string visual_symbol = 4;</code>
   */
  public com.google.protobuf.ByteString
      getVisualSymbolBytes() {
    java.lang.Object ref = visualSymbol_;
    if (ref instanceof java.lang.String) {
      com.google.protobuf.ByteString b = 
          com.google.protobuf.ByteString.copyFromUtf8(
              (java.lang.String) ref);
      visualSymbol_ = b;
      return b;
    } else {
      return (com.google.protobuf.ByteString) ref;
    }
  }

  public static final int TOOLING_SYMBOL_FIELD_NUMBER = 5;
  private volatile java.lang.Object toolingSymbol_;
  /**
   * <code>string tooling_symbol = 5;</code>
   */
  public java.lang.String getToolingSymbol() {
    java.lang.Object ref = toolingSymbol_;
    if (ref instanceof java.lang.String) {
      return (java.lang.String) ref;
    } else {
      com.google.protobuf.ByteString bs = 
          (com.google.protobuf.ByteString) ref;
      java.lang.String s = bs.toStringUtf8();
      toolingSymbol_ = s;
      return s;
    }
  }
  /**
   * <code>string tooling_symbol = 5;</code>
   */
  public com.google.protobuf.ByteString
      getToolingSymbolBytes() {
    java.lang.Object ref = toolingSymbol_;
    if (ref instanceof java.lang.String) {
      com.google.protobuf.ByteString b = 
          com.google.protobuf.ByteString.copyFromUtf8(
              (java.lang.String) ref);
      toolingSymbol_ = b;
      return b;
    } else {
      return (com.google.protobuf.ByteString) ref;
    }
  }

  private byte memoizedIsInitialized = -1;
  @java.lang.Override
  public final boolean isInitialized() {
    byte isInitialized = memoizedIsInitialized;
    if (isInitialized == 1) return true;
    if (isInitialized == 0) return false;

    memoizedIsInitialized = 1;
    return true;
  }

  @java.lang.Override
  public void writeTo(com.google.protobuf.CodedOutputStream output)
                      throws java.io.IOException {
    if (!getVisualSymbolBytes().isEmpty()) {
      com.google.protobuf.GeneratedMessageV3.writeString(output, 4, visualSymbol_);
    }
    if (!getToolingSymbolBytes().isEmpty()) {
      com.google.protobuf.GeneratedMessageV3.writeString(output, 5, toolingSymbol_);
    }
    unknownFields.writeTo(output);
  }

  @java.lang.Override
  public int getSerializedSize() {
    int size = memoizedSize;
    if (size != -1) return size;

    size = 0;
    if (!getVisualSymbolBytes().isEmpty()) {
      size += com.google.protobuf.GeneratedMessageV3.computeStringSize(4, visualSymbol_);
    }
    if (!getToolingSymbolBytes().isEmpty()) {
      size += com.google.protobuf.GeneratedMessageV3.computeStringSize(5, toolingSymbol_);
    }
    size += unknownFields.getSerializedSize();
    memoizedSize = size;
    return size;
  }

  @java.lang.Override
  public boolean equals(final java.lang.Object obj) {
    if (obj == this) {
     return true;
    }
    if (!(obj instanceof org.tti.tokens.model.artifact.ArtifactSymbol)) {
      return super.equals(obj);
    }
    org.tti.tokens.model.artifact.ArtifactSymbol other = (org.tti.tokens.model.artifact.ArtifactSymbol) obj;

    if (!getVisualSymbol()
        .equals(other.getVisualSymbol())) return false;
    if (!getToolingSymbol()
        .equals(other.getToolingSymbol())) return false;
    if (!unknownFields.equals(other.unknownFields)) return false;
    return true;
  }

  @java.lang.Override
  public int hashCode() {
    if (memoizedHashCode != 0) {
      return memoizedHashCode;
    }
    int hash = 41;
    hash = (19 * hash) + getDescriptor().hashCode();
    hash = (37 * hash) + VISUAL_SYMBOL_FIELD_NUMBER;
    hash = (53 * hash) + getVisualSymbol().hashCode();
    hash = (37 * hash) + TOOLING_SYMBOL_FIELD_NUMBER;
    hash = (53 * hash) + getToolingSymbol().hashCode();
    hash = (29 * hash) + unknownFields.hashCode();
    memoizedHashCode = hash;
    return hash;
  }

  public static org.tti.tokens.model.artifact.ArtifactSymbol parseFrom(
      java.nio.ByteBuffer data)
      throws com.google.protobuf.InvalidProtocolBufferException {
    return PARSER.parseFrom(data);
  }
  public static org.tti.tokens.model.artifact.ArtifactSymbol parseFrom(
      java.nio.ByteBuffer data,
      com.google.protobuf.ExtensionRegistryLite extensionRegistry)
      throws com.google.protobuf.InvalidProtocolBufferException {
    return PARSER.parseFrom(data, extensionRegistry);
  }
  public static org.tti.tokens.model.artifact.ArtifactSymbol parseFrom(
      com.google.protobuf.ByteString data)
      throws com.google.protobuf.InvalidProtocolBufferException {
    return PARSER.parseFrom(data);
  }
  public static org.tti.tokens.model.artifact.ArtifactSymbol parseFrom(
      com.google.protobuf.ByteString data,
      com.google.protobuf.ExtensionRegistryLite extensionRegistry)
      throws com.google.protobuf.InvalidProtocolBufferException {
    return PARSER.parseFrom(data, extensionRegistry);
  }
  public static org.tti.tokens.model.artifact.ArtifactSymbol parseFrom(byte[] data)
      throws com.google.protobuf.InvalidProtocolBufferException {
    return PARSER.parseFrom(data);
  }
  public static org.tti.tokens.model.artifact.ArtifactSymbol parseFrom(
      byte[] data,
      com.google.protobuf.ExtensionRegistryLite extensionRegistry)
      throws com.google.protobuf.InvalidProtocolBufferException {
    return PARSER.parseFrom(data, extensionRegistry);
  }
  public static org.tti.tokens.model.artifact.ArtifactSymbol parseFrom(java.io.InputStream input)
      throws java.io.IOException {
    return com.google.protobuf.GeneratedMessageV3
        .parseWithIOException(PARSER, input);
  }
  public static org.tti.tokens.model.artifact.ArtifactSymbol parseFrom(
      java.io.InputStream input,
      com.google.protobuf.ExtensionRegistryLite extensionRegistry)
      throws java.io.IOException {
    return com.google.protobuf.GeneratedMessageV3
        .parseWithIOException(PARSER, input, extensionRegistry);
  }
  public static org.tti.tokens.model.artifact.ArtifactSymbol parseDelimitedFrom(java.io.InputStream input)
      throws java.io.IOException {
    return com.google.protobuf.GeneratedMessageV3
        .parseDelimitedWithIOException(PARSER, input);
  }
  public static org.tti.tokens.model.artifact.ArtifactSymbol parseDelimitedFrom(
      java.io.InputStream input,
      com.google.protobuf.ExtensionRegistryLite extensionRegistry)
      throws java.io.IOException {
    return com.google.protobuf.GeneratedMessageV3
        .parseDelimitedWithIOException(PARSER, input, extensionRegistry);
  }
  public static org.tti.tokens.model.artifact.ArtifactSymbol parseFrom(
      com.google.protobuf.CodedInputStream input)
      throws java.io.IOException {
    return com.google.protobuf.GeneratedMessageV3
        .parseWithIOException(PARSER, input);
  }
  public static org.tti.tokens.model.artifact.ArtifactSymbol parseFrom(
      com.google.protobuf.CodedInputStream input,
      com.google.protobuf.ExtensionRegistryLite extensionRegistry)
      throws java.io.IOException {
    return com.google.protobuf.GeneratedMessageV3
        .parseWithIOException(PARSER, input, extensionRegistry);
  }

  @java.lang.Override
  public Builder newBuilderForType() { return newBuilder(); }
  public static Builder newBuilder() {
    return DEFAULT_INSTANCE.toBuilder();
  }
  public static Builder newBuilder(org.tti.tokens.model.artifact.ArtifactSymbol prototype) {
    return DEFAULT_INSTANCE.toBuilder().mergeFrom(prototype);
  }
  @java.lang.Override
  public Builder toBuilder() {
    return this == DEFAULT_INSTANCE
        ? new Builder() : new Builder().mergeFrom(this);
  }

  @java.lang.Override
  protected Builder newBuilderForType(
      com.google.protobuf.GeneratedMessageV3.BuilderParent parent) {
    Builder builder = new Builder(parent);
    return builder;
  }
  /**
   * Protobuf type {@code taxonomy.model.artifact.ArtifactSymbol}
   */
  public static final class Builder extends
      com.google.protobuf.GeneratedMessageV3.Builder<Builder> implements
      // @@protoc_insertion_point(builder_implements:taxonomy.model.artifact.ArtifactSymbol)
      org.tti.tokens.model.artifact.ArtifactSymbolOrBuilder {
    public static final com.google.protobuf.Descriptors.Descriptor
        getDescriptor() {
      return org.tti.tokens.model.artifact.ArtifactOuterClass.internal_static_taxonomy_model_artifact_ArtifactSymbol_descriptor;
    }

    @java.lang.Override
    protected com.google.protobuf.GeneratedMessageV3.FieldAccessorTable
        internalGetFieldAccessorTable() {
      return org.tti.tokens.model.artifact.ArtifactOuterClass.internal_static_taxonomy_model_artifact_ArtifactSymbol_fieldAccessorTable
          .ensureFieldAccessorsInitialized(
              org.tti.tokens.model.artifact.ArtifactSymbol.class, org.tti.tokens.model.artifact.ArtifactSymbol.Builder.class);
    }

    // Construct using org.tti.tokens.model.artifact.ArtifactSymbol.newBuilder()
    private Builder() {
      maybeForceBuilderInitialization();
    }

    private Builder(
        com.google.protobuf.GeneratedMessageV3.BuilderParent parent) {
      super(parent);
      maybeForceBuilderInitialization();
    }
    private void maybeForceBuilderInitialization() {
      if (com.google.protobuf.GeneratedMessageV3
              .alwaysUseFieldBuilders) {
      }
    }
    @java.lang.Override
    public Builder clear() {
      super.clear();
      visualSymbol_ = "";

      toolingSymbol_ = "";

      return this;
    }

    @java.lang.Override
    public com.google.protobuf.Descriptors.Descriptor
        getDescriptorForType() {
      return org.tti.tokens.model.artifact.ArtifactOuterClass.internal_static_taxonomy_model_artifact_ArtifactSymbol_descriptor;
    }

    @java.lang.Override
    public org.tti.tokens.model.artifact.ArtifactSymbol getDefaultInstanceForType() {
      return org.tti.tokens.model.artifact.ArtifactSymbol.getDefaultInstance();
    }

    @java.lang.Override
    public org.tti.tokens.model.artifact.ArtifactSymbol build() {
      org.tti.tokens.model.artifact.ArtifactSymbol result = buildPartial();
      if (!result.isInitialized()) {
        throw newUninitializedMessageException(result);
      }
      return result;
    }

    @java.lang.Override
    public org.tti.tokens.model.artifact.ArtifactSymbol buildPartial() {
      org.tti.tokens.model.artifact.ArtifactSymbol result = new org.tti.tokens.model.artifact.ArtifactSymbol(this);
      result.visualSymbol_ = visualSymbol_;
      result.toolingSymbol_ = toolingSymbol_;
      onBuilt();
      return result;
    }

    @java.lang.Override
    public Builder clone() {
      return super.clone();
    }
    @java.lang.Override
    public Builder setField(
        com.google.protobuf.Descriptors.FieldDescriptor field,
        java.lang.Object value) {
      return super.setField(field, value);
    }
    @java.lang.Override
    public Builder clearField(
        com.google.protobuf.Descriptors.FieldDescriptor field) {
      return super.clearField(field);
    }
    @java.lang.Override
    public Builder clearOneof(
        com.google.protobuf.Descriptors.OneofDescriptor oneof) {
      return super.clearOneof(oneof);
    }
    @java.lang.Override
    public Builder setRepeatedField(
        com.google.protobuf.Descriptors.FieldDescriptor field,
        int index, java.lang.Object value) {
      return super.setRepeatedField(field, index, value);
    }
    @java.lang.Override
    public Builder addRepeatedField(
        com.google.protobuf.Descriptors.FieldDescriptor field,
        java.lang.Object value) {
      return super.addRepeatedField(field, value);
    }
    @java.lang.Override
    public Builder mergeFrom(com.google.protobuf.Message other) {
      if (other instanceof org.tti.tokens.model.artifact.ArtifactSymbol) {
        return mergeFrom((org.tti.tokens.model.artifact.ArtifactSymbol)other);
      } else {
        super.mergeFrom(other);
        return this;
      }
    }

    public Builder mergeFrom(org.tti.tokens.model.artifact.ArtifactSymbol other) {
      if (other == org.tti.tokens.model.artifact.ArtifactSymbol.getDefaultInstance()) return this;
      if (!other.getVisualSymbol().isEmpty()) {
        visualSymbol_ = other.visualSymbol_;
        onChanged();
      }
      if (!other.getToolingSymbol().isEmpty()) {
        toolingSymbol_ = other.toolingSymbol_;
        onChanged();
      }
      this.mergeUnknownFields(other.unknownFields);
      onChanged();
      return this;
    }

    @java.lang.Override
    public final boolean isInitialized() {
      return true;
    }

    @java.lang.Override
    public Builder mergeFrom(
        com.google.protobuf.CodedInputStream input,
        com.google.protobuf.ExtensionRegistryLite extensionRegistry)
        throws java.io.IOException {
      org.tti.tokens.model.artifact.ArtifactSymbol parsedMessage = null;
      try {
        parsedMessage = PARSER.parsePartialFrom(input, extensionRegistry);
      } catch (com.google.protobuf.InvalidProtocolBufferException e) {
        parsedMessage = (org.tti.tokens.model.artifact.ArtifactSymbol) e.getUnfinishedMessage();
        throw e.unwrapIOException();
      } finally {
        if (parsedMessage != null) {
          mergeFrom(parsedMessage);
        }
      }
      return this;
    }

    private java.lang.Object visualSymbol_ = "";
    /**
     * <code>string visual_symbol = 4;</code>
     */
    public java.lang.String getVisualSymbol() {
      java.lang.Object ref = visualSymbol_;
      if (!(ref instanceof java.lang.String)) {
        com.google.protobuf.ByteString bs =
            (com.google.protobuf.ByteString) ref;
        java.lang.String s = bs.toStringUtf8();
        visualSymbol_ = s;
        return s;
      } else {
        return (java.lang.String) ref;
      }
    }
    /**
     * <code>string visual_symbol = 4;</code>
     */
    public com.google.protobuf.ByteString
        getVisualSymbolBytes() {
      java.lang.Object ref = visualSymbol_;
      if (ref instanceof String) {
        com.google.protobuf.ByteString b = 
            com.google.protobuf.ByteString.copyFromUtf8(
                (java.lang.String) ref);
        visualSymbol_ = b;
        return b;
      } else {
        return (com.google.protobuf.ByteString) ref;
      }
    }
    /**
     * <code>string visual_symbol = 4;</code>
     */
    public Builder setVisualSymbol(
        java.lang.String value) {
      if (value == null) {
    throw new NullPointerException();
  }
  
      visualSymbol_ = value;
      onChanged();
      return this;
    }
    /**
     * <code>string visual_symbol = 4;</code>
     */
    public Builder clearVisualSymbol() {
      
      visualSymbol_ = getDefaultInstance().getVisualSymbol();
      onChanged();
      return this;
    }
    /**
     * <code>string visual_symbol = 4;</code>
     */
    public Builder setVisualSymbolBytes(
        com.google.protobuf.ByteString value) {
      if (value == null) {
    throw new NullPointerException();
  }
  checkByteStringIsUtf8(value);
      
      visualSymbol_ = value;
      onChanged();
      return this;
    }

    private java.lang.Object toolingSymbol_ = "";
    /**
     * <code>string tooling_symbol = 5;</code>
     */
    public java.lang.String getToolingSymbol() {
      java.lang.Object ref = toolingSymbol_;
      if (!(ref instanceof java.lang.String)) {
        com.google.protobuf.ByteString bs =
            (com.google.protobuf.ByteString) ref;
        java.lang.String s = bs.toStringUtf8();
        toolingSymbol_ = s;
        return s;
      } else {
        return (java.lang.String) ref;
      }
    }
    /**
     * <code>string tooling_symbol = 5;</code>
     */
    public com.google.protobuf.ByteString
        getToolingSymbolBytes() {
      java.lang.Object ref = toolingSymbol_;
      if (ref instanceof String) {
        com.google.protobuf.ByteString b = 
            com.google.protobuf.ByteString.copyFromUtf8(
                (java.lang.String) ref);
        toolingSymbol_ = b;
        return b;
      } else {
        return (com.google.protobuf.ByteString) ref;
      }
    }
    /**
     * <code>string tooling_symbol = 5;</code>
     */
    public Builder setToolingSymbol(
        java.lang.String value) {
      if (value == null) {
    throw new NullPointerException();
  }
  
      toolingSymbol_ = value;
      onChanged();
      return this;
    }
    /**
     * <code>string tooling_symbol = 5;</code>
     */
    public Builder clearToolingSymbol() {
      
      toolingSymbol_ = getDefaultInstance().getToolingSymbol();
      onChanged();
      return this;
    }
    /**
     * <code>string tooling_symbol = 5;</code>
     */
    public Builder setToolingSymbolBytes(
        com.google.protobuf.ByteString value) {
      if (value == null) {
    throw new NullPointerException();
  }
  checkByteStringIsUtf8(value);
      
      toolingSymbol_ = value;
      onChanged();
      return this;
    }
    @java.lang.Override
    public final Builder setUnknownFields(
        final com.google.protobuf.UnknownFieldSet unknownFields) {
      return super.setUnknownFields(unknownFields);
    }

    @java.lang.Override
    public final Builder mergeUnknownFields(
        final com.google.protobuf.UnknownFieldSet unknownFields) {
      return super.mergeUnknownFields(unknownFields);
    }


    // @@protoc_insertion_point(builder_scope:taxonomy.model.artifact.ArtifactSymbol)
  }

  // @@protoc_insertion_point(class_scope:taxonomy.model.artifact.ArtifactSymbol)
  private static final org.tti.tokens.model.artifact.ArtifactSymbol DEFAULT_INSTANCE;
  static {
    DEFAULT_INSTANCE = new org.tti.tokens.model.artifact.ArtifactSymbol();
  }

  public static org.tti.tokens.model.artifact.ArtifactSymbol getDefaultInstance() {
    return DEFAULT_INSTANCE;
  }

  private static final com.google.protobuf.Parser<ArtifactSymbol>
      PARSER = new com.google.protobuf.AbstractParser<ArtifactSymbol>() {
    @java.lang.Override
    public ArtifactSymbol parsePartialFrom(
        com.google.protobuf.CodedInputStream input,
        com.google.protobuf.ExtensionRegistryLite extensionRegistry)
        throws com.google.protobuf.InvalidProtocolBufferException {
      return new ArtifactSymbol(input, extensionRegistry);
    }
  };

  public static com.google.protobuf.Parser<ArtifactSymbol> parser() {
    return PARSER;
  }

  @java.lang.Override
  public com.google.protobuf.Parser<ArtifactSymbol> getParserForType() {
    return PARSER;
  }

  @java.lang.Override
  public org.tti.tokens.model.artifact.ArtifactSymbol getDefaultInstanceForType() {
    return DEFAULT_INSTANCE;
  }

}


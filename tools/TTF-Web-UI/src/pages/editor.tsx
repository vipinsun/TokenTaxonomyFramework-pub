import React from "react";
import {connect} from 'react-redux';
import {
  Form,
  Input,
  Button
} from 'antd';
import KeyValue from "../components/form/keyvalue";
import {WrappedFormUtils} from "antd/lib/form/Form";
import {FormComponentProps} from "antd/es/form";
import {IStoreState} from "../store/IStoreState";
import TextArea from "antd/es/input/TextArea";
import {Artifact, ArtifactType, UpdateArtifactRequest} from "../model/artifact_pb";
import {Any} from "google-protobuf/google/protobuf/any_pb";
import {BinaryWriter} from "google-protobuf";
import {client} from "../state";

const editable = true;
const formItemLayout = {
  labelCol: {
    xs: {span: 3},
    sm: {span: 8},
  },
  wrapperCol: {
    xs: {span: 24},
    sm: {span: 16},
  },
};

interface BaseFormProps extends FormComponentProps {
  state: IStoreState;
  entityType: string | null;
  currentEntity: string | null;
}

function getCurrentArtifact(artifact: Artifact, id: string)  {
    if (artifact) {
      const symbol = artifact.getArtifactSymbol();
      if (symbol && symbol.getId() === id) {
        return true;
      }
    } else {
      return false;
    }
}

class BaseForm extends React.Component<BaseFormProps, any> {

  constructor(props: BaseFormProps) {
    super(props);
    this.handleSubmit = this.handleSubmit.bind(this)
  }

  public handleSubmit() {
    const form = this.props.form;
    const entityType = this.props.entityType;
    switch (entityType) {
      case "behavior":
        const behavior = this.props.state.ui.sidebarUI.behaviors
          .find((el) => getCurrentArtifact(el.getArtifact()!!, this.props.currentEntity!!));

        if (!behavior) {
          return
        }

        // edit

        const request = new UpdateArtifactRequest();
        request.setType(ArtifactType.BEHAVIOR);

        const serialized = behavior.serializeBinary();

        const any = new Any();
        any.pack(serialized, "proto.taxonomy.model.core.Behavior");

        request.setArtifactTypeObject(any);

        client.updateArtifact(request, null, (err, response) => {
          console.log('err - ', err);
          console.log('resp - ', response);
        });
    }
  }

  public render() {
    const form: WrappedFormUtils = this.props.form;
    const state = this.props.state;
    const currentID = this.props.currentEntity;
    const entityType = this.props.entityType;
    const selected = currentID === null ? undefined : state.entities.bases.byId.get(currentID) ||
      state.entities.behaviors.byId.get(currentID) ||
      state.entities.behaviorGroups.byId.get(currentID) ||
      state.entities.propertySets.byId.get(currentID) ||
      state.entities.templateDefinitions.byId.get(currentID);


    if (selected === undefined || entityType === null) {
      return <div/>;
    } else {
      const formName = selected.getName();

      return <div className={"form"}>
        <Form {...formItemLayout}>
          <h2>{formName}</h2>
          <div className="form-wrapper">
            <h3 dangerouslySetInnerHTML={{
              __html: "Edit " +
                selected.getArtifactSymbol()!.getVisual()
            }}/>
            {this.renderForm()}
            {editable ?
              <div className="submit-wrapper">
                <Form.Item
                  wrapperCol={{
                    xs: {span: 24, offset: 0},
                    sm: {span: 16, offset: 8},
                  }}
                >
                  <Button type="primary" htmlType="submit" onClick={this.handleSubmit} className="submit">Submit</Button>
                </Form.Item>
              </div> : null}
          </div>
        </Form>
      </div>;
    }
  }


  private renderForm() {
    const form = this.props.form;
    const {getFieldDecorator} = this.props.form;
    const entityType = this.props.entityType;
    switch (entityType) {
      case 'base':
        return (
          <React.Fragment>
            <div className="inputs-wrapper">
              <Form.Item label="Name">
                {getFieldDecorator('name', {
                  rules: [
                    {
                      required: false,
                      message: 'Please input a token name',
                    },
                  ],
                })(<Input disabled={!editable}/>)}
              </Form.Item>
              <Form.Item label="Symbol">
                {getFieldDecorator('symbol', {
                  rules: [
                    {
                      required: false,
                      message: 'Please input a token symbol',
                    },
                  ],
                })(<Input disabled={!editable}/>)}
              </Form.Item>
              <Form.Item label="Owner">
                {getFieldDecorator('owner', {
                  rules: [
                    {
                      required: false,
                      message: 'Please input a token owner',
                    },
                  ],
                })(<Input disabled={!editable}/>)}
              </Form.Item>
              <Form.Item label="Quantity">
                {getFieldDecorator('quantity', {
                  rules: [
                    {
                      required: false,
                      message: 'Please enter a quantity',
                    },
                  ],
                })(<Input disabled={!editable}/>)}
              </Form.Item>
              <Form.Item label="Decimals">
                {getFieldDecorator('decimals', {
                  rules: [
                    {
                      required: false,
                      message: 'Please enter the number of decimals allowed',
                    },
                  ],
                })(<Input disabled={!editable}/>)}
              </Form.Item>
              <Form.Item label="Constructor name">
                {getFieldDecorator('constructor_name', {
                  rules: [
                    {
                      required: false,
                      message: 'Please enter the name of the constructor',
                    },
                  ],
                })(<Input disabled={!editable}/>)}
              </Form.Item>
            </div>
            <div className="add-wrapper">
              <Form.Item
                wrapperCol={{
                  xs: {span: 24, offset: 8},
                  sm: {span: 20, offset: 5},
                }}
              >
                {new KeyValue({
                  field: "token_properties",
                  form: form,
                  label: "Token properties",
                  disabled: !editable
                }).render()}
              </Form.Item>
            </div>
          </React.Fragment>
        );
      case 'behavior':
        return (
          <React.Fragment>
            <div className="inputs-wrapper">
              <Form.Item label="Name">
                {getFieldDecorator('name', {
                  rules: [
                    {
                      required: false,
                      message: 'Please input a token name',
                    },
                  ],
                })(<Input disabled={!editable}/>)}
              </Form.Item>
              <Form.Item label="Symbol">
                {getFieldDecorator('symbol', {
                  rules: [
                    {
                      required: false,
                      message: 'Please input a token symbol',
                    },
                  ],
                })(<Input disabled={!editable}/>)}
              </Form.Item>
              <Form.Item label="Is external">
                {getFieldDecorator('isExternal', {
                  rules: [
                    {
                      required: false,
                      message: 'Please input a token is external',
                    },
                  ],
                })(<Input disabled={!editable}/>)}
              </Form.Item>
              <Form.Item label="Constructor type">
                {getFieldDecorator('constructorType', {
                  rules: [
                    {
                      required: false,
                      message: 'Please input a token constructor type',
                    },
                  ],
                })(<Input disabled={!editable}/>)}
              </Form.Item>
              <Form.Item label="Invocation list">
                {getFieldDecorator('invocationList', {
                  rules: [
                    {
                      required: false,
                      message: 'Please input a token invocation list',
                    },
                  ],
                })(<TextArea disabled={!editable}/>)}
              </Form.Item>
              <Form.Item label="Properties list">
                {getFieldDecorator('behaviorPropertiesList', {
                  rules: [
                    {
                      required: false,
                      message: 'Please input a token properties list',
                    },
                  ],
                })(<TextArea disabled={!editable}/>)}
              </Form.Item>
            </div>
          </React.Fragment>
        );
      case 'behaviorGroup':
        return (
          <React.Fragment>
            <div className="inputs-wrapper">
              <Form.Item label="Name">
                {getFieldDecorator('name', {
                  rules: [
                    {
                      required: false,
                      message: 'Please input a token name',
                    },
                  ],
                })(<Input disabled={!editable}/>)}
              </Form.Item>
              <Form.Item label="Symbol">
                {getFieldDecorator('symbol', {
                  rules: [
                    {
                      required: false,
                      message: 'Please input a token symbol',
                    },
                  ],
                })(<Input disabled={!editable}/>)}
              </Form.Item>
              <Form.Item label="Behaviors list">
                {getFieldDecorator('behaviorGroupBehaviorsList', {
                  rules: [
                    {
                      required: false,
                      message: 'Please input a token behaviors list',
                    },
                  ],
                })(<TextArea disabled={!editable}/>)}
              </Form.Item>
            </div>
          </React.Fragment>
        );
      case 'propertySet':
        return (
          <React.Fragment>
            <div className="inputs-wrapper">
              <Form.Item label="Name">
                {getFieldDecorator('name', {
                  rules: [
                    {
                      required: false,
                      message: 'Please input a token name',
                    },
                  ],
                })(<Input disabled={!editable}/>)}
              </Form.Item>
              <Form.Item label="Symbol">
                {getFieldDecorator('symbol', {
                  rules: [
                    {
                      required: false,
                      message: 'Please input a token symbol',
                    },
                  ],
                })(<Input disabled={!editable}/>)}
              </Form.Item>
              <Form.Item label="Properties list">
                {getFieldDecorator('propertySetPropertiesList', {
                  rules: [
                    {
                      required: false,
                      message: 'Please input a token properties list',
                    },
                  ],
                })(<Input disabled={!editable}/>)}
              </Form.Item>
            </div>
          </React.Fragment>
        );
      case 'templateDefinition':
        return (
          <React.Fragment>
            <div className="inputs-wrapper">
              <Form.Item label="Name">
                {getFieldDecorator('name', {
                  rules: [
                    {
                      required: false,
                      message: 'Please input a token name',
                    },
                  ],
                })(<Input disabled={!editable}/>)}
              </Form.Item>
              <Form.Item label="Symbol">
                {getFieldDecorator('symbol', {
                  rules: [
                    {
                      required: false,
                      message: 'Please input a token symbol',
                    },
                  ],
                })(<Input disabled={!editable}/>)}
              </Form.Item>
              <Form.Item label="Properties list">
                {getFieldDecorator('templateDefinitionPropertiesList', {
                  rules: [
                    {
                      required: false,
                      message: 'Please input a token symbol',
                    },
                  ],
                })(<TextArea disabled={!editable}/>)}
              </Form.Item>
              <Form.Item label="Formula reference">
                {getFieldDecorator('formulaReference', {
                  rules: [
                    {
                      required: false,
                      message: 'Please input a token symbol',
                    },
                  ],
                })(<TextArea disabled={!editable}/>)}
              </Form.Item>
              <Form.Item label="Behaviors list">
                {getFieldDecorator('templateDefinitionBehaviorsList', {
                  rules: [
                    {
                      required: false,
                      message: 'Please input a token symbol',
                    },
                  ],
                })(<TextArea disabled={!editable}/>)}
              </Form.Item>
              <Form.Item label="Behavior groups list">
                {getFieldDecorator('templateDefinitionBehaviorGroupsList', {
                  rules: [
                    {
                      required: false,
                      message: 'Please input a token symbol',
                    },
                  ],
                })(<TextArea disabled={!editable}/>)}
              </Form.Item>
              <Form.Item label="Child tokens list">
                {getFieldDecorator('templateDefinitionChildTokensList', {
                  rules: [
                    {
                      required: false,
                      message: 'Please input a token symbol',
                    },
                  ],
                })(<TextArea disabled={!editable}/>)}
              </Form.Item>
              <Form.Item label="Token base">
                {getFieldDecorator('templateDefinitionTokenBase', {
                  rules: [
                    {
                      required: false,
                      message: 'Please input a token symbol',
                    },
                  ],
                })(<TextArea disabled={!editable}/>)}
              </Form.Item>
            </div>
          </React.Fragment>
        );
      default: return;
    }
  }
}

const Editor = Form.create<BaseFormProps>({
  mapPropsToFields: (props: BaseFormProps) => {
    const state = props.state;
    const currentID = props.currentEntity;
    const entityType = props.entityType;
    const selected = currentID === null ? undefined : state.entities.bases.byId.get(currentID) ||
      state.entities.behaviors.byId.get(currentID) ||
      state.entities.behaviorGroups.byId.get(currentID) ||
      state.entities.propertySets.byId.get(currentID) ||
      state.entities.templateDefinitions.byId.get(currentID);
    if (selected === undefined) {
      return {};
    } else {
      const symbol = selected.getArtifactSymbol()!.getVisual();
      const name = selected.getName();
      let data, quantity, decimals, owner, constructorName;
      let isExternal, constructorType, invocationList, behaviorPropertiesList;
      let behaviorGroupBehaviorsList, propertySetPropertiesList;
      let templateDefinitionPropertiesList, formulaReference, templateDefinitionBehaviorsList,
        templateDefinitionBehaviorGroupsList, templateDefinitionChildTokensList, templateDefinitionTokenBase;

      function getCurrentArtifact(el: any)  {
        if (el) {
          const artifact = el.getArtifact();
          if (artifact) {
            const symbol = artifact.getArtifactSymbol();
            if (symbol && symbol.getId() === currentID) {
              return el;
            }
          }
        }
      }

      switch (entityType) {
        case 'base':
          data = props.state.ui.sidebarUI.bases.find(getCurrentArtifact);
          quantity = data && data.getQuantity();
          decimals = data && data.getDecimals();
          owner = data && data.getOwner();
          constructorName = data && data.getConstructorName();

          return {
            name: Form.createFormField({
              name: name,
              value: name,
            }),
            symbol: Form.createFormField({
              name: symbol,
              value: symbol,
            }),
            quantity: Form.createFormField({
              name: quantity,
              value: quantity,
            }),
            decimals: Form.createFormField({
              name: decimals,
              value: decimals,
            }),
            owner: Form.createFormField({
              name: owner,
              value: owner,
            }),
            constructorName: Form.createFormField({
              name: constructorName,
              value: constructorName,
            })
          };

        case 'behavior':
          data = props.state.ui.sidebarUI.behaviors.find(getCurrentArtifact);
          isExternal = data && data.getIsExternal();
          constructorType = data && data.getConstructorType();
          invocationList = data && data.getInvocationsList();
          behaviorPropertiesList = data && data.getPropertiesList();

          return {
            name: Form.createFormField({
              name: name,
              value: name,
            }),
            symbol: Form.createFormField({
              name: symbol,
              value: symbol,
            }),
            isExternal: Form.createFormField ({
              name: isExternal,
              value: isExternal
            }),
            constructorType: Form.createFormField ({
              name: constructorType,
              value: constructorType
            }),
            invocationList: Form.createFormField ({
              name: invocationList,
              value: invocationList
            }),
            behaviorPropertiesList: Form.createFormField ({
              name: behaviorPropertiesList,
              value: behaviorPropertiesList
            })
          };

        case 'behaviorGroup':
          data = props.state.ui.sidebarUI.behaviorGroups.find(getCurrentArtifact);
          behaviorGroupBehaviorsList = data && data.getBehaviorsList();

          return {
            name: Form.createFormField({
              name: name,
              value: name,
            }),
            symbol: Form.createFormField({
              name: symbol,
              value: symbol,
            }),
            behaviorGroupBehaviorsList: Form.createFormField({
              name: behaviorGroupBehaviorsList,
              value: behaviorGroupBehaviorsList,
            }),
          };

        case 'propertySet':
          data = props.state.ui.sidebarUI.propertySets.find(getCurrentArtifact);
          propertySetPropertiesList = data && data.getPropertiesList();

          return {
            name: Form.createFormField({
              name: name,
              value: name,
            }),
            symbol: Form.createFormField({
              name: symbol,
              value: symbol,
            }),
            propertySetPropertiesList: Form.createFormField({
              name: propertySetPropertiesList,
              value: propertySetPropertiesList,
            }),
          };

        case 'templateDefinition':
          data = props.state.ui.sidebarUI.templateDefinitions.find(getCurrentArtifact);
          templateDefinitionPropertiesList = data && data.getPropertySetsList();

          formulaReference = data && data.getFormulaReference();
          templateDefinitionBehaviorsList = data && data.getBehaviorsList();
          templateDefinitionBehaviorGroupsList = data && data.getBehaviorGroupsList();
          templateDefinitionChildTokensList = data && data.getChildTokensList();
          templateDefinitionTokenBase = data && data.getTokenBase();

          return {
            name: Form.createFormField({
              name: name,
              value: name,
            }),
            symbol: Form.createFormField({
              name: symbol,
              value: symbol,
            }),
            templateDefinitionPropertiesList: Form.createFormField({
              name: templateDefinitionPropertiesList,
              value: templateDefinitionPropertiesList,
            }),
            formulaReference: Form.createFormField({
              name: formulaReference,
              value: formulaReference,
            }),
            templateDefinitionBehaviorsList: Form.createFormField({
              name: templateDefinitionBehaviorsList,
              value: templateDefinitionBehaviorsList,
            }),
            templateDefinitionBehaviorGroupsList: Form.createFormField({
              name: templateDefinitionBehaviorGroupsList,
              value: templateDefinitionBehaviorGroupsList,
            }),
            templateDefinitionChildTokensList: Form.createFormField({
              name: templateDefinitionChildTokensList,
              value: templateDefinitionChildTokensList,
            }),
            templateDefinitionTokenBase: Form.createFormField({
              name: templateDefinitionTokenBase,
              value: templateDefinitionTokenBase,
            }),
          };

        default: break;
      }
    }
  }
})(BaseForm);

export default connect(
  (state: IStoreState) => {
    const urlSegments = window.location.href.split('/');
    if (urlSegments.length < 2) {
      return {
        state: state,
        currentEntity: null,
        entityType: null,
      };
    } else {
      return {
        entityType: urlSegments[urlSegments.length -2],
        currentEntity: urlSegments[urlSegments.length -1],
        state: state,
      };
    }
  },
  { },
  null,
  {
    pure: false
  }
)(Editor);
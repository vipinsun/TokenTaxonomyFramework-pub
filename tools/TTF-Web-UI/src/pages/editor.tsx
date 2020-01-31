import React from "react";
import {connect} from 'react-redux';
import {
  Form,
  Input,
  Button, Tabs
} from 'antd';
import KeyValue from "../components/form/keyvalue";
import {WrappedFormUtils} from "antd/lib/form/Form";
import {FormComponentProps} from "antd/es/form";
import {IStoreState} from "../store/IStoreState";
import TextArea from "antd/es/input/TextArea";

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

class BaseForm extends React.Component<BaseFormProps, any> {

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
      const {TabPane} = Tabs;

      return <div className={"form"}>
        <Form {...formItemLayout}>
          <h2>{formName}</h2>
          <div className="form-wrapper">
            <h3 dangerouslySetInnerHTML={{
              __html: "Edit " +
                selected.getArtifactSymbol()!.getVisual()
            }}/>
            <Tabs defaultActiveKey="1">
              <TabPane tab="General" key="1">
                {this.renderForm()}
              </TabPane>
              <TabPane tab="Artifact Definition" key="2">
                {this.renderArtifact()}
              </TabPane>
            </Tabs>

            {editable ?
              <div className="submit-wrapper">
                <Form.Item
                  wrapperCol={{
                    xs: {span: 24, offset: 0},
                    sm: {span: 16, offset: 8},
                  }}
                >
                  <Button type="primary" htmlType="submit" className="submit">Submit</Button>
                </Form.Item>
              </div> : null}
          </div>
        </Form>
      </div>;
    }
  }

  private renderArtifact() {
    const form = this.props.form;
    const {getFieldDecorator} = this.props.form;
    const entityType = this.props.entityType;
    return (
      <React.Fragment>
        <div className="inputs-wrapper">
          <Form.Item label="Business Description">
            {getFieldDecorator('businessDescription', {
              rules: [
                {
                  required: false,
                },
              ],
            })(<TextArea disabled={!editable}/>)}
          </Form.Item>
          <Form.Item label="Business Example">
            {getFieldDecorator('businessExample', {
              rules: [
                {
                  required: false,
                },
              ],
            })(<TextArea disabled={!editable}/>)}
          </Form.Item>
          {// TODO add analogies list
          }
          <Form.Item label="Comments">
            {getFieldDecorator('comments', {
              rules: [
                {
                  required: false,
                },
              ],
            })(<TextArea disabled={!editable}/>)}
          </Form.Item>
        </div>
      </React.Fragment>
    );
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

      const artifactValues = {
        businessDescription: Form.createFormField({
          name: 'BusinessDescription',
          value: selected.getArtifactDefinition()!.getBusinessDescription(),
        }),
        businessExample: Form.createFormField({
          name: 'BusinessExample',
          value: selected.getArtifactDefinition()!.getBusinessExample(),
        }),
        // TODO use a list editor
        analogies: Form.createFormField({
          name: 'Analogies',
          value: selected.getArtifactDefinition()!.getAnalogiesList(),
        }),
        comments: Form.createFormField({
          name: 'Comments',
          value: selected.getArtifactDefinition()!.getComments(),
        })
      };

      switch (entityType) {
        case 'base':
          data = props.state.ui.sidebarUI.bases.find(getCurrentArtifact);
          quantity = data && data.getQuantity();
          decimals = data && data.getDecimals();
          owner = data && data.getOwner();
          constructorName = data && data.getConstructorName();

          return {...artifactValues,
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

          return {...artifactValues,
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

          return {...artifactValues,
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

          return {...artifactValues,
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

          return {...artifactValues,
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
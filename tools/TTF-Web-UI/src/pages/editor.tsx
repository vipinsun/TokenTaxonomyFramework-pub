import React from "react";
import { connect } from 'react-redux';
import {
  Form,
  Input,
  Button
} from 'antd';
import KeyValue from "../components/form/keyvalue";
import {WrappedFormUtils} from "antd/lib/form/Form";
import {FormComponentProps} from "antd/es/form";
import {IStoreState} from "../store/IStoreState";

const editable: boolean = true;
const formItemLayout = {
  labelCol: {
    xs: { span: 3 },
    sm: { span: 8 },
  },
  wrapperCol: {
    xs: { span: 24 },
    sm: { span: 16 },
  },
};

interface BaseFormProps extends FormComponentProps {
  state: IStoreState,
  currentEntity: string,
}

class BaseForm extends React.Component<BaseFormProps, any> {

  public render() {
    const {getFieldDecorator} = this.props.form;
    const form: WrappedFormUtils = this.props.form;
    const state = this.props.state;
    const currentID = this.props.currentEntity;
    // @ts-ignore
    const currentUrl = this.props.match.path;
    let selected = state.entities.bases.byId.get(currentID) ||
      state.entities.behaviors.byId.get(currentID) ||
      state.entities.behaviorGroups.byId.get(currentID) ||
      state.entities.propertySets.byId.get(currentID) ||
      state.entities.templateDefinitions.byId.get(currentID);

    function createForm() {
      switch (currentUrl) {
        case '/base/*':
          return (
            <React.Fragment>
              <div className="inputs-wrapper">
                <Form.Item label="Name">
                  {getFieldDecorator('name', {
                    rules: [
                      {
                        required: true,
                        message: 'Please input a token name',
                      },
                    ],
                  })(<Input disabled={!editable}/>)}
                </Form.Item>
                <Form.Item label="Symbol">
                  {getFieldDecorator('symbol', {
                    rules: [
                      {
                        required: true,
                        message: 'Please input a token symbol',
                      },
                    ],
                  })(<Input disabled={!editable}/>)}
                </Form.Item>
                <Form.Item label="Owner">
                  {getFieldDecorator('owner', {
                    rules: [
                      {
                        required: true,
                        message: 'Please input a token owner',
                      },
                    ],
                  })(<Input disabled={!editable}/>)}
                </Form.Item>
                <Form.Item label="Quantity">
                  {getFieldDecorator('quantity', {
                    rules: [
                      {
                        required: true,
                        message: 'Please enter a quantity',
                      },
                    ],
                  })(<Input disabled={!editable}/>)}
                </Form.Item>
                <Form.Item label="Decimals">
                  {getFieldDecorator('decimals', {
                    rules: [
                      {
                        required: true,
                        message: 'Please enter the number of decimals allowed',
                      },
                    ],
                  })(<Input disabled={!editable}/>)}
                </Form.Item>
                <Form.Item label="Constructor name">
                  {getFieldDecorator('constructor_name', {
                    rules: [
                      {
                        required: true,
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
        case '/behavior/*':
          return (
              <React.Fragment>
                <div className="inputs-wrapper">
                  <Form.Item label="Name">
                    {getFieldDecorator('name', {
                      rules: [
                        {
                          required: true,
                          message: 'Please input a token name',
                        },
                      ],
                    })(<Input disabled={!editable}/>)}
                  </Form.Item>
                  <Form.Item label="Symbol">
                    {getFieldDecorator('symbol', {
                      rules: [
                        {
                          required: true,
                          message: 'Please input a token symbol',
                        },
                      ],
                    })(<Input disabled={!editable}/>)}
                  </Form.Item>
                  <Form.Item label="Is external">
                    {getFieldDecorator('isExternal', {
                      rules: [
                        {
                          required: true,
                          message: 'Please input a token is external',
                        },
                      ],
                    })(<Input disabled={!editable}/>)}
                  </Form.Item>
                  <Form.Item label="Constructor type">
                    {getFieldDecorator('constructorType', {
                      rules: [
                        {
                          required: true,
                          message: 'Please input a token constructor type',
                        },
                      ],
                    })(<Input disabled={!editable}/>)}
                  </Form.Item>

                </div>
              </React.Fragment>
          );
        case '/behaviorGroup/*':
          return (
              <React.Fragment>
                <div className="inputs-wrapper">
                  <Form.Item label="Name">
                    {getFieldDecorator('name', {
                      rules: [
                        {
                          required: true,
                          message: 'Please input a token name',
                        },
                      ],
                    })(<Input disabled={!editable}/>)}
                  </Form.Item>
                  <Form.Item label="Symbol">
                    {getFieldDecorator('symbol', {
                      rules: [
                        {
                          required: true,
                          message: 'Please input a token symbol',
                        },
                      ],
                    })(<Input disabled={!editable}/>)}
                  </Form.Item>
                </div>
              </React.Fragment>
          );
        case '/propertySet/*':
          return (
              <React.Fragment>
                <div className="inputs-wrapper">
                  <Form.Item label="Name">
                    {getFieldDecorator('name', {
                      rules: [
                        {
                          required: true,
                          message: 'Please input a token name',
                        },
                      ],
                    })(<Input disabled={!editable}/>)}
                  </Form.Item>
                  <Form.Item label="Symbol">
                    {getFieldDecorator('symbol', {
                      rules: [
                        {
                          required: true,
                          message: 'Please input a token symbol',
                        },
                      ],
                    })(<Input disabled={!editable}/>)}
                  </Form.Item>
                </div>
              </React.Fragment>
          );
        case '/templateDefinition/*':
          return (
              <React.Fragment>
                <div className="inputs-wrapper">
                  <Form.Item label="Name">
                    {getFieldDecorator('name', {
                      rules: [
                        {
                          required: true,
                          message: 'Please input a token name',
                        },
                      ],
                    })(<Input disabled={!editable}/>)}
                  </Form.Item>
                  <Form.Item label="Symbol">
                    {getFieldDecorator('symbol', {
                      rules: [
                        {
                          required: true,
                          message: 'Please input a token symbol',
                        },
                      ],
                    })(<Input disabled={!editable}/>)}
                  </Form.Item>
                </div>
              </React.Fragment>
          );
        default: return
      }
    }

    if (selected === undefined) {
      return <div/>
    } else {
      const formName = selected.getName();

      return <div className={"form"}>
        <Form {...formItemLayout}>
          <h2>{formName}</h2>
          <div className="form-wrapper">
            <h3 dangerouslySetInnerHTML={{
              __html: "Edit " +
                // @ts-ignore
                selected.getArtifactSymbol().getVisual()
            }}/>
            {createForm()}
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
      </div>
    }
  }
};

const Editor = Form.create<BaseFormProps>({
  mapPropsToFields: (props: BaseFormProps) => {
    const state = props.state;
    const currentID = props.currentEntity;
    // @ts-ignore
    const currentUrl = props.match.path;
    let selected = state.entities.bases.byId.get(currentID) ||
      state.entities.behaviors.byId.get(currentID) ||
      state.entities.behaviorGroups.byId.get(currentID) ||
      state.entities.propertySets.byId.get(currentID) ||
      state.entities.templateDefinitions.byId.get(currentID);
    if (selected === undefined) {
      return {};
    } else {
      // @ts-ignore
      const symbol = selected.getArtifactSymbol().getVisual();
      let data, quantity, decimals, owner, constructorName;
      let isExternal, constructorType, invocationList, propertiesList;

      function getCurrentArtifact(el: any)  {
        if (el) {
          const artifact = el.getArtifact();
          if (artifact) {
            const symbol = artifact.getArtifactSymbol()
            if (symbol && symbol.getId() === currentID) {
              return el
            }
          }
        }
      }

      switch (currentUrl) {
        case '/base/*':
          data = props.state.ui.sidebarUI.bases.find(getCurrentArtifact);
          quantity = data && data.getQuantity();
          decimals = data && data.getDecimals();
          owner = data && data.getOwner();
          constructorName = data && data.getConstructorName();

          return {
            name: Form.createFormField({
              name: selected.getName(),
              value: selected.getName(),
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
            constructor_name: Form.createFormField({
              name: constructorName,
              value: constructorName,
            })
          };

        case '/behavior/*':
          data = props.state.ui.sidebarUI.behaviors.find(getCurrentArtifact);
          isExternal = data && data.getIsExternal();
          constructorType = data && data.getConstructorType();

          return {
            name: Form.createFormField({
              name: selected.getName(),
              value: selected.getName(),
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
          };

        case '/behaviorGroup/*':
          data = props.state.ui.sidebarUI.behaviorGroups.find(getCurrentArtifact);
          return {
            name: Form.createFormField({
              name: selected.getName(),
              value: selected.getName(),
            }),
            symbol: Form.createFormField({
              name: symbol,
              value: symbol,
            }),
          };

        case '/propertySet/*':
          data = props.state.ui.sidebarUI.propertySets.find(getCurrentArtifact);
          return {
            name: Form.createFormField({
              name: selected.getName(),
              value: selected.getName(),
            }),
            symbol: Form.createFormField({
              name: symbol,
              value: symbol,
            }),
          };

        case '/templateDefinition/*':
          data = props.state.ui.sidebarUI.templateDefinitions.find(getCurrentArtifact);
          return {
            name: Form.createFormField({
              name: selected.getName(),
              value: selected.getName(),
            }),
            symbol: Form.createFormField({
              name: symbol,
              value: symbol,
            }),
          };

        default: break
      }
    }
  }
})(BaseForm);

export default connect(
  (state: IStoreState) => {
    const currentID = window.location.href.substring(window.location.href.lastIndexOf('/') + 1);
    return {
      currentEntity: currentID,
      state: state,
    }
  },
  { },
  null,
  {
    pure: false
  }
)(Editor);
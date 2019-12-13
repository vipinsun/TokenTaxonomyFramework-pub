// @ts-ignore
import React, {Component} from "react";
import { Scrollbars } from "react-custom-scrollbars";
import { connect } from 'react-redux';
import {Button, Form, Input, Select} from "antd";
import Checkbox from "../components/checkbox";

const { Option } = Select;
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

interface IState {
    step: number;
}

class Definitions extends React.Component<any> {

    state: IState = {
        step: 1
    };

    private nextStep = () => {
        this.setState({
            step: this.state.step + 1
        })
    };

    private prevStep = () => {
        this.setState({
            step: this.state.step - 1
        })
    };

    private renderStep1 = () => {
      return(
          <div>
              <Form.Item label="Base Token Types" className="custom-select">
                  <Select defaultValue="Hybrids">
                      <Option value="Hybrids">Hybrids</Option>
                      <Option value="Fungible">Fungible</Option>
                      <Option value="Non-fungible">Non-fungible</Option>
                  </Select>
              </Form.Item>
              <Form.Item label="Template Type" className="custom-select">
                  <Select defaultValue="Hybrids">
                      <Option value="Hybrids">Hybrids</Option>
                      <Option value="Fungible">Fungible</Option>
                      <Option value="Non-fungible">Non-fungible</Option>
                  </Select>
              </Form.Item>
              <div className="checkbox-wrapper">
                  <p>Behaviors</p>
                  <div className="checkbox-area">
                      <Scrollbars autoHeight autoHeightMin={100}>
                          <Checkbox label="attestable"/>
                          <Checkbox label="burnable"/>
                          <Checkbox label="creator"/>
                          <Checkbox label="delegable"/>
                          <Checkbox label="encumberable"/>
                          <Checkbox label="attestable"/>
                          <Checkbox label="burnable"/>
                          <Checkbox label="creator"/>
                          <Checkbox label="delegable"/>
                          <Checkbox label="encumberable"/>
                      </Scrollbars>
                  </div>
              </div>
          </div>
      )
    };

    private renderStep2 = () => {
        return(
            <div>
                <div className="checkbox-wrapper">
                    <p>Behaviors Group</p>
                    <div className="checkbox-area">
                        <Scrollbars autoHeight autoHeightMin={100}>
                            <Checkbox label="attestable"/>
                            <Checkbox label="burnable"/>
                            <Checkbox label="creator"/>
                            <Checkbox label="delegable"/>
                            <Checkbox label="encumberable"/>
                            <Checkbox label="attestable"/>
                            <Checkbox label="burnable"/>
                            <Checkbox label="creator"/>
                            <Checkbox label="delegable"/>
                            <Checkbox label="encumberable"/>
                        </Scrollbars>
                    </div>
                </div>
                <div className="checkbox-wrapper">
                    <p>Property Sets</p>
                    <div className="checkbox-area">
                        <Scrollbars autoHeight autoHeightMin={100}>
                            <Checkbox label="attestable"/>
                            <Checkbox label="burnable"/>
                            <Checkbox label="creator"/>
                            <Checkbox label="delegable"/>
                            <Checkbox label="encumberable"/>
                            <Checkbox label="attestable"/>
                            <Checkbox label="burnable"/>
                            <Checkbox label="creator"/>
                            <Checkbox label="delegable"/>
                            <Checkbox label="encumberable"/>
                        </Scrollbars>
                    </div>
                </div>
                <div className="formula-wrapper">
                    <p>Generated Formula</p>
                    <div className="formula">{"[tN{s,~t}(tF{~d,SC}]"}</div>
                </div>
            </div>
        )
    };

    public render() {
        return (
            <div className={"form creating-form"}>
                <Form {...formItemLayout}>
                    <h2>Formulas</h2>
                    <p className="subtitle">Welcome to the TTF Formula Wizard</p>
                    <div className="form-wrapper">
                        <div className="inputs-wrapper">
                            <p>Select Artifacts for each of the categories to construct a formula</p>
                            {this.state.step === 1 ?
                                this.renderStep1() :
                                this.renderStep2()}
                        </div>
                        <div className="submit-wrapper">
                            <Form.Item
                                wrapperCol={{
                                    xs: {span: 24, offset: 0},
                                    sm: {span: 16, offset: 8},
                                }}
                            >
                                {this.state.step === 1 ?
                                    <React.Fragment>
                                        <Button type="primary" className="submit" onClick={this.nextStep}>Next</Button>
                                    </React.Fragment> :
                                    <React.Fragment>
                                        <Button type="primary" htmlType="submit" className="submit" >Create</Button>
                                        <Button type="primary" className="cancel" onClick={this.prevStep}>Back</Button>
                                    </React.Fragment>}
                            </Form.Item>
                        </div>
                    </div>
                </Form>
            </div>
        );
    }
}

export default Definitions;

// export default connect(
//   function () {
//     return {
//     }
//   },
//   { },
//   null,
//   {
//     pure: false
//   }
// )(Home);
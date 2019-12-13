// @ts-ignore
import React, {Component} from "react";
import { connect } from 'react-redux';
import {Button, Form, Input, Select} from "antd";

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

interface State {

}

class Definitions extends React.Component<any> {

    public render() {
        return (
            <div className={"form creating-form"}>
                <Form {...formItemLayout}>
                    <h2>Definitions</h2>
                    <p className="subtitle">Welcome to the TTF Definition Wizard</p>
                    <div className="form-wrapper">
                        <div className="inputs-wrapper">
                            <p>Select Template Formula to create your definition</p>
                            <Form.Item label="Formula" className="custom-select">
                                <Select defaultValue="1">
                                    <Option value="1">{"[tF{~d,SC}]+phSKU"}</Option>
                                    <Option value="2">{"[tF{~d,SC}]+phSKU"}</Option>
                                    <Option value="3">{"[tF{~d,SC}]+phSKU"}</Option>
                                </Select>
                            </Form.Item>
                        </div>
                        <div className="submit-wrapper">
                            <Form.Item
                                wrapperCol={{
                                    xs: {span: 24, offset: 0},
                                    sm: {span: 16, offset: 8},
                                }}
                            >
                                <Button type="primary" htmlType="submit" className="submit">Create</Button>
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
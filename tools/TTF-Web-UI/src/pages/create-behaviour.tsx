// @ts-ignore
import React, {Component} from "react";
import { connect } from 'react-redux';
import {Button, Form, Input} from "antd";


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

class CreateBehaviour extends React.Component<any> {
  public render() {
     return (
      <div className={"form"}>
        <Form {...formItemLayout}>
          <h2>Create Behavior</h2>
          <div className="form-wrapper">
            <div className={`road-wrapper rs-${3}`}>
              <div className="road-step step-1">
                <span>1</span>
              </div>
              <div className="road-step step-2">
                <span>2</span>
              </div>
              <div className="road-step step-3">
                <span>3</span>
              </div>
              <div className="road-step step-4">
                <span>4</span>
              </div>
              <div className="road-line"/>
            </div>
            <div className="inputs-wrapper">
              <Form.Item label="Behavior Name">
                <Input/>
              </Form.Item>
              <Form.Item label="Symbol">
                <Input/>
              </Form.Item>
            </div>
            <div className="submit-wrapper">
              <Form.Item
                wrapperCol={{
                  xs: {span: 24, offset: 0},
                  sm: {span: 16, offset: 8},
                }}
              >
                <Button type="primary" htmlType="submit" className="submit">Next</Button>
                <Button type="primary" htmlType="button" className="cancel">Cancel</Button>
              </Form.Item>
            </div>
          </div>
        </Form>
      </div>
    );
  }
}

export default CreateBehaviour;

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
import React, {Component} from 'react';
import './App.css';
import { Layout } from 'antd';
import Sidebar from './components/sidebar/sidebar';
import AppRouter from './AppRouter';
import {BrowserRouter} from "react-router-dom";

import 'antd/dist/antd.css';


const { Content } = Layout;

type AppProps = {
  match: string,
  height: number
}

class App extends Component<AppProps> {
  render = () => {
    return (
      <BrowserRouter>
      <Layout>
          <Layout>
          <Sidebar />
          <Layout style={{ padding: '0 24px 24px' }}>
            <Content
              style={{
                background: '#fff',
                padding: 24,
                margin: 0,
                minHeight: 280,
              }}
            >
              <AppRouter style={{}}/>
            </Content>
          </Layout>
          </Layout>
      </Layout>
      </BrowserRouter>
    );
  };
}

export default App;

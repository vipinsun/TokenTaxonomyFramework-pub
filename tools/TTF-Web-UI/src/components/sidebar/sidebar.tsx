import React from 'react';
import {Layout, Menu} from 'antd';
import {ClickParam} from "antd/lib/menu";
import {Link} from "react-router-dom";

const { Sider } = Layout;

let  onClick : (param: ClickParam) => void = (param) => {

}

export const Sidebar = () =>
  <Sider width={200} style={{ background: '#fff' }}>
    <Menu
      mode="vertical"
      defaultSelectedKeys={['base']}
      defaultOpenKeys={['base']}
      style={{ height: '100%', borderRight: 0 }}
      onClick={onClick}
    >
      <Menu.Item key="base">
        <Link to="/">
          Base
        </Link>
      </Menu.Item>
      <Menu.Item key="behaviours">
        <Link to="/behaviours">
          Behaviours
        </Link>
      </Menu.Item>
      <Menu.Item key="behaviour-groups">
        <Link to="/behaviour-groups">
        Behaviour groups
      </Link></Menu.Item>
      <Menu.Item key="property-sets">
        <Link to="/property-sets">
          Property sets
      </Link></Menu.Item>
      <Menu.Item key="token-templates">
        <Link to="/token-templates">
          Token templates
        </Link>
      </Menu.Item>
    </Menu>
  </Sider>

export default Sidebar;
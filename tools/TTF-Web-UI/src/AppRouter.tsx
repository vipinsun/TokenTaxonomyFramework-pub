import React, {CSSProperties} from 'react';
import {Route} from 'react-router-dom';
import {asyncComponent} from "react-async-component";

const routes = [
  {
    path: 'basetable',
    component: asyncComponent({
      resolve: () => import('./containers/Page/BaseTable')
    }),
    exact: true,
  },
  {
    path: 'base/*',
    component: asyncComponent({
      resolve: () => import('./pages/editor')}),
    exact: false,
  },
  {
    path: 'behavior/*',
    component: asyncComponent({
      resolve: () => import('./pages/editor')}),
    exact: false,
  },
  {
    path: 'behaviorGroup/*',
    component: asyncComponent({
      resolve: () => import('./pages/editor')}),
    exact: false,
  },
  {
    path: 'propertySet/*',
    component: asyncComponent({
      resolve: () => import('./pages/editor')}),
    exact: false,
  },
  {
    path: 'templateDefinition/*',
    component: asyncComponent({
      resolve: () => import('./pages/editor')}),
    exact: false,
  },
  {
    path: 'token-templates',
    component: asyncComponent({
      resolve: () => import('./pages/token-templates')}),
    exact: true,
  },
  {
    path: 'create-behavior',
    component: asyncComponent({
      resolve: () => import('./pages/create-behaviour')}),
    exact: true,
  }
];

interface AppRouterProps {
  style: CSSProperties;
}

export const AppRouter = ({style}: AppRouterProps) =>
  <div style={style}>
    {routes.map(singleRoute => {
      const {path, exact, ...otherProps} = singleRoute;
      return (
        <Route
          exact={!exact ? false : true}
          key={singleRoute.path}
          path={`/${singleRoute.path}`}
          {...otherProps}
        />
      );
    })}
  </div>


export default AppRouter;

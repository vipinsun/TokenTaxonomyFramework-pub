import React, {CSSProperties} from 'react';
import { Route } from 'react-router-dom';
import asyncComponent from './helpers/AsyncFunc';

const routes = [
  {
    path: '',
    component: asyncComponent(() => import('./pages/base')),
    exact: true,
  },
  {
    path: 'behaviours',
    component: asyncComponent(() => import('./pages/behaviours')),
    exact: true,
  },
  {
    path: 'behaviour-groups',
    component: asyncComponent(() => import('./pages/behaviour-groups')),
    exact: true,
  },
  {
    path: 'property-sets',
    component: asyncComponent(() => import('./pages/property-sets')),
    exact: true,
  },
  {
    path: 'token-templates',
    component: asyncComponent(() => import('./pages/token-templates')),
    exact: true,
  },
];

type AppRouterProps = {
  style: CSSProperties
}

export const AppRouter = ({ style }: AppRouterProps) =>
      <div style={style}>
        {routes.map(singleRoute => {
          const { path, exact, ...otherProps } = singleRoute;
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

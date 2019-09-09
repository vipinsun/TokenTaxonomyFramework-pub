import {Action, Dispatch} from 'redux';
import {Base, Bases, Behavior, BehaviorGroup, PropertySet, TemplateDefinition} from "../model/core_pb";
import {
  getAllBases,
  getAllBehaviorGroups,
  getAllBehaviors,
  getAllPropertySets,
  getAllTemplateDefinitions
} from '../state';
import {IEntity} from "../store/IStoreState";


export const FETCH_SERVER_STATE = 'FETCH_SERVER_STATE';
export const FETCH_SERVER_STATE_SUCCESS = 'FETCH_SERVER_STATE_SUCCESS';
export const FETCH_SERVER_STATE_ERROR = 'FETCH_SERVER_STATE_ERROR';
export const SELECT_ENTITY = "SELECT_ENTITY";

export interface ISelectEntity extends Action {
  type: 'SELECT_ENTITY';
  payload: IEntity;
}

export interface IFetchServerState extends Action {
  type: 'FETCH_SERVER_STATE';
}

export interface IFetchServerStateSuccess extends Action {
  type: 'FETCH_SERVER_STATE_SUCCESS';
  bases: Base[];
  behaviors: Behavior[];
  behaviorGroups: BehaviorGroup[];
  propertySets: PropertySet[];
  templateDefinitions: TemplateDefinition[];
}

export interface IFetchServerStateError extends Action {
  type: 'FETCH_SERVER_STATE_ERROR';
  errorMsg: string;
}

export type AppActions = ISelectEntity | IFetchServerState | IFetchServerStateSuccess | IFetchServerStateError;

// Dispatchers
function dispatchSelectEntity(selected: IEntity): ISelectEntity {
  return {
    type: SELECT_ENTITY,
    payload: selected
  };
}
function dispatchFetchServerStateProgress(): IFetchServerState {
  return {
    type: FETCH_SERVER_STATE
  };
}

function dispatchFetchServerStateSuccess(bases: Base[], behaviors: Behavior[], behaviorGroups: BehaviorGroup[], propertySets: PropertySet[], templateDefinitions: TemplateDefinition[]): IFetchServerStateSuccess {
  return {
    type: FETCH_SERVER_STATE_SUCCESS,
    bases: bases,
    behaviors: behaviors,
    behaviorGroups: behaviorGroups,
    propertySets: propertySets,
    templateDefinitions: templateDefinitions,
  };
}

function dispatchFetchServerStateError(e: Error): IFetchServerStateError {
  return {
    type: FETCH_SERVER_STATE_ERROR,
    errorMsg: e.message
  };
}

// Action creator 
export function actionSelectEntity(selected: IEntity) {
  return (dispatch: Dispatch) => {
    return dispatch(dispatchSelectEntity(selected));
  }
}

export function actionFetchServerState() {
  return (dispatch: Dispatch) => {
    dispatch(dispatchFetchServerStateProgress());

    // TODO: getAllBases from state.tsx

    return Promise.all([getAllBases(),getAllBehaviors(), getAllBehaviorGroups(), getAllPropertySets(), getAllTemplateDefinitions()])
      .then((artifacts) => {
        const bases: Base[] = artifacts[0];
        const behaviors: Behavior[] = artifacts[1];
        const behaviorGroups: BehaviorGroup[] = artifacts[2];
        const propertySets: PropertySet[] = artifacts[3];
        const templateDefinitions: TemplateDefinition[] = artifacts[4];
        return dispatch(dispatchFetchServerStateSuccess(bases, behaviors, behaviorGroups, propertySets, templateDefinitions));
      })
      .catch((e: Error) => {
        return dispatch(dispatchFetchServerStateError(e));
      });
  };
}

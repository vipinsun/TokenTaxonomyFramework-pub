import {
  baseListReducer,
  BaseListState,
  defaultBaseListState,
  basesDictReducer, ArtifactDict, defaultArtifactDict,
} from './baseList';
import {Action} from 'redux';
import {BehaviorGroup, PropertySet, TemplateDefinition} from "../model/core_pb";

export interface AppState {
  entities: {
    bases: ArtifactDict;
    behaviors: ArtifactDict;
    behaviorGroups: ArtifactDict;
    propertySets: ArtifactDict;
    templateDefinitions: ArtifactDict;
  };
  ui: {
    sidebarUI: BaseListState;
  };
}

// initial State
export function defaultState() {
  return {
    entities: {
      bases: defaultArtifactDict(),
      behaviors: defaultArtifactDict(),
      behaviorGroups: defaultArtifactDict(),
      propertySets: defaultArtifactDict(),
      templateDefinitions: defaultArtifactDict()
    },
    ui: {
      sidebarUI: defaultBaseListState()
    },
  };
}

export function mainReducer(state: AppState = defaultState(), action: Action): AppState {
  return {
    entities: {
      bases: basesDictReducer(state.entities.bases, action, "bases"),
      behaviors: basesDictReducer(state.entities.behaviors, action, "behaviors"),
      behaviorGroups: basesDictReducer(state.entities.behaviorGroups, action, "behaviorGroups"),
      propertySets: basesDictReducer(state.entities.propertySets, action, "propertySets"),
      templateDefinitions: basesDictReducer(state.entities.templateDefinitions, action, "templateDefinitions"),
    },
    ui: {
      sidebarUI: baseListReducer(state.ui.sidebarUI, action),
    },
  };
}

/*export function fetchInitState(state: StoreState, action: FetchInitState):     
StoreState {
    switch (action.type) {
        case FETCH_SERVER_STATE:
            return {
                ...state,
                bases: action.bases,
            };
    }

    return state;
} */

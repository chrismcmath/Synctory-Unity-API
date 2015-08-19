using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Synctory.Objects;
using Synctory.Roots;
using Synctory.Routers;
using Synctory.Utils;

namespace Synctory {
    public class Synctory : MonoBehaviour {
        public const string ROOT_NAME = "Synctory";
        public const string LOCATIONS_ROOT_NAME = "Locations";
        public const string UNITS_ROOT_NAME = "Units";
        public const string STEPS_ROOT_NAME = "Steps";
        public const string ENTITIES_ROOT_NAME = "Entities";

        /* This needs to handle following cases:
           1) Loading a file from scratch (nothing exists)
           2) Reloading a file (things already exist)
        */
        private static GameObject _Root = null;
        public static GameObject Root {
            get {
                if (_Root == null && !TryGetRoot<MainRoot>(ref _Root)) {
                    _Root = UnityHelpers.CreateChild(ROOT_NAME);
                    _Root.AddComponent<ScriptMetadata>();
                    _Root.AddComponent<MainRoot>();
                    _Root.AddComponent<SynctoryClock>();
                }
                return _Root;
            }
        }

        private static SynctoryClock _Clock = null;
        public static SynctoryClock Clock {
            get {
                if (_Clock == null) {
                    _Clock = Root.GetComponent<SynctoryClock>();
                }
                return _Clock;
            }
        }

        private static GameObject _LocationsRoot = null;
        public static GameObject LocationsRoot {
            get {
                if (_LocationsRoot == null && !TryGetRoot<LocationsRoot>(ref _LocationsRoot)) {
                    _LocationsRoot = UnityHelpers.CreateChild(LOCATIONS_ROOT_NAME, Root);
                    _LocationsRoot.AddComponent<LocationsRoot>();
                }
                return _LocationsRoot;
            }
        }

        private static GameObject _UnitsRoot = null;
        public static GameObject UnitsRoot {
            get {
                if (_UnitsRoot == null && !TryGetRoot<UnitsRoot>(ref _UnitsRoot)) {
                    _UnitsRoot = UnityHelpers.CreateChild(UNITS_ROOT_NAME, Root);
                    _UnitsRoot.AddComponent<UnitsRoot>();
                }
                return _UnitsRoot;
            }
        }

        private static GameObject _EntitiesRoot = null;
        public static GameObject EntitiesRoot {
            get {
                if (_EntitiesRoot == null && !TryGetRoot<EntitiesRoot>(ref _EntitiesRoot)) {
                    _EntitiesRoot = UnityHelpers.CreateChild(ENTITIES_ROOT_NAME, Root);
                    _EntitiesRoot.AddComponent<EntitiesRoot>();
                }
                return _EntitiesRoot;
            }
        }

        private static GameObject _StepsRoot = null;
        public static GameObject StepsRoot {
            get {
                if (_StepsRoot == null && !TryGetRoot<StepsRoot>(ref _StepsRoot)) {
                    _StepsRoot = UnityHelpers.CreateChild(STEPS_ROOT_NAME, Root);
                    _StepsRoot.AddComponent<StepsRoot>();
                }
                return _StepsRoot;
            }
        }

        private static Dictionary<string, SynctoryRouter> _EntityRouters = new Dictionary<string, SynctoryRouter>();
        private static Dictionary<int, SynctoryRouter> _LocationRouters = new Dictionary<int, SynctoryRouter>();

        public static void RegisterRouter(string entityName, SynctoryRouter router) {
            if (!_EntityRouters.ContainsKey(entityName)) {
                _EntityRouters.Add(entityName, router);
            } else {
                Debug.LogError("[Synctory] An EntityRouter has already been registered with the name " + entityName);
            }
        }

        public static void RegisterRouter(int locationKey, SynctoryRouter router) {
            if (!_LocationRouters.ContainsKey(locationKey)) {
                _LocationRouters.Add(locationKey, router);
            } else {
                Debug.LogError("[Synctory] An LocationRouter has already been registered with the name " + locationKey);
            }
        }

        public static void TimeUpdated(TimeSpan time) {
            foreach (Location location in SynctoryHelpers.GetAllLocations()) {
                SynctoryFrameInfo info = location.UpdateTime(time);

                UpdateLocationRouter(location.Key, info);
                UpdateEntityRouters(location.CurrentUnit.Entities, info);
            }
        }

        private static void UpdateLocationRouter(int locationKey, SynctoryFrameInfo info) {
            if (_LocationRouters.ContainsKey(locationKey)) {
                _LocationRouters[locationKey].TimeUpdated(info);
            }
        }

        private static void UpdateEntityRouters(List<Entity> entities, SynctoryFrameInfo info) {
            foreach (Entity entity in entities) {
                if (_EntityRouters.ContainsKey(entity.Name)) {
                    _EntityRouters[entity.Name].TimeUpdated(info);
                }
            }
        }

        private static bool TryGetRoot<T>(ref GameObject root) where T : SynctoryRoot {
            T rootScript = FindObjectOfType<T>();
            if (rootScript != null) {
                root = rootScript.gameObject;
                return true;
            } 
            return false;
        }
    }
}

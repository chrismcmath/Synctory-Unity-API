using UnityEngine;
using System.Collections;

using Synctory.Objects;
using Synctory.Roots;
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
                }
                return _Root;
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

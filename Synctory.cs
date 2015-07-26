using UnityEngine;
using System.Collections;

using Synctory.Utils;

namespace Synctory {
    public class Synctory : MonoBehaviour {


        public const string ROOT_NAME = "Synctory";
        public const string LOCATIONS_ROOT_NAME = "Locations";
        public const string STEPS_ROOT_NAME = "Steps";

        /* This needs to handle following cases:
           1) Loading a file from scratch (nothing exists)
           2) Reloading a file (things already exist)
        */
        
        //TODO: Do a complete rewrite on loading (if have stuff already)
            // or allow loading over previous stuff? Don't think it's necessary

        private static GameObject _Root = null;
        public static GameObject Root {
            get {
                if (_Root == null) {
                    _Root = UnityHelpers.CreateChild(ROOT_NAME);
                }
                return _Root;
            }
        }

        private static GameObject _LocationsRoot = null;
        public static GameObject LocationsRoot {
            get {
                if (_LocationsRoot == null) {
                    _LocationsRoot = UnityHelpers.CreateChild(LOCATIONS_ROOT_NAME, Root);
                }
                return _LocationsRoot;
            }
        }
    }
}

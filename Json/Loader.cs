using UnityEngine;
using System.Collections;

namespace Synctory.Json {
    public static class Loader {
        public const string KEY = "key";

        private static bool _Error = false;

        public static void SetError() {
            _Error = true;
        }

        public static bool LoadJSON(string jsonText) {
            JSONObject j = new JSONObject(jsonText);
            return Process(j);
        }

        public static bool Process(JSONObject obj) {
            _Error = false;

            //NOTE: Order imp.
            MetadataLoader.LoadMetadata(obj);
            if (_Error) return false; // current idea is to put these between each bit (to abort early), must be a cleaner way
            StepLoader.LoadSteps(obj);
            if (_Error) return false; 
            LocationLoader.LoadLocations(obj);
            if (_Error) return false; 
            UnitLoader.LoadUnits(obj);
            if (_Error) return false;
            LocationLoader.UpdateLocationsWithUnits();

            return _Error;
        }

        private static void ErrorDelegate(string key) {
            SetError();
            Debug.LogError("[Loader] Couldn't find " + key + " in .synctory");
        }
    }
}

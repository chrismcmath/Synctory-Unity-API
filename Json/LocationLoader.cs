using UnityEngine;
using System.Collections;

using Synctory.Utils;

namespace Synctory.Json {
    public static class LocationLoader {
        public const string KEY_LOCATIONS = "locations";

        public const string KEY = "key";
        public const string KEY_NAME = "name";

        public static void LoadLocations(JSONObject obj) {
            obj.GetField(KEY_LOCATIONS, delegate(JSONObject o) {
                    foreach (JSONObject location in o.list) {
                        LoadLocation(location);
                    }
                }, ErrorDelegate);
        }

        private static void LoadLocation(JSONObject location) {
            int key = -1;
            string name = "";

            location.GetField(KEY, delegate(JSONObject o) {
                    key = (int) o.n;
                }, ErrorDelegate);
            location.GetField(KEY_NAME, delegate(JSONObject o) {
                    name = o.str;
                }, ErrorDelegate);

            GameObject go = UnityHelpers.CreateChild(key + name, Synctory.LocationsRoot);
        }

        private static void ErrorDelegate(string key) {
            Loader.SetError();
            Debug.LogError("[LocationLoader] Couldn't find " + key + " in .synctory");
        }
    }
}

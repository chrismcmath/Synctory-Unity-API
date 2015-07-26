using UnityEngine;
using System.Collections;

namespace Synctory.Json {
    public static class Loader {
        public const string KEY = "key";

        public const string KEY_AUTHOR = "author";

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

            string title, author;

            obj.GetField(KEY_AUTHOR, delegate(JSONObject o) {
                    title = o.str;
                }, ErrorDelegate);
            obj.GetField(KEY_AUTHOR, delegate(JSONObject o) {
                    author = o.str;
                }, ErrorDelegate);

            if (_Error) return false; // current idea is to put these between each bit (to abort early), must be a cleaner way

            //LoadMeta(title, author);

            //NOTE: Order imp.
            LocationLoader.LoadLocations(obj);
            UnitLoader.LoadUnits(obj);

            if (_Error) return false;

            return _Error;
        }

        private static void ErrorDelegate(string key) {
            SetError();
            Debug.LogError("[Loader] Couldn't find " + key + " in .synctory");
        }
    }
}

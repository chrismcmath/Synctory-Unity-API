using UnityEngine;
using System.Collections;

namespace Synctory {
    public static class Loader {
        public const string KEY_TITLE = "title";
        public const string KEY_AUTHOR = "author";
        public const string KEY_LOCATIONS = "locations";
        public const string KEY_UNITS = "units";
        public const string KEY_STEPS = "steps";

        public static bool Error = false;

        public static bool LoadJSON(string jsonText) {
            JSONObject j = new JSONObject(jsonText);
            return Process(j);
        }

        public static bool Process(JSONObject obj) {
            Error = false;

            string title, author;

            obj.GetField(KEY_AUTHOR, delegate(JSONObject o) {
                    title = o.str;
                }, ErrorDelegate);
            obj.GetField(KEY_AUTHOR, delegate(JSONObject o) {
                    author = o.str;
                }, ErrorDelegate);

            if (!Error) {
                //TODO: 
                //LoadMeta(title, author);
            }

            obj.GetField(KEY_LOCATIONS, delegate(JSONObject o) {
                    //TODO: 
                    //LoadLocations(o);
                }, ErrorDelegate);

            return Error;
        }

        private static void ErrorDelegate(string key) {
            Error = true;
            Debug.LogError("Couldn't find " + key + " in .synctory");
        }
    }
}

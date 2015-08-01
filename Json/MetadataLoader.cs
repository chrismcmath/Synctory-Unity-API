using UnityEngine;
using System.Collections;

using Synctory.Objects;
using Synctory.Utils;

namespace Synctory.Json {
    public static class MetadataLoader {
        public const string KEY_TITLE = "title";
        public const string KEY_AUTHOR = "author";

        public static void LoadMetadata(JSONObject obj) {
            string title = "";
            string author = "";

            obj.GetField(KEY_TITLE, delegate(JSONObject o) {
                    title = o.str;
                }, ErrorDelegate);
            obj.GetField(KEY_AUTHOR, delegate(JSONObject o) {
                    author = o.str;
                }, ErrorDelegate);

            ScriptMetadata metadata = Synctory.Root.GetComponent<ScriptMetadata>();
            metadata.Title = title;
            metadata.Author = author;
        }

        private static void ErrorDelegate(string key) {
            Loader.SetError();
            Debug.LogError("[MetadataLoader] Couldn't find " + key + " in .synctory");
        }
    }
}

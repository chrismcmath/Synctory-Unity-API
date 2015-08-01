using UnityEngine;
using System.Collections;

using Synctory.Objects;
using Synctory.Utils;

namespace Synctory.Json {
    public static class StepLoader {
        public const string KEY_STEPS = "steps";

        public const string KEY_STAMP = "stamp";

        public static void LoadSteps(JSONObject obj) {
            obj.GetField(KEY_STEPS, delegate(JSONObject o) {
                    foreach (JSONObject stepObj in o.list) {
                        LoadStep(stepObj);
                    }
                }, ErrorDelegate);
        }

        private static void LoadStep(JSONObject stepObj) {
            int key = -1;
            string stamp = "";

            stepObj.GetField(Loader.KEY, delegate(JSONObject o) {
                    key = (int) o.n;
                }, ErrorDelegate);
            stepObj.GetField(KEY_STAMP, delegate(JSONObject o) {
                    stamp = o.str;
                }, ErrorDelegate);

            GameObject go = UnityHelpers.CreateChild(GetName(key, stamp), Synctory.StepsRoot);
            Step step = go.AddComponent<Step>();
            step.Key = key;
            step.Stamp = stamp;
        }

        private static void ErrorDelegate(string key) {
            Loader.SetError();
            Debug.LogError("[StepLoader] Couldn't find " + key + " in .synctory");
        }

        private static string GetName(int key, string name) {
            return string.Format("[{0}] {1}", key, name);
        }
    }
}

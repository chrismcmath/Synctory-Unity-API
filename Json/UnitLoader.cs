using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Synctory.Objects;
using Synctory.Utils;

namespace Synctory.Json {
    public static class UnitLoader {
        public const string KEY_UNITS = "units";

        public const string KEY_ACTIVE = "active";
        public const string KEY_LOCATION = "location";
        public const string KEY_TEXT = "text";
        public const string KEY_ENTITIES = "entities";
        public const string KEY_STEPS = "steps";

        public static void LoadUnits(JSONObject obj) {
            obj.GetField(KEY_UNITS, delegate(JSONObject o) {
                    foreach (JSONObject unitObj in o.list) {
                        LoadUnit(unitObj);
                    }
                }, ErrorDelegate);
        }

        private static void LoadUnit(JSONObject unitObj) {
            int key = -1;
            bool active = false;
            int location = -1;
            string text = "";
            List<JSONObject> entities = null;
            List<JSONObject> steps = null;

            unitObj.GetField(Loader.KEY, delegate(JSONObject o) {
                    key = (int) o.n;
                }, ErrorDelegate);
            unitObj.GetField(KEY_ACTIVE, delegate(JSONObject o) {
                    active = o.b;
                }, ErrorDelegate);
            unitObj.GetField(KEY_LOCATION, delegate(JSONObject o) {
                    location = (int) o.n;
                }, ErrorDelegate);
            unitObj.GetField(KEY_TEXT, delegate(JSONObject o) {
                    text = o.str;
                }, ErrorDelegate);
            unitObj.GetField(KEY_ENTITIES, delegate(JSONObject o) {
                    entities = o.list;
                }, ErrorDelegate);
            unitObj.GetField(KEY_STEPS, delegate(JSONObject o) {
                    steps = o.list;
                }, ErrorDelegate);

            EntityLoader.LoadEntities(entities);

            GameObject go = UnityHelpers.CreateChild(GetName(key), Synctory.UnitsRoot);
            Unit unit = go.AddComponent<Unit>();
            unit.Key = key;
            unit.Active = active;
            unit.Entities = SynctoryHelpers.GetEntitiesFromNames(JSONUtils.ConvertToStrings(entities));
            unit.Steps = SynctoryHelpers.GetStepsFromKeys(JSONUtils.ConvertToInts(steps));
            unit.Location = SynctoryHelpers.GetLocationFromKey(location);
            unit.Text = text;
        }

        private static void ErrorDelegate(string key) {
            Loader.SetError();
            Debug.LogError("[LocationLoader] Couldn't find " + key + " in .synctory");
        }

        private static string GetName(int key) {
            return string.Format("{0}", key);
        }
    }
}

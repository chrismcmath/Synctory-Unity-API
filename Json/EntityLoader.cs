using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Synctory.Objects;
using Synctory.Utils;

namespace Synctory.Json {
    public static class EntityLoader {
        public const string KEY_NAME = "name";

        public static void LoadEntities(List<JSONObject> obj) {
            foreach (JSONObject entityObj in obj) {
                LoadEntity(entityObj);
            }
        }

        private static void LoadEntity(JSONObject entityObj) {
            string name = entityObj.str;

            if (SynctoryHelpers.GetEntityFromName(name) != null) {
                Debug.Log("[INFO] [EntityLoader] " + name + " already exists");
                return;
            }

            GameObject go = UnityHelpers.CreateChild(GetName(name), Synctory.EntitiesRoot);
            Entity entity = go.AddComponent<Entity>();
            entity.Name = name;
        }

        private static string GetName(string name) {
            return string.Format("{0}", name);
        }
    }
}

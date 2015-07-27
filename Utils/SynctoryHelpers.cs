using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Synctory;
using Synctory.Objects;

namespace Synctory.Utils {
    public static class SynctoryHelpers {
        public static List<Entity> GetEntitiesFromKeys(List<string> keys) {
            List<Entity> entities = new List<Entity>();
            entities.Add(new Entity());
            return entities;
        }

        public static List<Step> GetStepsFromKeys(List<string> keys) {
            List<Step> entities = new List<Step>();
            entities.Add(new Step());
            return entities;
        }

        public static Location GetLocationFromKey(int key) {
            return new Location();
        }

        public static List<Entity> GetEntitiesFromNames(List<string> names) {
            List<Entity> entities = new List<Entity>();
            foreach (string name in names) {
                Entity entity = GetEntityFromName(name);
                if (entity != null) {
                    entities.Add(entity);
                }
            }
            return entities;
        }

        public static Entity GetEntityFromName(string name) {
            return UnityHelpers.GetSynctoryObjectFromKey<Entity>(name, Synctory.EntitiesRoot);
        }
    }
}

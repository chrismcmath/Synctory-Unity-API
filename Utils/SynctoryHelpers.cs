using UnityEngine;
using System; //delete
using System.Collections;
using System.Collections.Generic;

using Synctory;
using Synctory.Objects;

namespace Synctory.Utils {
    public static class SynctoryHelpers {

        //TODO: move to date time utils
        public static string GetTimeStringFromSeconds(float seconds) {
            TimeSpan timeSpan = TimeSpan.FromSeconds(Mathf.Floor(seconds));
            return timeSpan.ToString();
        }

        /* Getters */

        public static Location GetLocationFromKey(int key) {
            return GetSynctoryObjectFromKey<Location>(key, Synctory.LocationsRoot);
        }

        public static List<Location> GetAllLocations() {
            return GetAllSynctoryObjectsOfType<Location>(Synctory.LocationsRoot);
        }

        public static List<Unit> GetAllUnits() {
            return GetAllSynctoryObjectsOfType<Unit>(Synctory.UnitsRoot);
        }

        public static List<Step> GetStepsFromKeys(List<int> keys) {
            return GetSynctoryObjectFromKeys<Step>(keys, Synctory.StepsRoot);
        }

        public static Step GetPreviousStep(Step step) {
            return GetSynctoryObjectFromKey<Step>(step.Key - 1, Synctory.StepsRoot);
        }

        /* The bit that actually does stuff */

        public static List<T> GetAllSynctoryObjectsOfType<T>(GameObject root) where T : UniqueObject {
            List<T> uniqueObjects = new List<T>();
            foreach (Transform child in root.transform) {
                UniqueObject uniqueObject = child.GetComponent<UniqueObject>();
                if (uniqueObject == null) {
                    Debug.Log("Couldn't find UniqueObject component on " + child.name);
                } else {
                    if (uniqueObject.GetType() == typeof(T)) {
                        uniqueObjects.Add(uniqueObject as T);
                    }
                }
            }
            return uniqueObjects;
        }

        public static List<T> GetSynctoryObjectFromKeys<T>(List<int> keys, GameObject root) where T : UniqueObject {
            List<T> uniqueObjects = new List<T>();
            foreach (int key in keys) {
                T uniqueObject = GetSynctoryObjectFromKey<T>(key, root);
                if (uniqueObject != null) {
                    uniqueObjects.Add(uniqueObject);
                }
            }
            return uniqueObjects;
        }

        public static T GetSynctoryObjectFromKey<T>(int key, GameObject root) where T : UniqueObject {
            Transform parent = root.transform;
            foreach (Transform child in parent) {
                UniqueObject uniqueObject = child.GetComponent<UniqueObject>();
                if (uniqueObject == null) {
                    Debug.Log("Couldn't find UniqueObject component on " + child.name);
                } else {
                    if (uniqueObject.Key == key && uniqueObject.GetType() == typeof(T)) {
                        return uniqueObject as T;
                    }
                }
            }
            return default(T);
        }


        public static List<Unit> GetUnitsFromLocation(Location location) {
            List<Unit> units = new List<Unit>();
            foreach (Unit unit in GetAllUnits()) {
                if (unit.Location.Key == location.Key) {
                    units.Add(unit);
                }
            }
            return units;
        }

        /* As Entities are the only Synctory Object that use strings as Keys,
         * keeping this non-generic for now */

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
            foreach (Transform child in Synctory.EntitiesRoot.transform) {
                Entity entity = child.GetComponent<Entity>();
                if (entity == null) {
                    Debug.Log("Couldn't find Entity component on " + child.name);
                } else {
                    if (entity.Name == name) {
                        return entity;
                    }
                }
            }
            return null;
        }
    }
}

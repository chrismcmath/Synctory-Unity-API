using UnityEngine;
using System.Collections;

namespace Synctory.Utils {
    public static class UnityHelpers {
        public static void CreateEmptyGameObject() {
        }

        public static GameObject CreateChild(string name) {
            return new GameObject(name);
        }

        public static GameObject CreateChild(string name, GameObject parent) {
            return CreateChild(name, parent.transform);
        }
        public static GameObject CreateChild(string name, Transform parent) {
            GameObject child = CreateChild(name);
            child.transform.parent = parent;
            child.transform.localPosition = Vector3.zero;
            child.transform.localRotation = Quaternion.identity;
            child.transform.localScale = Vector3.one;
            return child;
        }

        public static T GetSynctoryObjectFromKey<T> (string key, GameObject parent) {
            return GetSynctoryObjectFromKey<T>(key, parent.transform);
        }
        //NOTE: 
        ////NOTE: 
        /////TODO: totally wrong- should be checking the SyncotryObject components for the key
        public static T GetSynctoryObjectFromKey<T> (string key, Transform parent) {
            Transform target = parent.Find(key);
            if (target == null) { 
                Debug.Log(string.Format("[SYNCTORY] Could not find {0} under {1}", key, parent.name));
                return default(T);
            }

            T syncObj = target.GetComponent<T>();
            return syncObj;
        }
    }
}

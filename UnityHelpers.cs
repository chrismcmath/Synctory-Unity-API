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
    }
}

using UnityEngine;
using System.Collections;

namespace Synctory.Objects {
    public class SynctoryObject : MonoBehaviour {
        public int Key = -1;

        public void SetKey(int key) {
            Key = key;
        }
    }
}

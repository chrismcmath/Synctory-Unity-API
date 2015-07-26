using UnityEngine;
using System.Collections;

namespace Synctory.Objects {
    public class Location : SynctoryObject {
        public string Name = "";

        public void SetName(string name) {
            Name = name;
        }
    }
}

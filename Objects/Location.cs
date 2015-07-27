using System;
using System.Collections;

using UnityEngine;

namespace Synctory.Objects {
    public class Location : SynctoryObject {
        [SerializeField]
        private string _Name = "";
        public string Name {
            get { return _Name; }
            set { _Name = value; }
        }
    }
}

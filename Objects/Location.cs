using UnityEngine;
using System.Collections;

namespace Synctory.Objects {
    public class Location : SynctoryObject {
        private string _Name = "";
        public string Name {
            get { return _Name; }
            set { _Name = value; }
        }
    }
}

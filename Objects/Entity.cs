using UnityEngine;
using System.Collections;

namespace Synctory.Objects {
    public class Entity : SynctoryObject {
        [SerializeField]
        private string _Name = "";
        public string Name {
            get { return _Name; }
            set { _Name = value; }
        }
    }
}

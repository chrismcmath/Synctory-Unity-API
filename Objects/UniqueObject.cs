using UnityEngine;
using System.Collections;

namespace Synctory.Objects {
    public class UniqueObject : SynctoryObject {
        [SerializeField]
        private int _Key = -1;
        public int Key {
            get { return _Key; }
            set { _Key = value; }
        }
    }
}

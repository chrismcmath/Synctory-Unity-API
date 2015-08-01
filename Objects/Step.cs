using UnityEngine;
using System.Collections;

namespace Synctory.Objects {
    public class Step : UniqueObject {
        [SerializeField]
        private string _Stamp = "";
        public string Stamp {
            get { return _Stamp; }
            set { _Stamp = value; }
        }
    }
}

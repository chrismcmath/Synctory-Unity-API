using System;
using System.Collections;

using UnityEngine;

namespace Synctory.Objects {
    public class Step : UniqueObject {
        [SerializeField]
        private string _Stamp = "";
        public string Stamp {
            get { return _Stamp; }
            set { _Stamp = value; }
        }

        [SerializeField]
        private string _Timestamp = "";
        public TimeSpan Timestamp {
            get { return TimeSpan.Parse(_Timestamp); }
            set { _Timestamp = value.ToString(); }
        }
    }
}

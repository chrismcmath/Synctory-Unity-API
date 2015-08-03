using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Synctory.Objects {
    public class Location : UniqueObject {
        [SerializeField]
        private string _Name = "";
        public string Name {
            get { return _Name; }
            set { _Name = value; }
        }

        [SerializeField]
        private List<Unit> _Units = new List<Unit>();
        public  List<Unit> Units {
            get { return _Units; }
            set { _Units = value; }
        }
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Synctory.Objects {
    public class Unit : SynctoryObject {
        private bool _Active = false;
        public bool Active {
            get { return _Active; }
            set { _Active = value; }
        }

        private List<Entity> _Entities = new List<Entity>();
        public List<Entity> Entities {
            get { return _Entities; }
            set { _Entities = value; }
        }

        private List<Step> _Steps = new List<Step>();
        public List<Step> Steps {
            get { return _Steps; }
            set { _Steps = value; }
        }

        private Location _Location = null;
        public Location Location {
            get { return _Location; }
            set { _Location = value; }
        }

        private string _Text = "";
        public string Text {
            get { return _Text; }
            set { _Text = value; }
        }
    }
}

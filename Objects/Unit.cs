using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Synctory.Objects {
    public class Unit : UniqueObject {
        [SerializeField]
        private bool _Active = false;
        public bool Active {
            get { return _Active; }
            set { _Active = value; }
        }

        [SerializeField]
        private List<Entity> _Entities = new List<Entity>();
        public List<Entity> Entities {
            get { return _Entities; }
            set { _Entities = value; }
        }

        [SerializeField]
        private List<Step> _Steps = new List<Step>();
        public List<Step> Steps {
            get { return _Steps; }
            set { _Steps = value; }
        }

        [SerializeField]
        private Location _Location = null;
        public Location Location {
            get { return _Location; }
            set { _Location = value; }
        }

        [SerializeField]
        private string _Text = "";
        public string Text {
            get { return _Text; }
            set { _Text = value.Replace("\\n", "\r\n"); }
        }

        public TimeSpan StartTime {
            get {
                if (Steps.Count > 0) {
                    return Steps[0].Timestamp;
                } else {
                    return new TimeSpan();
                }
            }
        }
    }
}

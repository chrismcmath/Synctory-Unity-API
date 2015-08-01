using UnityEngine;
using System.Collections;

namespace Synctory.Objects {
    public class ScriptMetadata : SynctoryObject {
        [SerializeField]
        private string _Title = "";
        public string Title {
            get { return _Title; }
            set { _Title = value; }
        }

        [SerializeField]
        private string _Author = "";
        public string Author {
            get { return _Author; }
            set { _Author = value; }
        }

        public void LoadMetadata(string title, string author) {
            Title = title;
            Author = author;
        }
    }
}

using UnityEngine;
using System.Collections;

namespace Synctory.Routers {
    public class LocationRouter : SynctoryRouter {

        [SerializeField]
        protected int _LocationKey = -1;
        public int LocationKey {
            get { return _LocationKey; }
        }

        protected override void RegisterWithSynctory() {
            Synctory.RegisterRouter(LocationKey, this);
        }
    }
}

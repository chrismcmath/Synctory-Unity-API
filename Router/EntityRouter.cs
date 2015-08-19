using UnityEngine;
using System.Collections;

namespace Synctory.Routers {
    public class EntityRouter : SynctoryRouter {

        [SerializeField]
        protected string _EntityName = "";
        public string EntityName {
            get {
                return _EntityName.ToUpper(); // Entity names always upper-case
            }
        }

        protected override void RegisterWithSynctory() {
            Synctory.RegisterRouter(EntityName, this);
        }
    }
}

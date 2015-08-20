using System.Collections;
using UnityEngine;

using Synctory.Routers;

namespace Synctory.Binders {
    public abstract class SynctoryBinder : MonoBehaviour {
        public void Awake() {
            RegisterWithRouter();
        }

        protected void RegisterWithRouter() {
            SynctoryRouter router = GetRoutingParent();
            if (router != null) {
                router.RegisterBinder(this);
            } else {
                Debug.LogError("[SynctoryBinder] Could not find a SynctoryRouter anywhere above the binder. Did you forget to attach one?");
            }
        }

        protected SynctoryRouter GetRoutingParent() {
            Transform t = transform;

            while (t != null) {
                SynctoryRouter router = t.GetComponent<SynctoryRouter>();
                if (router != null) {
                    return router;
                }
                t = t.parent;
            }
            return null;
        }

        public abstract void UpdateInfo(SynctoryFrameInfo info);
    }
}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using Synctory.Binders;

namespace Synctory.Routers {
    public abstract class SynctoryRouter : MonoBehaviour {
        protected List<SynctoryBinder> _Binders = new List<SynctoryBinder>();

        public void Awake() {
            RegisterWithSynctory();
        }

        public void RegisterBinder(SynctoryBinder binder) {
            if (!_Binders.Contains(binder)) {
                _Binders.Add(binder);
            }
        }

        public void TimeUpdated(SynctoryFrameInfo info) {
            UpdateBinders(info);
        }

        protected void UpdateBinders(SynctoryFrameInfo info) {
            foreach (SynctoryBinder binder in _Binders) {
                binder.UpdateInfo(info);
            }
        }

        protected abstract void RegisterWithSynctory();
    }
}

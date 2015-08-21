using System;
using System.Collections;
using UnityEngine;

namespace Synctory.Binders {
    [RequireComponent (typeof (Animator))]
        public class AnimatorBinder : SynctoryBinder {

            private Animator _Animator;

            public void Start() {
                _Animator = GetComponent<Animator>();
            }

            public override void UpdateInfo(SynctoryFrameInfo info) {
                //TODO: Check state exists
                _Animator.Play(string.Format("{0}", info.Unit.Key), -1, info.UnitProgression());
            }
        }
}

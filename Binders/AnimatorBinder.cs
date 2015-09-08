using System;
using System.Collections;
using UnityEngine;

namespace Synctory.Binders {
    [RequireComponent (typeof (Animator))]
        public class AnimatorBinder : SynctoryBinder {
            public const string IDLE_STATE_KEY = "Idle";

            public enum Behaviour {FREEZE=0, IDLE};

            public Behaviour OnIdle = Behaviour.IDLE;

            private Animator _Animator;

            public void Start() {
                _Animator = GetComponent<Animator>();
            }

            public override void UpdateInfo(SynctoryFrameInfo info) {
                //TODO: Cache existence 
                int animHash = Animator.StringToHash(string.Format("{0}", info.Unit.Key));
                if (_Animator.HasState(0, animHash)) {
                    _Animator.Play(animHash, -1, info.UnitProgression());
                } else {
                    OnNoAnimation();
                }
            }

            private void OnNoAnimation() {
                switch (OnIdle) {
                    case Behaviour.FREEZE:
                        break;
                    case Behaviour.IDLE:
                        if (!_Animator.GetCurrentAnimatorStateInfo(0).IsName(IDLE_STATE_KEY)) {
                            _Animator.Play(IDLE_STATE_KEY);
                        }
                        break;
                }
            }
        }
}

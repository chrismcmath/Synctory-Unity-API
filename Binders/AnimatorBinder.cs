using System;
using System.Collections;
using UnityEngine;

namespace Synctory.Binders {
    [RequireComponent (typeof (Animator))]
        public class AnimatorBinder : SynctoryBinder {
            public enum Behaviour {FREEZE=0, IDLE};

            public const int ALL_LAYERS = -1;
            public const int DEFAULT_LAYER = 0;

            public const string IDLE_STATE_KEY = "Idle";

            public Behaviour OnIdle = Behaviour.IDLE;

            private Animator _Animator;

            public void Start() {
                _Animator = GetComponent<Animator>();
            }

            public override void UpdateInfo(SynctoryFrameInfo info) {
                //TODO: Cache existence 
                int animHash = Animator.StringToHash(string.Format("{0}", info.Unit.Key));
                if (HasStateHash(animHash)) {
                    PlayAnimHash(animHash, info.UnitProgression());
                } else {
                    OnNoAnimation();
                }
            }

            private void OnNoAnimation() {
                switch (OnIdle) {
                    case Behaviour.FREEZE:
                        _Animator.enabled = false;
                        break;
                    case Behaviour.IDLE:
                        if (!IsPlayingStateName(IDLE_STATE_KEY)) {
                            int animHash = Animator.StringToHash(IDLE_STATE_KEY);
                            if (HasStateHash(animHash)) {
                                PlayAnimHash(animHash);
                            }
                        }
                        break;
                }
            }

            private void PlayAnimHash(int animHash) {
                if (!_Animator.enabled) {
                    _Animator.enabled = true;
                }

                _Animator.Play(animHash);
            }
            private void PlayAnimHash(int animHash, float prog) {
                if (!_Animator.enabled) {
                    _Animator.enabled = true;
                }

                _Animator.Play(animHash, ALL_LAYERS, prog);
            }

            private bool IsPlayingStateName(string stateName) {
                return _Animator.GetCurrentAnimatorStateInfo(DEFAULT_LAYER).IsName(stateName);
            }

            private bool HasStateHash(int stateHash) {
                return _Animator.HasState(0, stateHash);
            }
        }
}

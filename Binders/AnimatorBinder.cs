using System;
using System.Collections;
using UnityEngine;

using Synctory.Utils;

namespace Synctory.Binders {
    [RequireComponent (typeof (Animator))]
        public class AnimatorBinder : SynctoryBinder {
            public enum BakeOption {NONE=0, MISSING_ANIMATIONS, OVERWRITE_ANIMATIONS};
            public enum Behaviour {FREEZE=0, IDLE};

            public const int ALL_LAYERS = -1;
            public const int DEFAULT_LAYER = 0;

            public const string IDLE_STATE_KEY = "Idle";

            public Behaviour OnIdle = Behaviour.IDLE;
            public BakeOption BakeMode = BakeOption.NONE;

            private Animator _Animator;
            private float _PrevXPosition;
            private float _PrevZPosition;

            public void Start() {
                _Animator = GetComponent<Animator>();
            }

            public override void UpdateInfo(SynctoryFrameInfo info) {
                int hash = GetHash(info.Unit.Key);
                switch (BakeMode) {
                    case BakeOption.NONE:
                        //TODO: Cache existence 
                        if (HasStateHash(hash)) {
                            PlayAnimHash(hash, info.UnitProgression);
                        } else {
                            OnNoAnimation();
                        }
                        break;
                    case BakeOption.MISSING_ANIMATIONS:
                        if (HasStateHash(hash)) {
                            PlayAnimHash(hash, info.UnitProgression);
                        } else {
                            BakeAnimation(info);
                        }
                        break;
                    case BakeOption.OVERWRITE_ANIMATIONS:
                        if (HasStateHash(hash)) {
                            OverwriteAnimation(info);
                        } else {
                            BakeAnimation(info);
                        }
                        break;
                }
            }

            private void BakeAnimation(SynctoryFrameInfo info) {
            }

            private void OverwriteAnimation(SynctoryFrameInfo info) {
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

            private int GetHash(int key) {
                return Animator.StringToHash(string.Format("{0}", key));
            }
        }
}

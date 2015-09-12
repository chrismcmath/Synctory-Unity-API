using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using Synctory.Utils;

namespace Synctory.Binders {
    [RequireComponent (typeof (Animation))]
        public class AnimationTransformBinder : SynctoryBinder {
            public enum BakeOption {NONE=0, MISSING_ANIMATIONS, OVERWRITE_ANIMATIONS};
            public enum AnimationMissingBehaviour {FREEZE=0, PLAY_IDLE_ANIMATION};

            public AnimationMissingBehaviour OnIdle = AnimationMissingBehaviour.FREEZE;
            public BakeOption BakeMode = BakeOption.NONE;
            public string AnimationRoot = "Assets/Animations/Resources/";
            public string AnimationsPath = "";
            public string IdleAnimationClip = "Idle";

            private Animation _Animation;
            private float _PrevXPosition;
            private float _PrevZPosition;
            private float _PrevTime;
            private List<string> _ClipsAtStartup = new List<string>();

            public void Start() {
                _Animation = GetComponent<Animation>();

                LoadStartupClips();
                ResetClipTrackers();

#if !UNITY_EDITOR
                if (BakeMode != BakeOption.NONE) {
                    Debug.LogError("[AnimationTransformBinder] Bake option not compatible with build");
                }
#endif
            }

            public override void UpdateInfo(SynctoryFrameInfo info) {
                string clipName = string.Format("{0}", info.Unit.Key);
                switch (BakeMode) {
                    case BakeOption.NONE:
                        //TODO: Cache existence 
                        if (HadClipAtStartup(clipName)) {
                            PlayClip(clipName, StampUtils.InfoToSeconds(info));
                        } else {
                            OnNoAnimation();
                        }
                        break;
                    case BakeOption.MISSING_ANIMATIONS:
                        if (HadClipAtStartup(clipName)) {
                            PlayClip(clipName, StampUtils.InfoToSeconds(info));
                        } else {
                            BakeAnimation(info);
                        }
                        break;
                    case BakeOption.OVERWRITE_ANIMATIONS:
                        if (HadClipAtStartup(clipName)) {
                            OverwriteAnimation(info);
                        } else {
                            BakeAnimation(info);
                        }
                        break;
                }
            }

            private void LoadStartupClips() {
                AnimationClip[] clips = Resources.LoadAll<AnimationClip>(AnimationsPath);

                foreach (AnimationClip clip in clips) {
                    clip.legacy = true;
                    _Animation.AddClip(clip, clip.name);
                    _ClipsAtStartup.Add(clip.name);
                }
            }

            private void BakeAnimation(SynctoryFrameInfo info) {
                string clipName = string.Format("{0}", info.Unit.Key);

                AnimationClip clip = GetAnimationClip(clipName);
                
                float currentTime = StampUtils.InfoToSeconds(info);

                AnimUtils.AddTransformCurve("m_LocalPosition.x", clip,
                        _PrevTime, _PrevXPosition,
                        currentTime, transform.localPosition.x);
                AnimUtils.AddTransformCurve("m_LocalPosition.z", clip,
                        _PrevTime, _PrevZPosition,
                        currentTime, transform.localPosition.z);

                Debug.Log("prev time " + _PrevTime + " current " + currentTime);
                _PrevXPosition = transform.localPosition.x;
                _PrevZPosition = transform.localPosition.z;
                _PrevTime = currentTime;
            }

            private void OverwriteAnimation(SynctoryFrameInfo info) {
                //TODO: 
            }

            private void OnNoAnimation() {
                switch (OnIdle) {
                    case AnimationMissingBehaviour.FREEZE:
                        _Animation.enabled = false;
                        break;
                    case AnimationMissingBehaviour.PLAY_IDLE_ANIMATION:
                        if (!IsPlayingClip(IdleAnimationClip)) {
                            if (HadClipAtStartup(IdleAnimationClip)) {
                                PlayClip(IdleAnimationClip);
                            }
                        }
                        break;
                }
            }

            private void PlayClip(string key) {
                if (!_Animation.enabled) {
                    _Animation.enabled = true;
                }

                _Animation.Play(key);
            }
            private void PlayClip(string key, float seconds) {
                if (!_Animation.enabled) {
                    _Animation.enabled = true;
                }

                _Animation.Play(key);

                _Animation[key].time = seconds;
                _Animation.Sample();
                _Animation.Stop();
            }

            private bool IsPlayingClip(string clipName) {
                return _Animation.IsPlaying(clipName);
            }

            private bool HadClipAtStartup(string clipName) {
                return _ClipsAtStartup.Contains(clipName);
            }

            private bool HasClipAtRuntime(string clipName) {
                return _Animation[clipName] != null;
            }

            private AnimationClip GetAnimationClip(string clipName) {
                if (_Animation[clipName] != null) {
                    return _Animation[clipName].clip;
                }

#if UNITY_EDITOR
                AnimationClip clip = new AnimationClip();
                clip.legacy = true;

                string filePath = string.Format("{0}{1}{2}.anim", AnimationRoot, AnimationsPath, clipName);
                AssetDatabase.CreateAsset(clip, filePath);

                _Animation.AddClip(clip, clipName);

                ResetClipTrackers();

                return clip;
#else 
                Debug.LogError("[AnimationTransformBinder] Could not find clipName: " + clipName);
                return null;
#endif
            }

            private void ResetClipTrackers() {
                Debug.Log("ResetClipTrackers");
                _PrevTime = 0f;
            }
        }
}

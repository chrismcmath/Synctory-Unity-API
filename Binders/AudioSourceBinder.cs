using System;
using System.Collections;
using UnityEngine;

namespace Synctory.Binders {
    [RequireComponent (typeof (AudioSource))]
        public class AudioSourceBinder : SynctoryBinder {
            public enum Behaviour {CONTINUE=0, STOP};

            public const float MIN_PITCH = 0.99f;
            public const float MAX_PITCH = 1.01f;

            //TODO: make this a function of the delta
            public const float PITCH_SEEK_DAMPENER = 10f;
            public const float SKIP_DELTA_THRESHOLD = 1f;

            public Behaviour OnEnd = Behaviour.CONTINUE;

            private float _TargetPitch;
            private int _CurrentUnitKey = -1;
            private AudioSource _AudioSource;

            public void Start() {
                _AudioSource = GetComponent<AudioSource>();
            }

            public override void UpdateInfo(SynctoryFrameInfo info) {
                if (!Synctory.Clock.IsPlaying()) {
                    _AudioSource.Stop();
                    return;
                }

                if (!TryUpdateClip(info.Unit.Key)) {
                    return;
                }

                float seconds = (float) TimeSpan.FromTicks((long) info.Ticks).TotalSeconds;
                if (seconds > _AudioSource.clip.length) {
                    OnClipEnd();
                } else {
                    PlayClip(info);
                }
            }

            private void PlayClip(SynctoryFrameInfo info) {
                UpdateTime(info);
                UpdatePitch(info);

                //TODO: skip time if delta too large
                //_AudioSource.time = (float) TimeSpan.FromTicks((long) info.Ticks).TotalSeconds;
                if (!_AudioSource.isPlaying) {
                    _AudioSource.Play();
                }
            }

            private void OnClipEnd() {
                switch (OnEnd) {
                    case Behaviour.CONTINUE:
                        break;
                    case Behaviour.STOP:
                        _AudioSource.Stop();
                        break;
                }
            }

            private void UpdateTime(SynctoryFrameInfo info) {
                float syncSeconds= (float) TimeSpan.FromTicks((long) info.Ticks).TotalSeconds;
                float playbackDelta = _AudioSource.time - syncSeconds;
                if (Mathf.Abs(playbackDelta) >= SKIP_DELTA_THRESHOLD) {
                    _AudioSource.time = (float) TimeSpan.FromTicks((long) info.Ticks).TotalSeconds;
                }
            }
            private void UpdatePitch(SynctoryFrameInfo info) {
                float syncSeconds= (float) TimeSpan.FromTicks((long) info.Ticks).TotalSeconds;
                float playbackDelta = _AudioSource.time - syncSeconds;

                float deltaTime = Time.smoothDeltaTime;
                float targetPitch = (deltaTime - playbackDelta) / deltaTime;

                _TargetPitch = Mathf.Min(MAX_PITCH, Mathf.Max(MIN_PITCH, targetPitch));
                _AudioSource.pitch += (_TargetPitch - _AudioSource.pitch) / PITCH_SEEK_DAMPENER;

                //Debug.Log("[AUDIO] Playback delta: " + playbackDelta + " pitch: " + _AudioSource.pitch);
            }

            private bool TryUpdateClip(int key) {
                if (key != _CurrentUnitKey) {
                    ChangeAudioClip(key);
                }

                return _AudioSource.clip != null;
            }

            private void ChangeAudioClip(int unitKey) {
                _CurrentUnitKey = unitKey;
                _AudioSource.clip = GetClipFromKey(unitKey);
            }

            private AudioClip GetClipFromKey(int unitKey) {
                return Resources.Load(string.Format("{0}", unitKey),typeof(AudioClip)) as AudioClip;
            }
        }
}

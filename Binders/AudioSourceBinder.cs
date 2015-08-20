using System;
using System.Collections;
using UnityEngine;

namespace Synctory.Binders {
    [RequireComponent (typeof (AudioSource))]
        public class AudioSourceBinder : SynctoryBinder {
            public const float MIN_PITCH = 0.9f;
            public const float MAX_PITCH = 1.1f;
            //TODO: make this a function of the delta
            public const float PITCH_SEEK_DAMPENER = 10f;
            public const float SKIP_DELTA_THRESHOLD = 1f;

            private AudioSource _AudioSource;

            private int _CurrentUnitKey = -1;
            private float _TargetPitch;

            public void Start() {
                _AudioSource = GetComponent<AudioSource>();
            }

            public override void UpdateInfo(SynctoryFrameInfo info) {
                if (info.Unit.Key != _CurrentUnitKey) {
                    ChangeAudioClip(info.Unit.Key);
                }

                if (_AudioSource.clip == null) {
                    return;
                }

                float seconds = (float) TimeSpan.FromTicks((long) info.Ticks).TotalSeconds;
                if (seconds > _AudioSource.clip.length) {
                    _AudioSource.Stop();
                } else {

                    UpdateTime(info);
                    UpdatePitch(info);

                    //TODO: skip time if delta too large
                    //_AudioSource.time = (float) TimeSpan.FromTicks((long) info.Ticks).TotalSeconds;
                    if (!_AudioSource.isPlaying) {
                        _AudioSource.Play();
                    }
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
                Debug.Log("[AUDIO] Playback delta: " + playbackDelta);

                float deltaTime = Time.smoothDeltaTime;
                float targetPitch = (deltaTime - playbackDelta) / deltaTime;

                _TargetPitch = Mathf.Min(MAX_PITCH, Mathf.Max(MIN_PITCH, targetPitch));
                _AudioSource.pitch += (_TargetPitch - _AudioSource.pitch) / PITCH_SEEK_DAMPENER;
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

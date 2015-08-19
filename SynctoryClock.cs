using System;
using System.Collections;

using UnityEngine;

namespace Synctory {
    public class SynctoryClock : MonoBehaviour {
        //TODO: prob better to implement this is playing_speed, allowing different playment times (and better scrubbing)
        public enum SynctoryState {PAUSED=0,PLAYING}

        private SynctoryState _State = SynctoryState.PLAYING;
        public SynctoryState State {
            get { return _State; }
            set {
                if (value != _State) {
                    _State = value;
                }
            }
        }

        private TimeSpan _SynctoryTime = new TimeSpan();
        public TimeSpan SynctoryTime {
            get { return _SynctoryTime; }
            set {
                if (_SynctoryTime != value) {
                    _SynctoryTime = value; 
                    _TimeChanged = true;
                }
            }
        }

        public float TotalSeconds {
            get {
                return (float) SynctoryTime.TotalSeconds;
            }
        }

        private bool _TimeChanged = true;
        
        public void Update() {
            if (IsPlaying()) {
                SynctoryTime += TimeSpan.FromSeconds(Time.deltaTime);
            }

            CheckTimeChanged();
        }

        public void CheckTimeChanged() {
            if (_TimeChanged) {
                Synctory.TimeUpdated(SynctoryTime);
                _TimeChanged = false;
            }
        }

        public void Play() {
            State = SynctoryState.PLAYING;
        }

        public void Pause() {
            State = SynctoryState.PAUSED;
        }

        public void Reset() {
            SynctoryTime = new TimeSpan();
        }

        public bool IsPlaying() {
            return State == SynctoryState.PLAYING;
        }
    }
}

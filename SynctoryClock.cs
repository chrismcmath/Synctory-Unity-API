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

        private bool _TimeChanged = true;
        
        public void Update() {
            if (_State == SynctoryState.PLAYING) {
                _SynctoryTime += TimeSpan.FromSeconds(Time.deltaTime);
                _TimeChanged = true;
            }

            if (_TimeChanged) {
                Synctory.UpdateTime(SynctoryTime);
                _TimeChanged = false;
            }
        }
    }
}

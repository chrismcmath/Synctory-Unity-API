using System;
using System.Collections;

using UnityEngine;

namespace Synctory {
    public class SynctoryClock : MonoBehaviour {
        //TODO: prob better to implement this is playing_speed, allowing different playment times (and better scrubbing)
        public enum SynctoryState {PAUSED=0,PLAYING}

        [SerializeField]
        private SynctoryState _State = SynctoryState.PLAYING;
        public SynctoryState State {
            get { return _State; }
            set {
                if (value != _State) {
                    _State = value;
                }
            }
        }

        [SerializeField]
        private TimeSpan _SynctoryTime = new TimeSpan();
        public TimeSpan SynctoryTime {
            get { return _SynctoryTime; }
        }
        
        public void Update() {
            //Debug.Log("Time: " + SynctoryTime);

            if (_State == SynctoryState.PLAYING) {
                _SynctoryTime += TimeSpan.FromSeconds(Time.deltaTime);
            }
        }

    }
}

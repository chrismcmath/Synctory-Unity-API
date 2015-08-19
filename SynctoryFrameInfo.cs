using UnityEngine;
using System.Collections;

using Synctory.Objects;

namespace Synctory {
    public class SynctoryFrameInfo {
        public Unit Unit;
        public float Ticks;
        public float TotalTicks;

        public float UnitProgression() {
            return Ticks / TotalTicks;
        }
    }
}

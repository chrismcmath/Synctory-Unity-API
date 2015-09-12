using UnityEngine;
using System.Collections;

using Synctory.Objects;

namespace Synctory {
    public class SynctoryFrameInfo {
        public Unit Unit;
        public long Ticks;
        public long TotalTicks;

        public float UnitProgression  {
            get {
                return (float) Ticks / (float) TotalTicks;
            }
        }
    }
}

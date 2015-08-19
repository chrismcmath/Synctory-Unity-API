using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Synctory.Objects {
    public class Location : UniqueObject {
        [SerializeField]
        private string _Name = "";
        public string Name {
            get { return _Name; }
            set { _Name = value; }
        }

        [SerializeField]
        private List<Unit> _CachedUnits = new List<Unit>();
        public  List<Unit> CachedUnits {
            get { return _CachedUnits; }
            set { _CachedUnits = value; }
        }

        [SerializeField]
        private Unit _CurrentUnit = null;
        public  Unit CurrentUnit {
            get { return _CurrentUnit; }
            set { _CurrentUnit = value; }
        }

        [SerializeField]
        private float _CurrentUnitProgression = 0f;
        public  float CurrentUnitProgression {
            get { return _CurrentUnitProgression; }
            set { _CurrentUnitProgression = value; }
        }

        //NOTE: Units MUST be in order
        ////TODO: also need to check steps are all sequetial (in timestamps) too
        public SynctoryFrameInfo UpdateTime(TimeSpan time) {
            if (CachedUnits.Count == 0) {
                Debug.LogError("[Location] No cached units in location");
                return null;
            }

            TimeSpan unitEnd = TimeSpan.MaxValue;
            Unit candidateUnit = null;

            foreach (Unit unit in CachedUnits) {
                if (time >= unit.StartTime) {
                    candidateUnit = unit;
                } else {
                    unitEnd = unit.StartTime;
                    break;
                }
            }

            CurrentUnit = candidateUnit;

            SynctoryFrameInfo info = new SynctoryFrameInfo();
            info.Unit = CurrentUnit;
            info.Ticks = time.Ticks - CurrentUnit.StartTime.Ticks;
            info.TotalTicks = unitEnd.Ticks - CurrentUnit.StartTime.Ticks;

            CurrentUnitProgression = info.UnitProgression();

            return info;
        }
    }
}

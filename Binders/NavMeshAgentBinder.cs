using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Synctory.Routers;

namespace Synctory.Binders {
    [RequireComponent (typeof (NavMeshAgent))]
        public class NavMeshAgentBinder : SynctoryBinder {
            private NavMeshAgent _Agent;

            public Dictionary<int, LocationDestinationBinder> _Locations = new Dictionary<int, LocationDestinationBinder>();

            public void Start() {
                _Agent = GetComponent<NavMeshAgent>();

                LocationDestinationBinder[] destBinders = FindObjectsOfType(typeof(LocationDestinationBinder)) as LocationDestinationBinder[];
                foreach (LocationDestinationBinder destBinder in destBinders) {
                    LocationRouter router = destBinder.GetComponentInParent<LocationRouter>();
                    if (router == null) {
                        Debug.LogError("[NavMeshAgentBinder] LocationDesinationBinders should be beneath or on the same level as a LocationRouter");
                    }
                    _Locations.Add(router.LocationKey, destBinder);
                }
            }

            public override void UpdateInfo(SynctoryFrameInfo info) {
                int locationKey = info.Unit.Location.Key;
                _Agent.SetDestination(_Locations[locationKey].transform.position);
            }
        }
}

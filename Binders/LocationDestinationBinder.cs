using UnityEngine;
using System.Collections;

namespace Synctory.Binders {
    public class LocationDestinationBinder : MonoBehaviour {
        public Bounds LocationBounds;

        public void OnDrawGizmos() {
            DebugUtils.DrawBounds(LocationBounds, transform.position);
        }
    }
}

using System.Collections;
using UnityEngine;

namespace Synctory.Binders {
    public class AudioBinder : SynctoryBinder {
        public override void UpdateInfo(SynctoryFrameInfo info) {
            Debug.Log(". . . . . . . . . .  audio binder, updated with: " + info);
        }
    }
}

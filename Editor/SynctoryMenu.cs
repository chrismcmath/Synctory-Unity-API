using UnityEditor;
using UnityEngine;
using System.Collections;

namespace Synctory.Editor {
    public class SynctoryMenu : MonoBehaviour {
        [MenuItem("Synctory/Script Importer")]
        public static void Import() {
            ImportWindow window = (ImportWindow) EditorWindow.GetWindow(typeof(ImportWindow));
            window.Show();
        }
    }
}


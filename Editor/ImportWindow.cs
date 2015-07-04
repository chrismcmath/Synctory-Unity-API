using UnityEngine;
using UnityEditor;

using Synctory;

namespace Synctory.Editor {
    public class ImportWindow : EditorWindow {
        public const string NO_PATH = "(no file selected)";

        private string _Path = null;
        
        public void Awake() {
            _Path = NO_PATH;
        }

        public void OnGUI () {
            GUILayout.Label("Import .synctory", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Path to file", _Path);
            //_Path = EditorGUILayout.TextField ("File to import(.synctory)", _Path);
            
            if (GUILayout.Button("Select File")) {
                //NOTE: Bug here with Unity/OSX, can't use multiple format types
                _Path = EditorUtility.OpenFilePanel( "File to import(.synctory)", "", "synctory");

                if (_Path.Length == 0) {
                    _Path = NO_PATH;
                }
            }

            EditorGUILayout.Separator();

            if (GUILayout.Button("Import")) {
                if (_Path.Length != 0 && _Path != NO_PATH) {
                    WWW www = new WWW("file:///" + _Path);

                    ContinuationManager.Add(() => www.isDone, () =>
                        {
                            if (!string.IsNullOrEmpty(www.error)) {
                                Debug.Log("WWW failed: " + www.error);
                                EditorUtility.DisplayDialog("Synctory Error", "Could not load file.\nError: " + www.error, "OK");
                                return;
                            }

                            LoadText(www.text);
                        });
                } else {
                    EditorUtility.DisplayDialog("Synctory Error", "Please select a file first.\nFiles just have a .synctory extension.", "OK");
                }
            }
        }

        private void LoadText(string jsonText) {
            if (!string.IsNullOrEmpty(jsonText)) {
                Debug.Log("got: " + jsonText);
                if (!Loader.LoadJSON(jsonText)) {
                } else {
                    EditorUtility.DisplayDialog("Synctory Error", "Could not load file.\nPlease ensure it is valid JSON.", "OK");
                }
                //EditorUtility.DisplayDialog("Synctory", "Imported!", "OK");
            } else {
                EditorUtility.DisplayDialog("Synctory Error", "Could not read file.\nThis could be due to the file being empty or corrupted.", "OK");
            }
        }
    }
}

using System.IO;

using UnityEngine;
using UnityEditor;

using Synctory;
using Synctory.Json;

namespace Synctory.Editor {
    public class ImportWindow : EditorWindow {
        public const string NO_PATH = "(no file selected)";

        private string _Path = null;
        private Object _Target = null;
        private TextAsset _File = null;
        
        public void Awake() {
            _Path = NO_PATH;
            titleContent.text = "Synctory Script Importer";
        }

        public void OnGUI () {

            Rect rect = EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            {
                EditorGUILayout.LabelField("Drag and drop a .synctory file here.");
                EditorGUILayout.Space();

                EditorGUILayout.PrefixLabel("Current Path:");
                EditorGUILayout.LabelField(_Path, EditorStyles.boldLabel);

                if (GUILayout.Button("Open File View")) {
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

                    EditorGUILayout.Space();
                }
            }
            EditorGUILayout.EndVertical();

            DragDrop(rect);
        }

        private void DragDrop(Rect rect)
        {
            Event evt = Event.current;

            switch (evt.type) {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!rect.Contains(evt.mousePosition)) break;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform) {
                        DragAndDrop.AcceptDrag();

                        if (DragAndDrop.paths.Length != 1) {
                            Debug.LogErrorFormat("[Synctory] [Import Window] Can't import more than one script");
                        } else {
                            _Path = string.Format("{0}/{1}", Directory.GetCurrentDirectory(), DragAndDrop.paths[0]);
                        }
                    }
                    break;
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

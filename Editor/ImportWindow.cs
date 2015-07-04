using UnityEngine;
using UnityEditor;

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
                //www.LoadImageIntoTexture(texture);
            } else {
                EditorUtility.DisplayDialog("Synctory", "Please select a file first.\nFiles just have a .synctory extension.", "OK");
            }

            //TODO: move this to loading file
            //EditorUtility.DisplayDialog("Synctory", "Imported!", "OK");
        }
    }
}

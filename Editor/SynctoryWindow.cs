using UnityEditor;
using UnityEngine;
using System.Collections;

namespace Synctory.Editor {
    public class SynctoryWindow : EditorWindow {
        public const int TIME_CONTROL_BUTTON_NUMBER = 2;
        public const int TIME_CONTROL_BUTTON_WIDTH = 25;

        [MenuItem("Synctory/Show Synctory Window")]
        public static void ShowWindow() {
            EditorWindow.GetWindow(typeof(SynctoryWindow), false, "Synctory");
        }

        public void OnGUI() {
            GUILayout.Label ("Time Controls", EditorStyles.boldLabel);
            Rect timeRect = EditorGUILayout.BeginHorizontal(GetTimeStyle());
                Rect controlsRect = EditorGUILayout.BeginHorizontal(GetTimeControlsStyle(timeRect));
                    if (GUILayout.Button(">", GetButtonGUIStyle(timeRect))) {
                        OnPlay();
                    }
                    if (GUILayout.Button(">|", GetButtonGUIStyle(timeRect))) {
                        OnStop();
                    }
                EditorGUILayout.EndHorizontal();
                Rect scrubRect = EditorGUILayout.BeginHorizontal(GetTimeScrubStyle());
                    if (GUILayout.Button("scrub", GetButtonGUIStyle(timeRect))) {
                        OnPlay();
                    }
                    if (GUILayout.Button("rub", GetButtonGUIStyle(timeRect))) {
                        OnStop();
                    }
                EditorGUILayout.EndHorizontal();
            //GUILayout.Label("I'm inside");
            //GUILayout.Label("Ditto");
            EditorGUILayout.EndHorizontal();
        }

        private void OnPlay() {
            Debug.Log("> pressed");
        }

        private void OnStop() {
            Debug.Log(">| pressed");
        }

        private GUIStyle GetButtonGUIStyle(Rect timeRect) {
            GUIStyle style = new GUIStyle("Button");
            style.fixedWidth = TIME_CONTROL_BUTTON_WIDTH;
            style.fixedHeight = TIME_CONTROL_BUTTON_WIDTH;
            return style;
        }

        private GUIStyle GetTimeStyle() {
            GUIStyle style = new GUIStyle();
            style.fixedHeight = TIME_CONTROL_BUTTON_WIDTH;
            style.stretchWidth = true;
            //style.alignment = TextAnchor.MiddleLeft;
            return style;
        }

        private GUIStyle GetTimeControlsStyle(Rect timeRect) {
            GUIStyle style = new GUIStyle();
            style.fixedWidth = TIME_CONTROL_BUTTON_NUMBER * TIME_CONTROL_BUTTON_WIDTH;
            style.alignment = TextAnchor.MiddleLeft;
            return style;
        }

        private GUIStyle GetTimeScrubStyle() {
            GUIStyle style = new GUIStyle();
            //style.fixedWidth = 100;
            //style.normal = GUIStyleState;
            return style;
        }
    }
}


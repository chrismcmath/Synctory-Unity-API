using UnityEditor;
using UnityEngine;
using System.Collections;

namespace Synctory.Editor {
    public class SynctoryWindow : EditorWindow {
        public const int TIME_CONTROL_BUTTON_NUMBER = 2;
        public const int TIME_CONTROL_BUTTON_WIDTH = 25;

        private static readonly Color COLOR_TIME_STYLE = new Color(0.1f, 0.2f, 0.3f);
        private static readonly Color COLOR_TIME_CONTROL_STYLE = Color.magenta;
        private static readonly Color COLOR_TIME_SCRUB_STYLE = Color.green;

        private Rect _TimeRect = new Rect();

        private Texture2D GetTextureFromColor (Color color) {
            Texture2D tex = new Texture2D(1,1);
            tex.SetPixel(1, 1, color);
            tex.Apply();
            return tex;
        }

        private GUIStyle _TimeStyle = null;
        public GUIStyle TimeStyle {
            get {
                Debug.Log("get timestyle");
                if (_TimeStyle == null) {
                    _TimeStyle = new GUIStyle();
                    //_TimeStyle.fixedHeight = TIME_CONTROL_BUTTON_WIDTH;
                    //_TimeStyle.stretchWidth = true;
                    _TimeStyle.normal.background = GetTextureFromColor(COLOR_TIME_STYLE);
                    //_TimeStyle.alignment = TextAnchor.MiddleLeft;
                }
                return _TimeStyle;
            }
        }

        private GUIStyle _TimeControlsStyle = null;
        public GUIStyle TimeControlsStyle {
            get {
                if (_TimeControlsStyle == null) {
                    _TimeControlsStyle = new GUIStyle();
                    _TimeControlsStyle.fixedWidth = TIME_CONTROL_BUTTON_NUMBER *
                        (TIME_CONTROL_BUTTON_WIDTH + ButtonStyle.margin.left + ButtonStyle.padding.left);
                    //_TimeControlsStyle.alignment = TextAnchor.MiddleLeft;
                    _TimeControlsStyle.normal.background = GetTextureFromColor(COLOR_TIME_CONTROL_STYLE);
                }
                return _TimeControlsStyle;
            }
        }

        private GUIStyle _ButtonStyle = null;
        public GUIStyle ButtonStyle {
            get {
                if (_ButtonStyle == null) {
                    _ButtonStyle = new GUIStyle("Button");
                    _ButtonStyle.fixedWidth = TIME_CONTROL_BUTTON_WIDTH;
                    _ButtonStyle.fixedHeight = TIME_CONTROL_BUTTON_WIDTH;
                    //_ButtonStyle.normal.background = TimeTexture;
                }
                return _ButtonStyle;
            }
        }

        private GUIStyle _TimeScrubStyle = null;
        public GUIStyle TimeScrubStyle {
            get {
                if (_TimeScrubStyle == null) {
                    _TimeScrubStyle = new GUIStyle();
                    //_TimeScrubStyle.fixedWidth = TIME_CONTROL_BUTTON_WIDTH;
                    //_TimeScrubStyle.fixedHeight = TIME_CONTROL_BUTTON_WIDTH;
                    _TimeScrubStyle.normal.background = GetTextureFromColor(COLOR_TIME_SCRUB_STYLE);
                }
                return _TimeScrubStyle;
            }
        }

        [MenuItem("Synctory/Show Synctory Window")]
        public static void ShowWindow() {
            EditorWindow.GetWindow(typeof(SynctoryWindow), false, "Synctory");
        }

        public void OnGUI() {
            GUILayout.Label ("Time Controls", EditorStyles.boldLabel);
            _TimeRect = EditorGUILayout.BeginHorizontal(TimeStyle);
                Rect controlsRect = EditorGUILayout.BeginHorizontal(TimeControlsStyle);
                    if (GUILayout.Button(">", ButtonStyle)) {
                        OnPlay();
                    }
                    if (GUILayout.Button(">|", ButtonStyle)) {
                        OnStop();
                    }
                EditorGUILayout.EndHorizontal();
                Rect scrubRect = EditorGUILayout.BeginHorizontal(TimeScrubStyle);
                    if (GUILayout.Button("P", ButtonStyle)) {
                        OnPlay();
                    }
                    if (GUILayout.Button("T", ButtonStyle)) {
                        OnStop();
                    }
                EditorGUILayout.EndHorizontal();
            //GUILayout.Label("I'm inside");
            //GUILayout.Label("Ditto");
            EditorGUILayout.EndHorizontal();
        }

        private void OnPlay() {
            Debug.Log("> pressed");
            ResetAllStyles();
        }

        private void OnStop() {
            Debug.Log(">| pressed");
        }

        private void ResetAllStyles() {
            _TimeStyle = null;
            _TimeControlsStyle = null;
            _ButtonStyle = null;
            _TimeScrubStyle = null;
        }
    }
}

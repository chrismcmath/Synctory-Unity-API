using UnityEditor;
using UnityEngine;
using System.Collections;

using Synctory.Utils;

namespace Synctory.Editor {
    public class SynctoryWindow : EditorWindow {
        public const int TIME_CONTROL_BUTTON_NUMBER = 2;
        public const int TIME_CONTROL_BUTTON_WIDTH = 25;
        public const int SCRUB_LABEL_WIDTH = 60;

        private static readonly Color COLOR_TIME_STYLE = new Color(0.1f, 0.2f, 0.3f);
        private static readonly Color COLOR_TIME_CONTROL_STYLE = Color.magenta;
        private static readonly Color COLOR_TIME_SCRUB_STYLE = Color.green;
        private static readonly Color COLOR_SCRUB_LABEL_STYLE = Color.red;

        private Rect _TimeRect = new Rect();
        private float _ScrubVal = 0f;

        private GUIStyle _TimeStyle = null;
        public GUIStyle TimeStyle {
            get {
                if (_TimeStyle == null) {
                    _TimeStyle = new GUIStyle();
                    _TimeStyle.normal.background = UnityHelpers.GetTextureFromColor(COLOR_TIME_STYLE);
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
                    //_TimeControlsStyle.normal.background = UnityHelpers.GetTextureFromColor(COLOR_TIME_CONTROL_STYLE);
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
                }
                return _ButtonStyle;
            }
        }

        private GUIStyle _TimeScrubStyle = null;
        public GUIStyle TimeScrubStyle {
            get {
                if (_TimeScrubStyle == null) {
                    _TimeScrubStyle = new GUIStyle();
                    _TimeScrubStyle.margin.top = (int) _TimeRect.height / 4;
                    _TimeScrubStyle.fixedHeight = 0;
                    //_TimeScrubStyle.normal.background = UnityHelpers.GetTextureFromColor(COLOR_TIME_SCRUB_STYLE);
                }
                return _TimeScrubStyle;
            }
        }

        private GUIStyle _ScrubLabelStyle = null;
        public GUIStyle ScrubLabelStyle {
            get {
                if (_ScrubLabelStyle == null) {
                    _ScrubLabelStyle = new GUIStyle();
                    _ScrubLabelStyle.alignment = TextAnchor.MiddleCenter;
                    _ScrubLabelStyle.normal.textColor = Color.white;
                    _ScrubLabelStyle.fixedWidth = SCRUB_LABEL_WIDTH;
                    _ScrubLabelStyle.fixedHeight = _TimeRect.height;
                    //_ScrubLabelStyle.normal.background = UnityHelpers.GetTextureFromColor(COLOR_SCRUB_LABEL_STYLE);
                }
                return _ScrubLabelStyle;
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
                    _ScrubVal = GUILayout.HorizontalSlider(_ScrubVal, 0f, 300f);
                EditorGUILayout.EndHorizontal();

                GUILayout.Label(SynctoryHelpers.GetTimeStringFromSeconds(_ScrubVal), ScrubLabelStyle);
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
            _ScrubLabelStyle = null;
        }
    }
}

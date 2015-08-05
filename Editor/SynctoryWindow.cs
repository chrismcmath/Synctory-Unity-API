using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;
using UnityEngine;

using Synctory.Objects;
using Synctory.Utils;

namespace Synctory.Editor {
    public class SynctoryWindow : EditorWindow {
        public const int TIME_CONTROL_BUTTON_NUMBER = 2;
        public const int TIME_CONTROL_BUTTON_WIDTH = 25;
        public const int SCRUB_LABEL_WIDTH = 60;
        public const int LOCATION_WIDTH = 200;

        private static readonly Color COLOR_TIME_STYLE = new Color(0.1f, 0.2f, 0.3f);
        private static readonly Color COLOR_TIME_CONTROL_STYLE = Color.magenta;
        private static readonly Color COLOR_TIME_SCRUB_STYLE = Color.green;
        private static readonly Color COLOR_SCRUB_LABEL_STYLE = Color.red;

        private Rect _TimeRect = new Rect();
        private float _ScrubVal = 0f;
        private Vector2 _LocationScrollLocation = Vector2.zero;
        private Vector2 _ScrollThisDraw = Vector2.zero;
        private Dictionary<int, Vector2> _LocationScrollLocations = new Dictionary<int, Vector2>();

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
                }
                return _ScrubLabelStyle;
            }
        }

        private GUIStyle _LocationsStyle = null;
        public GUIStyle LocationsStyle {
            get {
                if (_LocationsStyle == null) {
                    _LocationsStyle = new GUIStyle();
                    _LocationsStyle.stretchHeight = true;
                    _LocationsStyle.normal.background = UnityHelpers.GetTextureFromColor(Color.black);
                }
                return _LocationsStyle;
            }
        }

        private GUIStyle _InnerLocationsStyle = null;
        public GUIStyle InnerLocationsStyle {
            get {
                if (_InnerLocationsStyle == null) {
                    _InnerLocationsStyle = new GUIStyle();
                    _InnerLocationsStyle.stretchHeight = true;
                    _InnerLocationsStyle.normal.background = UnityHelpers.GetTextureFromColor(Color.green);
                }
                return _InnerLocationsStyle;
            }
        }

        private GUIStyle _LocationStyle = null;
        public GUIStyle LocationStyle {
            get {
                if (_LocationStyle == null) {
                    _LocationStyle = new GUIStyle();
                    _LocationStyle.fixedWidth = LOCATION_WIDTH;
                    _LocationStyle.stretchHeight = true;
                    _LocationStyle.margin = new RectOffset(10, 10, 10, 10);
                    _LocationStyle.normal.background = UnityHelpers.GetTextureFromColor(Color.gray);
                }
                return _LocationStyle;
            }
        }

        private GUIStyle _LocationScriptStyle = null;
        public GUIStyle LocationScriptStyle {
            get {
                if (_LocationScriptStyle == null) {
                    _LocationScriptStyle = new GUIStyle();
                    _LocationScriptStyle.fixedWidth = LOCATION_WIDTH - 20;
                    _LocationScriptStyle.alignment = TextAnchor.UpperCenter;
                    _LocationScriptStyle.clipping = TextClipping.Overflow;
                    _LocationScriptStyle.wordWrap = true;
                    _LocationScriptStyle.normal.background = UnityHelpers.GetTextureFromColor(Color.white);
                }
                return _LocationScriptStyle;
            }
        }

        [MenuItem("Synctory/Show Synctory Window")]
        public static void ShowWindow() {
            EditorWindow.GetWindow(typeof(SynctoryWindow), false, "Synctory");
        }

        public void OnGUI() {
            if (TexturesMissing()) {
                ResetAllStyles();
            }

            BuildTimeControls();
            BuildLocations();
        }

        private bool TexturesMissing() {
            return TimeStyle.normal.background == null;
        }

        /*
        private void CacheScrollInput() {
            if (Event.current != null && Event.current.type == EventType.scrollWheel) {
                _ScrollThisDraw = Event.current.delta;
            } else {
                _ScrollThisDraw = Vector2.zero;
            }
        }
        */

        private void BuildTimeControls() {
            GUILayout.Label ("Time Controls", EditorStyles.boldLabel);
            _TimeRect = EditorGUILayout.BeginHorizontal(TimeStyle);
                EditorGUILayout.BeginHorizontal(TimeControlsStyle);
                    if (GUILayout.Button(">", ButtonStyle)) {
                        OnPlay();
                    }
                    if (GUILayout.Button(">|", ButtonStyle)) {
                        OnStop();
                    }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal(TimeScrubStyle);
                    _ScrubVal = GUILayout.HorizontalSlider(_ScrubVal, 0f, 300f);
                EditorGUILayout.EndHorizontal();

                GUILayout.Label(SynctoryHelpers.GetTimeStringFromSeconds(_ScrubVal), ScrubLabelStyle);
            EditorGUILayout.EndHorizontal();
        }

        private void BuildLocations() {
            //GUILayout.Label("Locations", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal(LocationsStyle);
                _LocationScrollLocation = GUILayout.BeginScrollView(_LocationScrollLocation, true, false);
                    EditorGUILayout.BeginHorizontal(InnerLocationsStyle);
                        foreach (Location location in SynctoryHelpers.GetAllLocations()) {
                            LoadLocation(location);
                        }
                    EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndScrollView();
            EditorGUILayout.EndHorizontal();
        }

        private void LoadLocation(Location location) {
            Rect locRect = EditorGUILayout.BeginVertical(LocationStyle);
                GUILayout.Label(location.Name, EditorStyles.boldLabel);
                CheckLocationHasScroller(location.Key);
                _LocationScrollLocations[location.Key] = GUILayout.BeginScrollView(_LocationScrollLocations[location.Key], false, true);
                //Debug.Log("building scroll with pos: " + _LocationScrollLocations[location.Key]);
                //EditorGUILayout.BeginScrollView(_LocationScrollLocations[location.Key], new GUIStyle(), new GUIStyle());
                //Debug.Log("after building scroll, pos: " + _LocationScrollLocations[location.Key]);
                    GUILayout.TextArea("The start asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf The middle asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf  asdf the end", LocationScriptStyle);
                EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();



            /*
            Vector2 newScroll = _LocationScrollLocations[location.Key] + new Vector2(0f, _ScrollThisDraw.y);
            Debug.Log("set scroll to " + newScroll);
            _LocationScrollLocations[location.Key] = newScroll;
            //Debug.Log("current scroll: " + _LocationScrollLocations[location.Key]);
            //_LocationScrollLocations[location.Key] = new Vector2(10,10);
            */
        }

        private void Scroll(Vector2 scroll, Vector2 delta, bool scrollHorizontal, bool scrollVertical) {
            Debug.Log("scroll " + delta);
            /*
            float dt = Time.deltaTime / touch.deltaTime;
            if (dt == 0 || float.IsNaN(dt) || float.IsInfinity(dt)) {
                dt = 1.0f;
            }
            */
        }

        private void CheckLocationHasScroller(int key) {
            if (!_LocationScrollLocations.ContainsKey(key)) {
                _LocationScrollLocations.Add(key, Vector2.zero);
            }
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
            _LocationsStyle = null;
            _InnerLocationsStyle = null;
            _LocationStyle = null;
            _LocationScriptStyle = null;
        }
    }
}

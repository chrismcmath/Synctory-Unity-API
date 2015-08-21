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

        private GUIStyle _TimeStyle = null;
        public GUIStyle TimeStyle {
            get {
                if (_TimeStyle == null) {
                    _TimeStyle = new GUIStyle();
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
                    _LocationStyle.normal.background = UnityHelpers.GetTextureFromColor(Color.white);
                }
                return _LocationStyle;
            }
        }

        private GUIStyle _LocationHeaderStyle = null;
        public GUIStyle LocationHeaderStyle {
            get {
                if (_LocationHeaderStyle == null) {
                    _LocationHeaderStyle = new GUIStyle();
                    _LocationHeaderStyle.alignment = TextAnchor.UpperLeft;
                    _LocationHeaderStyle.fontSize = 12;
                    _LocationHeaderStyle.fontStyle = FontStyle.Bold;
                    _LocationHeaderStyle.normal.textColor = Color.gray;
                    _LocationHeaderStyle.normal.background = UnityHelpers.GetTextureFromColor(Color.black);
                }
                return _LocationHeaderStyle;
            }
        }

        private GUIStyle _LocationScriptStyle = null;
        public GUIStyle LocationScriptStyle {
            get {
                if (_LocationScriptStyle == null) {
                    _LocationScriptStyle = new GUIStyle();
                    _LocationScriptStyle.font = Resources.Load("Fonts/Courier") as Font;
                    _LocationScriptStyle.fixedWidth = LOCATION_WIDTH - 20;
                    _LocationScriptStyle.alignment = TextAnchor.UpperCenter;
                    _LocationScriptStyle.clipping = TextClipping.Overflow;
                    _LocationScriptStyle.normal.textColor = Color.black;
                    _LocationScriptStyle.fontStyle = FontStyle.Bold;
                    _LocationScriptStyle.wordWrap = true;
                }
                return _LocationScriptStyle;
            }
        }

        private Rect _TimeRect = new Rect();

        private float _ScrubVal = 0f;

        private Vector2 _LocationScrollLocation = Vector2.zero;

        private Dictionary<int, Vector2> _LocationScrollLocations = new Dictionary<int, Vector2>();

        [MenuItem("Synctory/Show Synctory Window")]
        public static void ShowWindow() {
            EditorWindow.GetWindow(typeof(SynctoryWindow), false, "Synctory");
        }

        public void OnInspectorUpdate() {
            //NOTE: Force an update when not in play mode
            if (!EditorApplication.isPlaying) {
                Synctory.Clock.CheckTimeChanged();
            }

            Repaint();
        }

        public void OnGUI() {
            if (TexturesMissing()) {
                ResetAllStyles();
            }

            BuildTimeControls();
            BuildLocations();

            UpdateClockOnScrub();
        }

        private void BuildTimeControls() {
            GUILayout.Label ("Time Controls", EditorStyles.boldLabel);
            _TimeRect = EditorGUILayout.BeginHorizontal(TimeStyle);
                EditorGUILayout.BeginHorizontal(TimeControlsStyle);
                    if (GUILayout.Button("|<", ButtonStyle)) {
                        OnStop();
                    }
                    if (Synctory.Clock.IsPlaying()) {
                        if (GUILayout.Button("||", ButtonStyle)) {
                            OnPause();
                        }
                    } else {
                        if (GUILayout.Button(">", ButtonStyle)) {
                            OnPlay();
                        }
                    }
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal(TimeScrubStyle);

                    _ScrubVal = GUILayout.HorizontalSlider(Synctory.Clock.TotalSeconds, 0f, (float) SynctoryHelpers.GetLastTimestamp().TotalSeconds);
                EditorGUILayout.EndHorizontal();

                GUILayout.Label(SynctoryHelpers.GetTimeStringFromSeconds(Synctory.Clock.TotalSeconds), ScrubLabelStyle);
            EditorGUILayout.EndHorizontal();
        }

        private void BuildLocations() {
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
            Rect locationRect = EditorGUILayout.BeginVertical(LocationStyle);

                GUIStyle locationHeaderStyle = LocationHeaderStyle;
                locationHeaderStyle.fixedWidth = location.CurrentUnitProgression * locationRect.width;
                GUILayout.Label(string.Format("{0} - {1}", location.Key, location.Name), locationHeaderStyle);
                if (location.LastFrameInfo != null) {
                    TimeSpan elapsed = TimeSpan.FromTicks((long) location.LastFrameInfo.Ticks);
                    TimeSpan total = TimeSpan.FromTicks((long) location.LastFrameInfo.TotalTicks);
                    GUILayout.Label(string.Format("Unit {0} ({1}/{2})",
                                location.CurrentUnit.Key,
                                StampUtils.FormatTimeSpan(elapsed),
                                StampUtils.FormatTimeSpan(total)),
                            locationHeaderStyle);
                } else {
                    GUILayout.Label("No Unit info", locationHeaderStyle);
                }

                CheckLocationHasScroller(location.Key);
                _LocationScrollLocations[location.Key] = GUILayout.BeginScrollView(_LocationScrollLocations[location.Key], false, true);
                if (location.CurrentUnit != null) {
                    GUILayout.TextArea(location.CurrentUnit.Text, LocationScriptStyle);
                } else {
                    GUILayout.TextArea("", LocationScriptStyle);
                }
                EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
        }

        private void UpdateClockOnScrub() {
            if (_ScrubVal != Synctory.Clock.TotalSeconds) {
                Synctory.Clock.SynctoryTime = TimeSpan.FromSeconds(_ScrubVal);
            }
        }

        private bool TexturesMissing() {
            return TimeStyle.normal.background == null;
        }


        private void CheckLocationHasScroller(int key) {
            if (!_LocationScrollLocations.ContainsKey(key)) {
                _LocationScrollLocations.Add(key, Vector2.zero);
            }
        }

        private void OnPlay() {
            Synctory.Clock.Play();
        }

        private void OnPause() {
            Synctory.Clock.Pause();
        }

        private void OnStop() {
            Synctory.Clock.Pause();
            Synctory.Clock.Reset();
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
            _LocationHeaderStyle = null;
            _LocationScriptStyle = null;
        }
    }
}

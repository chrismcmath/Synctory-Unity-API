using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Synctory.Utils {
    public static class AnimUtils {
        public static void AddIdleTransformCurve(string attribute, AnimationClip clip, float toTime, float value) {
#if UNITY_EDITOR
                AnimationCurve curve = AnimationCurve.Linear(
                        0f, value,
                        toTime, value);

                EditorCurveBinding curveBinding = new EditorCurveBinding();
                curveBinding.type = typeof(Transform);
                curveBinding.path = "";
                curveBinding.propertyName = attribute;
                    
                AnimationUtility.SetEditorCurve(clip, curveBinding, curve);
#endif
        }

        public static void AddTransformCurve(string attribute, AnimationClip clip, float fromTime, float fromValue, float toTime, float toValue) {
#if UNITY_EDITOR
                AnimationCurve curve = AnimationCurve.Linear(
                        fromTime, fromValue,
                        toTime, toValue);
                    
                clip.SetCurve("", typeof(Transform), attribute, curve);
#endif
        }
    }
}

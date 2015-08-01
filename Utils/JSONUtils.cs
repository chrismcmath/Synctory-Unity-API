using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Synctory.Utils {
    public static class JSONUtils {
        public static string ConvertToString(JSONObject obj) {
            return obj.str;
        }

        public static int ConvertToInt(JSONObject obj) {
            return (int) obj.n;
        }

        public static List<string> ConvertToStrings(List<JSONObject> objs) {
            List<string> strings = new List<string>();

            foreach (JSONObject obj in objs) {
                string str = ConvertToString(obj);
                if (!string.IsNullOrEmpty(str)) {
                    strings.Add(str);
                }
            }
            return strings;
        }

        public static List<int> ConvertToInts(List<JSONObject> objs) {
            List<int> ints = new List<int>();

            foreach (JSONObject obj in objs) {
                int key = ConvertToInt(obj);
                ints.Add(key);
            }
            return ints;
        }
    }
}

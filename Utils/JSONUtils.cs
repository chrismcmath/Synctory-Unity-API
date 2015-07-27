using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Synctory.Utils {
    public static class JSONUtils {
        public static string ConvertToString(JSONObject obj) {
            return obj.str;
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
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

using Synctory;
using Synctory.Objects;

namespace Synctory.Utils {
    public static class StampUtils {
        public const char TIMESTAMP_PREFIX = '[';
        public const char TIMESTAMP_SUFFIX = ']';
        public const char DENOMINATION_DIVIDER = ':';

        public const string HOUR_PREFIX = "00:";

        public static void LoadFromStamp(string fullStamp, Step step) {
            string trimmedStamp = fullStamp.Trim();
            if (trimmedStamp.IndexOf(TIMESTAMP_PREFIX) != -1 &&
                    trimmedStamp.IndexOf(TIMESTAMP_SUFFIX) != -1 &&
                    trimmedStamp.IndexOf(TIMESTAMP_PREFIX) < trimmedStamp.IndexOf(TIMESTAMP_SUFFIX)) {
                LoadStampAndTimestamp(trimmedStamp, step);
            } else {
                LoadStamp(trimmedStamp, step);
            }
        }

        public static void LoadStampAndTimestamp(string trimmedStamp, Step step) {
            string stamp = trimmedStamp.Remove(
                    trimmedStamp.IndexOf(TIMESTAMP_PREFIX),
                    trimmedStamp.IndexOf(TIMESTAMP_SUFFIX) - trimmedStamp.IndexOf(TIMESTAMP_PREFIX) + 1).Trim();
            string timestamp = trimmedStamp.Substring(
                    trimmedStamp.IndexOf(TIMESTAMP_PREFIX) + 1,
                    trimmedStamp.IndexOf(TIMESTAMP_SUFFIX) - trimmedStamp.IndexOf(TIMESTAMP_PREFIX) - 1).Trim();

            step.Stamp = stamp;

            TimeSpan time;
            TimeSpan.TryParse(FormatTimestamp(timestamp), out time);
            step.Timestamp = time;
        }

        public static void LoadStamp(string trimmedStamp, Step step) {
            step.Stamp = trimmedStamp;

            Step prevStep = SynctoryHelpers.GetPreviousStep(step);
            if (prevStep != null) {
                step.Timestamp = prevStep.Timestamp;
            } else {
                step.Timestamp = new TimeSpan(0);
            }
        }

        //NOTE: Make timestamp confirm to hh:mm:ss format
        private static string FormatTimestamp(string timestamp) {
            while (timestamp.Count(c => c == DENOMINATION_DIVIDER) < 2) {
                timestamp = string.Format("{0}{1}", HOUR_PREFIX, timestamp);
            }
            return timestamp;
        }
    }
}

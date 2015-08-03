using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using Synctory;
using Synctory.Objects;

namespace Synctory.Utils {
    public static class StampUtils {
        public const char TIMESTAMP_PREFIX = '[';
        public const char TIMESTAMP_SUFFIX = ']';

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
            step.Timestamp = TimeSpan.Parse(timestamp);
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
    }
}

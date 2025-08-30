using UnityEngine;

namespace BiruisredEngine
{
    /// <summary>
    /// Provides logging functionality for game systems, including standard logging, warnings, and errors.
    /// </summary>
    public static class LOG{
        /// <summary>
        /// Logs a message from a specific game system.
        /// </summary>
        /// <typeparam name="T">The type of the game system.</typeparam>
        /// <param name="system">The game system from which to log the message.</param>
        /// <param name="value">The message to log.</param>
        /// <param name="o">An optional Unity object associated with the log entry.</param>
        public static void Log<T>(this T system, object value, Object o = null) where T : GameSystem {
            PrintSystemLog(system, value, o);
        }

        /// <summary>
        /// Logs an error message from a specific game system.
        /// </summary>
        /// <typeparam name="T">The type of the game system.</typeparam>
        /// <param name="system">The game system from which to log the error.</param>
        /// <param name="value">The error message to log.</param>
        /// <param name="o">An optional Unity object associated with the log entry.</param>
        public static void LogError<T>(this T system, object value, Object o = null) where T : GameSystem {
            PrintSystemLog(system, value, o, LogType.Error);
        }

        /// <summary>
        /// Logs a warning message from a specific game system.
        /// </summary>
        /// <typeparam name="T">The type of the game system.</typeparam>
        /// <param name="system">The game system from which to log the warning.</param>
        /// <param name="value">The warning message to log.</param>
        /// <param name="o">An optional Unity object associated with the log entry.</param>
        public static void LogWarning<T>(this T system, object value, Object o = null) where T : GameSystem {
            PrintSystemLog(system, value, o, LogType.Warning);
        }

        /// <summary>
        /// Logs a general message.
        /// </summary>
        /// <param name="value">The message to log.</param>
        /// <param name="o">An optional Unity object associated with the log entry.</param>
        public static void Log(object value, Object o = null) {
            PrintLog($"<b>SYSTEM</b> {value}", o);
        }

        /// <summary>
        /// Logs a warning message.
        /// </summary>
        /// <param name="value">The warning message to log.</param>
        /// <param name="o">An optional Unity object associated with the log entry.</param>
        public static void LogWarning(object value, Object o = null) {
            PrintLog($"<b>SYSTEM</b> {value}", o, LogType.Warning);
        }

        /// <summary>
        /// Logs a suspicious message with a specified suspicious level.
        /// </summary>
        /// <param name="value">The suspicious message to log.</param>
        /// <param name="suspiciousLevel">A value between 1 and 100 indicating the level of suspicion.</param>
        public static void LogSuspicious(object value, int suspiciousLevel = 1) {
            if (suspiciousLevel < 1) suspiciousLevel = 1;
            PrintLog($"<b>SYSTEM</b> [sus:{suspiciousLevel}] - {value}", logType: LogType.Warning);
        }

        /// <summary>
        /// Logs an error message.
        /// </summary>
        /// <param name="value">The error message to log.</param>
        /// <param name="o">An optional Unity object associated with the log entry.</param>
        public static void LogError(object value, Object o = null) {
            PrintLog($"<b>SYSTEM</b> {value}", o, LogType.Error);
        }

        private static void PrintSystemLog<T>(T system, object value, Object o, LogType logType = LogType.Log)
            where T : GameSystem {
            PrintLog($"<b>{system.GetType().Name}</b> {value}", o);
        }

        private static void PrintLog(object value, Object o = null, LogType logType = LogType.Log) {
#if DEBUG
            // if (!EDITOR_PREFS.ENABLE_LOG) return;
            switch (logType) {
                case LogType.Log:
                    Debug.Log(value, o);
                    return;
                case LogType.Error:
                    Debug.LogError(value, o);
                    return;
                case LogType.Warning:
                    Debug.LogWarning(value, o);
                    return;
                case LogType.Exception:
                    Debug.LogError(value, o);
                    return;
                case LogType.Assert:
                    Debug.LogAssertion(value, o);
                    return;
            }
#endif
        }
    }
}
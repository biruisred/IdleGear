using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace BiruisredEngine
{
    public static class SYSTEM{
        public static UserSystem User { get; private set; }

        public static App App { get; internal set; }

        public static CancellationToken CtsOnDestroyToken => 
            _ctsOnDestroy?.Token ?? CancellationToken.None;

        private static readonly Dictionary<Type, GameSystem> _allSystems = new();
        private static CancellationTokenSource _ctsOnDestroy;

        public static T Get<T>() where T : GameSystem {
            if (_allSystems.TryGetValue(typeof(T), out var result)) return result as T;
            foreach (var (_, system) in _allSystems) {
                if (system is not T systemResult) continue;
                return systemResult;
            }

            // System must persist and should not be null. Log an error here to pause editor and identify the issue.
            LOG.LogError($"System {typeof(T).Name} not found");
            return null;
        }

        internal static void Create() {
            EVENT.Create();
            _ctsOnDestroy = new CancellationTokenSource();
            var gameSystemTypeTree = new TypeTree<GameSystem>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies) {
                foreach (var type in assembly.GetTypes()) {
                    EVENT.CollectEventFromType(type);
                    if (type.IsInterface || type.IsAbstract) continue;
                    if (!typeof(GameSystem).IsAssignableFrom(type)) continue;
                    gameSystemTypeTree.Add(type);
                }
            }

            var types = gameSystemTypeTree.GetLeaf();
            types.Remove(typeof(GameSystem));

            var userTypeCandidates = types.Where(x => typeof(UserSystem).IsAssignableFrom(x)).ToList();
            if (userTypeCandidates.Count > 1) {
                LOG.LogError("Multiple UserSystem implementations found. Please ensure only one exists.");
            }
            var userType = userTypeCandidates.FirstOrDefault();
            if (userType != null && Activator.CreateInstance(userType) is UserSystem userSystem) {
                User = userSystem;
                _allSystems.Add(userType, userSystem);
                types.Remove(userType);
            }

            var temp = new Dictionary<Type, GameSystem>();
            foreach (var type in types) {
                if (temp.ContainsKey(type)) continue;
                // Use Activator to create an instance of the GameSystem type
                var system = Activator.CreateInstance(type) as GameSystem;
                temp.Add(type, system);
            }

            var orderedSystem = temp.OrderBy(x => x.Value.Order);
            foreach (var (type, system) in orderedSystem) {
                LOG.Log($"{system.Order} {system.GetType().Name}");
                _allSystems.TryAdd(type, system);
            }
            LOG.Log($"Create {_allSystems.Count} systems");
        }
        
        internal static async UniTask Initialize() {
            LOG.Log("Initialize");
            foreach (var (_, system) in _allSystems) {
                system.Log("Initialize");
                await system.Initialize();
            }

            LOG.Log("Post initialize");
            foreach (var (_, system) in _allSystems) {
                system.Log("Post initialize");
                await system.PostInitialize();
            }
        }

        internal static async UniTask StartSession() {
            LOG.Log("Started session");
            foreach (var (_, system) in _allSystems) {
                system.Log("Started session");
                await system.StartSession();
            }

            LOG.Log("Post start session");
            foreach (var (_, system) in _allSystems) {
                system.Log("Post start session");
                await system.PostStartSession();
            }
        }

        internal static async UniTask EndSession() {
            LOG.Log("End session");
            foreach (var (_, system) in _allSystems) {
                system.Log("End session");
                await system.EndSession();
            }
        }

        public static async UniTask<bool> IsConnectToInternet() {
            try {
                using var www = UnityEngine.Networking.UnityWebRequest.Head("https://clients3.google.com/generate_204");
                www.timeout = 5;
                await www.SendWebRequest();
                return www.result == UnityEngine.Networking.UnityWebRequest.Result.Success;
            }
            catch {
                return false;
            }
        }

        /// <summary>
        /// Determines whether the application is running in a Google Play compatible environment on a PC.
        /// Refer to <a href="https://developer.android.com/games/playgames/pc-compatibility#c-sharp">PC Compatibility</a> for more details.
        /// </summary>
        /// <returns>
        /// Returns true if the application is running in a Google Play compatible environment on a PC (e.g., for Play Games integration);
        /// otherwise, returns false.
        /// </returns>
        private static bool IsRunningInGooglePlayPC() {
            if (Application.platform == RuntimePlatform.WindowsEditor) return false;
            try {
                var unityPlayerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                var currentActivity = unityPlayerClass.GetStatic<AndroidJavaObject>("currentActivity");
                var packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");
                return packageManager.Call<bool>("hasSystemFeature", "com.google.android.play.feature.HPE_EXPERIENCE");
            }
            catch (Exception e) {
                LOG.LogWarning($"Failed execute {nameof(IsRunningInGooglePlayPC)} : {e.Message}");
                return false;
            }
        }

        /// <summary>
        /// Retrieves the Android API level of the device the application is running on.
        /// Refer to <a href="https://docs.unity3d.com/ScriptReference/SystemInfo-operatingSystem.html">Unity Operating System</a> for more details.
        /// </summary>
        /// <returns>
        /// Returns the Android API level as an integer. If the platform is not Android or if 
        /// the API level cannot be parsed, returns -1.
        /// </returns>
        private static int GetAndroidAPILevel() {
            if (Application.platform != RuntimePlatform.Android) return -1;

            //regex for search "API-xx"
            const string regex = @"\bAPI-\d+\b";
            var match = Regex.Match(SystemInfo.operatingSystem, regex);
            if (!match.Success) return -1;
            var level = match.ToString()[4..]; // Skip the first 4 characters ("API-")
            if (!int.TryParse(level, out var result)) return -1;
            return result;
        }

        /// <summary>
        /// Cleans up and destroys all game systems.
        /// </summary>
        internal static void Destroy() {
            LOG.Log("Destroy");
            EVENT.Clear();
            _ctsOnDestroy?.Cancel();
            _ctsOnDestroy?.Dispose();
            _ctsOnDestroy = null;
            foreach (var (_, system) in _allSystems) {
                system.Destroy();
                system.Log("Destroy");
            }

            _allSystems.Clear();
        }
    }
}
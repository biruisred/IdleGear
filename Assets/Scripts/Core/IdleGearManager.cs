using System;
using BiruisredEngine;
using Firebase;
using Firebase.Auth;
using UnityEngine;

namespace IdleGear
{
    public class IdleGearManager : GameManager
    {
        private async void Start()
        {
            // try
            // {
            //     var status = await FirebaseApp.CheckAndFixDependenciesAsync();
            //     if (status != DependencyStatus.Available)
            //     {
            //         Debug.Log($"Could not resolve all Firebase databases dependencies: {status}");
            //         return;
            //     }
            //
            //     var auth = FirebaseAuth.DefaultInstance;
            //     await auth.SignInAnonymouslyAsync();
            //
            // }
            // catch (Exception e)
            // {
            //     Debug.LogException(e);
            // }
        }
    }
}

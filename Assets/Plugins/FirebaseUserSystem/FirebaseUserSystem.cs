// using BiruisredEngine;
// using Cysharp.Threading.Tasks;
// using Firebase;
// using Firebase.Auth;
// using Firebase.Functions;
// using UnityEngine;
//
// namespace Biruisred.Firebase
// {
//     public sealed class FirebaseUserSystem : UserSystem
//     {
//         protected override async UniTask<User> CreateUser()
//         {
//             var status = await FirebaseApp.CheckAndFixDependenciesAsync();
//             if (status != DependencyStatus.Available)
//             {
//                 Debug.Log($"Could not resolve all Firebase databases dependencies: {status}. Please check dependency information");
//                 return await base.CreateUser();
//             }
//
//             var auth = FirebaseAuth.DefaultInstance;
//             var user = auth.CurrentUser;
//
//             if (user == null) {
//                 var authResult = await auth.SignInAnonymouslyAsync();
// #if USE_AUTH_EMULATOR
//                 FirebaseFunctions.DefaultInstance.UseFunctionsEmulator("http://localhost:5001");
// #endif
//                 Debug.LogWarning("New user signed in successfully");
//                 return new User(authResult.User.UserId, authResult.User.DisplayName, true);
//             }
//             Debug.Log("Previous user signed in successfully");
//             return new User(user.UserId, user.DisplayName, true);
//         }
//     }
// }
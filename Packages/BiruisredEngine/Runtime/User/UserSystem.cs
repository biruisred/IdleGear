using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace BiruisredEngine
{
    /// <summary>
    /// Manages user side configuration, feature, user data .ect
    /// </summary>
    public class UserSystem : GameSystem{
        /// <summary>
        /// The unique identifier for the user.
        /// </summary>
        public string ID { get; private set; }

        /// <summary>
        /// Name of the user.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Whether the user is signed in.
        /// </summary>
        public bool IsSignedIn { get; private set; }

        /// <summary>
        /// Execution order in the system lifecycle.
        /// </summary>
        /// <remarks>
        /// The <see cref="UserSystem"/> always runs first in the system lifecycle, ensuring that user-related initialization
        /// is completed before any dependent systems, regardless of the order value.
        /// </remarks>
        public sealed override int Order => -1000;

        internal override async UniTask Initialize() {
            var user =  await CreateUser();
            ID = user.ID;
            Name = user.Name;
            IsSignedIn = user.IsSignedIn;
            this.Log($"User created, [ID : {ID}] [Name : {Name}] [Is signed in : {IsSignedIn}]");
            await base.Initialize();
        }

        /// <summary>
        /// Creates a new user.
        /// </summary>
        /// <remarks>
        /// This method provides a default implementation for creating a user. It attempts to retrieve the 
        /// user ID from the local preferences. If no ID is found, a new GUID is generated. The user ID is then 
        /// stored in preferences for future retrieval. Override this method to provide a custom implementation 
        /// for user creation tailored to your application's requirements.
        /// </remarks>
        /// <returns>
        /// A task representing the asynchronous operation, with the created <see cref="User"/> as the result.
        /// </returns>
        protected virtual UniTask<User> CreateUser() {
            // Retrieve the previous user ID or generate a new GUID if no ID exists.
            var userId = PlayerPrefs.GetString("default.userId", Guid.NewGuid().ToString());
            // Store the user ID for future retrieval.
            PlayerPrefs.SetString("default.userId", userId);
            return UniTask.FromResult(new User(userId, "Player Test", false));
        }
    }
    
    public interface IUserData{}
}
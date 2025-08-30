using Cysharp.Threading.Tasks;

namespace BiruisredEngine
{
    /// <summary>
    /// Serves as a base class for game systems, providing lifecycle management.
    /// </summary>
    public abstract class GameSystem
    {
        /// <summary>
        /// Gets the execution order of the game system. The default order is 999.
        /// </summary>
        public virtual int Order => 999;

        /// <summary>
        /// Initializes the game system asynchronously.
        /// </summary>
        internal virtual UniTask Initialize()
        {
            return OnInitialize();
        }

        /// <summary>
        /// Performs post-initialization.
        /// </summary>
        internal virtual UniTask PostInitialize()
        {
            return OnPostInitialize();
        }

        /// <summary>
        /// Starts the game system.
        /// </summary>
        internal virtual UniTask StartSession()
        {
            return OnStartSession();
        }

        /// <summary>
        /// Performs post-start.
        /// </summary>
        internal virtual UniTask PostStartSession()
        {
            return OnPostStartSession();
        }

        /// <summary>
        /// Ends the game system session. Can be overridden for custom session-end logic.
        /// </summary>
        internal virtual UniTask EndSession()
        {
            return OnEndSession();
        }

        /// <summary>
        /// Destroys the game system, performing necessary cleanup.
        /// </summary>
        internal virtual void Destroy()
        {
            OnDestroy();
        }

        /// <summary>
        /// Called during initialization. Can be overridden to perform custom initialization logic.
        /// </summary>
        protected virtual UniTask OnInitialize()
        {
            return UniTask.CompletedTask;
        }

        /// <summary>
        /// Called during post-initialization. Can be overridden to perform custom post-initialization logic.
        /// </summary>
        protected virtual UniTask OnPostInitialize()
        {
            return UniTask.CompletedTask;
        }

        /// <summary>
        /// Called when starting the game system. Can be overridden to perform custom start logic.
        /// </summary>
        protected virtual UniTask OnStartSession()
        {
            return UniTask.CompletedTask;
        }

        /// <summary>
        /// Called during post-start. Can be overridden to perform custom post-start logic.
        /// </summary>
        protected virtual UniTask OnPostStartSession()
        {
            return UniTask.CompletedTask;
        }

        /// <summary>
        /// Called when ending the game system session. Can be overridden for custom cleanup logic.
        /// </summary>
        protected virtual UniTask OnEndSession()
        {
            return UniTask.CompletedTask;
        }

        /// <summary>
        /// Called when destroying the game system. Can be overridden for custom cleanup logic.
        /// </summary>
        protected virtual void OnDestroy() { }
    }
}
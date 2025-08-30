using System;

namespace BiruisredEngine
{
    /// <summary>
    /// Represents a user with an ID and a name.
    /// </summary>
    public readonly struct User{
        public User(string id, string name, bool isSignedIn) {
            ID = id;
            Name = name;
            IsSignedIn = isSignedIn;
        }

        /// <summary>
        /// The unique identifier for the user.
        /// </summary>
        public readonly string ID;

        /// <summary>
        /// The name of the user.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// Whether the user is signed in.
        /// </summary>
        public readonly bool IsSignedIn;

        public bool Equals(User other) {
            return ID == other.ID && Name == other.Name;
        }

        public override bool Equals(object obj) {
            return obj is User other && Equals(other);
        }

        public override int GetHashCode() {
            return HashCode.Combine(ID, Name);
        }

        public static bool operator ==(User a, User b) {
            return a.ID == b.ID && a.Name == b.Name;
        }

        public static bool operator !=(User a, User b) {
            return !(a == b);
        }
    }
}
using System;
using System.Collections.Generic;

namespace BiruisredEngine
{
    /// <summary>
    /// Represents a tree structure for managing types in a hierarchical manner.
    /// <typeparam name="T">The base type for the tree.</typeparam>
    /// </summary>
    public sealed class TypeTree<T>{
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeTree{T}"/> class.
        /// </summary>
        public TypeTree() {
            _root = new Node(typeof(T));
        }

        private readonly Node _root;

        /// <summary>
        /// Adds a type to the tree.
        /// </summary>
        /// <param name="type">The type to add.</param>
        public void Add(Type type) {
            _root.TryAdd(type);
        }

        /// <summary>
        /// Retrieves a list of all leaf types in the tree.
        /// </summary>
        /// <returns>A list of leaf types.</returns>
        public List<Type> GetLeaf() {
            var result = new List<Type>();
            _root.CollectLeaf(result);
            return result;
        }

        /// <summary>
        /// Represents a node in the type tree.
        /// </summary>
        private class Node{
            /// <summary>
            /// Initializes a new instance of the <see cref="Node"/> class.
            /// </summary>
            /// <param name="type">The type represented by this node.</param>
            /// <param name="parent">The parent node.</param>
            public Node(Type type, Node parent = null) {
                _children = new();
                _type = type;
                _parent = parent;
            }

            private readonly Type _type;
            private Node _parent;
            private readonly List<Node> _children;

            /// <summary>
            /// Attempts to add a type to the node.
            /// </summary>
            /// <param name="type">The type to add.</param>
            /// <returns>True if the type was added successfully; otherwise, false.</returns>
            public bool TryAdd(Type type) {
                if (type.IsSubclassOf(_type)) {
                    foreach (var child in _children) {
                        if (child.TryAdd(type)) return true;
                    }

                    var childNode = new Node(type, this);
                    _children.Add(childNode);
                    return true;
                }

                if (!_type.IsSubclassOf(type)) return false;
                var parentNode = new Node(type, _parent);
                _parent._children.Add(parentNode);
                _parent._children.Remove(this);
                _parent = parentNode;
                parentNode._children.Add(this);
                return true;
            }

            /// <summary>
            /// Collects all leaf types from the node into a specified bucket.
            /// </summary>
            /// <param name="bucket">The list to collect leaf types into.</param>
            public void CollectLeaf(List<Type> bucket) {
                if (_children.Count == 0) {
                    bucket.Add(_type);
                    return;
                }

                foreach (var child in _children) {
                    child.CollectLeaf(bucket);
                }
            }
        }
    }
}
// Written by Nick Porter for CS 3500 - u0927946

using System;
using System.Collections.Generic;

namespace Dependencies
{
    /// <summary>
    /// Represents a node in a DependecyGraph.
    /// Contains representation for ordered pairs such that (s, this), and (this, s)
    /// Provides helper methods when working with nodes.
    /// The DependencyNodes in Dependees can be seen as a dependency represented as (Dependee, this)
    /// The DependencyNodes in Dependents can be seen as a dependency represented as (this, Dependent)
    /// </summary>
    class DependencyNode
    {
        /// <summary>
        /// Name of the node.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// An unordered set of dependees of this
        /// </summary>
        public HashSet<DependencyNode> Dependees { get; private set; }
        /// <summary>
        /// An unordered set of dependents of this
        /// </summary>
        public HashSet<DependencyNode> Dependents { get; private set; }

        /// <summary>
        /// Construcuts an empty node with a name of "name"
        /// </summary>
        /// <param name="name">The name of the node</param>
        public DependencyNode(string name)
        {
            Name = name;
            Dependees = new HashSet<DependencyNode>();
            Dependents = new HashSet<DependencyNode>();
        }

        /// <summary>
        /// Constructs this to be a "copy" of node
        /// </summary>
        /// <param name="node">The node to be copied.</param>
        public DependencyNode(DependencyNode node):this(node.Name)
        {

          

        }

        /// <summary>
        /// Adds dependent to this's Dependent's set.
        /// </summary>
        /// <param name="dependent">The node to be added</param>
        /// <returns>True if the node was successfully added, false otherwise.</returns>
        public bool AddDependent(DependencyNode dependent)
        {
            // HashSet does not allow duplicates.
            return Dependents.Add(dependent);
        }

        /// <summary>
        /// Adds dependee to this
        /// </summary>
        /// <param name="dependee">The node to be added.</param>
        /// <returns>True if the node was successfully added, false otherwise.</returns>
        public bool AddDependee(DependencyNode dependee)
        {
            // HashSet does not allow duplicates.
            return Dependees.Add(dependee);
        }

        /// <summary>
        /// Removes dependent from this
        /// </summary>
        /// <param name="dependent">The node to be removed from the Dependents.</param>
        /// <returns>True if the node was successfully removed from the set, false otherwise.</returns>
        public bool RemoveDependent(DependencyNode dependent)
        {
            return Dependents.Remove(dependent);
        }

        /// <summary>
        /// Removes dependee from this
        /// </summary>
        /// <param name="dependee">The node to be removed from the Dependees.</param>
        /// <returns>True if the node was successfully removed from the set, false otherwise.</returns>
        public bool RemoveDependee(DependencyNode dependee)
        {
            return Dependees.Remove(dependee);
        }

        /// <summary>
        /// If this has dependees
        /// </summary>
        /// <returns>True if this has dependees, oterhwise false.</returns>
        public bool HasDependees()
        {
            return Dependees.Count > 0;
        }

        /// <summary>
        /// If this has dependents
        /// </summary>
        /// <returns>True if this has dependents, oterhwise false.</returns>
        public bool HasDependents()
        {
            return Dependents.Count > 0;
        }

        /// <summary>
        /// Resets this node to contain no dependents and removes this as a dependees from all dependents
        /// </summary>
        public void clearDependents()
        {
            // Unlink this as a dependee of its dependents
            foreach (DependencyNode dependent in Dependents)
            {
                dependent.RemoveDependee(this);
            }
            Dependents.Clear();
        }

        /// <summary>
        /// Resets this node to contain no dependees and removes this as a dependent from all dependees
        /// </summary>
        public void clearDependees()
        {
            // Unlink this as a dependents of its dependee
            foreach (DependencyNode dependee in Dependees)
            {
                dependee.RemoveDependent(this);
            }
            Dependees.Clear();
        }
    }
}

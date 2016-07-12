// Skeleton implementation written by Joe Zachary for CS 3500, January 2015.
// Revised for CS 3500 by Joe Zachary, January 29, 2016
// Nick Porter - u0927946

using System;
using System.Collections.Generic;
using System.Linq;

namespace Dependencies
{
    /// <summary>
    /// A DependencyGraph can be modeled as a set of dependencies, where a dependency is an ordered 
    /// pair of strings.  Two dependencies (s1,t1) and (s2,t2) are considered equal if and only if 
    /// s1 equals s2 and t1 equals t2.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that the dependency (s,t) is in DG 
    ///    is called the dependents of s, which we will denote as dependents(s).
    ///        
    ///    (2) If t is a string, the set of all strings s such that the dependency (s,t) is in DG 
    ///    is called the dependees of t, which we will denote as dependees(t).
    ///    
    /// The notations dependents(s) and dependees(s) are used in the specification of the methods of this class.
    ///
    /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    ///     dependents("a") = {"b", "c"}
    ///     dependents("b") = {"d"}
    ///     dependents("c") = {}
    ///     dependents("d") = {"d"}
    ///     dependees("a") = {}
    ///     dependees("b") = {"a"}
    ///     dependees("c") = {"a"}
    ///     dependees("d") = {"b", "d"}
    ///     
    /// All of the methods below require their string parameters to be non-null.  This means that 
    /// the behavior of the method is undefined when a string parameter is null.  
    ///
    /// IMPORTANT IMPLEMENTATION NOTE
    /// 
    /// The simplest way to describe a DependencyGraph and its methods is as a set of dependencies, 
    /// as discussed above.
    /// 
    /// However, physically representing a DependencyGraph as, say, a set of ordered pairs will not
    /// yield an acceptably efficient representation.  DO NOT USE SUCH A REPRESENTATION.
    /// 
    /// You'll need to be more clever than that.  Design a representation that is both easy to work
    /// with as well acceptably efficient according to the guidelines in the PS3 writeup. Some of
    /// the test cases with which you will be graded will create massive DependencyGraphs.  If you
    /// build an inefficient DependencyGraph this week, you will be regretting it for the next month.
    /// </summary>
    public class DependencyGraph
    {
        /// <summary>
        /// Each string is the name of a node such as "a3", each string is mapped to a node representaiton.
        /// Each node contains a set of its depdents and dependees along with a name.
        /// </summary>
        private Dictionary<string, DependencyNode> nodes = new Dictionary<string, DependencyNode>();
        /// <summary>
        /// The number of dependeinces in the DependencyGraph.
        /// </summary>
        private int size;

        /// <summary>
        /// The number of dependencies in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get { return size; }
        }

        /// <summary>
        /// Constructs this to be a copy of graph
        /// </summary>
        /// <param name="graph">The graph to be copied</param>
        public DependencyGraph(DependencyGraph graph)
        {
            if(graph != null)
            {
                nodes = new Dictionary<string, DependencyNode>();
                foreach(KeyValuePair<string, DependencyNode> pair in graph.nodes)
                {

                    foreach(DependencyNode node in pair.Value.Dependents)
                    {
                        AddDependency(pair.Key, node.Name);
                    }

                }


            } else
            {
                throw new ArgumentNullException();
            }
        }

        /// <summary>
        /// Default 0 parameter constructor.
        /// I would normally leave this out, however the PS3GradingTests do not build without it.
        /// </summary>
        public DependencyGraph()
        {

        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.  Requires s != null.
        /// <throws>ArgumentNullException is s is null</throws>
        /// </summary>
        public bool HasDependents(string s)
        {
            if(s != null)
            {
                DependencyNode node;
                if(nodes.TryGetValue(s, out node))
                {
                    return node.HasDependents();
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
            return false;
        }

        /// <summary>
        /// Reports whether dependees(s) is non-empty.  Requires s != null.
        /// <throws>ArgumentNullException is s is null</throws>
        /// </summary>
        public bool HasDependees(string s)
        {
            if (s != null)
            {
                DependencyNode node;
                if (nodes.TryGetValue(s, out node))
                {
                    return node.HasDependees();
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
            return false;
        }

        /// <summary>
        /// Enumerates dependents(s).  Requires s != null.
        /// <throws>ArgumentNullException is s is null</throws>
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            if (s != null)
            {
                DependencyNode node;
                if (nodes.TryGetValue(s, out node))
                {
                    foreach (DependencyNode dependent in node.Dependents)
                    {
                        yield return dependent.Name;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException();
            }

            yield break;
        }

        /// <summary>
        /// Enumerates dependees(s).  Requires s != null.
        /// <throws>ArgumentNullException is s is null</throws>
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            if (s != null)
            {
                DependencyNode node;
                if (nodes.TryGetValue(s, out node))
                {
                    foreach (DependencyNode dependee in node.Dependees)
                    {
                        yield return dependee.Name;
                    }
                }
            }
            else
            {
                throw new ArgumentNullException();
            }

            yield break;
        }

        /// <summary>
        /// Adds the dependency (s,t) to this DependencyGraph.
        /// This has no effect if (s,t) already belongs to this DependencyGraph.
        /// Requires s != null and t != null.
        /// <throws>ArgumentNullException is s or t is null</throws>
        /// </summary>
        public void AddDependency(string s, string t)
        {
            
            if(s != null && t != null)
            {
                DependencyNode node1, node2;
                if(!nodes.TryGetValue(s, out node1))
                {
                    // Create new node becuase one does not exsist and add it to the verticie dict.
                    node1 = new DependencyNode(s);
                    nodes.Add(s, node1);
                } 

                if (!nodes.TryGetValue(t, out node2))
                {
                    // Create new node becuase one does not exsist and add it to the verticie dict.
                    node2 = new DependencyNode(t);
                    nodes.Add(t, node2);
                }
                // (depenee, dependent)
                if(node1.AddDependent(node2) && node2.AddDependee(node1))
                {
                    size++;
                }
            }
            else
            {
                throw new ArgumentNullException();
            }

        }

        /// <summary>
        /// Removes the dependency (s,t) from this DependencyGraph.
        /// Does nothing if (s,t) doesn't belong to this DependencyGraph.
        /// Requires s != null and t != null.
        /// <throws>ArgumentNullException is s or t is null</throws>
        /// </summary>
        public void RemoveDependency(string s, string t)
        {

            if (s != null && t != null)
            {
                DependencyNode node1, node2;
                if(nodes.TryGetValue(s, out node1) && nodes.TryGetValue(t, out node2))
                {
                    // Attempt to remove both node 1 and node 2, if both succeed decrement the size.
                    if(node1.RemoveDependent(node2) && node2.RemoveDependee(node1))
                    {
                        size--;
                    }

                    // If the node is 'empty' remove it from the dict
                    if(node1.Dependees.Count == 0 && node1.Dependents.Count == 0)
                    {
                        nodes.Remove(node1.Name);
                    }
                    // Same with the second node
                    if (node2.Dependees.Count == 0 && node2.Dependents.Count == 0)
                    {
                        nodes.Remove(node2.Name);
                    }

                }
            }
            else
            {
                throw new ArgumentNullException();
            }

        }

        /// <summary>
        /// Removes all existing dependencies of the form (s,r).  Then, for each
        /// t in newDependents, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// <throws>ArgumentNullException is s or t is null</throws>
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if(s != null && newDependents != null)
            {
                DependencyNode node;
                if(nodes.TryGetValue(s, out node)) {

                    // Update size
                    size -= node.Dependents.Count;
                    // Clear dependents & dereference node as a dependee in its dependents.
                    node.clearDependents();

                    // Add the new dependents
                    foreach (string nodeName in newDependents)
                    {
                        AddDependency(s, nodeName);
                    }

                }
                else
                {
                    nodes.Add(s, new DependencyNode(s));
                    ReplaceDependents(s, newDependents);
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

        /// <summary>
        /// Removes all existing dependencies of the form (r,t).  Then, for each 
        /// s in newDependees, adds the dependency (s,t).
        /// Requires s != null and t != null.
        /// <throws>ArgumentNullException is s or t is null</throws>
        /// </summary>
        public void ReplaceDependees(string t, IEnumerable<string> newDependees)
        {
            if (t != null && newDependees != null)
            {
                DependencyNode node;
                if(nodes.TryGetValue(t, out node))
                {
                    // Update size
                    size -= node.Dependees.Count;
                    // Clear the dependees
                    node.clearDependees();
                    // Add the new dependencies
                    foreach (string nodeName in newDependees)
                    {
                        AddDependency(nodeName, t);
                    }
                } else
                {
                    nodes.Add(t, new DependencyNode(t));
                    ReplaceDependees(t, newDependees);
                }
            }
            else
            {
                throw new ArgumentNullException();
            }
        }

    }
}
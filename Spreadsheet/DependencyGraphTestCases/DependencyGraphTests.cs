using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Dependencies;
using System.Collections.Generic;

namespace DependencyGraphTestCases
{

    /// <summary>
    /// This class provides comprehensive unit testing for the DependencyGraph class.
    /// </summary>
    [TestClass]
    public class DependencyGraphTests
    {

        /// <summary>
        /// Tests the default initialization of the dependency graph
        /// </summary>
        [TestMethod]
        public void Constructor()
        {
            DependencyGraph g = new DependencyGraph();
            Assert.IsFalse(g.HasDependees("a3"));
            Assert.AreEqual(g.Size, 0);
        }

        /// <summary>
        /// Simple size test
        /// </summary>
        [TestMethod]
        public void Size()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("s1", "t1");
            Assert.AreEqual(g.Size, 1);
        }

        /// <summary>
        /// Test size when adding duplicate dependency
        /// </summary>
        [TestMethod]
        public void Size1()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("s1", "t1");
            g.AddDependency("s1", "t1");
            Assert.AreEqual(g.Size, 1);
        }

        /// <summary>
        /// Simple size test
        /// </summary>
        [TestMethod]
        public void Size2()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("s1", "t1");
            g.AddDependency("s1", "t4");
            g.AddDependency("s2", "t1");
            g.AddDependency("s1", "t2");
            Assert.AreEqual(g.Size, 4);
        }

        /// <summary>
        /// Simple size test adding duplicate
        /// </summary>
        [TestMethod]
        public void Size3()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("s1", "t1");
            g.AddDependency("s1", "t4");
            g.AddDependency("s2", "t1");
            g.AddDependency("s1", "t2");
            g.AddDependency("s1", "t4");
            Assert.AreEqual(g.Size, 4);
        }

        /// <summary>
        /// Testing size when adding a null string
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Size4()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("s1", null);
            Assert.AreEqual(g.Size, 0);
        }

        /// <summary>
        /// Testing size when adding a null string
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Size5()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency(null, "s2");
            Assert.AreEqual(g.Size, 0);
        }

        /// <summary>
        /// Simple size test removing a dependecny that does not exist.
        /// </summary>
        [TestMethod]
        public void Size6()
        {
            DependencyGraph g = new DependencyGraph();
            g.RemoveDependency("s1", "t1");
            Assert.AreEqual(g.Size, 0);
        }

        /// <summary>
        /// Simple size test
        /// </summary>
        [TestMethod]
        public void Size7()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("s1", "t1");
            g.RemoveDependency("s1", "t1");
            Assert.AreEqual(g.Size, 0);
        }

        /// <summary>
        /// Simple size test trying to remove the same dependency twice
        /// </summary>
        [TestMethod]
        public void Size8()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("s1", "t1");
            g.AddDependency("s1", "t2");
            // Remove same dependency twice.
            g.RemoveDependency("s1", "t1");
            g.RemoveDependency("s1", "t1");
            Assert.AreEqual(g.Size, 1);
        }

        /// <summary>
        /// Simple size test 
        /// </summary>
        [TestMethod]
        public void Siz9()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("s1", "t1");
            g.AddDependency("s1", "t4");
            g.AddDependency("s2", "t1");
            g.AddDependency("s1", "t2");
            g.RemoveDependency("s1", "t1");

            Assert.AreEqual(g.Size, 3);
        }

        /// <summary>
        /// Simple size test removing all known dependencies
        /// </summary>
        [TestMethod]
        public void Siz10()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("s1", "t1");
            g.AddDependency("s1", "t4");
            g.AddDependency("s2", "t1");
            g.AddDependency("s1", "t2");
            g.RemoveDependency("s1", "t1");
            g.RemoveDependency("s1", "t4");
            g.RemoveDependency("s2", "t1");
            g.RemoveDependency("s1", "t2");

            Assert.AreEqual(g.Size, 0);
        }

        /// <summary>
        /// Testing size when removing a null string
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Size11()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("s1", "s2");
            g.RemoveDependency("s1", null);
            Assert.AreEqual(g.Size, 1);
        }

        /// <summary>
        /// Testing size when removing a null string
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Size12()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("s1", "s2");
            g.RemoveDependency(null, "s2");
            Assert.AreEqual(g.Size, 1);
        }

        /// <summary>
        /// Testing when s1 does not exsist
        /// </summary>
        [TestMethod]
        public void HasDependents()
        {
            DependencyGraph g = new DependencyGraph();
            Assert.IsFalse(g.HasDependents("s1"));
        }

        /// <summary>
        /// Testing when passing null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HasDependents1()
        {
            DependencyGraph g = new DependencyGraph();
            Assert.IsFalse(g.HasDependents(null));
        }

        /// <summary>
        /// Simple test
        /// </summary>
        [TestMethod]
        public void HasDependents2()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("a2", "a1");
            Assert.IsTrue(g.HasDependents("a2"));
            Assert.IsTrue(g.HasDependees("a1"));

        }

        /// <summary>
        /// Add the dependencies then remove
        /// </summary>
        [TestMethod]
        public void HasDependents3()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("a2", "a1");
            Assert.IsTrue(g.HasDependents("a2"));
            Assert.IsTrue(g.HasDependees("a1"));
            Assert.IsFalse(g.HasDependents("a1"));
            Assert.IsFalse(g.HasDependees("a2"));

            g.RemoveDependency("a2", "a1");
            Assert.IsFalse(g.HasDependents("a2"));
            Assert.IsFalse(g.HasDependees("a1"));
        }


        /// <summary>
        /// Add the dependencies then remove
        /// </summary>
        [TestMethod]
        public void HasDependents4()
        {
            // outward arrows means it has dependents
            // inward arrows means it has dependees
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("a4", "a2");
            g.AddDependency("a3", "a1");
            g.AddDependency("a2", "a1");
            g.AddDependency("a3", "a2");

            // test a4
            Assert.IsFalse(g.HasDependees("a4"));
            Assert.IsTrue(g.HasDependents("a4"));
            // test a2
            Assert.IsTrue(g.HasDependees("a2"));
            Assert.IsTrue(g.HasDependents("a2"));
            // test a3
            Assert.IsFalse(g.HasDependees("a3"));
            Assert.IsTrue(g.HasDependents("a3"));
            // test a1
            Assert.IsTrue(g.HasDependees("a1"));
            Assert.IsFalse(g.HasDependents("a1"));
        }

        /// <summary>
        /// Add the dependencies then remove
        /// </summary>
        [TestMethod]
        public void HasDependents5()
        {
            // outward arrows means it has dependents
            // inward arrows means it has dependees
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("a4", "a2");
            g.AddDependency("a3", "a1");
            g.AddDependency("a2", "a1");
            g.AddDependency("a3", "a2");

            g.RemoveDependency("a3", "a1");
            g.RemoveDependency("a2", "a1");
            Assert.IsTrue(g.HasDependents("a3"));
            Assert.IsFalse(g.HasDependents("a2"));
        }

        /// <summary>
        /// Tests the "null" case on HasDependees
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void HasDependees()
        {
            DependencyGraph g = new DependencyGraph();
            Assert.IsFalse(g.HasDependees(null));
        }

        /// <summary>
        /// Enumerates though GetDependents and checks to ensure it has the correct size and contains the correct verticies.
        /// </summary>
        [TestMethod] 
        public void GetDependents1()
        {
            DependencyGraph g = new DependencyGraph();
            string[] dependentArray = { "a1", "a3", "a4", "a5", "a7", "a9" };
            HashSet<string> dependents = new HashSet<string>(dependentArray);

            g.AddDependency("a2", "a1");
            g.AddDependency("a2", "a3");
            g.AddDependency("a2", "a4");
            g.AddDependency("a2", "a5");
            g.AddDependency("a2", "a7");
            g.AddDependency("a2", "a9");

            int counter = 0;
            foreach (string s in g.GetDependents("a2"))
            {
                Assert.IsTrue(dependents.Remove(s));
                counter++;
            }
            Assert.AreEqual(dependents.Count, 0);
            Assert.AreEqual(counter, 6);
        }

        /// <summary>
        /// Ensures nothing is returned when null is passed.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDependents2()
        {
            DependencyGraph g = new DependencyGraph();
            string[] dependentArray = { "a1", "a3", "a4", "a5", "a7", "a9" };
            HashSet<string> dependents = new HashSet<string>(dependentArray);

            g.AddDependency("a2", "a1");
            g.AddDependency("a2", "a3");
            g.AddDependency("a2", "a4");
            g.AddDependency("a2", "a5");
            g.AddDependency("a2", "a7");
            g.AddDependency("a2", "a9");

            int counter = 0;
            foreach (string s in g.GetDependents(null))
            {
                Assert.IsTrue(dependents.Remove(s));
                counter++;
            }
            Assert.AreEqual(dependents.Count, 6);
            Assert.AreEqual(counter, 0);
        }

        /// <summary>
        /// Ensures nothing is returned when a value with no dependents is passed.
        /// </summary>
        [TestMethod]
        public void GetDependents3()
        {
            DependencyGraph g = new DependencyGraph();
            string[] dependentArray = { "a1", "a3", "a4", "a5", "a7", "a9" };
            HashSet<string> dependents = new HashSet<string>(dependentArray);

            g.AddDependency("a2", "a1");
            g.AddDependency("a2", "a3");
            g.AddDependency("a2", "a4");
            g.AddDependency("a2", "a5");
            g.AddDependency("a2", "a7");
            g.AddDependency("a2", "a9");

            int counter = 0;
            foreach (string s in g.GetDependents("a9"))
            {
                Assert.IsTrue(dependents.Remove(s));
                counter++;
            }
            Assert.AreEqual(dependents.Count, 6);
            Assert.AreEqual(counter, 0);
        }

        /// <summary>
        /// Enumerates though GetDependees and checks to ensure it has the correct size and contains the correct verticies.
        /// </summary>
        [TestMethod]
        public void GetDependees1()
        {
            DependencyGraph g = new DependencyGraph();
            string[] dependeesArray = { "a2", "a4", "a5", "a7", "a8", "a9" };
            HashSet<string> dependees = new HashSet<string>(dependeesArray);

            g.AddDependency("a2", "a3");
            g.AddDependency("a4", "a3");
            g.AddDependency("a5", "a3");
            g.AddDependency("a7", "a3");
            g.AddDependency("a8", "a3");
            g.AddDependency("a9", "a3");

            int counter = 0;
            foreach (string s in g.GetDependees("a3"))
            {
                Assert.IsTrue(dependees.Remove(s));
                counter++;
            }
            Assert.AreEqual(dependees.Count, 0);
            Assert.AreEqual(counter, 6);
        }

        /// <summary>
        /// Ensures nothing is returned when null is passed.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetDependees2()
        {
            DependencyGraph g = new DependencyGraph();
            string[] dependeesArray = { "a2", "a4", "a5", "a7", "a8", "a9" };
            HashSet<string> dependees = new HashSet<string>(dependeesArray);

            g.AddDependency("a2", "a3");
            g.AddDependency("a4", "a3");
            g.AddDependency("a5", "a3");
            g.AddDependency("a7", "a3");
            g.AddDependency("a8", "a3");
            g.AddDependency("a9", "a3");

            int counter = 0;
            foreach (string s in g.GetDependees(null))
            {
                Assert.IsTrue(dependees.Remove(s));
                counter++;
            }
            Assert.AreEqual(dependees.Count, 6);
            Assert.AreEqual(counter, 0);
        }

        /// <summary>
        /// Ensures nothing is returned when a value with no dependents is passed.
        /// </summary>
        [TestMethod]
        public void GetDependees3()
        {
            DependencyGraph g = new DependencyGraph();
            string[] dependeesArray = { "a2", "a4", "a5", "a7", "a8", "a9" };
            HashSet<string> dependees = new HashSet<string>(dependeesArray);

            g.AddDependency("a2", "a3");
            g.AddDependency("a4", "a3");
            g.AddDependency("a5", "a3");
            g.AddDependency("a7", "a3");
            g.AddDependency("a8", "a3");
            g.AddDependency("a9", "a3");

            int counter = 0;
            foreach (string s in g.GetDependees("a9"))
            {
                Assert.IsTrue(dependees.Remove(s));
                counter++;
            }
            Assert.AreEqual(dependees.Count, 6);
            Assert.AreEqual(counter, 0);
        }

        /// <summary>
        /// Replaces all dependents. Re-adds one existing dependent and one new one.
        /// </summary>
        [TestMethod]
        public void ReplaceDependents()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("a2", "a1");
            g.AddDependency("a2", "a3");
            g.AddDependency("a2", "a4");
            g.AddDependency("a2", "a5");
            g.AddDependency("a2", "a7");
            g.AddDependency("a2", "a9");

            Assert.IsTrue(g.HasDependents("a2"));
            Assert.IsTrue(g.HasDependees("a1"));
            Assert.IsTrue(g.HasDependees("a3"));
            Assert.IsTrue(g.HasDependees("a4"));
            Assert.IsTrue(g.HasDependees("a5"));
            Assert.IsTrue(g.HasDependees("a7"));
            Assert.IsTrue(g.HasDependees("a9"));

            List<string> newDependents = new List<string>();
            newDependents.Add("a4");
            newDependents.Add("a10");
            g.ReplaceDependents("a2", newDependents);

            Assert.IsTrue(g.HasDependents("a2"));
            Assert.IsFalse(g.HasDependees("a1"));
            Assert.IsFalse(g.HasDependees("a3"));
            Assert.IsTrue(g.HasDependees("a4"));
            Assert.IsFalse(g.HasDependees("a5"));
            Assert.IsFalse(g.HasDependees("a7"));
            Assert.IsFalse(g.HasDependees("a9"));
            Assert.IsTrue(g.HasDependees("a10"));
            Assert.AreEqual(g.Size, 2);
        }

        /// <summary>
        /// Passing an empty list into ReplaceDependents()
        /// </summary>
        [TestMethod]
        public void ReplaceDependents2()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("a2", "a1");
            g.AddDependency("a2", "a3");
            g.AddDependency("a2", "a4");
            g.AddDependency("a2", "a5");
            g.AddDependency("a2", "a7");
            g.AddDependency("a2", "a9");

            Assert.IsTrue(g.HasDependents("a2"));
            Assert.IsTrue(g.HasDependees("a1"));
            Assert.IsTrue(g.HasDependees("a3"));
            Assert.IsTrue(g.HasDependees("a4"));
            Assert.IsTrue(g.HasDependees("a5"));
            Assert.IsTrue(g.HasDependees("a7"));
            Assert.IsTrue(g.HasDependees("a9"));

            List<string> newDependents = new List<string>();
            g.ReplaceDependents("a2", newDependents);

            Assert.IsFalse(g.HasDependents("a2"));
            Assert.IsFalse(g.HasDependees("a1"));
            Assert.IsFalse(g.HasDependees("a3"));
            Assert.IsFalse(g.HasDependees("a4"));
            Assert.IsFalse(g.HasDependees("a5"));
            Assert.IsFalse(g.HasDependees("a7"));
            Assert.IsFalse(g.HasDependees("a9"));
            Assert.IsFalse(g.HasDependees("a10"));
            Assert.AreEqual(g.Size, 0);
        }

        /// <summary>
        /// Passing null to be the cell replaced.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReplaceDependents3()
        {
            DependencyGraph g = new DependencyGraph();
            g.ReplaceDependents(null, new List<string>());
            Assert.AreEqual(g.Size, 0);
        }

        /// <summary>
        /// Passing a null list.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReplaceDependents4()
        {
            DependencyGraph g = new DependencyGraph();
            g.ReplaceDependents("a3", null);
            Assert.AreEqual(g.Size, 0);
        }

        /// <summary>
        /// Replaces all dependees. Re-adds one existing dependent and one new one.
        /// </summary>
        [TestMethod]
        public void ReplaceDependees()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("a2", "a3");
            g.AddDependency("a4", "a3");
            g.AddDependency("a5", "a3");
            g.AddDependency("a7", "a3");
            g.AddDependency("a8", "a3");
            g.AddDependency("a9", "a3");

            Assert.IsTrue(g.HasDependees("a3"));
            Assert.IsTrue(g.HasDependents("a2"));
            Assert.IsTrue(g.HasDependents("a4"));
            Assert.IsTrue(g.HasDependents("a5"));
            Assert.IsTrue(g.HasDependents("a7"));
            Assert.IsTrue(g.HasDependents("a8"));
            Assert.IsTrue(g.HasDependents("a9"));

            List<string> newDependees = new List<string>();
            newDependees.Add("a4");
            newDependees.Add("a10");
            g.ReplaceDependees("a3", newDependees);

            Assert.IsTrue(g.HasDependees("a3"));
            Assert.IsFalse(g.HasDependents("a2"));
            Assert.IsTrue(g.HasDependents("a4"));
            Assert.IsFalse(g.HasDependents("a5"));
            Assert.IsFalse(g.HasDependents("a7"));
            Assert.IsFalse(g.HasDependents("a8"));
            Assert.IsFalse(g.HasDependents("a9"));
            Assert.IsTrue(g.HasDependents("a10"));
            Assert.AreEqual(g.Size, 2);
        }

        /// <summary>
        /// Passes an empty list into ReplaceDependees()
        /// </summary>
        [TestMethod]
        public void ReplaceDependees2()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("a2", "a3");
            g.AddDependency("a4", "a3");
            g.AddDependency("a5", "a3");
            g.AddDependency("a7", "a3");
            g.AddDependency("a8", "a3");
            g.AddDependency("a9", "a3");

            Assert.IsTrue(g.HasDependees("a3"));
            Assert.IsTrue(g.HasDependents("a2"));
            Assert.IsTrue(g.HasDependents("a4"));
            Assert.IsTrue(g.HasDependents("a5"));
            Assert.IsTrue(g.HasDependents("a7"));
            Assert.IsTrue(g.HasDependents("a8"));
            Assert.IsTrue(g.HasDependents("a9"));

            List<string> newDependees = new List<string>();
            g.ReplaceDependees("a3", newDependees);

            Assert.IsFalse(g.HasDependees("a3"));
            Assert.IsFalse(g.HasDependents("a2"));
            Assert.IsFalse(g.HasDependents("a4"));
            Assert.IsFalse(g.HasDependents("a5"));
            Assert.IsFalse(g.HasDependents("a7"));
            Assert.IsFalse(g.HasDependents("a8"));
            Assert.IsFalse(g.HasDependents("a9"));
            Assert.IsFalse(g.HasDependents("a10"));
            Assert.AreEqual(g.Size, 0);
        }

        /// <summary>
        /// Test method to test adding 100,000 dependencies then replacing them.
        /// </summary>
        [TestMethod]
        public void ReplaceDepenees3()
        {
            DependencyGraph g = new DependencyGraph();

            for(int i = 0; i < 100000; i++)
            {
                g.AddDependency("a" + i, "a3");
            }

            List<string> newDependees = new List<string>();
            newDependees.Add("a4");
            newDependees.Add("a10");
            g.ReplaceDependees("a3", newDependees);
            Assert.IsTrue(g.HasDependees("a3"));
            Assert.IsTrue(g.HasDependents("a4"));
            Assert.IsTrue(g.HasDependents("a10"));
            Assert.AreEqual(g.Size, 2);
        }

        /// <summary>
        /// Passing null to be the cell replaced.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReplaceDependees4()
        {
            DependencyGraph g = new DependencyGraph();
            g.ReplaceDependees(null, new List<string>());
            Assert.AreEqual(g.Size, 0);
        }

        /// <summary>
        /// Passing a null list.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReplaceDependees5()
        {
            DependencyGraph g = new DependencyGraph();
            g.ReplaceDependees("a3", null);
            Assert.AreEqual(g.Size, 0);
        }

        /// <summary>
        /// Tests the following DG
        /// For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
        /// dependents("a") = {"b", "c"}
        /// dependents("b") = {"d"}
        /// dependents("c") = {}
        /// dependents("d") = {"d"}
        /// dependees("a") = {}
        /// dependees("b") = {"a"}
        /// dependees("c") = {"a"}
        /// dependees("d") = {"b", "d"}
        /// </summary>
        [TestMethod]
        public void SpecTest()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("a", "b");
            g.AddDependency("a", "c");
            g.AddDependency("b", "d");
            g.AddDependency("d", "d");

            Assert.IsTrue(g.HasDependents("a"));
            Assert.IsTrue(g.HasDependents("b"));
            Assert.IsFalse(g.HasDependents("c"));
            Assert.IsTrue(g.HasDependents("d"));

            Assert.IsFalse(g.HasDependees("a"));
            Assert.IsTrue(g.HasDependees("b"));
            Assert.IsTrue(g.HasDependees("c"));
            Assert.IsTrue(g.HasDependees("d"));

            // Test "a"
            HashSet<string> aDependents = new HashSet<string>();
            aDependents.Add("b");
            aDependents.Add("c");
            HashSet<string> aDependees = new HashSet<string>();
            foreach(string aDependent in g.GetDependents("a"))
            {
                Assert.IsTrue(aDependents.Remove(aDependent));
            }
            Assert.AreEqual(aDependents.Count, 0);
            foreach (string aDependee in g.GetDependees("a"))
            {
                Assert.IsTrue(aDependees.Remove(aDependee));
            }
            Assert.AreEqual(aDependees.Count, 0);


            // Test "b"
            HashSet<string> bDependents = new HashSet<string>();
            bDependents.Add("d");
            HashSet<string> bDependees = new HashSet<string>();
            bDependees.Add("a");
            foreach (string bDependent in g.GetDependents("b"))
            {
                Assert.IsTrue(bDependents.Remove(bDependent));
            }
            Assert.AreEqual(bDependents.Count, 0);
            foreach (string bDependee in g.GetDependees("b"))
            {
                Assert.IsTrue(bDependees.Remove(bDependee));
            }
            Assert.AreEqual(bDependees.Count, 0);


            // Test "c"
            HashSet<string> cDependents = new HashSet<string>();
            HashSet<string> cDependees = new HashSet<string>();
            cDependees.Add("a");
            foreach (string cDependent in g.GetDependents("c"))
            {
                Assert.IsTrue(cDependents.Remove(cDependent));
            }
            Assert.AreEqual(cDependents.Count, 0);
            foreach (string cDependee in g.GetDependees("c"))
            {
                Assert.IsTrue(cDependees.Remove(cDependee));
            }
            Assert.AreEqual(cDependees.Count, 0);


            // Test "d"
            HashSet<string> dDependents = new HashSet<string>();
            dDependents.Add("d");
            HashSet<string> dDependees = new HashSet<string>();
            dDependees.Add("d");
            dDependees.Add("b");
            foreach (string dDependent in g.GetDependents("d"))
            {
                Assert.IsTrue(dDependents.Remove(dDependent));
            }
            Assert.AreEqual(dDependents.Count, 0);
            foreach (string dDependee in g.GetDependees("d"))
            {
                Assert.IsTrue(dDependees.Remove(dDependee));
            }
            Assert.AreEqual(dDependees.Count, 0);

        }

        /// <summary>
        /// Tests the constructor that takes in a DG.
        /// </summary>
        [TestMethod]
        public void DependencyGraph()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("a4", "a9");
            g.AddDependency("a2", "a9");
            g.AddDependency("a3", "a9");

            DependencyGraph g1 = new DependencyGraph(g);

            Assert.IsTrue(g.HasDependees("a9"));
            Assert.IsTrue(g.HasDependents("a2"));
            Assert.IsTrue(g.HasDependents("a3"));
            Assert.IsTrue(g.HasDependents("a4"));


            Assert.IsTrue(g1.HasDependees("a9"));
            Assert.IsTrue(g1.HasDependents("a2"));
            Assert.IsTrue(g1.HasDependents("a3"));
            Assert.IsTrue(g1.HasDependents("a4"));

            g.RemoveDependency("a2", "a9");
            g.RemoveDependency("a3", "a9");
            g.RemoveDependency("a4", "a9");

            Assert.IsFalse(g.HasDependees("a9"));
            Assert.IsFalse(g.HasDependents("a2"));
            Assert.IsFalse(g.HasDependents("a3"));
            Assert.IsFalse(g.HasDependents("a4"));


            Assert.IsTrue(g1.HasDependees("a9"));
            Assert.IsTrue(g1.HasDependents("a2"));
            Assert.IsTrue(g1.HasDependents("a3"));
            Assert.IsTrue(g1.HasDependents("a4"));

        }

        /// <summary>
        /// Tests the constructor that takes in a DG.
        /// </summary>
        [TestMethod]
        public void DependencyGraph1()
        {
            DependencyGraph g = new DependencyGraph();
            g.AddDependency("a4", "a9");
            g.AddDependency("a3", "a9");
            g.AddDependency("a2", "a9");
            g.AddDependency("a1", "a9");

            DependencyGraph g1 = new DependencyGraph(g);

            g1.ReplaceDependees("a9", new List<string>());
            Assert.IsTrue(g.HasDependees("a9"));
            Assert.IsTrue(g.HasDependents("a4"));
            Assert.IsTrue(g.HasDependents("a1"));
            Assert.IsTrue(g.HasDependents("a3"));

            Assert.IsFalse(g1.HasDependees("a9"));
            Assert.IsFalse(g1.HasDependents("a4"));
            Assert.IsFalse(g1.HasDependents("a1"));
            Assert.IsFalse(g1.HasDependents("a3"));
        }

    }
}

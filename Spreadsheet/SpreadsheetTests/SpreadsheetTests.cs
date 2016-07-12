using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using System.Collections.Generic;
using Formulas;
using System.IO;
using System.Text.RegularExpressions;

namespace SpreadsheetTests
{
    /// <summary>
    /// Test class for Spreadsheet
    /// </summary>
    [TestClass]
    public class SpreadsheetTests
    {
        /// <summary>
        /// Tests an empty Spreadsheet
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCells()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            int count = 0;
            foreach (string s in sheet.GetNamesOfAllNonemptyCells())
            {
                count++;
            }
            Assert.AreEqual(count, 0);
        }

        /// <summary>
        /// Tests a basic sheet
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCells1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "one");
            sheet.SetContentsOfCell("a2", "two");
            sheet.SetContentsOfCell("a3", "three");
            List<string> nonEmpty = new List<string>();
           
            foreach(string s in sheet.GetNamesOfAllNonemptyCells())
            {
                nonEmpty.Add(s);
            }
            Assert.AreEqual(nonEmpty.Count, 3);
            Assert.IsTrue(nonEmpty.Contains("A1"));
            Assert.IsTrue(nonEmpty.Contains("A2"));
            Assert.IsTrue(nonEmpty.Contains("A3"));
        }

        /// <summary>
        /// Removing a cell
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCells2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "one");
            sheet.SetContentsOfCell("a2", "two");
            sheet.SetContentsOfCell("a3", "three");
            // Remove a2
            sheet.SetContentsOfCell("a2", "");
            List<string> nonEmpty = new List<string>();

            foreach (string s in sheet.GetNamesOfAllNonemptyCells())
            {
                nonEmpty.Add(s);
            }
            Assert.AreEqual(nonEmpty.Count, 2);
            Assert.IsTrue(nonEmpty.Contains("A1"));
            Assert.IsFalse(nonEmpty.Contains("A2"));
            Assert.IsTrue(nonEmpty.Contains("A3"));
        }

        /// <summary>
        /// Tests get cell contents
        /// </summary>
        [TestMethod]
        public void GetCellContents()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "one");
            sheet.SetContentsOfCell("a2", "3.3");

            sheet.SetContentsOfCell("a3", "=4 + 2");

            string a1 = (string) sheet.GetCellContents("a1");
            double a2 = (double) sheet.GetCellContents("a2");
            Formula a3 = (Formula) sheet.GetCellContents("a3");

            Assert.AreEqual(a1, "one");
            Assert.AreEqual(a2, 3.3);
            Assert.AreEqual(a3.ToString(), "4+2");

        }

        /// <summary>
        /// Passes null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellContents1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellContents(null);
        }

        /// <summary>
        /// Passes null into the name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsDouble()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(null, "1");
        }

        /// <summary>
        /// Invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsDouble1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("X0", "1");
        }

        /// <summary>
        /// Null string
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetCellContentsString()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", null);
        }

        /// <summary>
        /// Passes null into the name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsString1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(null, "1");
        }

        /// <summary>
        /// Invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsString2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("X0", "1");
        }

        /// <summary>
        /// Null formula
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void SetCellContentsFormula()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", null);
        }

        /// <summary>
        /// Passes null into the name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsFormula1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell(null, "1");
        }

        /// <summary>
        /// Invalid name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SetCellContentsFormula2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("X0", "1");
        }

        /// <summary>
        /// Circular exception, A1 depends on itself
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetCellContentsFormula3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("A1", "=B1*2");
            sheet.SetContentsOfCell("B1", "=C1*2");
            sheet.SetContentsOfCell("C1", "=A1*2");
            Assert.AreEqual("", sheet.GetCellContents("C1"));
        }

        /// <summary>
        /// Circular exception, A1 depends on itself
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetCellContentsFormula4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("C1", "11");
            sheet.SetContentsOfCell("A1", "=B1*2");
            sheet.SetContentsOfCell("B1", "=C1*2");
            sheet.SetContentsOfCell("C1", "=A1*2");
            Assert.AreEqual(33, sheet.GetCellContents("C1"));
        }

        /// <summary>
        /// Circular exception, A1 depends on itself
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void SetCellContentsFormula5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("C1", "=11*2");
            sheet.SetContentsOfCell("A1", "=B1*2");
            sheet.SetContentsOfCell("B1", "=C1*2");
            sheet.SetContentsOfCell("C1", "=A1*2");
            Assert.AreEqual("11*2", sheet.GetCellContents("C1").ToString());
        }

        /// <summary>
        /// Tests get cell contents with lower and uppercase cell names
        /// </summary>
        [TestMethod]
        public void GetCellContents2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("B1", "=A1*2");
            sheet.SetContentsOfCell("C1", "=B1+A1");
            sheet.SetContentsOfCell("D1", "=33");

            Assert.AreEqual(sheet.GetCellContents("b1").ToString(), "A1*2");
            Assert.AreEqual(sheet.GetCellContents("C1").ToString(), "B1+A1");
            Assert.AreEqual(sheet.GetCellContents("d1").ToString(), "33");
        }

        /// <summary>
        /// Tests Set and Get cell contents
        /// </summary>
        [TestMethod]
        public void GetCellContents3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("e1", "CS 1410");
            sheet.SetContentsOfCell("C1", "CS 2420");
            sheet.SetContentsOfCell("D12", "CS 3500");

            Assert.AreEqual(sheet.GetCellContents("E1"), "CS 1410");
            Assert.AreEqual(sheet.GetCellContents("c1"), "CS 2420");
            Assert.AreEqual(sheet.GetCellContents("d12"), "CS 3500");

            HashSet<string> e1 = (HashSet<string>) sheet.SetContentsOfCell("e1", "CS");
            Assert.AreEqual(e1.Count, 1);
            Assert.IsTrue(e1.Contains("E1"));
        }

        /// <summary>
        /// Ensure that an empty cell returns ""
        /// </summary>
        [TestMethod]
        public void GetCellContents6()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("e1", "CS 1410");
            sheet.SetContentsOfCell("C1", "CS 2420");
            sheet.SetContentsOfCell("D12", "CS 3500");

            Assert.AreEqual(sheet.GetCellContents("E1"), "CS 1410");
            Assert.AreEqual(sheet.GetCellContents("c1"), "CS 2420");
            Assert.AreEqual(sheet.GetCellContents("d12"), "CS 3500");
            Assert.AreEqual(sheet.GetCellContents("A33"), "");
        }

        /// <summary>
        /// Simple get value test
        /// </summary>
        [TestMethod]
        public void GetCellValue()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "11");
            sheet.SetContentsOfCell("a3", "2");
            sheet.SetContentsOfCell("a2", "=a1 * a3");

            Assert.AreEqual((double) sheet.GetCellValue("a2"), 22);
            sheet.SetContentsOfCell("a3", "3");
            Assert.AreEqual((double)sheet.GetCellValue("a2"), 33);
        }

        /// <summary>
        /// Passes null into GetCellValue
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellValue1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellValue(null);
        }

        /// <summary>
        /// Passes an invalid cell name.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void GetCellValue2()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.GetCellValue("000");
        }

        /// <summary>
        /// Test GetCellValue for doubles
        /// </summary>
        [TestMethod]
        public void GetCellValue3()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "11");
            sheet.SetContentsOfCell("a3", "2");
            sheet.SetContentsOfCell("a2", "=a1 * a3");

            Assert.AreEqual(11.0, sheet.GetCellValue("a1"));
            Assert.AreEqual(2.0, sheet.GetCellValue("a3"));
        }

        /// <summary>
        /// Test GetCellValue for strings
        /// </summary>
        [TestMethod]
        public void GetCellValue4()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "yo");
            sheet.SetContentsOfCell("a3", "hey");
            sheet.SetContentsOfCell("a2", "=a1 * a3");

            Assert.AreEqual("yo", sheet.GetCellValue("a1"));
            Assert.AreEqual("hey", sheet.GetCellValue("a3"));
            Assert.IsTrue(sheet.GetCellValue("a2") is FormulaError);
        }

        /// <summary>
        /// Test FormulaError.Reason for dividing by 0.
        /// </summary>
        [TestMethod]
        public void GetCellValue5()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "12");
            sheet.SetContentsOfCell("a3", "0");
            sheet.SetContentsOfCell("a2", "=a1 / a3");

            Assert.AreEqual(12.0, sheet.GetCellValue("a1"));
            Assert.AreEqual(0.0, sheet.GetCellValue("a3"));
            Assert.IsTrue(sheet.GetCellValue("a2") is FormulaError);
            FormulaError error = (FormulaError)sheet.GetCellValue("a2");

            Assert.AreEqual("Division by 0 is not allowed.", error.Reason);
        }

        /// <summary>
        /// Simple save test.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(IOException))]
        public void Save()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "1.5");
            sheet.SetContentsOfCell("a2", "8.0");
            sheet.SetContentsOfCell("a3", "=a1*a2+23");
            sheet.SetContentsOfCell("a4", "Hello");
            sheet.Save(null);
        }

        /// <summary>
        /// Save method.
        /// </summary>
        [TestMethod]
        public void Save1()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            sheet.SetContentsOfCell("a1", "1.5");
            sheet.SetContentsOfCell("a2", "8.0");
            sheet.SetContentsOfCell("a3", "=a1*a2+23");
            sheet.SetContentsOfCell("a4", "Hello");
            sheet.Save(File.CreateText("spreadsheet1.xml"));
        }

        /// <summary>
        /// Simple method to test Changed
        /// </summary>
        [TestMethod]
        public void Changed()
        {
            AbstractSpreadsheet sheet = new Spreadsheet();
            Assert.IsFalse(sheet.Changed);
            sheet.SetContentsOfCell("a1", "1.5");
            Assert.IsTrue(sheet.Changed);
        }

        /// <summary>
        /// Tests regex
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void ConstructRegex()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(new Regex("[a-zA-Z]*"));
            sheet.SetContentsOfCell("aa", "33");
            Assert.AreEqual(sheet.GetCellContents("aa"), 33);
        }

        /// <summary>
        /// Test create spreadsheet from .xml file.
        /// </summary>
        [TestMethod]
        public void Open()
        {
            AbstractSpreadsheet sheet = new Spreadsheet(File.OpenText("spreadsheet2.xml"));

            Assert.IsFalse(sheet.Changed);
            Assert.AreEqual(sheet.GetCellContents("a1"), 1.5);
            Assert.AreEqual(sheet.GetCellContents("a2"), 8.0);
            Assert.AreEqual(sheet.GetCellContents("a3").ToString(), "A1*A2+23");
            Assert.AreEqual(sheet.GetCellValue("a3"), 35.0);
            Assert.AreEqual(sheet.GetCellContents("a4"), "Hello");
        }

    }
}

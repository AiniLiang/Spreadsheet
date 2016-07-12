using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetGUI;
using SS;

namespace ControllerTester
{
    /// <summary>
    /// Class that uses a stub of a ISpreadsheetView to test the logic of a Controller
    /// </summary>
    [TestClass]
    public class ControllerTester
    {
        /// <summary>
        /// General purpose test.
        /// </summary>
        [TestMethod]
        public void Test1()
        {
            SpreadsheetViewStub stub = new SpreadsheetViewStub();
            Controller controller = new Controller(stub);

            stub.FireCellContentsChange("A1", "CS 1410");
            stub.FireCellContentsChange("A2", "CS 2420");
            stub.FireCellContentsChange("A3", "CS 3500");
            stub.FireCellContentsChange("A4", "CS 3810");

            stub.FireCellSelected(0, 0);
            Assert.AreEqual("A1", stub.CurrentCellName);
            Assert.AreEqual("CS 1410", stub.CurrentCellContents);
            Assert.AreEqual("CS 1410", stub.CurrentCellValue);

            stub.FireCellSelected(0, 1);
            Assert.AreEqual("A2", stub.CurrentCellName);
            Assert.AreEqual("CS 2420", stub.CurrentCellContents);
            Assert.AreEqual("CS 2420", stub.CurrentCellValue);

            stub.FireCellSelected(0, 2);
            Assert.AreEqual("A3", stub.CurrentCellName);
            Assert.AreEqual("CS 3500", stub.CurrentCellContents);
            Assert.AreEqual("CS 3500", stub.CurrentCellValue);

            stub.FireCellSelected(0, 3);
            Assert.AreEqual("A4", stub.CurrentCellName);
            Assert.AreEqual("CS 3810", stub.CurrentCellContents);
            Assert.AreEqual("CS 3810", stub.CurrentCellValue);

            stub.FireCloseEvent();
            Assert.IsTrue(stub.CalledDoClose);
            Assert.IsTrue(stub.CalledAskSave);
            Assert.IsFalse(stub.CalledOpenNew);
        }

        /// <summary>
        /// General purpose test
        /// </summary>
        [TestMethod]
        public void Test2()
        {
            SpreadsheetViewStub stub = new SpreadsheetViewStub();
            Controller controller = new Controller(stub);

            stub.FireCellContentsChange("A1", "11");
            stub.FireCellContentsChange("A2", "2");
            stub.FireCellContentsChange("A3", "=A1 * A2");
            stub.FireCellContentsChange("A4", "22");

            stub.FireCellSelected(0, 0);
            Assert.AreEqual("A1", stub.CurrentCellName);
            Assert.AreEqual("11", stub.CurrentCellContents.ToString());
            Assert.AreEqual(11.0, stub.CurrentCellValue);

            stub.FireCellSelected(0, 1);
            Assert.AreEqual("A2", stub.CurrentCellName);
            Assert.AreEqual("2", stub.CurrentCellContents.ToString());
            Assert.AreEqual(2.0, stub.CurrentCellValue);

            stub.FireCellSelected(0, 2);
            Assert.AreEqual("A3", stub.CurrentCellName);
            Assert.AreEqual("A1*A2", stub.CurrentCellContents.ToString());
            Assert.AreEqual(22.0, stub.CurrentCellValue);

            stub.FireCellSelected(0, 3);
            Assert.AreEqual("A4", stub.CurrentCellName);
            Assert.AreEqual("22", stub.CurrentCellContents.ToString());
            Assert.AreEqual(22.0, stub.CurrentCellValue);

            stub.FireCloseEvent();
            Assert.IsTrue(stub.CalledDoClose);
            Assert.IsTrue(stub.CalledAskSave);
            Assert.IsFalse(stub.CalledOpenNew);
        }

        /// <summary>
        /// Enters an invalid formula and checks to ensure we have notified the user.
        /// </summary>
        [TestMethod]
        public void Test3()
        {
            SpreadsheetViewStub stub = new SpreadsheetViewStub();
            Controller controller = new Controller(stub);

            stub.FireCellContentsChange("A1", "=2*/22");
            Assert.IsTrue(stub.DidSendMessage);
        }

        /// <summary>
        /// Tests the load method.
        /// </summary>
        [TestMethod]
        public void Test4()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("A1", "CS 1410");
            spreadsheet.SetContentsOfCell("A2", "CS 2420");
            spreadsheet.SetContentsOfCell("A3", "CS 3500");
            spreadsheet.SetContentsOfCell("A4", "CS 3810");

            SpreadsheetViewStub stub = new SpreadsheetViewStub();
            Controller controller = new Controller(stub, spreadsheet);

            stub.FireCellSelected(0, 0);
            Assert.AreEqual("A1", stub.CurrentCellName);
            Assert.AreEqual("CS 1410", stub.CurrentCellContents);
            Assert.AreEqual("CS 1410", stub.CurrentCellValue);

            stub.FireCellSelected(0, 1);
            Assert.AreEqual("A2", stub.CurrentCellName);
            Assert.AreEqual("CS 2420", stub.CurrentCellContents);
            Assert.AreEqual("CS 2420", stub.CurrentCellValue);

            stub.FireCellSelected(0, 2);
            Assert.AreEqual("A3", stub.CurrentCellName);
            Assert.AreEqual("CS 3500", stub.CurrentCellContents);
            Assert.AreEqual("CS 3500", stub.CurrentCellValue);

            stub.FireCellSelected(0, 3);
            Assert.AreEqual("A4", stub.CurrentCellName);
            Assert.AreEqual("CS 3810", stub.CurrentCellContents);
            Assert.AreEqual("CS 3810", stub.CurrentCellValue);

            stub.FireCloseEvent();
            Assert.IsTrue(stub.CalledDoClose);
            Assert.IsTrue(stub.CalledAskSave);
            Assert.IsFalse(stub.CalledOpenNew);
        }

        /// <summary>
        /// Ensures that the save was successful
        /// </summary>
        [TestMethod]
        public void Test5()
        {
            SpreadsheetViewStub stub = new SpreadsheetViewStub();
            Controller controller = new Controller(stub);

            stub.FireCellContentsChange("A1", "CS 1410");
            stub.FireCellContentsChange("A2", "CS 2420");
            stub.FireCellContentsChange("A3", "CS 3500");
            stub.FireCellContentsChange("A4", "CS 3810");

            stub.FireSaveEvent("savedSpreadsheet1.xml");
            Assert.IsFalse(stub.DidSendMessage);
        }

        /// <summary>
        /// Ensures that the load was successful
        /// </summary>
        [TestMethod]
        public void Test6()
        {
            SpreadsheetViewStub stub = new SpreadsheetViewStub();
            Controller controller = new Controller(stub);
            stub.FireFileChosenEvent("savedSpreadsheet.xml");
            Assert.IsFalse(stub.DidSendMessage);
        }

        /// <summary>
        /// Ensures that the new event was fired
        /// </summary>
        [TestMethod]
        public void Test7()
        {
            SpreadsheetViewStub stub = new SpreadsheetViewStub();
            Controller controller = new Controller(stub);
            stub.FireNewEvent();
            Assert.IsTrue(stub.DidFireNewEvent);
        }

    }
}

using Formulas;
using SS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Controls the model and the view. Seperates them for easier testing.
    /// </summary>
    public class Controller
    {
        /// <summary>
        /// The window being controlled
        /// </summary>
        private ISpreadsheetView window;

        /// <summary>
        /// The model
        /// </summary>
        private Spreadsheet spreadsheet;

        /// <summary>
        /// Creates a new controller with a new spreadsheet.
        /// </summary>
        /// <param name="window">The view of the MVC</param>
        public Controller(ISpreadsheetView window)
        {
            this.window = window;
            spreadsheet = new Spreadsheet(new Regex("^[A-Za-z][1-9][0-9]?$"));
            window.CellContentsChanged += HandleCellContentsChanged;
            window.CellSelected += HandleCellSelected;
            window.FileChosenEvent += HandleOpen;
            window.SaveEvent += HandleSave;
            window.CloseEvent += HandleClose;
            window.NewEvent += HandleNew;
            HandleCellSelected(0, 0);
        }

        /// <summary>
        /// Creates a controller with a saved spreadsheet
        /// </summary>
        /// <param name="window">The view of the MVC</param>
        /// <param name="spreadsheet">The saved spreadsheet</param>
        public Controller(ISpreadsheetView window, Spreadsheet spreadsheet): this(window)
        {
            this.spreadsheet = spreadsheet;
            foreach (string cellName in spreadsheet.GetNamesOfAllNonemptyCells())
            {
                HandleCellContentsChanged(cellName, spreadsheet.GetCellContents(cellName).ToString());
            }
        }

        /// <summary>
        /// The method which handles when a spreadsheet is edited in the view.
        /// </summary>
        /// <param name="name">The name of the cell</param>
        /// <param name="contents">The new contents of the cell</param>
        private void HandleCellContentsChanged(string name, string contents)
        {
            HashSet<string> cellsToUpdate = new HashSet<string>();

            try
            {
                cellsToUpdate = (HashSet<string>)spreadsheet.SetContentsOfCell(name, contents);
            } catch(Exception e)
            {
                window.Message = e.Message;
                return;
            }
            window.CurrentCellName = name;
            window.CurrentCellContents = spreadsheet.GetCellContents(name);
            object value = spreadsheet.GetCellValue(name);
            window.CurrentCellValue = value;

            foreach (string cellName in cellsToUpdate)
            {
                int col, row;
                reverseCellName(cellName, out col, out row);
                value = spreadsheet.GetCellValue(cellName);
                window.SetCellValueLabel(col, row, value);
            }
        }

        /// <summary>
        /// Takes a cell name A1 and maps it to a col and row
        /// </summary>
        /// <param name="cellName">The cell name</param>
        /// <param name="col">Col  of the cell name</param>
        /// <param name="row">Row of the cell name</param>
        private void reverseCellName(string cellName, out int col, out int row)
        {
            col = cellName[0];
            col -= 65;
            row = Int32.Parse(cellName.Substring(1)) - 1;
        }

        /// <summary>
        /// Handles when the view selects a new cell
        /// </summary>
        /// <param name="col">Col of the selected cell</param>
        /// <param name="row">Row of the selected cell</param>
        private void HandleCellSelected(int col, int row)
        {
            string cellName = calculateCellName(col, row);
            window.CurrentCellName = cellName;
            window.CurrentCellContents = spreadsheet.GetCellContents(cellName);
            window.CurrentCellValue = spreadsheet.GetCellValue(cellName);
        }

        /// <summary>
        /// Takes a cell col and row and turns it into a valid cell name
        /// </summary>
        /// <param name="col">The col of the cell</param>
        /// <param name="row">The row of the cell</param>
        /// <returns>A formatted cell name</returns>
        private string calculateCellName(int col, int row)
        {
            row++;
            col += 65;
            string cellName = "";
            cellName = cellName + Convert.ToChar(col);
            return (cellName += row).ToUpper();
        }

        /// <summary>
        /// Opens the spreadsheet at source in a new window.
        /// </summary>
        /// <param name="source">Path of the spreadsheet</param>
        private void HandleOpen(string source)
        {
            try
            {
                Spreadsheet newSpreadsheet = new Spreadsheet(File.OpenText(source));
                SpreadsheetApplicationContext.GetContext().RunNewWithSpreadsheet(newSpreadsheet);
            }catch(IOException)
            {
                window.Message = "Error opening file";
            }
        }

        /// <summary>
        /// Saves the spreadsheet
        /// </summary>
        /// <param name="dest">The destination of where the spreadsheet should be saved</param>
        private void HandleSave(string dest)
        {
            try
            {
                spreadsheet.Save(File.CreateText(dest));
            }catch(IOException)
            {
                window.Message = "Error saving file.";
            }
        }

        /// <summary>
        /// Closes this spreadsheet, and asks to save before it closes.
        /// </summary>
        private void HandleClose()
        {
            //Ask if they want to save if changed.
            if (spreadsheet.Changed)
            {
                window.AskSave();
            }
            window.DoClose();
        }

        /// <summary>
        /// Opens a new spreadsheet
        /// </summary>
        private void HandleNew()
        {
            window.OpenNew();
        }

    }
}


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Controllable interface of SpreadsheetWindow
    /// </summary>
    public interface ISpreadsheetView
    {
        /// <summary>
        /// Fire when a new spreadsheet is selected to open.
        /// </summary>
        event Action<string> FileChosenEvent;
        /// <summary>
        /// Fired when the spreadsheet should be saved.
        /// </summary>
        event Action<string> SaveEvent;
        /// <summary>
        /// Fired when the window should close.
        /// </summary>
        event Action CloseEvent;
        /// <summary>
        /// Fired when a new spreadsheet should be opened.
        /// </summary>
        event Action NewEvent;
        /// <summary>
        /// Col, row
        /// </summary>
        event Action<int, int> CellSelected;
        /// <summary>
        /// Cell name, contents
        /// </summary>
        event Action<string, string> CellContentsChanged;
        /// <summary>
        /// Property to store the cell name of the selected cell
        /// </summary>
        string CurrentCellName { set; }
        /// <summary>
        /// Property to store the cell contents of the selected cell
        /// </summary>
        object CurrentCellContents { set; }
        /// <summary>
        /// Property to store the cell value of the selected cell
        /// </summary>
        object CurrentCellValue { set; }
        /// <summary>
        /// Used to present dialogs to the user
        /// </summary>
        string Message { set; }
        /// <summary>
        /// Handles closing a spreadsheet.
        /// </summary>
        void DoClose();
        /// <summary>
        /// Handles opening a new spreadsheet.
        /// </summary>
        void OpenNew();
        /// <summary>
        /// Sets the cell text in the spreadsheet
        /// </summary>
        void SetCellValueLabel(int col, int row, object value);
        /// <summary>
        /// Prompts the user to save the current spreadsheet if it has been modified.
        /// </summary>
        void AskSave();
    }
}

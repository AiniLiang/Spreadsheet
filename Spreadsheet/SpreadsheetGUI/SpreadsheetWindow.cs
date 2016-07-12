using SSGui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Manages a window of a spreadsheet application.
    /// </summary>
    public partial class SpreadsheetWindow : Form, ISpreadsheetView
    {
        /// <summary>
        /// Keeps track if the form is closing
        /// </summary>
        private bool formClosing;

        public event Action<string> FileChosenEvent;
        public event Action<string> SaveEvent;
        public event Action CloseEvent;
        public event Action NewEvent;
        public event Action<int, int> CellSelected;
        public event Action<string, string> CellContentsChanged;

        /// <summary>
        /// Constructs a new spreadsheet window.
        /// </summary>
        public SpreadsheetWindow()
        {
            InitializeComponent();
            this.ActiveControl = contentsTextBox;
        }

        /// <summary>
        /// Called when the enter key is pressed in the contents text box. Updates the contents to the new contents.
        /// </summary>
        private void contentsKeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (CellContentsChanged != null)
                {
                    // Fire the contents changedhandler
                    CellContentsChanged(cellNameTextBox.Text, contentsTextBox.Text);
                }
            }
        }

        /// <summary>
        /// Sets the specfied cell in the spreadsheet to contain the string of value.ToString()
        /// </summary>
        public void SetCellValueLabel(int col, int row, object value)
        {
            spreadsheetPanel1.SetValue(col, row, value.ToString());
        }

        /// <summary>
        /// Called when the panel selected a new cell
        /// </summary>
        /// <param name="panel"></param>
        public void SpreadsheetDidSelect(SpreadsheetPanel panel)
        {
            int column, row;
            spreadsheetPanel1.GetSelection(out column, out row);
            if(CellSelected != null)
            {
                CellSelected(column, row);
            }
        }

        /// <summary>
        /// Sets the current cell name in the UI.
        /// </summary>
        public string CurrentCellName
        {
            set { cellNameTextBox.Text = value.ToString(); }
        }

        /// <summary>
        /// Sets the content label in the UI.
        /// </summary>
        public object CurrentCellContents
        {
            set { contentsTextBox.Text = value.ToString(); }
        }

        /// <summary>
        /// Sets the value label in the UI.
        /// </summary>
        public object CurrentCellValue
        {
            set { valueTextBox.Text = value.ToString(); }
        }

        /// <summary>
        /// Shows the message in the UI.
        /// </summary>
        public string Message
        {
            set { MessageBox.Show(value); }
        }

        /// <summary>
        /// Closes this window
        /// </summary>
        public void DoClose()
        {
            Close();
        }

        /// <summary>
        /// Opens a new blank spreadsheet
        /// </summary>
        public void OpenNew()
        {
            SpreadsheetApplicationContext.GetContext().RunNew();
        }

        /// <summary>
        /// Handles the open click.
        /// </summary>
        private void OpenItem_Click(object sender, EventArgs e)
        {
            DialogResult result = fileDialog.ShowDialog();
            if(result == DialogResult.Yes || result ==DialogResult.OK)
            {
                if(FileChosenEvent!= null)
                {
                    FileChosenEvent(fileDialog.FileName);
                }
            }
        }

        /// <summary>
        /// Handles the click event of the close item control.
        /// </summary>
        private void CloseItem_Click(object sender, EventArgs e)
        {
            if(CloseEvent!= null)
            {
                CloseEvent();
            }
        }

        /// <summary>
        /// Presents the dialog box to save the spreadsheet
        /// </summary>
        private void SaveItem_Click(object sender, EventArgs e)
        {
            DialogResult result = saveDialog.ShowDialog();
            if (result == DialogResult.Yes || result == DialogResult.OK)
            {
                if (SaveEvent != null)
                {
                    SaveEvent(saveDialog.FileName);
                }
            }
        }

        /// <summary>
        /// Presents a dialog box to inform how to use the spreadsheet
        /// </summary>
        private void HelpItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("To edit a cell, simply enter the contents and then hit enter. The value of the selected cell is shown in the spreadsheet and in the value text box" +
               System.Environment.NewLine + System.Environment.NewLine + "You are also able to work on several spreadsheets at a time by using going to File->New. In the file menu you can also save your work or open a spreadsheet that has been previously saved. This application supports reading spreadsheet files with the extension of .ss", "Spreadsheet Help");
        }

        /// <summary>
        /// Fires the NewEvent() event
        /// </summary>
        private void NewItem_Click(object sender, EventArgs e)
        {
            if(NewEvent != null)
            {
                NewEvent();
            }
        }

        /// <summary>
        /// Prompts the user to save the current spreadsheet
        /// </summary>
        public void AskSave()
        {
            if (MessageBox.Show("Save your changes before exit?", "Save changes?", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                SaveItem_Click(null, null);
            }
            DoClose();
        }

        private void SpreadsheetWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (CloseEvent != null && !formClosing)
            {
                formClosing = true;
                CloseEvent();
            }
        }
    }
}




using SpreadsheetGUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ControllerTester
{
    class SpreadsheetViewStub : ISpreadsheetView
    {
        // Flags checking if methods were called
        public bool CalledDoClose { get; private set; }
        public bool CalledOpenNew { get; private set; }
        public bool CalledAskSave { get; private set; }
        public bool DidSendMessage { get; private set; }
        public bool DidFireNewEvent { get; private set; }

        // These 6 methods cause events to be fired.
        public void FireCellContentsChange(string cellName, string contents)
        {
            if (CellContentsChanged != null)
            {
                CellContentsChanged(cellName, contents);
            }
        }

        public void FireCellSelected(int col, int row)
        {
            if (CellSelected != null)
            {
                CellSelected(col, row);
            }
        }

        public void FireCloseEvent()
        {
            if(CloseEvent != null)
            {
                CloseEvent();
            }
        }

        public void FireFileChosenEvent(string path)
        {
            if (FileChosenEvent != null)
            {
                FileChosenEvent(path); 
            }
        }

        public void FireNewEvent()
        {
            if (NewEvent != null)
            {
                DidFireNewEvent = true;
                NewEvent();
            }
        }

        public void FireSaveEvent(string dest)
        {
            if (SaveEvent != null)
            {
                SaveEvent(dest);
            }
        }

        // Interface
        public object CurrentCellContents
        {
            set; get;
        }

        public string CurrentCellName
        {
            set; get;
        }

        public object CurrentCellValue
        {
            set; get;
        }

        public string Message
        {
            set
            {
                DidSendMessage = true;
            }
            
        }

        public event Action<string, string> CellContentsChanged;
        public event Action<int, int> CellSelected;
        public event Action CloseEvent;
        public event Action<string> FileChosenEvent;
        public event Action NewEvent;
        public event Action<string> SaveEvent;

        public void AskSave()
        {
            CalledAskSave = true;
        }

        public void DoClose()
        {
            CalledDoClose = true;
        }

        public void OpenNew()
        {
            CalledOpenNew = true;
        }

        public void SetCellValueLabel(int col, int row, object value)
        {
            // Would handle the panel.
        }
    }
}

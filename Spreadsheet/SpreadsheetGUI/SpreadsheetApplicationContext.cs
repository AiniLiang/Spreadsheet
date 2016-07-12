using SS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpreadsheetGUI
{
    /// <summary>
    /// Keeps track of how many top-level are running, shuts down the application when there are no more.
    /// </summary>
    class SpreadsheetApplicationContext : ApplicationContext
    {
        // Number of open forms
        private int windowCount = 0;

        // Singleton ApplicationContext
        private static SpreadsheetApplicationContext context;

        /// <summary>
        /// Private constructor for singleton
        /// </summary>
        private SpreadsheetApplicationContext()
        {

        }

        /// <summary>
        /// Returns the context
        /// </summary>
        public static SpreadsheetApplicationContext GetContext()
        {
            if(context == null)
            {
                context = new SpreadsheetApplicationContext();
            }
            return context;
        }

        /// <summary>
        /// Runs a new form in this context
        /// </summary>
        public void RunNew()
        {
            SpreadsheetWindow window = new SpreadsheetWindow();
            new Controller(window);
            windowCount++;

            window.FormClosed += (o, e) => { if (--windowCount <= 0) ExitThread(); };
            window.Show();
        }

        /// <summary>
        /// Runs a new form in this context
        /// </summary>
        public void RunNewWithSpreadsheet(Spreadsheet spreadsheet)
        {
            SpreadsheetWindow window = new SpreadsheetWindow();
            new Controller(window, spreadsheet);
            windowCount++;

            window.FormClosed += (o, e) => { if (--windowCount <= 0) ExitThread(); };
            window.Show();
        }

    }
}

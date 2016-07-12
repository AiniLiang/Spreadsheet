using Formulas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SS
{

    class Cell
    {
        /// <summary>
        /// An enum that represnts which represents which kind of cell this is.
        /// </summary>
        private enum CellType { String, Double, Formula };

        /// <summary>
        /// Represents the current type of this.
        /// </summary>
        private CellType cellType;

        /// <summary>
        /// Represenets the name of the cell.
        /// </summary>
        public object Name { get; private set; }

        /// <summary>
        /// Holds either a string, double or a Formula for the contents of the cell.
        /// </summary>
        public object Contents { get; private set; }

        /// <summary>
        /// Holds the value of the cell.
        /// </summary>
        public object Value { get; private set; }

        /// <summary>
        /// Constructs a cell of type string.
        /// </summary>
        /// <param name="name">Name of the cell</param>
        /// <param name="contents">Contents of the cell</param>
        public Cell(string name, string contents)
        {
            Name = name;
            Contents = contents;
            cellType = CellType.String;
        }

        /// <summary>
        /// Constructs a cell of type double.
        /// </summary>
        /// <param name="name">Name of the cell</param>
        /// <param name="contents">Contents of the cell</param>
        public Cell(string name, double contents)
        {
            Name = name;
            Contents = contents;
            cellType = CellType.Double;
        }

        /// <summary>
        /// Constructs a cell of type Formula.
        /// </summary>
        /// <param name="name">Name of the cell</param>
        /// <param name="contents">Contents of the cell</param>
        public Cell(string name, Formula contents)
        {
            Name = name;
            Contents = contents;
            cellType = CellType.Formula;
        }

        /// <summary>
        /// Sets the contents of this to a string.
        /// </summary>
        /// <param name="contents">The string contents</param>
        public void SetContents(String contents)
        {
            Contents = contents;
            cellType = CellType.String;
        }

        /// <summary>
        /// Sets the contents of this to a double.
        /// </summary>
        /// <param name="contents">The double contents</param>
        public void SetContents(double contents)
        {
            Contents = contents;
            cellType = CellType.Double;
        }

        /// <summary>
        /// Sets the contents of this to a Formula.
        /// </summary>
        /// <param name="contents">The Formula contents</param>
        public void SetContents(Formula contents)
        {
            Contents = contents;
            cellType = CellType.Formula;
        }

        /// <summary>
        /// Returns the value of the cell.
        /// </summary>
        public void CalculateValue(Lookup lookup)
        {
            if(cellType == CellType.String || cellType == CellType.Double)
            {
                Value = Contents;
                return;
            }

            // Handle Formula.
            Formula f = (Formula)Contents;
            try
            {
                Value = f.Evaluate(lookup);
            } catch(Exception e)
            {
                Value = new FormulaError(e.Message);
            }


        }

        /// <summary>
        /// Returns true if the cell is empty.
        /// </summary>
        /// <returns>True if the cell is empty, false otherwise.</returns>
        public bool IsEmpty()
        {
            return cellType == CellType.String && (string)Value == "";
        }

        /// <summary>
        /// Returns how the contents are represented in XML.
        /// </summary>
        /// <returns>An XML representation of the contents.</returns>
        public string ContentsToXml()
        {
            if(cellType == CellType.String)
            {
                return (string) Contents;
            }
            if (cellType == CellType.Double)
            {
                return Contents.ToString();
            }

            // Is a formula
            return "=" + Contents.ToString();

        }

    }
}
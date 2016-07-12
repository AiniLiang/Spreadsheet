using System;
using System.Collections.Generic;
using Formulas;
using System.Text.RegularExpressions;
using Dependencies;
using System.IO;
using System.Xml;
using System.Xml.Schema;

namespace SS
{
    /// <summary>
    /// Spreadsheet implements the abstract methods define in AbstractSpreadsheet.
    /// 
    /// An AbstractSpreadsheet object represents the state of a simple spreadsheet.  A 
    /// spreadsheet consists of an infinite number of named cells.
    /// 
    /// A string is a cell name if and only if it consists of one or more letters, 
    /// followed by a non-zero digit, followed by zero or more digits.  Cell names
    /// are not case sensitive.
    /// 
    /// For example, "A15", "a15", "XY32", and "BC7" are cell names.  (Note that 
    /// "A15" and "a15" name the same cell.)  On the other hand, "Z", "X07", and 
    /// "hello" are not cell names."
    /// 
    /// A spreadsheet contains a cell corresponding to every possible cell name.  
    /// In addition to a name, each cell has a contents and a value.  The distinction is
    /// important, and it is important that you understand the distinction and use
    /// the right term when writing code, writing comments, and asking questions.
    /// 
    /// The contents of a cell can be (1) a string, (2) a double, or (3) a Formula.  If the
    /// contents is an empty string, we say that the cell is empty.  (By analogy, the contents
    /// of a cell in Excel is what is displayed on the editing line when the cell is selected.)
    /// 
    /// In an empty spreadsheet, the contents of every cell is the empty string.
    ///  
    /// The value of a cell can be (1) a string, (2) a double, or (3) a FormulaError.  
    /// (By analogy, the value of an Excel cell is what is displayed in that cell's position
    /// in the grid.)
    /// 
    /// If a cell's contents is a string, its value is that string.
    /// 
    /// If a cell's contents is a double, its value is that double.
    /// 
    /// If a cell's contents is a Formula, its value is either a double or a FormulaError.
    /// The value of a Formula, of course, can depend on the values of variables.  The value 
    /// of a Formula variable is the value of the spreadsheet cell it names (if that cell's 
    /// value is a double) or is undefined (otherwise).  If a Formula depends on an undefined
    /// variable or on a division by zero, its value is a FormulaError.  Otherwise, its value
    /// is a double, as specified in Formula.Evaluate.
    /// 
    /// Spreadsheets are never allowed to contain a combination of Formulas that establish
    /// a circular dependency.  A circular dependency exists when a cell depends on itself.
    /// For example, suppose that A1 contains B1*2, B1 contains C1*2, and C1 contains A1*2.
    /// A1 depends on B1, which depends on C1, which depends on A1.  That's a circular
    /// dependency.
    /// </summary>
    public class Spreadsheet : AbstractSpreadsheet
    {
        /// <summary>
        /// Maps a cell name to a cell. 
        /// The key is the name of the cell.
        /// The cell is the corresponding cell.
        /// All keys and cell names are converted to uppercase.
        /// </summary>
        private Dictionary<string, Cell> cells = new Dictionary<string, Cell>();

        /// <summary>
        /// Dependency graph to keep track of how each cell depends on each other.
        /// All dependencies use the uppercase version of the cell name.
        /// </summary>
        private DependencyGraph graph = new DependencyGraph();

        /// <summary>
        /// Regex pattern to validate cell names.
        /// </summary>
        private Regex isValid;

        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed
        {
            get;

            protected set;
        }

        /// <summary>
        /// Creates a spreadsheet with a regex that will accept any string.
        /// </summary>
        public Spreadsheet()
        {
            isValid = new Regex("^.*$");
        }

        /// <summary>
        /// Creates an empty Spreadsheet whose IsValid regular expression is provided as the parameter
        /// </summary>
        /// <param name="isValid">Regex pattern to be used to validate cell names.</param>
        public Spreadsheet(Regex isValid)
        {
            this.isValid = isValid;
        }

        /// Creates a Spreadsheet that is a duplicate of the spreadsheet saved in source.
        /// See the AbstractSpreadsheet.Save method and Spreadsheet.xsd for the file format 
        /// specification.  If there's a problem reading source, throws an IOException
        /// If the contents of source are not consistent with the schema in Spreadsheet.xsd, 
        /// throws a SpreadsheetReadException.  If there is an invalid cell name, or a 
        /// duplicate cell name, or an invalid formula in the source, throws a SpreadsheetReadException.
        /// If there's a Formula that causes a circular dependency, throws a SpreadsheetReadException. 
        public Spreadsheet(TextReader source)
        {
            XmlSchemaSet sc = new XmlSchemaSet();
            sc.Add(null, "Spreadsheet.xsd");

            // Configure validation.
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas = sc;
            settings.ValidationEventHandler += ValidationCallback;

            try
            {
                using (XmlReader reader = XmlReader.Create(source, settings))
                {
                    while (reader.Read())
                    {
                        if (reader.IsStartElement())
                        {

                            if(reader.Name == "spreadsheet")
                            {
                                this.isValid = new Regex(reader["IsValid"]);
                                continue;
                            }

                            if (reader.Name == "cell")
                            {
                                string cellName = reader["name"].ToUpper();

                                // Check if cell already exists.
                                if (GetCellContents(cellName).ToString() != "")
                                {
                                    throw new SpreadsheetReadException("Duplicate cells");
                                }
                                try
                                {
                                    SetContentsOfCell(cellName, reader["contents"]);
                                }
                                catch (Exception e)
                                {
                                    throw new SpreadsheetReadException(e.Message);
                                }
                            }
                        }
                    }
                }
            } catch(Exception e)
            {
                throw new IOException();
            }
            Changed = false;
        }

        /// <summary>
        /// Handles the XmlReader validation event
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">Arguments of the event</param>
        private void ValidationCallback(object sender, ValidationEventArgs e)
        {
            throw new SpreadsheetReadException(String.Format(" *** Validation Error: {0}", e.Message));
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        public override object GetCellContents(string name)
        {
            if (name == null || !cellNameIsValid(name))
            {
                throw new InvalidNameException();
            }

            Cell cell;
            // All cell names are converted to uppercase internally.
            if (cells.TryGetValue(name.ToUpper(), out cell))
            {
                return cell.Contents;
            }
            // Return an empty string to indicate this cell is empty
            return "";
        }

        /// <summary>
        /// Helper method to ensure that the cell name is valid.
        /// </summary>
        /// <param name="name">Name of the cell to be checked</param>
        /// <returns>True if the cell name is invalid, false otherwise</returns>
        private bool cellNameIsValid(string name)
        {
            return Regex.IsMatch(name, "^[A-Za-z]*[1-9]+[0-9]*$") && isValid.IsMatch(name.ToUpper());
        }

        /// <summary>
        /// Enumerates the names of all the non-empty cells in the spreadsheet.
        /// </summary>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            foreach (KeyValuePair<string, Cell> pair in cells)
            {
                yield return pair.Key;
            }
        }

        /// <summary>
        /// If formula parameter is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, if changing the contents of the named cell to be the formula would cause a 
        /// circular dependency, throws a CircularException.
        /// 
        /// Otherwise, the contents of the named cell becomes formula.  The method returns a
        /// Set consisting of name plus the names of all other cells whose value depends,
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, Formula formula)
        {
            Cell cell;
            name = name.ToUpper();
            // Hold the old cell contents incase the update causes a circular dependency
            object oldContents = GetCellContents(name);
            // Create cell if needed
            if (!cells.TryGetValue(name, out cell))
            {
                cell = new Cell(name, formula);
                cells.Add(name, cell);
            }
            // Set the contents of the cell incase it already existed
            cell.SetContents(formula);
            graph.ReplaceDependees(name, formula.GetVariables());

            HashSet<string> cellsToRecalculate = new HashSet<string>();
            // If the update causes a cirulcar exception catch it
            try
            {
                cellsToRecalculate = new HashSet<string>((LinkedList<string>)GetCellsToRecalculate(name));
            }
            catch (CircularException)
            {
                // Reset the cell back to its original state and throw a CircularException.
                setCellContents(name, oldContents);
                if(oldContents is string && (string)oldContents == "")
                {
                    cells.Remove(name);
                }
                throw new CircularException();
            }
            return cellsToRecalculate;
        }

        /// <summary>
        /// If text is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes text.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, string text)
        {
            Cell cell;
            name = name.ToUpper();
            // Create cell if needed
            if (!cells.TryGetValue(name, out cell))
            {
                cell = new Cell(name, text);
                cells.Add(name, cell);
            }
            // Set the contents of the cell incase it already existed
            cell.SetContents(text);
            graph.ReplaceDependees(name, new HashSet<string>());

            return new HashSet<string>((LinkedList<string>)GetCellsToRecalculate(name));
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, the contents of the named cell becomes number.  The method returns a
        /// set consisting of name plus the names of all other cells whose value depends, 
        /// directly or indirectly, on the named cell.
        /// 
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        protected override ISet<string> SetCellContents(string name, double number)
        {
            Cell cell;
            name = name.ToUpper();
            // Create cell if needed and add it to the cells dict
            if (!cells.TryGetValue(name, out cell))
            {
                cell = new Cell(name, number);
                cells.Add(name, cell);
            }
            // Set the contents of the cell incase it already existed
            cell.SetContents(number);
            graph.ReplaceDependees(name, new HashSet<string>());
            return new HashSet<string>((LinkedList<string>)GetCellsToRecalculate(name));
        }

        /// <summary>
        /// Private convience method
        /// </summary>
        /// <param name="name">Name of the cell</param>
        /// <param name="contents">Contents of the cell</param>
        /// <returns></returns>
        private ISet<string> setCellContents(string name, object contents)
        {
            if (contents is double)
            {
                return SetCellContents(name, (double)contents);
            }
            if (contents is Formula)
            {
                return SetCellContents(name, (Formula)contents);
            }
            // Is a string
            return SetCellContents(name, (string)contents);
        }

        /// <summary>
        /// If name is null, throws an ArgumentNullException.
        /// 
        /// Otherwise, if name isn't a valid cell name, throws an InvalidNameException.
        /// 
        /// Otherwise, returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// 
        /// For example, suppose that
        /// A1 contains 3
        /// B1 contains the formula A1 * A1
        /// C1 contains the formula B1 + A1
        /// D1 contains the formula B1 - C1
        /// The direct dependents of A1 are B1 and C1
        /// </summary>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            // Error checking has already been done.
            return graph.GetDependents(name.ToUpper());
        }

        // ADDED FOR PS6
        /// <summary>
        /// Writes the contents of this spreadsheet to dest using an XML format.
        /// The XML elements should be structured as follows:
        ///
        /// <spreadsheet IsValid="IsValid regex goes here">
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        ///   <cell name="cell name goes here" contents="cell contents go here"></cell>
        /// </spreadsheet>
        ///
        /// The value of the isvalid attribute should be IsValid.ToString()
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.
        /// If the cell contains a string, the string (without surrounding double quotes) should be written as the contents.
        /// If the cell contains a double d, d.ToString() should be written as the contents.
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        ///
        /// If there are any problems writing to dest, the method should throw an IOException.
        /// </summary>
        public override void Save(TextWriter dest)
        {
            try
            {
                using (XmlWriter writer = XmlWriter.Create(dest))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("IsValid", isValid.ToString());

                    foreach (KeyValuePair<string, Cell> pair in cells)
                    {
                        writer.WriteStartElement("cell");
                        writer.WriteAttributeString("name", pair.Key);
                        writer.WriteAttributeString("contents", pair.Value.ContentsToXml());
                        writer.WriteEndElement();
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }

                Changed = false;
            }
            catch (Exception)
            {
                throw new IOException();
            }
        }

        // ADDED FOR PS6
        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, returns the value (as opposed to the contents) of the named cell.  The return
        /// value should be either a string, a double, or a FormulaError.
        /// </summary>
        public override object GetCellValue(string name)
        {
            if (name == null || !cellNameIsValid(name))
            {
                throw new InvalidNameException();
            }
            Cell cell;
            if(cells.TryGetValue(name.ToUpper(), out cell))
            {
                return cell.Value;
            }

            return "";
        }

        // ADDED FOR PS6
        /// <summary>
        /// If content is null, throws an ArgumentNullException.
        ///
        /// Otherwise, if name is null or invalid, throws an InvalidNameException.
        ///
        /// Otherwise, if content parses as a double, the contents of the named
        /// cell becomes that double.
        ///
        /// Otherwise, if content begins with the character '=', an attempt is made
        /// to parse the remainder of content into a Formula f using the Formula
        /// constructor with s => s.ToUpper() as the normalizer and a validator that
        /// checks that s is a valid cell name as defined in the AbstractSpreadsheet
        /// class comment.  There are then three possibilities:
        ///
        ///   (1) If the remainder of content cannot be parsed into a Formula, a
        ///       Formulas.FormulaFormatException is thrown.
        ///
        ///   (2) Otherwise, if changing the contents of the named cell to be f
        ///       would cause a circular dependency, a CircularException is thrown.
        ///
        ///   (3) Otherwise, the contents of the named cell becomes f.
        ///
        /// Otherwise, the contents of the named cell becomes content.
        ///
        /// If an exception is not thrown, the method returns a set consisting of
        /// name plus the names of all other cells whose value depends, directly
        /// or indirectly, on the named cell.
        ///
        /// For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        /// set {A1, B1, C1} is returned.
        /// </summary>
        public override ISet<string> SetContentsOfCell(string name, string content)
        {
            if(content == null)
            {
                throw new ArgumentNullException();
            }
            if (name == null || !cellNameIsValid(name))
            {
                throw new InvalidNameException();
            }

            name = name.ToUpper();
            // Set of the cells that will need to be updated when this cell's contents change
            HashSet<string> toBeUpdated = new HashSet<string>();

            // Check if contents is a double.
            double d;
            if(Double.TryParse(content, out d))
            {
                // Is a double
                toBeUpdated = (HashSet<string>) SetCellContents(name, d);
            } else if(!String.IsNullOrEmpty(content) && content[0] == '=')
            {
                // Is a Formula
                toBeUpdated = (HashSet<string>) SetCellContents(name, new Formula(content.Substring(1), s => s.ToUpper(), cellNameIsValid));
            } else
            {
                // Is a string
                toBeUpdated = (HashSet<string>)SetCellContents(name, content);
            }

            // Recalculate cells.
            foreach(string cellName in toBeUpdated)
            {
                cells[cellName].CalculateValue((s => (double)GetCellValue(s)));
            }

            // If the cell was updated to an empty string remove it.
            if (cells[name].IsEmpty())
            {
                cells.Remove(name);
                graph.ReplaceDependees(name, new HashSet<string>());
            }
            Changed = true;
            return toBeUpdated;
        }

    }
}
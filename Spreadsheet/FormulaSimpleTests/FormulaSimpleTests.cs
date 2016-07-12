// Written by Joe Zachary for CS 3500, January 2016.
// Reapired error in Evaluate5.  Added TestMethod Attribute
//    for Evaluate4 and Evaluate5 - JLZ January 25, 2016

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Formulas;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace FormulaTestCases
{
    /// <summary>
    /// These test cases are in no sense comprehensive!  They are intended to show you how
    /// client code can make use of the Formula class, and to show you how to create your
    /// own (which we strongly recommend).  To run them, pull down the Test menu and do
    /// Run > All Tests.
    /// </summary>
    [TestClass]
    public class UnitTests
    {
        /// <summary>
        /// This tests that a syntactically incorrect parameter to Formula results
        /// in a FormulaFormatException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct1()
        {
            Formula f = new Formula("_");
        }

        /// <summary>
        /// This is another syntax error
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct2()
        {
            Formula f = new Formula("2++3");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct3()
        {
            Formula f = new Formula("2 3");
        }

        /// <summary>
        /// Valid formula.
        /// </summary>
        [TestMethod]
        public void Construct4()
        {
            Formula f = new Formula("2.5e9 + x5 / 17");
        }

        /// <summary>
        /// Another syntax error, negative number.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct5()
        {
            Formula f = new Formula("-23 + 33");
        }

        /// <summary>
        /// Valid formula.
        /// </summary>
        [TestMethod]
        public void Construct6()
        {
            Formula f = new Formula("(2.5e9 + (x5 / 17))");
        }

        /// <summary>
        /// Another syntax error, mismatch parenthesis 
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct7()
        {
            Formula f = new Formula("(-23 + (33 / 2)");
        }

        /// <summary>
        /// Valid formula.
        /// </summary>
        [TestMethod]
        public void Construct8()
        {
            Formula f = new Formula("3.14");
        }

        /// <summary>
        /// Another syntax error, brackets.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct9()
        {
            Formula f = new Formula("[33 + 33] - 22");
        }

        /// <summary>
        /// Another syntax error.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct10()
        {
            Formula f = new Formula("--23");
        }

        /// <summary>
        /// Another syntax error, invalid tokens.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct11()
        {
            Formula f = new Formula("(33 + 33) & 22");
        }

        /// <summary>
        /// Valid formula.
        /// </summary>
        [TestMethod]
        public void Construct12()
        {
            Formula f = new Formula("33 - 44");
        }

        /// <summary>
        /// Another syntax error, invalid tokens.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct13()
        {
            Formula f = new Formula("   ");
        }

        /// <summary>
        /// Another syntax error, invalid variable.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct14()
        {
            Formula f = new Formula("33 + 11 - x4&4");
        }

        /// <summary>
        /// Middle opening paren is followed by a closing paren
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct15()
        {
            Formula f = new Formula("((()))");
        }

        /// <summary>
        /// Valid formula.
        /// </summary>
        [TestMethod]
        public void Construct16()
        {
            Formula f = new Formula("(5 * x77x) + 8");
        }

        /// <summary>
        /// Number followed by an opening paren
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct17()
        {
            Formula f = new Formula("22 * 11 (44 + 11)");
        }

        /// <summary>
        /// Variable followed by an opening paren
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct18()
        {
            Formula f = new Formula("22 * x7 (44 + 11)");
        }

        /// <summary>
        /// Closing paren followed by an opening paren
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct19()
        {
            Formula f = new Formula("(22 * x7) (44 + 11)");
        }

        /// <summary>
        /// Another syntax error, -8.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct20()
        {
            Formula f = new Formula("(5 * x77x) + -8");
        }

        /// <summary>
        /// Valid formula.
        /// </summary>
        [TestMethod]
        public void Construct21()
        {
            Formula f = new Formula("(33 + 44) + 44");
        }

        /// <summary>
        /// Another syntax error, operator after opening paren
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct22()
        {
            Formula f = new Formula("44 + (+ 44)");
        }

        /// <summary>
        /// Another syntax error, closing paren following opening paren.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct23()
        {
            Formula f = new Formula("44 + () 44)");
        }

        /// <summary>
        /// Another syntax error, last token is a operator.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct24()
        {
            Formula f = new Formula("44 + (+ 44) *");
        }

        /// <summary>
        /// Another syntax error, last token is an opening paren.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct25()
        {
            Formula f = new Formula("44 + (+ 44) (");
        }

        /// <summary>
        /// Valid formula.
        /// </summary>
        [TestMethod]
        public void Construct26()
        {
            Formula f = new Formula("x + y");
        }

        /// <summary>
        /// More closing paren
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct27()
        {
            Formula f = new Formula("(22 + 22)) + (33+1)");
        }

        /// <summary>
        /// Last token is not a number, variable or a closing paren
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct28()
        {
            Formula f = new Formula("(22 + 22) + (33+1)+");
        }

        /// <summary>
        /// More opening paren
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct29()
        {
            Formula f = new Formula("(22 + 22) + ((33+1)");
        }

        /// <summary>
        /// Passing null into the constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Construct30()
        {
            Formula f = new Formula(null);
        }

        /// <summary>
        /// Passing negative 3 into the constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct31()
        {
            Formula f = new Formula("-3");
        }

        /// <summary>
        /// Tests: The first token of a formula must be a number, a variable, or an opening parenthesis.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct32()
        {
            Formula f = new Formula(") 33 + 33");
        }

        /// <summary>
        /// Tests: The last token of a formula must be a number, a variable, or a closing parenthesis.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct33()
        {
            Formula f = new Formula("33 + 33 (");
        }

        /// <summary>
        /// Tests: Any token that immediately follows an opening parenthesis or an operator must be either a number, a variable, or an opening parenthesis.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct34()
        {
            Formula f = new Formula("(+33)");
        }

        /// <summary>
        /// Tests: Any token that immediately follows a number, a variable, or a closing parenthesis must be either an operator or a closing parenthesis.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct35()
        {
            Formula f = new Formula("33 + 33 )");
        }

        /// <summary>
        /// Makes sure that "2+3" evaluates to 5.  Since the Formula
        /// contains no variables, the delegate passed in as the
        /// parameter doesn't matter.  We are passing in one that
        /// maps all variables to zero.
        /// </summary>
        [TestMethod]
        public void Evaluate1()
        {
            Formula f = new Formula("2+3");
            Assert.AreEqual(f.Evaluate(v => 0), 5.0, 1e-6);
        }

        /// <summary>
        /// The Formula consists of a single variable (x5).  The value of
        /// the Formula depends on the value of x5, which is determined by
        /// the delegate passed to Evaluate.  Since this delegate maps all
        /// variables to 22.5, the return value should be 22.5.
        /// </summary>
        [TestMethod]
        public void Evaluate2()
        {
            Formula f = new Formula("x5");
            Assert.AreEqual(f.Evaluate(v => 22.5), 22.5, 1e-6);
        }

        /// <summary>
        /// Here, the delegate passed to Evaluate always throws a
        /// FormulaEvaluationException (meaning that no variables have
        /// values).  The test case checks that the result of
        /// evaluating the Formula is a FormulaEvaluationException.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate3()
        {
            Formula f = new Formula("x + y");
            f.Evaluate(v => { throw new UndefinedVariableException(v); });
        }

        /// <summary>
        /// The delegate passed to Evaluate is defined below.  We check
        /// that evaluating the formula returns in 10.
        /// </summary>
        public void Evaluate4()
        {
            Formula f = new Formula("x + y");
            Assert.AreEqual(f.Evaluate(Lookup4), 10.0, 1e-6);
        }

        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        public void Evaluate5()
        {
            Formula f = new Formula("(x + y) * (z / x) * 1.0");
            Assert.AreEqual(f.Evaluate(Lookup4), 20.0, 1e-6);
        }

        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        public void Evaluate6()
        {
            Formula f = new Formula("5x");
            Assert.AreEqual(f.Evaluate(Lookup4), 20.0, 1e-6);
        }

        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        public void Evaluate7()
        {
            Formula f = new Formula("7 * (5x / 20)");
            Assert.AreEqual(f.Evaluate(Lookup4), 7.0, 1e-6);
        }

        /// <summary>
        /// This uses one of each kind of token.
        /// </summary>
        public void Evaluate8()
        {
            Formula f = new Formula("33 + 27 * (44 * 1)");
            Assert.AreEqual(f.Evaluate(Lookup4), 1221.0, 1e-6);
        }

        /// <summary>
        /// Equals 25.
        /// </summary>
        [TestMethod]
        public void Evaluate9()
        {
            Formula f = new Formula("(4 + 6) * (2 + 3) / 2");
            Assert.AreEqual(f.Evaluate(Lookup4), 25.0, 1e-6);
        }

        /// <summary>
        /// Cannot divide by 0.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate10()
        {
            Formula f = new Formula("(4 + 6) * (2 + 3) / (10 * 0)");
            Assert.AreEqual(f.Evaluate(Lookup4), 25.0, 1e-6);
        }

        /// <summary>
        /// Another test
        /// </summary>
        [TestMethod]
        public void Evaluate11()
        {
            Formula f = new Formula("11 - 15");
            Assert.AreEqual(f.Evaluate(Lookup4), -4, 1e-6);
        }

        /// <summary>
        /// Another test
        /// </summary>
        [TestMethod]
        public void Evaluate12()
        {
            Formula f = new Formula("85 / 5 - 10");
            Assert.AreEqual(f.Evaluate(Lookup4), 7, 1e-6);
        }

        /// <summary>
        /// Another test
        /// </summary>
        [TestMethod]
        public void Evaluate13()
        {
            Formula f = new Formula("6/3*2");
            Assert.AreEqual(f.Evaluate(Lookup4),4.0, 1e-6);
        }

        /// <summary>
        /// Another test
        /// </summary>
        [TestMethod]
        public void Evaluate14()
        {
            Formula f = new Formula("(2334 * 11)+(33 - 11)/(555 + 1)/(22 + 22 * 22)");
            Assert.AreEqual(f.Evaluate(Lookup4), 25674.0000781983, 1e-6);
        }

        /// <summary>
        /// v does not have a value
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaEvaluationException))]
        public void Evaluate15()
        {
            Formula f = new Formula("(4 + 6) * (2 + 3) / (10 * v)");
            Assert.AreEqual(f.Evaluate(Lookup4), 25.0, 1e-6);
        }

        /// <summary>
        /// Another test.
        /// </summary>
        [TestMethod]
        public void Evaluate16()
        {
            Formula f = new Formula("4 + 6 - 2");
            Assert.AreEqual(f.Evaluate(Lookup4), 8.0, 1e-6);
        }

        /// <summary>
        /// Token mapping test, TokenType.BinaryOperator
        /// </summary>
        [TestMethod]
        public void GetTokenType()
        {
            Formula f = new Formula("4 + 6 - 2");
            PrivateObject formulaAccessor = new PrivateObject(f);
            object[] parameters = { "+" };
            int tokenTypeInt = (int) formulaAccessor.Invoke("GetTokenType", parameters);
            Assert.AreEqual(tokenTypeInt, 3);
        }

        /// <summary>
        /// Token mapping test, TokenType.OpenPren
        /// </summary>
        [TestMethod]
        public void GetTokenType1()
        {
            Formula f = new Formula("4 + 6 - 2");
            PrivateObject formulaAccessor = new PrivateObject(f);
            object[] parameters = { "(" };
            int tokenTypeInt = (int)formulaAccessor.Invoke("GetTokenType", parameters);
            Assert.AreEqual(tokenTypeInt, 1);
        }

        /// <summary>
        /// Token mapping test, TokenType.CloseParen
        /// </summary>
        [TestMethod]
        public void GetTokenType2()
        {
            Formula f = new Formula("4 + 6 - 2");
            PrivateObject formulaAccessor = new PrivateObject(f);
            object[] parameters = { ")" };
            int tokenTypeInt = (int)formulaAccessor.Invoke("GetTokenType", parameters);
            Assert.AreEqual(tokenTypeInt, 2);
        }

        /// <summary>
        /// Token mapping test, TokenType.Double
        /// </summary>
        [TestMethod]
        public void GetTokenType3()
        {
            Formula f = new Formula("4 + 6 - 2");
            PrivateObject formulaAccessor = new PrivateObject(f);
            object[] parameters = { "3.14" };
            int tokenTypeInt = (int)formulaAccessor.Invoke("GetTokenType", parameters);
            Assert.AreEqual(tokenTypeInt, 4);
        }

        /// <summary>
        /// Token mapping test, TokenType.Variable
        /// </summary>
        [TestMethod]
        public void GetTokenType5()
        {
            Formula f = new Formula("4 + 6 - 2");
            PrivateObject formulaAccessor = new PrivateObject(f);
            object[] parameters = { "x42x" };
            int tokenTypeInt = (int)formulaAccessor.Invoke("GetTokenType", parameters);
            Assert.AreEqual(tokenTypeInt, 5);
        }

        /// <summary>
        /// Token mapping test, TokenType.None
        /// </summary>
        [TestMethod]
        public void GetTokenType6()
        {
            Formula f = new Formula("4 + 6 - 2");
            PrivateObject formulaAccessor = new PrivateObject(f);
            object[] parameters = { "$" };
            int tokenTypeInt = (int)formulaAccessor.Invoke("GetTokenType", parameters);
            Assert.AreEqual(tokenTypeInt, 0);
        }


        /// <summary>
        /// A Lookup method that maps x to 4.0, y to 6.0, and z to 8.0.
        /// All other variables result in an UndefinedVariableException.
        /// </summary>
        /// <param name="v"></param>
        /// <returns></returns>
        public double Lookup4(String v)
        {
            switch (v)
            {
                case "x": return 4.0;
                case "y": return 6.0;
                case "z": return 8.0;
                default: throw new UndefinedVariableException(v);
            }
        }

        /// <summary>
        /// Tests of syntax errors detected by the constructor
        /// </summary>
        [TestMethod()]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Test1()
        {
            Formula f = new Formula("        ");
        }

        /// <summary>
        /// Simple test to test GetVariables()
        /// </summary>
        [TestMethod]
        public void GetVariables()
        {
            Formula f = new Formula("((((x1+x2)+x3)+x4)+x5)+x6");
            HashSet<string> variableSet = new HashSet<string>();
            variableSet.Add("x1");
            variableSet.Add("x2");
            variableSet.Add("x3");
            variableSet.Add("x4");
            variableSet.Add("x5");
            variableSet.Add("x6");

            Assert.IsTrue(f.GetVariables().SetEquals(variableSet));
        }

        /// <summary>
        /// Simple tests of GetVariables()
        /// </summary>
        [TestMethod]
        public void GetVariables1()
        {
            Formula f = new Formula("22 + 44 + x34 + 55 * 33 +x3");
            HashSet<string> variableSet = new HashSet<string>();
            variableSet.Add("x34");
            variableSet.Add("x3");
            Assert.IsTrue(f.GetVariables().SetEquals(variableSet));
        }

        /// <summary>
        /// Normalizer converts to uppercase and validator ensures it is in uppercase.
        /// Ensures that the normalized version of the variable is returned in GetVariables()
        /// </summary>
        [TestMethod]
        public void GetVariables2()
        {
            Formula f = new Formula("((((x1 + x2) + x3) + x4) + x5) + x6", s => s.ToUpper(), s => s == s.ToUpper());
            HashSet<string> variableSet = new HashSet<string>();
            variableSet.Add("X1");
            variableSet.Add("X2");
            variableSet.Add("X3");
            variableSet.Add("X4");
            variableSet.Add("X5");
            variableSet.Add("X6");

            Assert.IsTrue(f.GetVariables().SetEquals(variableSet));
        }

        /// <summary>
        /// Normalizer converts to uppercase and validator ensures it is in uppercase.
        /// </summary>
        [TestMethod]
        public void Construct36()
        {
            Formula f = new Formula("((((x1 + x2) + x3) + x4) + x5) + x6", s => s.ToUpper(), s => s == s.ToUpper());
        }

        /// <summary>
        /// Normalizer converts to uppercase and validator fails becuase it expects lower case.
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct37()
        {
            Formula f = new Formula("((((x1 + x2) + x3) + x4) + x5) + x6", s => s.ToUpper(), s => s == s.ToLower());
        }

        /// <summary>
        /// Test case from the PS4 writeup
        /// </summary>
        [TestMethod]
        public void Construct38()
        {
            Formula f = new Formula("x2+y3", s => s.ToUpper(), s => Regex.IsMatch(s, "[a-zA-Z][0-9]"));
            Assert.AreEqual(f.ToString(), "X2+Y3");
        }

        /// <summary>
        /// Test case from the PS4 writeup
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Construct39()
        {
            Formula f = new Formula("x+y3", s => s.ToUpper(), s => Regex.IsMatch(s, "[a-zA-Z][0-9]"));
        }

        /// <summary>
        /// Tests the parameterless constructor
        /// </summary>
        [TestMethod]
        public void Construct40()
        {
            Formula f1 = new Formula();

            Formula f2 = new Formula("0");
            Assert.AreEqual(f1.ToString(), f2.ToString());
            f1 = new Formula();
            Assert.IsTrue(f1.GetVariables().SetEquals(f2.GetVariables()));
            f1 = new Formula();
            Assert.AreEqual(f1.Evaluate(Lookup4), f2.Evaluate(Lookup4));
        }

        /// <summary>
        /// Simple toString() test.
        /// </summary>
        [TestMethod]
        public void ToString1()
        {
            Formula f1 = new Formula("a + b * c - d + 3 * 3.0 - 3.0e0 / 0.003e3", s => s.ToUpper(), s => s == s.ToUpper());
            Formula f2 = new Formula(f1.ToString(), s => s.ToUpper(), s => s == s.ToUpper());
            Assert.AreEqual(f1.ToString(), f2.ToString());
        }

        /// <summary>
        /// Tests normalizer
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(FormulaFormatException))]
        public void Constructor40()
        {
            Formula f = new Formula("x + y", s => "@#$@#$" + s, s => true);
        }

    }
}

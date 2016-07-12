// Skeleton written by Joe Zachary for CS 3500, January 2015
// Revised by Joe Zachary, January 2016
// JLZ Repaired pair of mistakes, January 23, 2016
// Nick Porter - u0927946

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Formulas
{

    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  Provides a means to evaluate Formulas.  Formulas can be composed of
    /// non-negative floating-point numbers, variables, left and right parentheses, and
    /// the four binary operator symbols +, -, *, and /.  (The unary operators + and -
    /// are not allowed.)
    /// </summary>
    public struct Formula
    {
        /// <summary>
        /// Used to represent a formula by categorizing tokens.
        /// Enum that will be mapped to a token based on it contents.
        /// None is the default value of the enum,
        /// "(" = LeftParen, ")" = RightParen, "+" or "-" or "/" or "*" = BinaryOperator, 3.13 = Double, x = Variable
        /// </summary>
        private enum TokenType { None, OpenParen, CloseParen, BinaryOperator, Double, Variable };

        // List mapping the tokens to their TokenType.
        // For example "3.14" will be mapped to TokenType.Double
        // For example "+" will be mapped to TokenType.BinaryOperator, ect
        private List<KeyValuePair<string, TokenType>> tokenMap;

        /// <summary>
        /// Creates a Formula from a string that consists of a standard infix expression composed
        /// from non-negative floating-point numbers (using C#-like syntax for double/int literals), 
        /// variable symbols (a letter followed by zero or more letters and/or digits), left and right
        /// parentheses, and the four binary operator symbols +, -, *, and /.  White space is
        /// permitted between tokens, but is not required.
        /// 
        /// Examples of a valid parameter to this constructor are:
        ///     "2.5e9 + x5 / 17"
        ///     "(5 * 2) + 8"
        ///     "x*y-2+35/9"
        ///     
        /// Examples of invalid parameters are:
        ///     "_"
        ///     "-5.3"
        ///     "2 5 + 3"
        /// 
        /// If the formula is syntacticaly invalid, throws a FormulaFormatException with an 
        /// explanatory Message.
        /// </summary>
        public Formula(String formula) : this(formula, s => s, s => true)
        {   
        }

        /// <summary>
        /// Constructs a Formula with the given string.
        /// Each variable is normalized by normalizer.
        /// Each variable is also validated by validator.
        /// </summary>
        /// <param name="formula">String representation of the formula</param>
        /// <param name="normalizer">Normalizer delegate to put variables in noramlized form</param>
        /// <param name="validator">Delegate to validate variables</param>
        public Formula(String formula, Normalizer normalizer, Validator validator)
        {
            // Break the formula into tokens.
            IEnumerable<string> tokens = GetTokens(formula);
            tokenMap = new List<KeyValuePair<string, TokenType>>();
            ValidateExpression(tokens, normalizer, validator);
        }
        /// <summary>
        /// Evaluates this Formula, using the Lookup delegate to determine the values of variables.  (The
        /// delegate takes a variable name as a parameter and returns its value (if it has one) or throws
        /// an UndefinedVariableException (otherwise).  Uses the standard precedence rules when doing the evaluation.
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, its value is returned.  Otherwise, throws a FormulaEvaluationException  
        /// with an explanatory Message.
        /// </summary>
        public double Evaluate(Lookup lookup)
        {
            normalize();
            Stack<double> valueStack = new Stack<double>();
            Stack<string> operatorStack = new Stack<string>();

            for(int i = 0; i < tokenMap.Count; i++)
            {
                string token = tokenMap[i].Key;
                TokenType tokenType = tokenMap[i].Value;

                // If the token is a variable then we will simply replace the value with a double and change the type to a double.
                if(tokenType == TokenType.Variable)
                {
                    // Look it up and place value back into token and switch type to Double
                    try
                    {
                        token = lookup(token).ToString();
                    }
                    catch(UndefinedVariableException)
                    {
                        // lookup throws UndefinedVariableException, however the spec requires FormulaEvaluationException to be thrown when a variable cannot be mapped to a value.
                        throw new FormulaEvaluationException("The passed formula has an unfefined variable");
                    }
                    tokenType = TokenType.Double;
                }

                if (tokenType == TokenType.Double)
                {
                    // If * or / is at the top of the operator stack
                    if(operatorStack.Count != 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                    {
                        // Pop value and operator stack
                        // Evaluate the current token with the popped value and the operator
                        valueStack.Push(Evaluate(Convert.ToDouble(token), valueStack.Pop(), operatorStack.Pop()));

                    } else
                    {
                        // Else just push the value to the stack
                        valueStack.Push(Convert.ToDouble(token));
                    } 
                } else if(tokenType == TokenType.BinaryOperator)
                {
                    // If a + or - is at the top of the operator stack pop it and evaluate it with the 2 top values from the value stack
                    if((token == "+" || token == "-") && (operatorStack.Count != 0 && (operatorStack.Peek() == "+" || operatorStack.Peek() == "-")))
                    {
                        valueStack.Push(Evaluate(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop()));
                    }
                    // Either way push the new operator to the stack
                    operatorStack.Push(token);
                } else if (tokenType == TokenType.OpenParen)
                {
                    // Simply push open paren to the stack
                    operatorStack.Push(token);
                } else if(tokenType == TokenType.CloseParen)
                {
                    // If a + or - is at the top of the operator stack pop it and evaluate it with the 2 top values from the value stack
                    if (operatorStack.Count != 0 && (operatorStack.Peek() == "+" || operatorStack.Peek()== "-"))
                    {
                        valueStack.Push(Evaluate(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop()));
                    }

                    operatorStack.Pop();
                    // If a * or / is at the top of the operator stack pop it and evaluate it with the 2 top values from the value stack
                    if (operatorStack.Count != 0 && (operatorStack.Peek() == "*" || operatorStack.Peek() == "/"))
                    {
                        valueStack.Push(Evaluate(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop()));
                    }

                }

            }

            // Operator stack is empty, return the top of the value stack.
            if(operatorStack.Count == 0)
            {
                return valueStack.Pop();
            }
            
            // Operator stack is not empty, pop the last operator and evaluate with the last 2 values in the value stack
            return Evaluate(valueStack.Pop(), valueStack.Pop(), operatorStack.Pop());
        }

        /// <summary>
        /// This method takes two double values and computes the result based on the 
        /// passed operator
        /// </summary>
        /// <param name="firstValue">The first operand</param>
        /// <param name="secondValue">Second operand</param>
        /// <param name="oper">The operator to be applied to the two values</param>
        /// <returns>The result after applying the operator to the two values.</returns>
        private double Evaluate(double secondValue, double firstValue , string oper)
        {
            if (oper == "+")
            {
                return firstValue + secondValue;
            }
            if (oper == "-")
            {
                return firstValue - secondValue;
            }

            if (oper == "*")
            {
                return firstValue * secondValue;
            }

            if(secondValue == 0)
            {
                throw new FormulaEvaluationException("Division by 0 is not allowed.");
            }
            return firstValue / secondValue;
        }

        /// <summary>
        /// Given a formula, enumerates the tokens that compose it.  Tokens are left paren,
        /// right paren, one of the four operator symbols, a string consisting of a letter followed by
        /// zero or more digits and/or letters, a double literal, and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z][0-9a-zA-Z]*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: e[\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }
        }

        /// <summary>
        /// Used to ensure that given formula is valid anf ollows the rules.
        /// Throws FormulaFormatException if at any point we determine the formula or if the formula contains invalid characters.
        /// is an invalid formula.
        /// If no exception is thrown then the formula was successfully validated.
        /// </summary>
        /// <param name="tokens">The formula broken up into tokens</param>
        /// <param name="Normalizer">The normalizer delegate</param>
        /// <param name="Validator">The delegate to validate variables</param>
        private void ValidateExpression(IEnumerable<string> tokens, Normalizer Normalizer, Validator Validator)
        {
            // Check to make sure that there is at least one token.
            if (!tokens.Any())
            {
                throw new FormulaFormatException("The formula is empty.");
            }

            // Track the times we have encourtered opening and closing parenthesis.
            int leftParenCount = 0;
            int rightParenCount = 0;
            // Hold the previous operator
            TokenType prevTokenType = TokenType.None;

            foreach(string token in tokens)
            {
                string tokenCopy = token;
                TokenType currentTokenType = GetTokenType(tokenCopy);

                if(currentTokenType == TokenType.Variable)
                {
                    // Normalize the variable
                    tokenCopy = Normalizer(tokenCopy);

                    // Ensure that the normlized token is a valid variable based on standard Formula rules.
                    // Returns TokenType.Variable if it is, otherwise TokenType.None.

                    currentTokenType = GetTokenType(tokenCopy);

                    if(!Validator(tokenCopy))
                    {
                        throw new FormulaFormatException("The formula contains invalid variables.");
                    }
                }

                if(currentTokenType == TokenType.None)
                {
                    throw new FormulaFormatException("The formula contains invalid characters.");
                }

                // Add the token to the token type, to be used in evaluating the expression.
                tokenMap.Add(new KeyValuePair<string, TokenType>(tokenCopy, currentTokenType));

                // prevTokenType is only set to none when enumerating over the first token
                // Formula must start with a number, variable, or a opening parenthesis
                if (prevTokenType == TokenType.None && IsOperatorOrParenType(currentTokenType, TokenType.CloseParen))
                {
                    throw new FormulaFormatException("The first token must be a number, variable, or a opening parenthesis.");
                }

                // Increment counts of parenthesis
                if (currentTokenType == TokenType.OpenParen)
                {
                    leftParenCount++;
                } else if(currentTokenType == TokenType.CloseParen)
                {
                    rightParenCount++;
                }

                // Any token that follows an opening parenthesis or operator must either be a number, variable, opening parenthesis.
                if (IsOperatorOrParenType(prevTokenType, TokenType.OpenParen) && !CanFollowLeftParenOrOperator(currentTokenType))
                {
                    throw new FormulaFormatException("A token that follows an opening parenthesis or operator must either be a number, variable, opening parenthesis.");
                }

                // Any token that immediately follows a number, a variable, or a closing parenthesis must either be an operator or a closing parenthesis.
                if((prevTokenType == TokenType.Double || prevTokenType == TokenType.Variable || prevTokenType == TokenType.CloseParen) 
                    && !CanFollowNumberOrVariableOrRightParen(currentTokenType))
                {
                    throw new FormulaFormatException("A token that immediately follows a number, a variable, or a closing parenthesis must either be an operator or a closing parenthesis.");
                }

                // Reading left to right, the number of closing parenthesis should never be greater than opening parenthesis.
                if (rightParenCount > leftParenCount)
                {
                    throw new FormulaFormatException("Invalid expression, mismatched parenthesis.");
                }
                prevTokenType = currentTokenType;
            }

            // The total number of opening parenthesis and closing parenthesis must be equal.
            if(leftParenCount != rightParenCount)
            {
                throw new FormulaFormatException("Invalid expression, mismatched parenthesis.");
            }
            // Last token must be either a number, variable or closing parenthesis.
            if (IsOperatorOrParenType(prevTokenType, TokenType.OpenParen))
            {
                throw new FormulaFormatException("The last token must be a number, variable, or a closing parenthesis.");
            }
        }

        /// <summary>
        /// Used to determine what kind of token a token is.
        /// </summary>
        /// <param name="token">The token to be mapped to a TokenType</param>
        /// <returns>The corresponding TokenType to token</returns>
        private TokenType GetTokenType(string token)
        {
            // Is a left parenthesis
            if (token == "(")
            {
                return TokenType.OpenParen;
            }
            // Is a right parenthesis
            if (token == ")")
            {
                return TokenType.CloseParen;
            }
            // Is a binary operator
            if(token == "+" || token == "-" || token == "/" || token == "*")
            {
                return TokenType.BinaryOperator;
            }

            // Is a number
            double n = 0;
            if(Double.TryParse(token, out n))
            {

                return TokenType.Double;
            }

            // Is a variable
            // a string consisting of a letter followed by one or more digits and/or letters
            if (Regex.IsMatch(token, "^[a-zA-Z][0-9a-zA-Z]*$"))
            {
                return TokenType.Variable;
            }

            // Token does not map to a TokenType therfore it is an invalid character.
            return TokenType.None;
        }

        /// <summary>
        /// Returns a set of all the variables contained in this formula
        /// </summary>
        /// <returns>A set containing all the variables in this formula</returns>
        public ISet<string> GetVariables()
        {
            normalize();
            HashSet<string> variableSet = new HashSet<string>();
            foreach(KeyValuePair<string, TokenType> pair in tokenMap)
            {
                if(pair.Value == TokenType.Variable)
                {
                    variableSet.Add(pair.Key);
                }
            }
            return variableSet;
        }

        /// <summary>
        /// This method is used to ensure that the token map is not null
        /// Only does something if the zero parameter constructor was used.
        /// </summary>
        private void normalize()
        {
            if(tokenMap == null)
            {
                // Zero parameter constructor was used.
                tokenMap = new List<KeyValuePair<string, TokenType>>();
                tokenMap.Add(new KeyValuePair<string, TokenType>("0", TokenType.Double));
            }
        }

        /// <summary>
        /// This method checks to see if token is either a BinaryOperator or the passed parenthesis
        /// </summary>
        /// <param name="token">The token to be checked.</param>
        /// <param name="parenthesis">Either Token.LeftParen or Token.RightParen, this method ensures that token is this.</param>
        /// <returns>True if token is either a number, variable, or the passed parenthesis.</returns>
        private bool IsOperatorOrParenType(TokenType token, TokenType parenthesis)
        {
            return token == TokenType.BinaryOperator || token == parenthesis;
        }

        /// <summary>
        /// This method ensures that the passed token is allowed to follow either an opening parenthesis or an operator.
        /// </summary>
        /// <param name="token">The token to be checked.</param>
        /// <returns>True if token can follow an opening parenthesis or an operator.</returns>
        private bool CanFollowLeftParenOrOperator(TokenType token)
        {
            return token == TokenType.Double || token == TokenType.Variable || token == TokenType.OpenParen;
        }

        /// <summary>
        /// This method ensures that the passed token is allowed to follow a number, variable, or a closing parenthesis.
        /// </summary>
        /// <param name="token">The token to be checked.</param>
        /// <returns>True if token can follow a number, variable, or a closing parenthesis.</returns>
        private bool CanFollowNumberOrVariableOrRightParen(TokenType token)
        {
            return IsOperatorOrParenType(token, TokenType.CloseParen);
        }

        /// <summary>
        /// Returns a string representation of the formula
        /// </summary>
        /// <returns>Returns a string thath represents the formlula simlar to 4x+33/2</returns>
        public override string ToString()
        {
            normalize();
            string toString = "";
            foreach(KeyValuePair<string,TokenType> token in tokenMap)
            {
                toString += token.Key;
            }

            return toString;
        }

    }

    /// <summary>
    /// A Lookup method is one that maps some strings to double values.  Given a string,
    /// such a function can either return a double (meaning that the string maps to the
    /// double) or throw an UndefinedVariableException (meaning that the string is unmapped 
    /// to a value. Exactly how a Lookup method decides which strings map to doubles and which
    /// don't is up to the implementation of the method.
    /// </summary>
    public delegate double Lookup(string s);

    /// <summary>
    /// Takes a variable and returns it in normalized form.
    /// </summary>
    /// <param name="s">The variable to be normalized</param>
    /// <returns>s in normalized form</returns>
    public delegate string Normalizer(string s);

    /// <summary>
    /// Returns true if the variable is in valid form
    /// </summary>
    /// <param name="s">The variable to be validated</param>
    /// <returns>True if the variable is valid, false otherwise</returns>
    public delegate bool Validator(string s);

    /// <summary>
    /// Used to report that a Lookup delegate is unable to determine the value
    /// of a variable.
    /// </summary>
    public class UndefinedVariableException : Exception
    {
        /// <summary>
        /// Constructs an UndefinedVariableException containing whose message is the
        /// undefined variable.
        /// </summary>
        /// <param name="variable"></param>
        public UndefinedVariableException(String variable)
            : base(variable)
        {
        }
    }

    /// <summary>
    /// Used to report syntactic errors in the parameter to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message) : base(message)
        {
        }
    }

    /// <summary>
    /// Used to report errors that occur when evaluating a Formula.
    /// </summary>
    public class FormulaEvaluationException : Exception
    {
        /// <summary>
        /// Constructs a FormulaEvaluationException containing the explanatory message.
        /// </summary>
        public FormulaEvaluationException(String message) : base(message)
        {
        }
    }
}

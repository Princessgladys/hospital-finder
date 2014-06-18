using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HospitalF.Models;
using HospitalF.Constant;
using System.Text.RegularExpressions;

namespace HospitalF.Utilities
{
    /// <summary>
    /// Class define methods to handle string
    /// </summary>
    public class StringUtil
    {
        #region Boyer Moore matching algorithm

        /// <summary>
        /// Prepare bad match table for Boyer Moore algorithm
        /// </summary>
        /// <param name="pattern">Pattern that needed to find in a string</param>
        /// <returns>
        /// Integer array that contains shifting index
        /// for each character in the input pattern
        /// </returns>
        public static int[] CreateBadMatchTable(string pattern)
        {
            int a = 0;
            // Create an array of shifting index
            int[] occurrence = new int[10000];
            // Assign shifting index
            for (int n = 0; n < pattern.Length; n++)
            {
                char b = pattern[n];
                occurrence[pattern[n]] = n;
                a = occurrence[pattern[n]];
            }
            // Return bad match table of a pattern
            return occurrence;
        }

        /// <summary>
        /// Implement Boyer Moore algorithm
        /// </summary>
        /// <param name="text">Input query text</param>
        /// <param name="pattern">Pattern that needed to find in a string</param>
        /// <returns>Boolean value indicating a pattern is matched with the given text</returns>
        public static bool IsPatternMatched(string text, string pattern)
        {
            // Shifiting index
            int shift = 0;

            // Take text length and pattern length
            int textLength = text.Length;
            int patternLength = pattern.Length;
            // Create bad match table
            int[] occurrence = CreateBadMatchTable(pattern);

            // Compare the pattern int the text and using bad match table to
            // shift the pattern to the right of the string for continuing comparation
            for (int n = 0; n <= textLength - patternLength; n += shift)
            {
                shift = 0;
                for (int i = patternLength - 1; i >= 0; i--)
                {
                    char a = pattern[i];
                    char b = text[n + i];
                    if (pattern[i] != text[n + i])
                    {
                        int bc = occurrence[text[n + i]];
                        shift = Math.Max(1, i - occurrence[text[n + i]]);
                        break;
                    }
                }

                // Return true if pattern is match
                if (shift == 0)
                {
                    return true;
                }
            }
            // Return false as default
            return false;
        }

        /// <summary>
        /// Implement Boyer Moore algorithm
        /// </summary>
        /// <param name="text">Input query text</param>
        /// <param name="pattern">Pattern that needed to find in a string</param>
        /// <returns>9999: Not matched, #9999: Matching position</returns>
        public static int TakeMatchedStringPosition(string text, string pattern)
        {
            // Shifiting index
            int shift = 0;

            // Take text length and pattern length
            int textLength = text.Length;
            int patternLength = pattern.Length;
            // Create bad match table
            int[] occurrence = CreateBadMatchTable(pattern);

            // Compare the pattern int the text and using bad match table to
            // shift the pattern to the right of the string for continuing comparation
            for (int n = 0; n <= textLength - patternLength; n += shift)
            {
                shift = 0;
                for (int i = patternLength - 1; i >= 0; i--)
                {
                    char a = pattern[i];
                    char b = text[n + i];
                    if (pattern[i] != text[n + i])
                    {
                        int bc = occurrence[text[n + i]];
                        shift = Math.Max(1, i - occurrence[text[n + i]]);
                        break;
                    }
                }

                // Return matched position if pattern is matched
                if (shift == 0)
                {
                    return n;
                }
            }
            // Return 9999 as default
            return Constants.DefaultMatchingValue;
        }

        #endregion

        /// <summary>
        /// Split words in a string to every token
        /// </summary>
        /// <param name="inputStr">Input String</param>
        /// <returns>List[string] that contains list of tokens</returns>
        public static List<string> StringTokenizer(string inputStr)
        {
            // Create regular express to split white spaces between words
            Regex regex = new Regex(Constants.FindWhiteSpaceRegex);
            // Split words base on regular expression
            MatchCollection matches = regex.Matches(inputStr.Trim());

            // Add tokens to list
            List<string> tokens = new List<string>();
            foreach (Match match in matches)
            {
                tokens.Add(match.Value);
            }

            // Return list of tokens
            return tokens;
        }

        /// <summary>
        /// Concate tokens in a list
        /// </summary>
        /// <param name="tokenList">Input token list</param>
        /// <param name="begin">Begin index</param>
        /// <param name="end">End index</param>
        /// <returns>String of concatenated tokens</returns>
        public static string ConcatTokens(List<string> tokenList, int beginIndex, int endIndex)
        {
            string result = string.Empty;
            for (int n = beginIndex; n <= endIndex; n++)
            {
                result += " " + tokenList[n];
            }
            // Remove leading white space and return value
            return result.Trim();
        }
    }
}
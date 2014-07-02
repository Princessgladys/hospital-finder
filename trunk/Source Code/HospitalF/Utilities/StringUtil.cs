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

        #region Algorithm to calculate the percentage of similarity between 2 strings

        /// <summary>
        /// Take pairs of letters in a word
        /// </summary>
        /// <param name="inputWord">Input word</param>
        /// <returns>List[string] that contains pairs of letters in an input word</returns>
        private static List<string> TakePairsOfLetters(string inputWord)
        {
            // Declare a list to store pairs of letters
            List<string> pairsList = new List<string>();
            // Count number of pairs in string
            int numberOfPairs = inputWord.Length - 1;

            // Add pairs of letters in input string to list
            for (int n = 0; n < numberOfPairs; n++)
            {
                pairsList.Add(inputWord.Substring(n, 2));
            }

            // Return list of pairs
            return pairsList;
        }

        /// <summary>
        /// Take pairs of letters of each word in  list of tokens
        /// </summary>
        /// <param name="tokenList">List of tokens</param>
        /// <returns>
        /// List[string] that contains all pairs in each word
        /// in a list of tokens
        /// </returns>
        private static List<String> TakePairsOfLettersInWord(List<string> tokenList)
        {
            // Declare a list to store all pairs of letters in token list
            List<string> allPairs = new List<string>();
            // Declare a list to store pairs of letters in a word
            List<string> pairsInWord = new List<string>();

            // Take pairs of letters in each word of token list
            foreach (string word in tokenList)
            {
                pairsInWord = TakePairsOfLetters(word);
                foreach(string pair in pairsInWord)
                {
                    allPairs.Add(pair);
                }
            }

            // Return all pairs of letters in a list of tokens
            return allPairs;
        }

        /// <summary>
        /// Calculate the percentage of similarity between 2 strings
        /// </summary>
        /// <param name="inputStr1">Input string 1</param>
        /// <param name="inputStr2">Input string 2</param>
        /// <returns>Percentage of similarity between string 1 and string 2</returns>
        public static float CompareString(string inputStr1, string inputStr2)
        {
            // Declare variable to store union and intersection of pairs in 
            // string 1 and string 2
            int union = 0;
            int intersection = 0;

            // Tokenizer two string and take pairs of letters in them
            List<string> pairsInStr1 = TakePairsOfLettersInWord(StringTokenizer(inputStr1.Trim()));
            List<string> pairsInStr2 = TakePairsOfLettersInWord(StringTokenizer(inputStr2.Trim()));
            union = pairsInStr1.Count + pairsInStr2.Count;

            // Calculate intersection between 2 input strings
            foreach (string pairStr1 in pairsInStr1)
            {
                foreach (string pairStr2 in pairsInStr2)
                {
                    if (pairStr1.Equals(pairStr2))
                    {
                        intersection++;
                    }
                }
            }

            // Return percentage of similarity between 2 input strings
            return ((2 * intersection) * 1.0f) / (union * 1.0f);
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
                result += Constants.WhiteSpace + tokenList[n];
            }
            // Remove leading white space and return value
            return result.Trim();
        }
    }
}
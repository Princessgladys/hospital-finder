using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HospitalF.Models;
using HospitalF.Constant;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using NHunspell;
using System.Text;

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
        private static int[] CreateBadMatchTable(string pattern)
        {
            // Create an array of shifting index
            int[] occurrence = new int[10000];
            // Assign shifting index
            for (int n = 0; n < pattern.Length; n++)
            {
                char a = pattern[n];
                occurrence[pattern[n]] = n;
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
                    char a = text[n + i]; 
                    char b = pattern[i];
                    if (pattern[i] != text[n + i])
                    {
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
        /// <returns>9999: Not matched, #9999: Matched position</returns>
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
                    if (pattern[i] != text[n + i])
                    {
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

        #region Vietnamese vocabulary checking

        /// <summary>
        /// Load list of string from bytes
        /// </summary>
        /// <param name="byteList">Byte array</param>
        /// <param name="inputWord">Word</param>
        /// <returns>String array</returns>
        private static List<string> TakeSuggestionFromDictionary(byte[] byteList, string inputWord)
        {
            string dicFileString = System.Text.Encoding.UTF8.GetString(byteList);
            List<string> suggestionList = dicFileString.Split(Constants.Enter).ToList();
            List<string> returnList = new List<string>();

            foreach (string word in suggestionList)
            {
                if (CompareString(word, inputWord) >= 0.5f)
                {
                    returnList.Add(word);
                }
            }

            // Return suggestion words in dictionary
            return returnList;
        }

        /// <summary>
        /// Take reletive Vietnamese diacritic characters
        /// </summary>
        /// <param name="word">Input word</param>
        /// <returns>Relative Vietnamese diacritic characters</returns>
        private static List<char> TakeRelativeDiacriticChars(string word)
        {
            int matchedPosition = -1;
            char[] basicDiacriticChars = Constants.BasicDiacriticVietnameseCharacters.ToCharArray();
            List<char> relativeDiacraticChars = new List<char>();
            char[] tempCharArray = null;

            for (int n = 0; n < word.Length; n++)
            {
                matchedPosition = TakeMatchedStringPosition(Constants.DiacriticVietnameseCharacters,
                    word[n].ToString());
                // Check if matched position is valid
                if (!Constants.DefaultMatchingValue.Equals(matchedPosition))
                {
                    tempCharArray = Constants.GetDiacriticWords(basicDiacriticChars[matchedPosition]);
                    foreach (char c in tempCharArray)
                    {
                        relativeDiacraticChars.Add(c);
                    }
                }
            }

            // Return list of relative Vietnamese diacritic characters
            return relativeDiacraticChars;
        }

        /// <summary>
        /// Find best suggestion words according to suggestion list in dictionary
        /// </summary>
        /// <param name="suggestionsListInDictionary">
        /// List[string] that contains suggestion words in dictionary
        /// </param>
        /// <param name="relativeDiacraticChars">
        /// List[char] that contains relative Vietnamese diacritic characters
        /// </param>
        /// <returns>List of best suggestion words</returns>
        private static List<string> FindBestSuggestionWords(
            List<string> suggestionsListInDictionary, List<char> relativeDiacraticChars)
        {
            List<string> bestSuggestionList = new List<string>();

            // Check if according diacratic list is not empty
            if (relativeDiacraticChars.Count != 0)
            {
                // Compare every relative diacritic characters with every word in suggestion list
                for (int n = 0; n < relativeDiacraticChars.Count(); n++)
                {
                    // Find best value for each suggesion word
                    foreach (string suggestion in suggestionsListInDictionary)
                    {
                        // Check if suggestion word contain relative diacritive character
                        if (IsPatternMatched(suggestion, relativeDiacraticChars[n].ToString()) &&
                            !bestSuggestionList.Contains(suggestion))
                        {
                            bestSuggestionList.Add(suggestion);
                        }
                    }
                }
            }
            
            // Return list of best suggestion words
            return bestSuggestionList;
        }

        /// <summary>
        /// Check every word in an input string if they are all correct
        /// according to Vietnamese vocabularies
        /// </summary>
        /// <param name="inputStr">Input sentence</param>
        /// <returns>
        /// string[] contains at least 1 and maximum 2 records
        /// Record 1: True / False that indicates input value is correct or not
        /// Record 2 (if any): Suggest a correct string value base on input value
        /// </returns>
        public static string[] CheckVocabulary(string inputStr)
        {
            // Declare list of tokens and list of returned results
            string[] resultList = new string[2];
            resultList[0] = Constants.True;
            List<string> tokenList = new List<string>();

            // Normalize input string
            inputStr = inputStr.Trim().ToLower();
            // Tokenizer input string
            tokenList = StringTokenizer(inputStr);

            // Read configutation detail from Web.config
            string viVnAffUrl = WebConfigurationManager.
                AppSettings[Constants.ViVnAffUrl];
            string viVnDicUrl = WebConfigurationManager.
                AppSettings[Constants.ViVnDicUrl];
            // Return null if configuration detail is not correct
            if (viVnAffUrl == null || viVnDicUrl == null)
            {
                return null;
            }

            // Read from diacritic Vietnamese words dictionary
            byte[] affFile = System.IO.File.ReadAllBytes(viVnAffUrl);
            // Read from single Vietnamese words dictionary
            byte[] dicFile = System.IO.File.ReadAllBytes(viVnDicUrl);

            // Check if input string is correct
            Hunspell hunspell = new Hunspell();
            hunspell.Load(affFile, dicFile);

            using (hunspell)
            {
                // Set default value for suggest string
                string result = string.Empty;

                // Check every word in the list of tokens
                foreach (string token in tokenList)
                {
                    string word = token;
                    // Check if the word a correct according to Vietnamese single words dictionary 
                    if (hunspell.Spell(word))
                    {
                        result += (word + Constants.WhiteSpace);
                    }
                    else
                    {
                        resultList[0] = Constants.False;

                        // Find relative Vietnamese diacritic characters
                        List<char> relativeDiacraticChars = TakeRelativeDiacriticChars(word);

                        // Load list of suggestion words in dictionary
                        List<string> suggestionsListInDictionary = hunspell.Suggest(word);

                        // Create list of best suggestion words
                        List<string> bestSuggestionList = new List<string>();

                        if (suggestionsListInDictionary.Count > 1)
                        {
                            bestSuggestionList =
                                FindBestSuggestionWords(suggestionsListInDictionary, relativeDiacraticChars);
                        }

                        if ((bestSuggestionList.Count == 0))
                        {
                            bestSuggestionList = suggestionsListInDictionary;
                        }

                        // Find best value for each best suggesion word
                        string bestSuggestion = string.Empty;
                        int max = -1;

                        foreach (string suggestion in bestSuggestionList)
                        {
                            // Case of best suggestion contains in the input word
                            if (IsPatternMatched(suggestion, word) || IsPatternMatched(word, suggestion))
                            {
                                bestSuggestion = suggestion;
                                break;
                            }

                            // Process best suggestion word
                            int temp = 0;
                            int shorterLength = (suggestion.Length < word.Length ? suggestion.Length : word.Length);
                            for (int i = 0; i < shorterLength; i++)
                            {
                                if (word[i] == suggestion[i])
                                {
                                    temp += i;
                                }
                            }

                            // Assign best suggestion word
                            if (temp > max)
                            {
                                bestSuggestion = suggestion;
                                max = temp;
                            }
                        }

                        // Concate result string
                        result += (bestSuggestion + Constants.WhiteSpace);
                    }
                }

                // Add suggest string to result list
                resultList[1] = result.Trim();
                // Return result
                return resultList;
            }
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

        /// <summary>
        /// Remove Vietnamese diacritic marks
        /// </summary>
        /// <param name="inputStr">Input Vietnamse sentence with diacritics marks</param>
        /// <returns>Non-diacritic string</returns>
        public string RemoveDiacriticMarks(string inputStr)
        {
            Regex v_reg_regex = new Regex(Constants.CheckDiacriticalMark);
            string v_str_FormD = inputStr.Normalize(NormalizationForm.FormD);
            return v_reg_regex.Replace(v_str_FormD, String.Empty).
                Replace(Constants.LatinSmallLetterDWithStroke, 'd').
                Replace(Constants.LatinCapitalLetterDWithStroke, 'D');
        } 
    }
}
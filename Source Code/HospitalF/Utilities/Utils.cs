using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace HospitalF.Utilities
{
    /// <summary>
    /// Class define public methods that being used in project
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// Remove special character in a string and replace with white space
        /// ',' '.' ';' ''' '"' '[' ']' '{' '}' '\' '/' '<' '>' '-' '_' '+' '=' '*'
        /// </summary>
        /// <param name="inputStr">Input string</param>
        /// <returns>String that does not contain special character</returns>
        public static string RemoveSpecialCharacter(string inputStr)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char character in inputStr)
            {
                if ((character >= '0' && character <= '9') ||
                    (character >= 'A' && character <= 'Z') ||
                    (character >= 'a' && character <= 'z') ||
                    character == ' ')
                {
                    sb.Append(character);
                }
            }
            return sb.ToString();
        }
    }
}
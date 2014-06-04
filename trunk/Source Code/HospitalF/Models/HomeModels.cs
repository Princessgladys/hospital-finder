using System;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using HospitalF.Entities;
using HospitalF.Constant;
using HospitalF.Models;

namespace HospitalF.Models
{
    public class HomeModels
    {
        /// <summary>
        /// Get/Set value for property SearchValue
        /// </summary>
        [Display(Name = Constants.SearchValue)]
        [StringLength(100, ErrorMessage = ErrorMessage.CEM003)]
        public string SearchValue { get; set; }

        /// <summary>
        /// Get/Set value for property CityID
        /// </summary>
        [Display(Name = Constants.City)]
        [Required(ErrorMessage = ErrorMessage.CEM011)]
        public int CityID { get; set; }

        /// <summary>
        /// Get/Set value for property CityID
        /// </summary>
        public string CityName { get; set; }

        /// <summary>
        /// Get/Set value for property DistrictID
        /// </summary>
        [Display(Name = Constants.District)]
        [Required(ErrorMessage = ErrorMessage.CEM011)]
        public int DistrictID { get; set; }

        /// <summary>
        /// Get/Set value for property DistrictName
        /// </summary>
        public string DistrictName { get; set; }

        /// <summary>
        /// Get/Set value for property SpecialityID
        /// </summary>
        [Display(Name = Constants.Speciality)]
        public int SpecialityID { get; set; }

        /// <summary>
        /// Get/Set value for property SpecialityName
        /// </summary>
        public string SpecialityName { get; set; }

        /// <summary>
        /// Get/Set value for property DiseaseID
        /// </summary>
        [Display(Name = Constants.Disease)]
        public int DiseaseID { get; set; }

        /// <summary>
        /// Get/Set value for property DiseaseName
        /// </summary>
        [Display(Name = Constants.Disease)]
        [StringLength(64, ErrorMessage = ErrorMessage.CEM003)]
        public string DiseaseName { get; set; }

        /// <summary>
        /// Get/Set value for property CurrentLocation
        /// </summary>
        [Display(Name = Constants.CurrentLocation)]
        public string CurrentLocation { get; set; }

        /// <summary>
        /// Get/Set value for property AppointedLocation
        /// </summary>
        [Display(Name = Constants.AppointedAddress)]
        [StringLength(128, ErrorMessage = ErrorMessage.CEM003)]
        public string AppointedAddress { get; set; }

        /// <summary>
        /// Split words in a string to every token
        /// </summary>
        /// <param name="inputStr">Input String</param>
        /// <returns>List[string] that contains list of tokens</returns>
        private List<string> StringTokenizer(string inputStr)
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
        /// Concatenate tokens in a list
        /// </summary>
        /// <param name="tokenList">Input token list</param>
        /// <param name="begin">Begin index</param>
        /// <param name="end">End index</param>
        /// <returns>String of concatenated tokens</returns>
        private string ConcatTokens(List<string> tokenList, int beginIndex, int endIndex)
        {
            string result = string.Empty;
            for (int n = beginIndex; n <= endIndex; n++)
            {
                result += " " + tokenList[n];
            }
            // Remove leading white space and return value
            return result.Trim();
        }

        /// <summary>
        /// Load word dictionary in database
        /// </summary>
        /// <returns>List[string] of words</returns>
        private async Task<List<string>> LoadWordDictionaryAsync()
        {
            // Create an instance of Linq database
            LinqDBDataContext data = new LinqDBDataContext();
            // Return list of dictionary words
            try
            {
                return await Task.Run(() =>
                    (from w in data.WordDictionaries
                     where w.Priority == 1
                     select w.Word).ToList());
            }
            catch (Exception)
            {
                Console.WriteLine(ErrorMessage.SEM001);
                return null;
            }
        }

        /// <summary>
        /// Load a list of locations in database
        /// </summary>
        /// <returns>List[HomeModels] that contains a list of locations</returns>
        private async Task<List<HomeModels>> LoadLocationDictionaryAsync()
        {
            // Create an instance of Linq database
            LinqDBDataContext data = new LinqDBDataContext();
            // Return list of dictionary words
            try
            {
                return await Task.Run(() =>
                    (from l in data.Cities
                     from d in data.Districts
                     select new HomeModels
                    {
                        CityID = l.City_ID,
                        CityName = l.City_Name,
                        DistrictID = d.District_ID,
                        DistrictName = d.District_Name
                    }).ToList());
            }
            catch (Exception)
            {
                Console.WriteLine(ErrorMessage.SEM001);
                return null;
            }
        }

        /// <summary>
        /// Check if input token is a relation word
        /// </summary>
        /// <param name="inputToken">Input token</param>
        /// <param name="dictionary">Word dictionary</param>
        /// <returns>Boolean indicating if a token is a relation word</returns>
        private bool IsValidRelationWord(string inputStr, List<string> dictionary)
        {
            // Check every word in dictionary to see in the input token is match
            foreach (string word in dictionary)
            {
                // Find matching result
                if (word.Equals(inputStr))
                {
                    return true;
                }
            }
            // Return false as default
            return false;
        }

        /// <summary>
        /// Check if input token is a relation word
        /// </summary>
        /// <param name="inputToken">Input token</param>
        /// <param name="dictionary">Location dictionary</param>
        /// <returns>Boolean indicating if a token is a location phrase</returns>
        private bool IsValidWherePhrase(string inputStr, List<HomeModels> dictionary)
        {
            // Check every word in dictionary to see in the input token is match
            foreach (HomeModels model in dictionary)
            {
                // Find matching result for cities
                if (model.CityName.Equals(inputStr))
                {
                    return true;
                }
                else
                {
                    // Find matching reuslt for district
                    if (model.DistrictName.Equals(inputStr))
                    {
                        return true;
                    }
                }
            }
            // Return false as default
            return false;
        }

        /// <summary>
        /// Geography Information Retrieval (WHAT - REL - WHERE)
        /// Analyze input query to 3 different phases of What - Relation - Where
        /// </summary>
        /// <param name="inputQuery">inputQuery</param>
        public async Task GIRQueryAnalyzerAsync(string inputQuery)
        {
            string what = string.Empty;
            string tempWhat = string.Empty;
            string relation = string.Empty;
            string tempRelation = string.Empty;
            string where = string.Empty;
            string tempWhere = string.Empty;

            // Create a list of tokens
            List<string> tokens = StringTokenizer(inputQuery);
            int sizeOfTokens = tokens.Count();

            // Load word dictionary
            List<string> wordDic = await LoadWordDictionaryAsync();
            // Load location dictionary
            List<HomeModels> locationDic = await LoadLocationDictionaryAsync();

            what = ConcatTokens(tokens, 0, sizeOfTokens - 1);

            // Check every token in the list
            for (int n = 0; n < sizeOfTokens; n++)
            {
                for (int i = n; i < sizeOfTokens; i++)
                {
                    tempRelation = ConcatTokens(tokens, n, i);
                    if (IsValidRelationWord(tempRelation, wordDic))
                    {
                        tempWhat = inputQuery.Substring(0, inputQuery.IndexOf(tempRelation));
                        where = ConcatTokens(tokens, i + 1, sizeOfTokens - 1);
                        if (IsValidWherePhrase(where, locationDic))
                        {
                            relation = tempRelation;
                            // Assign n value again to break the outside loop
                            n = sizeOfTokens;
                            break;
                        }
                        relation = tempRelation;
                    }
                }
            }

            // Handle query in case of input string is not well-formed
            // with What - Relation - Where condition
            if (string.IsNullOrEmpty(relation) && string.IsNullOrEmpty(where))
            {
                int i = inputQuery.IndexOf("HCM");
                if (i >= 0)
                {
                    tempWhat = inputQuery.Substring(0, i);
                    where = inputQuery.Substring(i);
                    relation = "ở";
                }
            }

            if (!IsValidWherePhrase(where, locationDic))
            {
                tempWhat = what;
                relation = string.Empty;
                where = string.Empty;

            }

            if (!string.IsNullOrEmpty(tempWhat))
            {
                what = tempWhat;
            }
        }
    }
}
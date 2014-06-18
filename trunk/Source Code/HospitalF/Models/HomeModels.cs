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
using HospitalF.Utilities;

namespace HospitalF.Models
{
    public class HomeModels
    {
        #region Properties

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
        /// Get/Set value for property HospitalName
        /// </summary>
        [Display(Name = Constants.HospitalAddress)]
        public string HospitalName { get; set; }

        /// <summary>
        /// Get/Set value for property Coordinate
        /// </summary>
        [Display(Name = Constants.Coordinate)]
        public string Coordinate { get; set; }

        /// <summary>
        /// Get/Set value for property HospitalAddres
        /// </summary>
        [Display(Name = Constants.HospitalAddress)]
        public string HospitalAddres { get; set; }

        #endregion

        #region GIR query analyzer

        /// <summary>
        /// Concate tokens in a list
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
        private async Task<List<string>> LoadRelationWordDictionaryAsync()
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
        /// Load a list of citites in database
        /// </summary>
        /// <returns>List[HomeModels] that contains a list of cities</returns>
        private async Task<List<HomeModels>> LoadCityAsync()
        {
            // Create an instance of Linq database
            LinqDBDataContext data = new LinqDBDataContext();
            // Return list of dictionary words
            try
            {
                return await Task.Run(() =>
                    (from c in data.Cities
                     select new HomeModels
                    {
                        CityID = c.City_ID,
                        CityName = c.City_Name,
                    }).OrderBy(c => c.CityName).ToList());
            }
            catch (Exception)
            {
                Console.WriteLine(ErrorMessage.SEM001);
                return null;
            }
        }

        /// <summary>
        /// Load a list of districts in database
        /// </summary>
        /// <returns>List[HomeModels] that contains a list of districts</returns>
        private async Task<List<HomeModels>> LoadDistrictAsync()
        {
            // Create an instance of Linq database
            LinqDBDataContext data = new LinqDBDataContext();
            // Return list of dictionary words
            try
            {
                return await Task.Run(() =>
                    (from d in data.Districts
                     select new HomeModels
                     {
                         DistrictID = d.District_ID,
                         DistrictName = d.Type + Constants.WhiteSpace + d.District_Name,
                     }).OrderBy(d => d.DistrictName).ToList());
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
                if (word.ToLower().Equals(inputStr))
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
        /// <param name="cityList">List of cities</param>
        /// <param name="districtList">List of districts</param>
        /// <returns>Boolean indicating if a token is a location phrase</returns>
        private bool IsValidWherePhrase(string inputStr,
            List<HomeModels> cityList, List<HomeModels> districtList)
        {
            bool isCityFount = false;               // Indicate if city is found
            bool isDistrictFount = false;           // Indicate if district is found

            // Check every word in city list to see if the input token is match
            foreach (HomeModels model in cityList)
            {
                // Find matching result for cities
                if (!string.IsNullOrEmpty(model.CityName) &&
                    StringUtil.IsPatternMatched(inputStr, model.CityName.ToLower()))
                {
                    this.CityID = model.CityID;
                    this.CityName = model.CityName;
                    isCityFount = true;
                    break;
                }
            }

            // Check every word in district list to see if the input token is match
            foreach (HomeModels model in districtList)
            {
                // Find matching result for districts
                if (!string.IsNullOrEmpty(model.DistrictName) &&
                    StringUtil.IsPatternMatched(inputStr, model.DistrictName.ToLower()))
                {
                    this.DistrictID = model.DistrictID;
                    this.DistrictName = model.DistrictName;
                    isDistrictFount = true;
                    break;
                }
            }

            // Check if any city or district is found
            if (isCityFount || isDistrictFount)
            {
                return true;
            }

            // Return false as default
            return false;
        }

        /// <summary>
        /// Take first location in input query string
        /// </summary>
        /// <param name="queryStr">Query string</param>
        /// <param name="cityList">List of cities</param>
        /// <param name="districtList">List of districts</param>
        /// <returns>First location in Where phrase</returns>
        private string TakeFirstLocationInQueryString(string queryStr,
            List<HomeModels> cityList, List<HomeModels> districtList)
        {
            int cityPosition = 0;                   // City index in Where phrase
            int tempCityIndex = 0;                  // Temp city index
            bool isCityFound = false;               // Indicate if a city is found
            string tempCity = string.Empty;         // Indicate temporary city value
            int districtPosition = 0;               // District index in Where phrase
            int tempDistrictIndex = 0;              // Temp district index
            bool isDistrictFound = false;           // Indicate if a district is found
            string tempDistrict = string.Empty;     // Indicate temporary district value

            // Check every word in city list to see in the input token is match
            foreach (HomeModels model in cityList)
            {
                // Find matching result for cities
                if (!string.IsNullOrEmpty(model.CityName))
                {
                    tempCityIndex = StringUtil.TakeMatchedStringPosition(
                        queryStr, model.CityName.ToLower());
                    if (tempCityIndex != Constants.DefaultMatchingValue)
                    {
                        cityPosition = tempCityIndex;
                        this.CityID = model.CityID;
                        this.CityName = model.CityName;
                        tempCity = model.CityName;
                        isCityFound = true;
                        break;
                    }
                }
            }

            // Check every word in district list to see in the input token is match
            foreach (HomeModels model in districtList)
            {
                if (!string.IsNullOrEmpty(model.DistrictName))
                {
                    tempDistrictIndex = StringUtil.TakeMatchedStringPosition(
                        queryStr, model.DistrictName.ToLower());
                    if (tempDistrictIndex != Constants.DefaultMatchingValue)
                    {
                        districtPosition = tempDistrictIndex;
                        this.DistrictID = model.DistrictID;
                        this.DistrictName = model.DistrictName;
                        tempDistrict = model.DistrictName;
                        isDistrictFound = true;
                        break;
                    }
                }
            }

            // Check if there is any city or district are found
            if (isCityFound || isDistrictFound)
            {
                // Check to see whether City appears first or District appears First
                if (cityPosition < districtPosition)
                {
                    return tempCity;
                }

                if (districtPosition < cityPosition)
                {
                    return tempDistrict;
                }
            }

            // Return null as default
            return null;
        }

        /// <summary>
        /// Load all specialities in database
        /// </summary>
        /// <returns></returns>
        private async Task<List<HomeModels>> LoadSpecialityAsync()
        {
            // Create an instance of Linq database
            LinqDBDataContext data = new LinqDBDataContext();
            // Return list of specialities
            try
            {
                return await Task.Run(() =>
                    (from s in data.Specialities
                     select new HomeModels
                    {
                        SpecialityID = s.Speciality_ID,
                        SpecialityName = s.Speciality_Name
                    }).ToList());
            }
            catch (Exception)
            {
                Console.WriteLine(ErrorMessage.SEM001);
                return null;
            }
        }

        /// <summary>
        /// Load all diseases in database
        /// </summary>
        /// <returns></returns>
        private async Task<List<HomeModels>> LoadDiseaseAsync()
        {
            // Create an instance of Linq database
            LinqDBDataContext data = new LinqDBDataContext();
            // Return list of diseases
            try
            {
                return await Task.Run(() =>
                    (from d in data.Diseases
                     select new HomeModels
                     {
                         DiseaseID = d.Disease_ID,
                         DiseaseName = d.Disease_Name
                     }).ToList());
            }
            catch (Exception)
            {
                Console.WriteLine(ErrorMessage.SEM001);
                return null;
            }
        }

        /// <summary>
        /// Handle well-formed What phrase
        /// </summary>
        /// <param name="whatPhrase">Well-formed What phrase</param>
        /// <param name="specialityList">Speciality List</param>
        /// <param name="diseaseList">Disease List</param>
        private void HandleWellFormedWhatPhrase(string whatPhrase,
            List<HomeModels> specialityList, List<HomeModels> diseaseList)
        {
            // Check every word in speciality list to see in the input token is match
            foreach (HomeModels model in specialityList)
            {
                // Find matching result for speciality
                if (!string.IsNullOrEmpty(model.SpecialityName) &&
                    StringUtil.IsPatternMatched(whatPhrase, model.SpecialityName.ToLower()))
                {
                    this.SpecialityID = model.SpecialityID;
                    this.SpecialityName = model.SpecialityName;
                    break;
                }
            }

            // Check every word in disease list to see in the input token is match
            foreach (HomeModels model in diseaseList)
            {
                // Find matching reuslt for disease
                if (!string.IsNullOrEmpty(model.DiseaseName) &&
                    StringUtil.IsPatternMatched(whatPhrase, model.DiseaseName.ToLower()))
                {
                    this.DiseaseID = model.DiseaseID;
                    this.DiseaseName = model.DiseaseName;
                    break;
                }
            }
        }

        /// <summary>
        /// Geography Information Retrieval (WHAT - REL - WHERE)
        /// Analyze input query to 3 different phases of What - Relation - Where
        /// </summary>
        /// <param name="inputQuery">inputQuery</param>
        public async Task<string> GIRQueryAnalyzerAsync(string inputQuery)
        {
            string what = string.Empty;             // What phrase
            string tempWhat = string.Empty;         // Temporary value for What phrase
            string relation = string.Empty;         // Relation word
            string tempRelation = string.Empty;     // Temporary value for Relation word 
            string where = string.Empty;            // Where phrase
            string tempWhere = string.Empty;        // Temporary value for Where phrase
            bool isComplete = false;                // Indicate if the process is complete or not

            // Create a list of tokens
            List<string> tokens = StringUtil.StringTokenizer(inputQuery);
            int sizeOfTokens = tokens.Count();

            // Load relation word dictionary
            List<string> wordDic = await LoadRelationWordDictionaryAsync();
            // Load list of cities
            List<HomeModels> cityList = await LoadCityAsync();
            // Load list of districts
            List<HomeModels> districtList = await LoadDistrictAsync();

            // Check if the lists are load successfully
            if ((wordDic == null) &&
                (cityList == null) &&
                (districtList == null))
            {
                return ErrorMessage.CEM001;
            }

            what = ConcatTokens(tokens, 0, sizeOfTokens - 1);

            // Check every token in the list
            for (int n = 0; n < sizeOfTokens; n++)
            {
                for (int i = n; i < sizeOfTokens; i++)
                {
                    // Concate tokens to create a string with the original starting
                    // word is the first token, and this word is shift to left one index every n loop
                    // if relaton word is not found.
                    // New tokens is add to original token every i loop to check for valid relation word
                    tempRelation = ConcatTokens(tokens, n, i);

                    // Check if token string is matched with relation word in database
                    if (IsValidRelationWord(tempRelation, wordDic))
                    {
                        // If it matches, assign temporary What phrase value with 
                        // the value of leading words before Relation word
                        tempWhat = inputQuery.Substring(0, inputQuery.IndexOf(tempRelation) - 1);

                        // Assign Where phrase value with the value of trailing
                        // words after Relation word
                        where = ConcatTokens(tokens, i + 1, sizeOfTokens - 1);

                        // Check if Where phrase is matched with locaitons in database
                        // and handle Where phrase to locate exactly search locations
                        if (IsValidWherePhrase(where, cityList, districtList))
                        {
                            // Change status of isComplete varialbe
                            isComplete = true;
                            // If matches, assign Relation word is with the value
                            // of temporary relation 
                            relation = tempRelation;
                            // Assign n value again to break the outside loop
                            n = sizeOfTokens;
                            break;
                        }

                        // Assign Relation word with the value of temporary relation
                        // every i loop
                        relation = tempRelation;
                    }
                }
            }

            // Check if the process is completely finished
            if (!isComplete)
            {
                // Handle query in case of input string is not well-formed
                // with Relation word and Where phrase is not found.
                // Auto check Where phrase with the first location value in input query,
                // if Where phrase is valid, auto assign Relation word with default value.
                if (string.IsNullOrEmpty(relation) && string.IsNullOrEmpty(where))
                {
                    int i = 0;

                    // Take first location in input query string
                    // and handle Where phrase (if any) to locate exactly search locations
                    string firstLocation = TakeFirstLocationInQueryString(inputQuery, cityList, districtList);
                    if (!string.IsNullOrEmpty(firstLocation))
                    {
                        i = inputQuery.IndexOf(firstLocation.ToLower());
                        tempWhat = inputQuery.Substring(0, i);
                        where = inputQuery.Substring(i);
                    }
                    else
                    {
                        tempWhat = what;
                        relation = string.Empty;
                        where = string.Empty;
                    }
                }
            }

            // Make sure What phrase have the value.
            // At the worst case if the input query is not well-formed,
            // assign What phrase with the input query
            if (!string.IsNullOrEmpty(tempWhat))
            {
                what = tempWhat;
            }

            // Handle What phrase
            // Load relation word dictionary
            List<HomeModels> specialityList = await LoadSpecialityAsync();
            // Load location dictionary
            List<HomeModels> diseaseList = await LoadDiseaseAsync();

            // Check if the lists are load successfully
            if ((specialityList != null) && (diseaseList != null))
            {
                // Handle well-formed What phrase
                HandleWellFormedWhatPhrase(what, specialityList, diseaseList);
            }
            else
            {
                return ErrorMessage.CEM001;
            }

            string a = string.Format("[{0}][{1}][{2}]", what, relation, where);
            int cityId = this.CityID;
            string cityName = this.CityName;
            int districtId = this.DistrictID;
            string districtName = this.DistrictName;
            int specialityId = this.SpecialityID;
            string speacialityName = this.SpecialityName;
            int diseaseId = this.DiseaseID;
            string diseaseName = this.DiseaseName;

            // Return value of What - Relation - Where
            return string.Format("{0}-{1}-{2}-{3}", cityId, districtId, specialityId, diseaseId);
        }

        #endregion

        #region Search Hospital

        /// <summary>
        ///  Search hospitals in database
        /// </summary>
        /// <param name="inputValue"></param>
        /// <returns>List[HomeModels]</returns>
        public async Task<List<HospitalEntity>> SearchHospital()
        {
            // Create an instance of Linq database
            LinqDBDataContext data = new LinqDBDataContext();

            int cityId = this.CityID;
            int districtId = this.DistrictID;
            int specialityId = this.SpecialityID;
            int diseaseId = this.DiseaseID;

            var result = await Task.Run(() =>
                data.SP_SEARCH_HOSPITAL(cityId, districtId, specialityId, diseaseId).ToList());


            List<HospitalEntity> list = new List<HospitalEntity>();

            foreach (SP_SEARCH_HOSPITALResult re in result)
            {
                HospitalEntity hospital = new HospitalEntity();
                hospital.HospitalName = re.Hospital_Name;
                hospital.Address = re.Address;
                list.Add(hospital);
            }

            return list;
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using HospitalF.Models;

namespace HospitalF.Utilities
{
    /// <summary>
    /// Class define methods to handle dictionary
    /// </summary>
    public class DictionaryUtil
    {
        /// <summary>
        /// Load word dictionary in database
        /// </summary>
        /// <returns>List[string] of words</returns>
        public static async Task<List<string>> LoadRelationWordAsync()
        {
            // Return list of dictionary words
            using (LinqDBDataContext data = new LinqDBDataContext())
            {
                return await Task.Run(() =>
                    (from w in data.WordDictionaries
                     where w.Priority == 1
                     select w.Word).ToList());
            }
        }
    }
}
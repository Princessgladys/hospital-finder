using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HospitalF.Entities
{
    public class SentenceDictionaryEntity
    {
        #region SentenceDictionaryEntity property
        /// <summary>
        /// Property for Sentence_ID attribute
        /// </summary>
        public int Sentence_ID { get; set; }

        /// <summary>
        /// Property for Sentence attribute
        /// </summary>
        public string Sentence { get; set; }

        /// <summary>
        /// Property for Search_Date attribute
        /// </summary>
        public DateTime? Search_Date { get; set; }

        /// <summary>
        /// Property for Result_Count attribute
        /// </summary>
        public int? Result_Count { get; set; }
        #endregion
    }
}
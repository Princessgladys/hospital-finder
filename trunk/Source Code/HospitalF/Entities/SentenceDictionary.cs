using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for SentenceDictionary table
    /// <summary>
    public class SentenceDictionaryEntity
    {
        #region SentenceDictionary Properties

        /// <summary>
        /// Property for Sentence_ID attribute
        /// <summary>
        public int SentenceID { get; set; }

        /// <summary>
        /// Property for Sentence attribute
        /// <summary>
        public string Sentence { get; set; }

        /// <summary>
        /// Property for Search_Date attribute
        /// <summary>
        public DateTime SearchDate { get; set; }

        #endregion
    }
}

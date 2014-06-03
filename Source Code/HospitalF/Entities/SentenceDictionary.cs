﻿using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for SentenceDictionary table
    /// <summary>
    public class SentenceDictionary
    {
        #region SentenceDictionary Properties

        /// <summary>
        /// Property for Sentence_ID attribute
        /// <summary>
        public int SentenceID;

        /// <summary>
        /// Property for Sentence attribute
        /// <summary>
        public string Sentence;

        /// <summary>
        /// Property for Search_Date attribute
        /// <summary>
        public DateTime? SearchDate;

        #endregion
    }
}

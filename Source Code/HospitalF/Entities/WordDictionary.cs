using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for WordDictionary table
    /// <summary>
    public class WordDictionaryEntity
    {
        #region WordDictionary Properties

        /// <summary>
        /// Property for Word_ID attribute
        /// <summary>
        public int WordID;

        /// <summary>
        /// Property for Word attribute
        /// <summary>
        public string Word;

        /// <summary>
        /// Property for Priority attribute
        /// <summary>
        public int? Priority;

        #endregion
    }
}

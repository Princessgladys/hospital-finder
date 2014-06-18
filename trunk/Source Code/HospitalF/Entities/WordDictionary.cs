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
        public int WordID { get; set; }

        /// <summary>
        /// Property for Word attribute
        /// <summary>
        public string Word { get; set; }

        /// <summary>
        /// Property for Priority attribute
        /// <summary>
        public int Priority { get; set; }

        #endregion
    }
}

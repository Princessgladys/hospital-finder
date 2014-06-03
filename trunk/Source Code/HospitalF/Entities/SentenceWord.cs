using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for Sentence_Word table
    /// <summary>
    public class SentenceWord
    {
        #region Sentence_Word Properties

        /// <summary>
        /// Property for Sentence_ID attribute
        /// <summary>
        public int SentenceID;

        /// <summary>
        /// Property for Word_ID attribute
        /// <summary>
        public int WordID;

        /// <summary>
        /// Property for Added_Date attribute
        /// <summary>
        public DateTime? AddedDate;

        #endregion
    }
}

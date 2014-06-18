using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for Sentence_Word table
    /// <summary>
    public class SentenceWordEntity
    {
        #region Sentence_Word Properties

        /// <summary>
        /// Property for Sentence_ID attribute
        /// <summary>
        public int SentenceID { get; set; }

        /// <summary>
        /// Property for Word_ID attribute
        /// <summary>
        public int WordID { get; set; }

        /// <summary>
        /// Property for Added_Date attribute
        /// <summary>
        public DateTime AddedDate { get; set; }

        #endregion
    }
}

using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for Rating table
    /// <summary>
    public class RatingEntity
    {
        #region Rating Properties

        /// <summary>
        /// Property for Rating_ID attribute
        /// <summary>
        public int RatingID { get; set; }

        /// <summary>
        /// Property for Score attribute
        /// <summary>
        public int Score { get; set; }

        /// <summary>
        /// Property for Hospital_ID attribute
        /// <summary>
        public int HospitalID { get; set; }

        /// <summary>
        /// Property for Created_Person attribute
        /// <summary>
        public int CreatedPerson { get; set; }

        #endregion
    }
}

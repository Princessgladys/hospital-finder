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
        public int RatingID;

        /// <summary>
        /// Property for Score attribute
        /// <summary>
        public int Score;

        /// <summary>
        /// Property for Hospital_ID attribute
        /// <summary>
        public int HospitalID;

        /// <summary>
        /// Property for Created_Person attribute
        /// <summary>
        public int CreatedPerson;

        #endregion
    }
}

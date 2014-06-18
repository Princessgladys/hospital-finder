using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for Disease table
    /// <summary>
    public class DiseaseEntity
    {
        #region Disease Properties

        /// <summary>
        /// Property for Disease_ID attribute
        /// <summary>
        public int DiseaseID { get; set; }

        /// <summary>
        /// Property for Disease_Name attribute
        /// <summary>
        public string DiseaseName { get; set; }

        #endregion
    }
}

using System;

namespace HospitalF.Entities
{
    /// <summary>
    /// Class defines properties for Photo table
    /// <summary>
    public class PhotoEntity
    {
        #region Photo Properties

        /// <summary>
        /// Property for Photo_ID attribute
        /// <summary>
        public int PhotoID;

        /// <summary>
        /// Property for File_Path attribute
        /// <summary>
        public string FilePath;

        /// <summary>
        /// Property for Caption attribute
        /// <summary>
        public string Caption;

        /// <summary>
        /// Property for Add_Date attribute
        /// <summary>
        public DateTime? AddDate;

        /// <summary>
        /// Property for Hospital_ID attribute
        /// <summary>
        public int? HospitalID;

        /// <summary>
        /// Property for Uploaded_Person attribute
        /// <summary>
        public int? UploadedPerson;

        /// <summary>
        /// Property for Is_Active attribute
        /// <summary>
        public bool? IsActive;

        #endregion
    }
}

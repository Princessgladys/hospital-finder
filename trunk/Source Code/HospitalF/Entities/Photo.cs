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
        public int PhotoID { get; set; }

        /// <summary>
        /// Property for File_Path attribute
        /// <summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Property for Caption attribute
        /// <summary>
        public string Caption { get; set; }

        /// <summary>
        /// Property for Add_Date attribute
        /// <summary>
        public DateTime AddDate { get; set; }

        /// <summary>
        /// Property for Target_Type attribute
        /// <summary>
        public int TargetType { get; set; }

        /// <summary>
        /// Property for Target_ID attribute
        /// <summary>
        public int TargetID { get; set; }

        /// <summary>
        /// Property for Uploaded_Person attribute
        /// <summary>
        public int UploadedPerson { get; set; }

        /// <summary>
        /// Property for Is_Active attribute
        /// <summary>
        public bool IsActive { get; set; }

        #endregion
    }
}

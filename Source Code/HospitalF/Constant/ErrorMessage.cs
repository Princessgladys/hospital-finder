namespace HospitalF.Constant
{
    /// <summary>
    /// Class define error messages for project
    /// </summary>
    public class ErrorMessage
    {
        #region Client Error Messages

        /// <summary>
        /// Required field is blank
        /// </summary>
        public const string CEM001 = "Xin hãy nhập {0}.";

        /// <summary>
        /// Field's length is not valid
        /// </summary>
        public const string CEM002 = "{0} hợp lệ trong khoảng từ {1} đến {2} ký tự.";

        /// <summary>
        /// Max length is not valid
        /// </summary>
        public const string CEM003 = "{0} không thể vượt quá {1} ký tự.";

        /// <summary>
        /// Field's length (min,max) is not valid
        /// </summary>
        public const string CEM004 = "{0} hợp lệ trong khoảng từ {2} đến {1} ký tự.";

        /// <summary>
        /// Filed's expression is invalid
        /// </summary>
        public const string CEM005 = "{0} không hợp lệ.";

        /// <summary>
        /// Drop down list required field is blank
        /// </summary>
        public const string CEM011 = "Xin hãy chọn {0}";

        #endregion

        #region Server Error Messages

        /// <summary>
        /// System failure
        /// </summary>
        public const string SEM001 = "Đã có lỗi xảy ra trong quá trình xử lý thông tin. Mong quý vị thông cảm và thử lại sau vài phút.";

        #endregion
    }
}
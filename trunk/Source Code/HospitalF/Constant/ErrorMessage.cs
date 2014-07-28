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
        /// Field's value's existed already
        /// </summary>
        public const string CEM006 = "{0} này đã được sử dụng.";

        /// <summary>
        /// Field's value is not existed
        /// </summary>
        public const string CEM007 = "{0} không tồn tại.";

        /// <summary>
        /// Drop down list required field is blank
        /// </summary>
        public const string CEM011 = "Xin hãy chọn {0}.";

        /// <summary>
        /// Min length is not valid
        /// </summary>
        public const string CEM012 = "{0} phải có ít nhất {1} ký tự.";

        /// <summary>
        /// No white space is allow
        /// </summary>
        public const string CEM013 = "{0} không được chứa khoảng trắng.";

        #endregion

        #region Server Error Messages

        /// <summary>
        /// System failure
        /// </summary>
        public const string SEM001 = "Đã có lỗi xảy ra trong quá trình xử lý thông tin. Mong quý vị thông cảm và thử lại sau vài phút.";

        /// <summary>
        /// Update hospital status failed
        /// </summary>
        public const string SEM010 = "Không thay đổi được trạng thái của ";

        #endregion
    }
}
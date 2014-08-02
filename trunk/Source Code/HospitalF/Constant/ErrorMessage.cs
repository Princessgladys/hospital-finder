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
        /// Re-type password is blank
        /// </summary>
        public const string CEM008 = "Mật khẩu mới phải được nhập 2 lần để xác nhận mật khẩu.";

        /// <summary>
        /// Two field's values are duplicated
        /// </summary>
        public const string CEM009 = "{0} phải khác {1}.";

        /// <summary>
        /// Two field's values are different
        /// </summary>
        public const string CEM010 = "{0} không trùng khớp.";

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
        public const string CEM013 = "{0} không được chứa khoảng trắng và ký tự đặc biệt.";

        /// <summary>
        /// Required a specific location of the map
        /// </summary>
        public const string CEM014 = "Xin hãy chọn địa điểm bệnh viện trên bản đồ.";

        /// <summary>
        /// Full address is not valid
        /// </summary>
        public const string CEM015 = "Thiếu thông tin Địa chỉ.";

        /// <summary>
        /// Full address is valid
        /// </summary>
        public const string CEM016 = "Địa chỉ phù hợp.";

        /// <summary>
        /// Address is duplicated with another hospital
        /// </summary>
        public const string CEM017 = "Địa chỉ trùng lắp với bệnh viện khác.";

        /// <summary>
        /// Address on map is located
        /// </summary>
        public const string CEM018 = "Địa điểm đã được xác định.";

        /// <summary>
        /// Person in charged is valid
        /// </summary>
        public const string CEM019 = "Người chịu trách nhiệm hợp lệ.";

        /// <summary>
        /// Person in charged is in charged with another hospital
        /// </summary>
        public const string CEM020 = "Người dùng hiện đang chịu trách nhiệm một bệnh viện khác.";

        /// <summary>
        /// Person in charged is not exist in database
        /// </summary>
        public const string CEM021 = "Người dùng không tồn tại trong hệ thống.";

        /// <summary>
        /// Checking address information in database failed
        /// </summary>
        public const string CEM022 = "Không kiểm tra được thông tin địa chỉ. Xin hãy thử lại sau.";

        /// <summary>
        /// Time value is not valid
        /// </summary>
        public const string CEM023 = "Thời gian không phù hợp.";

        /// <summary>
        /// Value does not allow white space and special character
        /// </summary>
        public const string CEM024 = "Giá trị không được chứa khoảng trắng và ký tự đặc biệt.";

        /// <summary>
        /// Primary email is duplicated with secondary email
        /// </summary>
        public const string CEM025 = "Email phụ không được trùng với Email chính.";

        /// <summary>
        /// Browser does not support identifying location
        /// </summary>
        public const string CEM026 = "Trình duyệt không hỗ trợ xác định vị trí.";

        /// <summary>
        /// System is processing information
        /// </summary>
        public const string CEM027 = "Đang xử lý thông tin.";

        /// <summary>
        /// Location address is missing
        /// </summary>
        public const string CEM028 = "Thiếu thông tin Tên đường.";

        // <summary>
        /// City address is missing
        /// </summary>
        public const string CEM029 = "Thiếu thông tin Tỉnh / thành phố.";

        // <summary>
        /// District address is missing
        /// </summary>
        public const string CEM030 = "Thiếu thông tin Quận / huyện.";

        // <summary>
        /// Ward address is missing
        /// </summary>
        public const string CEM031 = "Thiếu thông tin Phường / xã.";

        // <summary>
        /// Cannot locate address on map
        /// </summary>
        public const string CEM032 = "Không xác định được vị trí, hãy thử lại với địa chỉ khác.";

        // <summary>
        /// Cannot locate address on map
        /// </summary>
        public const string CEM033 = "Không thể tải nhiều hơn {0} hình.";

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
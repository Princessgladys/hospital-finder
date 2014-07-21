using System.Collections.Generic;

namespace HospitalF.Constant
{
    /// <summary>
    /// Class define constant values for project
    /// </summary>
    public class Constants
    {
        #region Controller, View and Methods

        /// <summary>
        /// Name of _HomeLayout
        /// </summary>
        public const string HomeLayout = "_HomeLayout";

        /// <summary>
        /// Name of _AdminLayout
        /// </summary>
        public const string AdmidLayout = "_AdminLayout";

        /// <summary>
        /// Name of _HospitalUserLayout
        /// </summary>
        public const string HospitalUserLayout = "_HospitalUserLayout";

        /// <summary>
        /// Name of SystemFailureHomeAction
        /// </summary>
        public const string SystemFailureHomeAction = "SystemFailureHome";

        /// <summary>
        /// Name of SystemFailureAdminAction
        /// </summary>
        public const string SystemFailureAdminAction = "SystemFailureAdmin";

        /// <summary>
        /// Name of SystemFailureHospitalUserAction
        /// </summary>
        public const string SystemFailureHospitalUserAction = "SystemFailureHospitalUser";

        /// <summary>
        /// Name of HomeController
        /// </summary>
        public const string HomeController = "Home";

        /// <summary>
        /// Name of ErrorController
        /// </summary>
        public const string ErrorController = "Error";

        /// <summary>
        /// Name of AppointmentController
        /// </summary>
        public const string AppointmentController = "Appointment";

        /// <summary>
        /// Name of Hospital controller
        /// </summary>
        public const string HospitalController = "Hospital";

        /// <summary>
        /// Name of AccountController
        /// </summary>
        public const string AccountController = "Account";

        /// <summary>
        /// Name of CreateAppointmentAction
        /// </summary>
        public const string CreateAppointmentAction = "CreateAppointment";

        /// <summary>
        /// Name of GetDoctorBySpecialityAction
        /// </summary>
        public const string GetDoctorBySpecialityAction = "GetDoctorBySpeciality";

        /// <summary>
        /// Name of SearchDoctorAction
        /// </summary>
        public const string SearchDoctorAction = "SearchDoctor";

        /// <summary>
        /// Name of ViewDoctorDetailAction
        /// </summary>
        public const string ViewDoctorDetailAction = "ViewDoctorDetail";

        /// <summary>
        /// Name of GetWorkingDayAction
        /// </summary>
        public const string GetWorkingDayAction = "GetWorkingDay";

        /// <summary>
        /// Name of IndexAction
        /// </summary>
        public const string IndexAction = "Index";

        /// <summary>
        /// Name of SearchResultAction
        /// </summary>
        public const string SearchResultAction = "SearchResult";

        /// <summary>
        /// Name of FilterResultAction
        /// </summary>
        public const string FilterResultAction = "FilterResult";

        /// <summary>
        /// Name of LoginAction
        /// </summary>
        public const string LoginAction = "Login";

        /// <summary>
        /// Name of Logout method in AccountController
        /// </summary>
        public const string LogoutAction = "Logout";

        /// <summary>
        /// Name of ChangePassword method in AccountController
        /// </summary>
        public const string ChangePasswordAction = "ChangePassword";

        /// <summary>
        /// Name of Profile method in AccountController
        /// </summary>
        public const string DisplayProfileAction = "Profile";

        /// <summary>
        /// Name of GetDistrictByCityAction method in HomeController
        /// </summary>
        public const string GetDistrictByCityAction = "GetDistrictByCity";

        /// <summary>
        /// Name of GetWardByDistritctyAction method in HospitalController
        /// </summary>
        public const string GetWardByDistrictAction = "GetWardByDistritct";

        /// <summary>
        /// Name of GetDeseaseBySpecialityAction in HomeController
        /// </summary>
        public const string GetDeseaseBySpecialityAction = "GetDeseaseBySpeciality";

        /// <summary>
        /// Name of LoadSuggestSentenceAction method in HomeController
        /// </summary>
        public const string LoadSuggestSentenceAction = "LoadSuggestSentence";

        /// <summary>
        /// Name of HospitalList method in HospitalController
        /// </summary>
        public const string InitialHospitalListAction = "HospitalList";

        /// <summary>
        /// Name of HospitalList method in HospitalController
        /// </summary>
        public const string DisplayHospitalListAction = "DisplayHospitalList";

        /// <summary>
        /// Name of ChangeHospitalStatusAction method in HospitalController
        /// </summary>
        public const string ChangeHospitalStatusAction = "ChangeHospitalStatus";

        /// <summary>
        /// Name of AddHospital method in HospitalController
        /// </summary>
        public const string AddHospitalAction = "AddHospital";

        #endregion

        #region AccountModel

        #endregion

        #region HomeModel

        #region Property Vietnamese name

        /// <summary>
        /// Vietnamese name of property SearchValue
        /// </summary>
        public const string SearchValue = "Giá trị tìm kiếm";

        /// <summary>
        /// Vietnamese name of property Speciality
        /// </summary>
        public const string Speciality = "Chuyên khoa";

        /// <summary>
        /// Vietnamese name of property InputDisease
        /// </summary>
        public const string Disease = "Triệu chứng";

        /// <summary>
        /// Vietnamese name of property Province
        /// </summary>
        public const string City = "Tỉnh thành";

        /// <summary>
        /// Vietnamese name of property District
        /// </summary>
        public const string District = "Quận / Huyện";

        /// <summary>
        ///  Vietnamese name of property CurrentLocation
        /// </summary>
        public const string CurrentLocation = "Địa điểm hiện tại";

        /// <summary>
        ///  Vietnamese name of property AppointedAddress
        /// </summary>
        public const string AppointedAddress = "Địa điểm chỉ định";

        /// <summary>
        /// Vietnamese name of property HospitalName
        /// </summary>
        public const string HospitalName = "Tên bệnh viện";

        /// <summary>
        /// Vietnamese name of property HospitalAddress
        /// </summary>
        public const string HospitalAddress = "Địa chỉ bệnh viện";

        /// <summary>
        /// Vietnamese name of property Coordinate
        /// </summary>
        public const string Coordinate = "Tọa độ";

        /// <summary>
        /// Vietnamese name of property DoctorNameViet
        /// </summary>
        public const string DoctorNameViet = "Tên bác sĩ";

        #endregion

        #region Property English Name

        // <summary>
        /// English name of property HospitalTypeID
        /// </summary>
        public const string HospitalTypeID = "Type_ID";

        /// <summary>
        /// English name of property HospitalTypeName
        /// </summary>
        public const string HospitalTypeName = "Type_Name";

        /// <summary>
        /// English name of property CityID
        /// </summary>
        public const string CityID = "City_ID";

        /// <summary>
        /// English name of property CityName
        /// </summary>
        public const string CityName = "City_Name";

        /// <summary>
        /// English name of property DistrictID
        /// </summary>
        public const string DistrictID = "District_ID";

        /// <summary>
        /// English name of property DistrictName
        /// </summary>
        public const string DistrictName = "District_Name";

        /// <summary>
        /// English name of property WardID
        /// </summary>
        public const string WardID = "Ward_ID";

        /// <summary>
        /// English name of property WardName
        /// </summary>
        public const string WardName = "Ward_Name";

        /// <summary>
        ///  English name of property SpecialityID
        /// </summary>
        public const string SpecialityID = "Speciality_ID";

        /// <summary>
        /// English name of property SpecialityName
        /// </summary>
        public const string SpecialityName = "Speciality_Name";

        /// <summary>
        /// English name of property DiseaseID
        /// </summary>
        public const string DiseaseID = "Disease_ID";

        /// <summary>
        /// English name of property DiseaseName
        /// </summary>
        public const string DiseaseName = "Disease_Name";

        #endregion

        /// <summary>
        /// Vietnamese message request for City
        /// </summary>
        public const string RequireCity = "-- Xin chọn tỉnh / thành phố --";

        /// <summary>
        /// Vietnamese message request for District
        /// </summary>
        public const string RequireDistrict = "-- Xin chọn quận / huyện --";

        /// <summary>
        /// Vietnamese message request for Ward
        /// </summary>
        public const string RequireWard = "-- Xin chọn phường / xã --";

        /// <summary>
        /// Vietnamese message request for Speciality
        /// </summary>
        public const string RequireSpeciality = "-- Xin chọn chuyên khoa --";

        /// <summary>
        /// Vietnamese message request for Disease
        /// </summary>
        public const string RequireDisease = "-- Xin chọn triệu chứng --";

        /// <summary>
        /// Vietnamese message request for Hospital Type
        /// </summary>
        public const string RequireHospitalType = "-- Xin chọn loại bệnh viện --";

        /// <summary>
        /// Default matching value
        /// </summary>
        public const int DefaultMatchingValue = 9999;

        /// <summary>
        /// Constant for Tìm kiếm cơ bản form
        /// </summary>
        public const string NormalSearchForm = "normal-search-form";

        /// <summary>
        /// Constant for Tìm kiếm theo vị trí form
        /// </summary>
        public const string LocationSearchForm = "location-search-form";

        /// <summary>
        /// Constant for Tìm kiếm nâng cao form
        /// </summary>
        public const string AdvancedSearchForm = "advanced-search-form";

        #endregion

        #region AppointmentModel

        #region Vietnames Name

        /// <summary>
        /// Vietnamese name of property FullName
        /// </summary>
        public const string FullName = "Họ tên";

        /// <summary>
        /// Vietnames name of property Gender
        /// </summary>
        public const string Gender = "Giới tính";

        /// <summary>
        /// Vietnames name of property Birthday
        /// </summary>
        public const string Birthday = "Ngày sinh";

        /// <summary>
        /// Vietnames name of property Email
        /// </summary>
        public const string Email = "Địa chỉ email";

        /// <summary>
        /// Vietnames name of property PhoneNo
        /// </summary>
        public const string PhoneNo = "Số điện thoại";

        /// <summary>
        /// Vietnames name of property Doctor
        /// </summary>
        public const string Doctor = "Bác sĩ";

        /// <summary>
        /// Vietnames name of property Appointment Date
        /// </summary>
        public const string App_Date = "Ngày khám";

        /// <summary>
        /// Vietnames name of property StartTime
        /// </summary>
        public const string StartTime = "Giờ khám";

        #endregion

        #region English Name
        public const string DoctorID = "Doctor_ID";
        public const string DoctorName = "Doctor_Name";
        #endregion

        /// <summary>
        /// Vietnamese message request for Doctor
        /// </summary>
        public const string RequireDoctor = "-- Xin chọn bác sĩ --";

        #endregion

        #region HospitalModel

        #region English name

        /// <summary>
        /// English name of property FacilityName
        /// </summary>
        public const string FacilityName = "Facility_Name";

        /// <summary>
        /// English name of property FacilityID
        /// </summary>
        public const string FacilityID = "Facility_ID";

        /// <summary>
        /// English name of property ServiceName
        /// </summary>
        public const string ServiceName = "Service_Name";

        /// <summary>
        /// English name of property ServiceID
        /// </summary>
        public const string ServiceID = "Service_ID";



        #endregion

        #endregion

        #region Regular Expression

        /// <summary>
        /// Regular expression to find white spaces
        /// </summary>
        public const string FindWhiteSpaceRegex = @"\w+";
        /// <summary>
        /// Regular expression to validate email
        /// </summary>
        public const string EmailRegex = @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|
                                        (([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)";
        /// <summary>
        /// Regular expression to validate Cell phone number
        /// </summary>
        public const string CellPhoneNoRegex = @"^\d{3,5}(-|\s)?\d{3}(-|\s)?\d{3,4}$";

        /// <summary>
        /// Constant to format day with 2 numbers
        /// </summary>
        public const string DayMonthWith2Number = "00";

        /// <summary>
        /// Regular expression to check diacritical marks
        /// </summary>
        public const string CheckDiacriticalMark = @"\\p{IsCombiningDiacriticalMarks}+";

        /// <summary>
        /// Constant for unicode code of letter đ
        /// </summary>
        public const char LatinSmallLetterDWithStroke = '\u0111';

        /// <summary>
        /// Constant for unicode code of letter Đ
        /// </summary>
        public const char LatinCapitalLetterDWithStroke = '\u0110';

        #endregion

        #region Constant value in database

        /// <summary>
        /// Value of AdministratorRoleId
        /// </summary>
        public const int AdministratorRoleId = 1;

        /// <summary>
        /// Value of UserRoleId
        /// </summary>
        public const int UserRoleId = 2;

        /// <summary>
        /// Value of HospitalUserRoleId
        /// </summary>
        public const int HospitalUserRoleId = 3;

        /// <summary>
        /// Value of AdministratorRoleName
        /// </summary>
        public const string AdministratorRoleName = "Administrator";

        /// <summary>
        /// Value of UserRoleName
        /// </summary>
        public const string UserRoleName = "User";

        /// <summary>
        /// Value of HospitalUserRoleName
        /// </summary>
        public const string HospitalUserRoleName = "Hospital User";

        #endregion

        #region Characters

        /// <summary>
        /// Constant for character " "
        /// </summary>
        public const string WhiteSpace = " ";

        /// <summary>
        /// Constant for character "\\"
        /// </summary>
        public const string DoubleReverseSlash = "\\";

        /// <summary>
        /// Constant for character "-"
        /// </summary>
        public const string Minus = "-";

        /// <summary>
        /// Constant for Enter character
        /// </summary>
        public const char Enter = '\n';

        #endregion

        #region Utilities

        /// <summary>
        /// Constant for LoggingPartition key in appSettings section of Web.config
        /// </summary>
        public const string LoggingPartition = "LoggingPartition";

        /// <summary>
        /// Constant for txt extension
        /// </summary>
        public const string TxtFile = ".txt";

        /// <summary>
        /// Constant for opening statement of logging file
        /// </summary>
        public const string OpenLogFileStatement = "File ghi tình trạng lỗi hệ thống ngày {0}\r\n=================================================================\r\n";

        /// <summary>
        /// Constant for writing log file format
        /// </summary>
        public const string LogFileFormat = "{0}\r\nMethod: {1}\r\nError Reason: {2}\r\n";

        /// <summary>
        /// Constant for default logging partition
        /// </summary>
        public const string DefaultLoggingPartition = @"C:\Hospital Finder Logging";

        /// <summary>
        /// Constant for TRUE boolean
        /// </summary>
        public const string True = "true";

        /// <summary>
        /// Constant for FALSE boolean
        /// </summary>
        public const string False = "false";

        /// <summary>
        /// Constant for ViVnAffUrl key in appSettings section of Web.config
        /// </summary>
        public const string ViVnAffUrl = "ViVnAffUrl";

        /// <summary>
        /// Constant for ViVnDicUrl key in appSettings section of Web.config
        /// </summary>
        public const string ViVnDicUrl = "ViVnDicUrl";

        /// <summary>
        /// Constant for Hospital noun in Vietnamese
        /// </summary>
        public const string BệnhViện = "bệnh viện";

        /// <summary>
        /// Constant for Hospital noun in short Vietnamese
        /// </summary>
        public const string Bv = "bv";

        /// <summary>
        /// Constant for every button's name
        /// </summary>
        public const string Button = "button";

        /// <summary>
        /// Constant for diaritic Vietnamese character
        /// </summary>
        public const string DiacriticVietnameseCharacters = @"ăâđêôơưàảãạáằẳẵặắầẩẫậấèẻẽẹéềểễệếìỉĩịíòỏõọóồổỗộốờởỡợớùủũụúừửữựứỳỷỹỵý";

        /// <summary>
        /// Constant for basic diacritic Vietnamsese character
        /// </summary>
        public const string BasicDiacriticVietnameseCharacters = @"ăâđêôơưaaaaaăăăăăâââââeeeeeêêêêêiiiiioooooôôôôôơơơơơuuuuuưưưưưyyyyy";

        /// <summary>
        /// Constant for non-diacrtic Vietnamese characteres
        /// </summary>
        public const string NonDiacriticVietnameseCharacters = "aadeoouaaaaaaaaaaaaaaaeeeeeeeeeeiiiiiooooooooooooooouuuuuuuuuuyyyyy";

        /// <summary>
        /// Constant for search result page size
        /// </summary>
        public const int PageSize = 5;

        /// <summary>
        /// Constant for element 'page' in URL rewriting
        /// </summary>
        public const string PageUrlRewriting = "page";

        /// <summary>
        /// Take relative diacritic Vietnamese characters according to input letter
        /// </summary>
        /// <param name="letter">Input letter (a ă â e ê o ô ơ u ư i y đ)</param>
        /// <returns>char[] that contains relative diacritic Vietnamese characters</returns>
        public static char[] GetDiacriticWords(char letter)
        {
            switch (letter)
            {
                case 'a': return new char[] { 'a', 'á', 'à', 'ả', 'ã', 'ạ' };
                case 'ă': return new char[] { 'ă', 'ắ', 'ằ', 'ẳ', 'ẵ', 'ặ' };
                case 'â': return new char[] { 'â', 'ấ', 'ầ', 'ẩ', 'ẫ', 'ậ' };
                case 'e': return new char[] { 'e', 'é', 'è', 'ẻ', 'ẽ', 'ẹ' };
                case 'ê': return new char[] { 'ê', 'ế', 'ề', 'ể', 'ễ', 'ệ' };
                case 'o': return new char[] { 'o', 'ó', 'ò', 'ỏ', 'õ', 'ọ' };
                case 'ô': return new char[] { 'ô', 'ố', 'ồ', 'ổ', 'ỗ', 'ộ' };
                case 'ơ': return new char[] { 'ơ', 'ớ', 'ờ', 'ở', 'ỡ', 'ợ' };
                case 'u': return new char[] { 'u', 'ú', 'ù', 'ủ', 'ũ', 'ụ' };
                case 'ư': return new char[] { 'ư', 'ứ', 'ừ', 'ử', 'ữ', 'ự' };
                case 'i': return new char[] { 'i', 'í', 'ì', 'ỉ', 'ĩ', 'ị' };
                case 'y': return new char[] { 'y', 'ý', 'ỳ', 'ỷ', 'ỹ', 'ỵ' };
                case 'đ': return new char[] { 'đ' };
                default: return null;
            }
        }

        #endregion
    }
}
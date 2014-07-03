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
        /// Name of SystemFailureHome page
        /// </summary>
        public const string HomeErrorPage = "SystemFailureHome";

        /// <summary>
        /// Name of SystemFailureAdmin page
        /// </summary>
        public const string AdminErrorPage = "SystemFailureAdmin";

        /// <summary>
        /// Name of SystemFailureHospitalUser page
        /// </summary>
        public const string HospitalUserErrorPage = "SystemFailureHospitalUser";

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
        /// Name of CreateAppoinment method
        /// </summary>
        public const string CreateAppointmentMethod = "CreateAppointment";

        /// <summary>
        /// Name of load doctor list method
        /// </summary>
        public const string GetDoctorBySpecialiyMethod = "GetDoctorBySpeciality";

        /// <summary>
        /// Name of load doctor's working day method
        /// </summary>
        public const string GetDoctorWorkingDayMethod = "GetWorkingDay";

        /// <summary>
        /// Name of Index method
        /// </summary>
        public const string IndexMethod = "Index";

        /// <summary>
        /// Name of SearchResult method
        /// </summary>
        public const string SearchResultMethod = "SearchResult";

        /// <summary>
        /// Name of SearchResult method
        /// </summary>
        public const string FilterResultMethod = "FilterResult";

        /// <summary>
        /// Name of GetDistrictByCityMethod method in HomeController
        /// </summary>
        public const string GetDistrictByCityMethod = "GetDistrictByCity";

        /// <summary>
        /// Name of GetDeseaseBySpeciality method in HomeController
        /// </summary>
        public const string GetDeseaseBySpecialityMethod = "GetDeseaseBySpeciality";

        /// <summary>
        /// Name of LoadSuggestSentenceMethod method in HomeController
        /// </summary>
        public const string LoadSuggestSentenceMethod = "LoadSuggestSentence";

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

        #endregion

        #region Property English Name

        // <summary>
        /// English name of property HospitalTypeID
        /// </summary>
        public const string HospitalTypeID = "Type_ID";

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

        // <summary>
        /// English name of property HospitalTypeName
        /// </summary>
        public const string HospitalTypeName = "Type_Name";

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
        /// Vietnamese message request for Speciality
        /// </summary>
        public const string RequireSpeciality = "-- Xin chọn chuyên khoa --";

        /// <summary>
        /// Vietnamese message request for Disease
        /// </summary>
        public const string RequireDisease = "-- Xin chọn triệu chứng --";

        /// <summary>
        /// Default matching value
        /// </summary>
        public const int DefaultMatchingValue = 9999;

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
        public const string LogFileFormat = "{0}\r\nThư mục: {1}\r\nNội dung: {2}\r\n";

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

        #endregion
    }
}
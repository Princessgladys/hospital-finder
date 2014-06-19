using System.Collections.Generic;
namespace HospitalF.Constant
{
    /// <summary>
    /// Class define constant values for project
    /// </summary>
    public class Constants
    {
        #region Controller and View name

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
        /// Name of Index method
        /// </summary>
        public const string IndexMethod = "Index";

        /// <summary>
        /// Name of SearchResult methodd
        /// </summary>
        public const string SearchResultMethod = "SearchResult";

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

        /// <summary>
        /// English name of property CityID
        /// </summary>
        public const string CityID = "CityID";

        /// <summary>
        /// English name of property CityName
        /// </summary>
        public const string CityName = "CityName";

        /// <summary>
        /// English name of property DistrictID
        /// </summary>
        public const string DistrictID = "DistrictID";

        /// <summary>
        /// English name of property DistrictName
        /// </summary>
        public const string DistrictName = "DistrictName";

        /// <summary>
        ///  English name of property SpecialityID
        /// </summary>
        public const string SpecialityID = "SpecialityID";

        /// <summary>
        /// English name of property SpecialityName
        /// </summary>
        public const string SpecialityName = "SpecialityName";

        /// <summary>
        /// English name of property DiseaseID
        /// </summary>
        public const string DiseaseID = "DiseaseID";

        /// <summary>
        /// English name of property DiseaseName
        /// </summary>
        public const string DiseaseName = "DiseaseName";

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

        #region Regular express

        /// <summary>
        /// Regular expression to find white spaces
        /// </summary>
        public const string FindWhiteSpaceRegex = @"\w+";

        #endregion

        #region Constant value in database



        #endregion

        #region Characters

        /// <summary>
        /// Constant for character " "
        /// </summary>
        public const string WhiteSpace = " ";

        #endregion
    }
}
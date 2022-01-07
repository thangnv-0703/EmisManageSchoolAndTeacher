using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Exam.Core.Entities
{
    public class Employee
    {
        #region Constructor
        public Employee()
        {

        }
        #endregion

        #region Properties
        /// <summary>
        /// Khóa chính - id nhân viên
        /// </summary>
        public Guid EmployeeId { get; set; }
        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [MISARequired("Mã giáo viên không được phép để trống")]
        public string EmployeeCode { get; set; }
        /// <summary>
        /// Họ tên
        /// </summary>
        [MISARequired("Họ tên giáo viên không được phép để trống")]
        public string FullName { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [MISAValidEmail("Email không đúng định dạng")]
        public string Email { get; set; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        [MISAValidPhoneNumber("Số điện thoại không đúng định dạng")]
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Id phòng ban
        /// </summary>
        public Guid DepartmentId { get; set; }
        /// <summary>
        /// Ngày nghỉ việc
        /// </summary>
        public DateTime? EndDate { get; set; }
        /// <summary>
        /// Tình trạng công việc
        /// </summary>
        public int? WorkStatus { get; set; }
        /// <summary>
        /// Đào tạo quản lý thiết bị
        /// </summary>
        public int ManageService { get; set; }
        /// <summary>
        /// Danh sách môn giảng dạy
        /// </summary>
        public List<Guid> SubjectList { get; set; }
        /// <summary>
        /// Danh sách kho phòng quản lý
        /// </summary>
        public List<Guid> RoomList { get; set; }
        /// <summary>
        /// Tên phòng ban
        /// </summary>
        public string DepartmentName { get; set; }
        /// <summary>
        /// Xâu môn học giảng dạy
        /// </summary>
        public string SubjectListString { get; set; }
        /// <summary>
        /// Xâu kho phòng quản lý
        /// </summary>
        public string RoomListString { get; set; }
        #endregion

        #region Other
        /// <summary>
        /// Ngày khởi tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Người khởi tạo
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// Ngày chỉnh sửa
        /// </summary>
        public DateTime? ModifiedDate { get; set; }
        /// <summary>
        /// Người chỉnh sửa
        /// </summary>
        public string ModifiedBy { get; set; }
        #endregion
    }
}

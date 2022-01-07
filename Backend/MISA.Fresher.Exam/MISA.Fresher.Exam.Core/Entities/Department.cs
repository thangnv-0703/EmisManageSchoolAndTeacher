using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Exam.Core.Entities
{
    public class Department
    {
        #region Constructor
        public Department()
        {
                
        }
        #endregion

        #region Properties
        /// <summary>
        /// Id tổ bộ môn
        /// </summary>
        public Guid DepartmentId { get; set; }
        /// <summary>
        /// Tên tổ bộ môn
        /// </summary>
        public string DepartmentName { get; set; }
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Exam.Core.Entities
{
    public class ServiceResult
    {
        #region Constructor
        public ServiceResult()
        {

        }
        #endregion

        #region Properties
        /// <summary>
        /// thông báo cho dev
        /// </summary>
        public string devMsg { get; set; }
        /// <summary>
        /// thông báo cho người dùng
        /// </summary>
        public string userMsg { get; set; }
        /// <summary>
        /// mã lỗi
        /// </summary>
        public string errorCode { get; set; }
        /// <summary>
        /// thông tin thêm
        /// </summary>
        public string moreInfo { get; set; }
        /// <summary>
        /// dữ liệu trả về
        /// </summary>
        public object data { get; set; }
        /// <summary>
        /// kết quả trả về thành công hay thất bại
        /// </summary>
        public bool Success { get; set; } = false;
        #endregion
    }
}

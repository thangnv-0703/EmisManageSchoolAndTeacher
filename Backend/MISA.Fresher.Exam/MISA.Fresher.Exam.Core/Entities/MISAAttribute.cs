using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Exam.Core.Entities
{
    /// <summary>
    /// Class Attribute đánh dấu thông tin bắt buộc nhập
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    class MISARequired : Attribute
    {
        public string ErrorMsg;
        public MISARequired(string errorMsg = null)
        {
            ErrorMsg = errorMsg;
        }
    }

    /// <summary>
    /// Class Attribute đánh dấu kiểm tra email có hợp lệ không
    /// </summary>
    class MISAValidEmail : Attribute
    {
        public string ErrorMsg;
        public MISAValidEmail(string errorMsg = null)
        {
            ErrorMsg = errorMsg;
        }
    }

    /// <summary>
    /// Class Attribute đánh dấu kiểm tra số điện thoại có hợp lệ không
    /// </summary>
    class MISAValidPhoneNumber : Attribute
    {
        public string ErrorMsg;
        public MISAValidPhoneNumber(string errorMsg = null)
        {
            ErrorMsg = errorMsg;
        }
    }
}

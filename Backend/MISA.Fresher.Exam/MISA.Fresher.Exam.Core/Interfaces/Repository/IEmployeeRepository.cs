using MISA.Fresher.Exam.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Exam.Core.Interfaces.Repository
{
    public interface IEmployeeRepository: IBaseRepository<Employee>
    {
        /// <summary>
        /// Kiểm tra id nhân viên có tồn tại trong hệ thống không
        /// </summary>
        /// <param name="EmployeeId">Id nhân viên</param>
        /// <returns>true = đã có, false = chưa có</returns>
        /// Created by: NVTHANG(20/11/2021)
        public bool CheckExistId(Guid EmployeeId);
        /// <summary>
        /// Kiểm tra mã nhân viên có tồn tại trong hệ thống không 
        /// </summary>
        /// <param name="EmployeeCode">Mã nhân viên</param>
        /// <param name="EmployeeId">Id nhân viên</param>
        /// <returns>true = đã có, false = chưa có</returns>
        /// Created by: NVTHANG(20/11/2021)
        public bool CheckDuplicateCode(string EmployeeCode, Guid EmployeeId);
        /// <summary>
        /// search thông tin theo mã hoặc họ tên nhân viên với thông tin về số bản ghi/1 trang và số thứ tự trang
        /// </summary>
        /// <param name="searchText"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns>Danh sách nhân viên sau khi search, tổng số bản ghi, tổng số trang</returns>
        public object GetEmployeePaging(string searchText, Guid? departmentId, int pageSize, int pageIndex);
        /// <summary>
        /// Lấy mã nhân viên mởi bằng cách lấy mã lớn nhất + 1
        /// </summary>
        /// <returns>Mã nhân viên mới</returns>
        public string GetNewEmployeeCode();
        /// <summary>
        /// Xóa nhiều nhân viên theo danh sách id đưa vào
        /// </summary>
        /// <param name="employeeListId">chuỗi chứa id các nhân viên cần xóa</param>
        /// <returns>Só nhân viên đã xóa thành công</returns>
        public int DeleteMultiEmployee(string employeeListId);
        /// <summary>
        /// Lấy danh sách dữ liệu nhân viên để xuất khẩu
        /// </summary>
        /// <returns>danh sách nhân viên</returns>
        public List<Employee> GetAllEmployeeToExport();
    }
}

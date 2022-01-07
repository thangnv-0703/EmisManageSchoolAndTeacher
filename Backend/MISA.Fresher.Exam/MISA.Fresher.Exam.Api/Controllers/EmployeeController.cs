using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Fresher.Exam.Core.Entities;
using MISA.Fresher.Exam.Core.Interfaces.Repository;
using MISA.Fresher.Exam.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MISA.Fresher.Exam.Api.Controllers
{
    public class EmployeeController : MISAController<Employee>
    {
        IEmployeeRepository _employeeRepository;
        IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService, IEmployeeRepository employeeRepository) : base(employeeService, employeeRepository)
        {
            _employeeRepository = employeeRepository;
            _employeeService = employeeService;
        }

        [HttpGet("search")]
        public IActionResult Get(string searchText, Guid? departmentId, int pageSize, int pageIndex)
        {
            try
            {
                // Lấy dữ liệu từ database
                var employee = _employeeRepository.GetEmployeePaging(searchText, departmentId, pageSize, pageIndex);
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }


        /// <summary>
        /// Lấy mã nhân viên mới
        /// </summary>
        /// <returns>
        /// 200 - thành công
        /// 204 - không có dữ liệu
        /// 400 - lỗi đầu vào
        /// 500 - lỗi server side
        /// </returns>
        /// createdBy: NVThang (18/11)
        [HttpGet("newEmployeeCode")]
        public IActionResult Get()
        {
            try
            {
                // Lấy dữ liệu từ database
                var employee = _employeeRepository.GetNewEmployeeCode();
                return Ok(employee);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Xóa dữ liệu nhiều nhân viên
        /// </summary>
        /// <returns>
        /// 200 - thành công
        /// 204 - không có dữ liệu
        /// 400 - lỗi đầu vào
        /// 500 - lỗi server side
        /// </returns>
        /// createdBy: NVThang (08/11)
        [HttpDelete("multi/{EmployeeListId}")]
        public IActionResult Delete(string EmployeeListId)
        {
            try
            {
                var res = _employeeRepository.DeleteMultiEmployee(EmployeeListId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        [HttpGet("export")]
        public IActionResult Export()
        {
            try
            {
                // Gọi lại hàm để tạo file excel
                var stream = _employeeService.CreateExcelFile();
                // Tạo buffer memory strean để hứng file excel
                var buffer = stream as MemoryStream;
/*                using (var file = System.IO.File.Open("D:\\" + "Danh_sach_can_bo_giao_vien" + ".xlsx", System.IO.FileMode.CreateNew))
                {
                    stream.Position = 0; // reset the position of the memory stream
                    stream.CopyTo(file); // copy the memory stream to the file stream
                }*/
                return File(fileContents: buffer.ToArray(), contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",

            // By setting a file download name the framework will
            // automatically add the attachment Content-Disposition header
            fileDownloadName: "Danh_sach_can_bo_giao_vien.xlsx");
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}

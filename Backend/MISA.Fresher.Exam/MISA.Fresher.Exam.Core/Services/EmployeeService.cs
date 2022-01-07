using DocumentFormat.OpenXml.Spreadsheet;
using MISA.Fresher.Exam.Core.Entities;
using MISA.Fresher.Exam.Core.Interfaces.Repository;
using MISA.Fresher.Exam.Core.Interfaces.Service;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using OfficeOpenXml.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Exam.Core.Services
{
    public class EmployeeService:  BaseService<Employee>, IEmployeeService
    {
        #region Field
        IEmployeeRepository _employeeRepository;
        #endregion

        public EmployeeService(IEmployeeRepository employeeRepository) : base(employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public override ServiceResult Update(Employee employee, Guid employeeId)
        {

            var isExist = _employeeRepository.CheckExistId(employeeId);
            if (isExist == false)
            {
                _serviceResult.devMsg = Resource.Resource.InvalidData;
                _serviceResult.userMsg = Resource.Resource.IdNotExist;
                _serviceResult.errorCode = Resource.Resource.IdNotExistErrorCode;
                _serviceResult.moreInfo = "http://google.com";
                return _serviceResult;
            }

            var isDuplicateCode = _employeeRepository.CheckDuplicateCode(employee.EmployeeCode, employeeId);
            if (isDuplicateCode)
            {
                _serviceResult.devMsg = Resource.Resource.InvalidData;
                _serviceResult.userMsg = Resource.Resource.DuplicateCode;
                _serviceResult.errorCode = Resource.Resource.DuplicateCodeErrorCode;
                _serviceResult.moreInfo = "http://google.com";
                return _serviceResult;
            }
            var isValid = ValidateObject(employee);
            if (isValid)
            {
                isValid = ValidateCustom(employee);
            }
            if (isValid)
            {
                _serviceResult.data = _employeeRepository.Update(employee, employeeId);
                _serviceResult.Success = true;
            }
            else
            {
                _serviceResult.userMsg = Resource.Resource.InvalidData;
                _serviceResult.errorCode = Resource.Resource.InvalidDataErrorCode;
            }
            return _serviceResult;
        }

        public override ServiceResult Insert(Employee employee)
        {
            var isDuplicateCode = _employeeRepository.CheckDuplicateCode(employee.EmployeeCode, employee.EmployeeId);
            if (isDuplicateCode)
            {
                _serviceResult.devMsg = Resource.Resource.InvalidData;
                _serviceResult.userMsg = Resource.Resource.DuplicateCode;
                _serviceResult.errorCode = Resource.Resource.DuplicateCodeErrorCode;
                _serviceResult.moreInfo = "http://google.com";
                return _serviceResult;
            }
            var isValid = ValidateObject(employee);
            if (isValid)
            {
                isValid = ValidateCustom(employee);
            }
            if (isValid)
            {
                _serviceResult.data = _employeeRepository.Insert(employee);
                _serviceResult.Success = true;
            }
            else
            {
                _serviceResult.userMsg = Resource.Resource.InvalidData;
                _serviceResult.errorCode = Resource.Resource.InvalidDataErrorCode;
            }
            return _serviceResult;
        }

        protected virtual bool ValidateCustom(Employee employee)
        {
            return true;
        }

        public Stream CreateExcelFile(Stream stream = null)
        {
            var list = _employeeRepository.GetAllEmployeeToExport();
            using (var excelPackage = new ExcelPackage(stream ?? new MemoryStream()))
            {
                // Tạo author cho file Excel
                excelPackage.Workbook.Properties.Author = Resource.Resource.Excel_AuthorProperty;
                // Tạo title cho file Excel
                excelPackage.Workbook.Properties.Title = Resource.Resource.Excel_TitleProperty;
                // Add Sheet vào file Excel
                excelPackage.Workbook.Worksheets.Add(Resource.Resource.Excel_SheetName);
                // Lấy Sheet bạn vừa mới tạo ra để thao tác 
                var workSheet = excelPackage.Workbook.Worksheets[0];
                // Đổ data vào Excel file
                //workSheet.Cells[1, 1].LoadFromCollection(list, true, TableStyles.Dark9);
                BindingFormatForExcel(workSheet, list);
                excelPackage.Save();
                return excelPackage.Stream;
            }
        }

        public void BindingFormatForExcel(ExcelWorksheet worksheet, List<Employee> listItems)
        {
            worksheet.Cells.Style.WrapText = true;
            // Set default width cho tất cả column
            worksheet.Cells["A1:I1"].Merge = true;
            worksheet.Cells[1, 1].Value = Resource.Resource.Excel_Title;
            worksheet.Cells[1, 1].Style.Font.Size = 16;
            worksheet.Cells[1, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
            worksheet.Cells[1, 1].Style.Font.Bold = true;
            worksheet.Cells[1, 1].Style.WrapText = false;
            worksheet.Cells["A2:I2"].Merge = true;

            // Tự động xuống hàng khi text quá dài
            worksheet.Row(1).Height = 20;
            worksheet.Cells.Style.WrapText = true;
            // Tạo header
            worksheet.Cells[3, 1].Value = Resource.Resource.Excel_NumberCount;
            worksheet.Cells[3, 2].Value = Resource.Resource.Excel_EmployeeCode;
            worksheet.Cells[3, 3].Value = Resource.Resource.Excel_FullName;
            worksheet.Cells[3, 4].Value = Resource.Resource.Excel_PhoneNumber;
            worksheet.Cells[3, 5].Value = Resource.Resource.Excel_Department;
            worksheet.Cells[3, 6].Value = Resource.Resource.Excel_Subject;
            worksheet.Cells[3, 7].Value = Resource.Resource.Excel_Room;
            worksheet.Cells[3, 8].Value = Resource.Resource.Excel_ManageService;
            worksheet.Cells[3, 9].Value = Resource.Resource.Excel_WorkStatus;
            // Lấy range vào tạo format cho range đó ở đây là từ A1 tới D1
            using (var range = worksheet.Cells["A3:I3"])
            {
                range.Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                range.Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.MediumGray;
                range.Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Gray);
                range.Style.WrapText = false;
                range.Style.Font.Bold = true;
                range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
            }

            //format cho từng cột
            worksheet.Cells[$"A3:${listItems.Count}"].AutoFitColumns();
            worksheet.Column(3).Width = 30;
            worksheet.Column(1).Width = 5;
            worksheet.Column(6).Width = 30;
            worksheet.Column(7).Width = 30;

            // Đỗ dữ liệu từ list vào 
            for (int i = 0; i < listItems.Count; i++)
            {
                var item = listItems[i];
                worksheet.Cells[i + 4, 1].Value = i + 1;
                worksheet.Cells[i + 4, 2].Value = item.EmployeeCode;
                worksheet.Cells[i + 4, 3].Value = item.FullName;
                worksheet.Cells[i + 4, 4].Value = item.PhoneNumber;
                worksheet.Cells[i + 4, 5].Value = item.DepartmentName;
                worksheet.Cells[i + 4, 6].Value = item.SubjectListString;
                worksheet.Cells[i + 4, 7].Value = item.RoomListString;
                worksheet.Cells[i + 4, 8].Value = item.ManageService == 1 ? "x" : "";
                worksheet.Cells[i + 4, 9].Value = item.WorkStatus == 1 ? "x" : "";

                
                // Format lại color nếu như thỏa điều kiện
                // Ở đây chúng ta sẽ format lại theo dạng fromRow,fromCol,toRow,toCol
                using (var range = worksheet.Cells[i + 4, 8, i + 4, 9])
                {
                    // Format text đỏ và đậm
                    range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                }

            }

           

        }
    }
    public class ColConfig
    {
        public string Name { get; set; }
        //sstt
        //căm tráo  hay phải
        //cột nào
    }
}


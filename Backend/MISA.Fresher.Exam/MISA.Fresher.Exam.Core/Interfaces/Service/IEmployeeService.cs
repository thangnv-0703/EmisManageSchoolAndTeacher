using MISA.Fresher.Exam.Core.Entities;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Exam.Core.Interfaces.Service
{
    public interface IEmployeeService: IBaseService<Employee>
    {
        public Stream CreateExcelFile(Stream stream = null);
        public void BindingFormatForExcel(ExcelWorksheet worksheet, List<Employee> listItems);
    }
}

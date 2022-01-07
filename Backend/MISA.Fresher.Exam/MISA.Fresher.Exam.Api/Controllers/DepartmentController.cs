using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MISA.Fresher.Exam.Core.Entities;
using MISA.Fresher.Exam.Core.Interfaces.Repository;
using MISA.Fresher.Exam.Core.Interfaces.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MISA.Fresher.Exam.Api.Controllers
{
    public class DepartmentController : MISAController<Department> 
    {
        public DepartmentController(IBaseService<Department> baseService, IBaseRepository<Department> baseRepository): base(baseService, baseRepository)
        {

        }
    }
}

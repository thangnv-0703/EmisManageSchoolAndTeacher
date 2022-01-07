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
    public class SubjectController : MISAController<Subject>
    {
        public SubjectController(IBaseService<Subject> baseService, IBaseRepository<Subject> baseRepository): base(baseService, baseRepository)
        {

        }
    }
}

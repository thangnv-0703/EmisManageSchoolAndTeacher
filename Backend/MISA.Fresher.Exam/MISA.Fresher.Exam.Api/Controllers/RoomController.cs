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
    public class RoomController : MISAController<Room>
    {
        public RoomController(IBaseService<Room> baseService, IBaseRepository<Room> baseRepository): base(baseService, baseRepository)
        {

        }
    }
}

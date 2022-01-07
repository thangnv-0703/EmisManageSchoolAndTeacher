using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MISA.Fresher.Exam.Core.Entities;
using MISA.Fresher.Exam.Core.Interfaces.Repository;
using MISA.Fresher.Exam.Core.Interfaces.Service;
using MISA.Fresher.Exam.Core.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MISA.Fresher.Exam.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    /*[EnableCors("AllowOrigin")]*/
    public class MISAController<TEntity> : ControllerBase
    {
        IBaseService<TEntity> _baseService;
        IBaseRepository<TEntity> _baseRepository;

        public MISAController(IBaseService<TEntity> baseService, IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
            _baseService = baseService;
        }

        /// <summary>
        /// Lấy dữ liệu 
        /// </summary>
        /// <returns>
        /// 200 - thành công
        /// 204 - không có dữ liệu
        /// 400 - lỗi đầu vào
        /// 500 - lỗi server side
        /// </returns>
        /// createdBy: NVThang (18/11)
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                // Lấy dữ liệu từ database
                var entities = _baseRepository.GetAll();
                return StatusCode(200, entities);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Lấy dữ liệu  theo id
        /// </summary>
        /// <returns>
        /// 200 - thành công
        /// 204 - không có dữ liệu
        /// 400 - lỗi đầu vào
        /// 500 - lỗi server side
        /// </returns>
        /// createdBy: NVThang (09/11)
        [HttpGet("{entityId}")]
        public IActionResult Get(Guid entityId)
        {
            try
            {
                var entity = _baseRepository.GetById(entityId);
                return StatusCode(200, entity);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Thêm mới 
        /// </summary>
        /// <returns>
        /// 201 - thêm mới thành công
        /// 204 - không có dữ liệu
        /// 400 - lỗi đầu vào
        /// 500 - lỗi server side
        /// </returns>
        /// createdBy: NVThang (09/11)
        [HttpPost]
        public IActionResult Post(TEntity entity)
        {
            try
            {
                var res = _baseService.Insert(entity);
                if (res.Success)
                {
                    return StatusCode(201, res.data);
                }
                else
                {
                    return BadRequest(res);
                }
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Sửa thông tin 
        /// </summary>
        /// <returns>
        /// 200 - thành công
        /// 204 - không có dữ liệu
        /// 400 - lỗi đầu vào
        /// 500 - lỗi server side
        /// </returns>
        /// createdBy: NVThang (09/11)
        [HttpPut("{enittyId}")]
        public IActionResult Put(TEntity entity, Guid enittyId)
        {
            try
            {
                var service = _baseService.Update(entity, enittyId);
                if (service.Success)
                {
                    return StatusCode(200, service.data);
                }
                else
                {
                    return BadRequest(service);
                }

            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Xóa  theo id
        /// </summary>
        /// <returns>
        /// 200 - thành công
        /// 204 - không có dữ liệu
        /// 400 - lỗi đầu vào
        /// 500 - lỗi server side
        /// </returns>
        /// createdBy: NVThang (08/11)
        [HttpDelete("{enittyId}")]
        public IActionResult Delete(Guid enittyId)
        {
            try
            {
                var service = _baseService.Delete(enittyId);
                return Ok(service.data);
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }

        /// <summary>
        /// Hàm xử lý exception
        /// </summary>
        /// <param name="ex"><Exception/param>
        /// <returns>ObjectResult</returns>
        protected ObjectResult HandleException(Exception ex)
        {
            var serviceResult = new ServiceResult();
            serviceResult.devMsg = ex.Message;
            serviceResult.userMsg = Resource.ServerError;
            serviceResult.errorCode = Resource.ServerErrorCode;
            return StatusCode(500, serviceResult);
        }
    }
}

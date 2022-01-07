using MISA.Fresher.Exam.Core.Entities;
using MISA.Fresher.Exam.Core.Interfaces.Repository;
using MISA.Fresher.Exam.Core.Interfaces.Service;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace MISA.Fresher.Exam.Core.Services
{
    public class BaseService<TEntity> : IBaseService<TEntity>
    {
        #region Field
        IBaseRepository<TEntity> _baseRepository;
        protected ServiceResult _serviceResult;
        #endregion

        #region Constructor
        public BaseService(IBaseRepository<TEntity> baseRepository)
        {
            _baseRepository = baseRepository;
            _serviceResult = new ServiceResult();
        }
        #endregion
        public ServiceResult Delete(Guid entityId)
        {
            var res = _baseRepository.Delete(entityId);
            if (res == 0)
            {
                _serviceResult.userMsg = Resource.Resource.IdNotExist;
                _serviceResult.errorCode = Resource.Resource.IdNotExistErrorCode;
                _serviceResult.moreInfo = "http://google.com";
            }
            else
            {
                _serviceResult.Success = true;
            }
            _serviceResult.data = res;
            return _serviceResult;
        }

        public virtual ServiceResult Insert(TEntity entity)
        {
            var isValid = ValidateObject(entity);
            if (isValid)
            {
                isValid = ValidateCustom(entity);
            }
            if (isValid)
            {
                _serviceResult.data = _baseRepository.Insert(entity);
                _serviceResult.Success = true;
            }
            else
            {
                _serviceResult.userMsg = Resource.Resource.InvalidData;
                _serviceResult.errorCode = Resource.Resource.InvalidDataErrorCode;
            }
            return _serviceResult;
        }

        public virtual ServiceResult Update(TEntity entity, Guid entityId)
        {
            var isValid = ValidateObject(entity);
            if (isValid)
            {
                isValid = ValidateCustom(entity);
            }
            if (isValid)
            {
                _serviceResult.data = _baseRepository.Update(entity, entityId);
                _serviceResult.Success = true;
            }
            else
            {
                _serviceResult.userMsg = Resource.Resource.InvalidData;
                _serviceResult.errorCode = Resource.Resource.InvalidDataErrorCode;
            }
            return _serviceResult;
        }

        protected bool ValidateObject(TEntity entity)
        {
            // Validate chung
            // 1. Bắt buộc nhập
            var props = entity.GetType().GetProperties();
            foreach (var prop in props)
            {
                var propValue = prop.GetValue(entity);
                var propName = prop.Name;
                var propType = prop.PropertyType;

                // Kiểm tra dữ liệu không được để trống
                var requiredAttribute = prop.GetCustomAttributes(typeof(MISARequired), false);
                if (requiredAttribute.Length > 0)
                {
                    if (propType == typeof(string) && (propValue == null || string.IsNullOrEmpty(propValue.ToString())))
                    {
                        var attributeMsg = (requiredAttribute[0] as MISARequired).ErrorMsg;
                        attributeMsg = (attributeMsg == null) ? propName : attributeMsg;
                        _serviceResult.userMsg = attributeMsg;
                        _serviceResult.errorCode = Resource.Resource.RequiredErrorCode;
                        return false;
                    }
                }

                // Kiểm tra email đúng định dạng
                var validEmailAttribute = prop.GetCustomAttributes(typeof(MISAValidEmail), false);
                if (validEmailAttribute.Length > 0)
                {

                    var emailTest = new EmailAddressAttribute();
                    if (propValue != null && !string.IsNullOrEmpty(propValue.ToString()) && !emailTest.IsValid(propValue.ToString()))
                    {
                        var attributeMsg = (validEmailAttribute[0] as MISAValidEmail).ErrorMsg;
                        attributeMsg = (attributeMsg == null) ? propName : attributeMsg;
                        _serviceResult.userMsg = attributeMsg;
                        _serviceResult.errorCode = Resource.Resource.InvalidDataErrorCode;
                        return false;
                    }
                }

                // Kiểm tra số điện thoại đúng định dạng
                var validPhoneNumberAttribute = prop.GetCustomAttributes(typeof(MISAValidPhoneNumber), false);
                if (validPhoneNumberAttribute.Length > 0)
                {
                    if (propValue != null && !string.IsNullOrEmpty(propValue.ToString()) && !Regex.Match(propValue.ToString(), @"^(0[0-9]{9})$").Success)
                    {
                        var attributeMsg = (validPhoneNumberAttribute[0] as MISAValidPhoneNumber).ErrorMsg;
                        attributeMsg = (attributeMsg == null) ? propName : attributeMsg;
                        _serviceResult.userMsg = attributeMsg;
                        _serviceResult.errorCode = Resource.Resource.InvalidDataErrorCode;
                        return false;
                    }
                }
            }

            return true;
        }

        protected virtual bool ValidateCustom(TEntity entity)
        {
            return true;
        }
    }
}

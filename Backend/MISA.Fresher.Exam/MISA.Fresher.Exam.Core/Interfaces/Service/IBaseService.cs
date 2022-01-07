using MISA.Fresher.Exam.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Exam.Core.Interfaces.Service
{
    public interface IBaseService<TEntity>
    {
        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="entity">Thông tin thực thể</param>
        /// <returns>Service Result</returns>
        public ServiceResult Insert(TEntity entity);
        /// <summary>
        /// Sửa
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="entityId"></param>
        /// <returns>Service Result</returns>
        public ServiceResult Update(TEntity entity, Guid entityId);
        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>Service Result</returns>
        public ServiceResult Delete(Guid entityId);
    }
}

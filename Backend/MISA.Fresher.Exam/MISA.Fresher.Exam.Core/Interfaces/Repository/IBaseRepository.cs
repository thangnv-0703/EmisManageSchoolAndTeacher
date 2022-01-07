using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Exam.Core.Interfaces.Repository
{
    /// <summary>
    /// Interface các service phục vụ cho Infrastructure
    /// Created by NVTHANG (11/11/2021)
    /// </summary>
    public interface IBaseRepository<TEntity>
    {
        /// <summary>
        /// Lấy toàn bộ dữ liẹu vật thể
        /// </summary>
        /// <returns>List đối tượng</returns>
        /// Created by: NVTHANG(18/11/2021)
        public List<TEntity> GetAll();
        /// <summary>
        /// Lấy dữ liệu theo Id
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns>Đối tượng</returns>
        /// Created by: NVTHANG(18/11/2021)
        public TEntity GetById(Guid entityId);
        /// <summary>
        /// Thêm mới
        /// </summary>
        /// <param name="entity">Thông tin thực thể</param>
        /// <returns>số lượng nhân viên thêm mới thành công</returns>
        /// Created by: NVTHANG(18/11/2021)
        public int Insert(TEntity entity);
        /// <summary>
        /// Sửa
        /// </summary>
        /// <param name="entityId">Id thực thể</param>
        /// <param name="entity">Thông tin thực thể</param>
        /// <returns>số lượng nhân viên sửa thành công</returns>
        /// Created by: NVTHANG(18/11/2021)
        public int Update(TEntity entity, Guid entityId);
        /// <summary>
        /// Xóa
        /// </summary>
        /// <param name="entityId">Id thực thể</param>
        /// <returns>số lượng nhân xóa thành công</returns>
        /// Created by: NVTHANG(18/11/2021)
        public int Delete(Guid entityId);
    }
}

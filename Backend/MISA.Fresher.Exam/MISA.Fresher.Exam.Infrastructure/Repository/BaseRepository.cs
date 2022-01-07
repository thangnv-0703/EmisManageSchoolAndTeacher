using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Fresher.Exam.Core.Interfaces.Repository;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Exam.Infrastructure.Repository
{
    public class BaseRepository<TEntity> : IBaseRepository<TEntity>
    {
        #region Field
        /// <summary>
        /// Chuỗi kết nối
        /// </summary>
        protected string _connectionString;
        /// <summary>
        /// Tên bảng
        /// </summary>
        protected string _tableName;
        /// <summary>
        /// MySQL connection
        /// </summary>
        protected MySqlConnection _sqlConnection;
        #endregion

        #region Constructor
        public BaseRepository(IConfiguration configuration)
        {
            // Khai báo thông tin kết nối với database
            _connectionString = configuration.GetConnectionString("ExamConnection");
            // Khởi tạo kết nối
            // Thực hiện kết nối với database
            _sqlConnection = new MySqlConnection(_connectionString);
            _tableName = typeof(TEntity).Name;
        }
        #endregion

        public virtual int Delete(Guid entityId)
        {
            using(_sqlConnection = new MySqlConnection(_connectionString))
            {
                _sqlConnection.Open();
                var sqlTransaction = _sqlConnection.BeginTransaction();
                var sqlCommand = $"DELETE FROM {_tableName} WHERE {_tableName} = @{_tableName}Id";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"@{_tableName}", entityId);
                var res = _sqlConnection.Execute(sqlCommand, param: parameters, commandType: System.Data.CommandType.Text);
                sqlTransaction.Commit();
                return res;
            }
        }

        public virtual List<TEntity> GetAll()
        {
            using (_sqlConnection = new MySqlConnection(_connectionString))

            {
                _sqlConnection.Open();
                var sqlTransaction = _sqlConnection.BeginTransaction();
                var sqlCommand = $"SELECT * FROM {_tableName}";
                var entities = _sqlConnection.Query<TEntity>(sqlCommand,transaction: sqlTransaction);
                sqlTransaction.Commit();
                return entities.ToList();
            }
        }

        public virtual TEntity GetById(Guid entityId)
        {
            using (_sqlConnection = new MySqlConnection(_connectionString))
            {
                _sqlConnection.Open();
                var sqlTransaction = _sqlConnection.BeginTransaction();
                var sqlCommand = $"SELECT * FROM {_tableName}  WHERE {_tableName}Id = @{_tableName}Id";
                // Tạo dynamic parameters
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"@{_tableName}Id", entityId);
                var entity = _sqlConnection.QueryFirstOrDefault<TEntity>(sqlCommand, param: parameters, transaction: sqlTransaction);
                sqlTransaction.Commit();
                return entity;
            }
        }

        public virtual int Insert(TEntity entity)
        {
            // Khai báo chuỗi SQL động
            var sqlDynamicParam = "";
            var sqlDynamicColumn = "";

            DynamicParameters dynamicParam = new DynamicParameters();
            // Lây ra các properties của đối tượng
            var props = entity.GetType().GetProperties();
            // Duyệt từng properties
            foreach (var prop in props)
            {
                // Lây tên của property
                var propName = prop.Name;
                // Lấy giá trị của property
                var propValue = prop.GetValue(entity);

                // Lấy ra kiểu dữ liệu của property
                var propType = prop.PropertyType;

                if (propName == $"{_tableName}Id" && propType == typeof(Guid))
                {
                    // Tạo id 
                    propValue = Guid.NewGuid();
                }

                // Bổ sung thông tin vào chuỗi 
                sqlDynamicColumn += $"{propName},";
                sqlDynamicParam += $"@{propName},";
                dynamicParam.Add($"@{propName}", propValue);

            }
            // Cắt dấu  phẩy ở cuối
            sqlDynamicColumn = sqlDynamicColumn.Substring(0, sqlDynamicColumn.Length - 1);
            sqlDynamicParam = sqlDynamicParam.Substring(0, sqlDynamicParam.Length - 1);
            var sqlDynamicCommand = $"INSERT INTO {_tableName}({sqlDynamicColumn}) VALUES({sqlDynamicParam})";

            // Thêm mới nhân viên vào hệ thống 
            var res = _sqlConnection.Execute(sqlDynamicCommand, commandType: System.Data.CommandType.Text, param: dynamicParam);
            return res;
        }

        public virtual int Update(TEntity entity, Guid entityId)
        {
            var sqlCommand = $"DELETE FROM {_tableName} WHERE {_tableName}Id = @{_tableName}Id";
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add($"@{_tableName}Id", entityId);
            var result = _sqlConnection.Execute(sqlCommand, param: parameters, commandType: System.Data.CommandType.Text);

            // Khai báo chuỗi SQL động
            var sqlDynamicParam = "";
            var sqlDynamicColumn = "";

            DynamicParameters dynamicParam = new DynamicParameters();
            // Lây ra các properties của đối tượng
            var props = entity.GetType().GetProperties();
            // Duyệt từng properties
            foreach (var prop in props)
            {
                // Lây tên của properties
                var propName = prop.Name;
                // Lấy giá trị của properties
                var propValue = prop.GetValue(entity);

                // Bổ sung thông tin vào chuỗi 
                sqlDynamicColumn += $"{propName},";
                sqlDynamicParam += $"@{propName},";
                dynamicParam.Add($"@{propName}", propValue);

            }
            // Cắt dấu  phẩy ở cuối
            sqlDynamicColumn = sqlDynamicColumn.Substring(0, sqlDynamicColumn.Length - 1);
            sqlDynamicParam = sqlDynamicParam.Substring(0, sqlDynamicParam.Length - 1);
            var sqlDynamicCommand = $"INSERT INTO {_tableName}({sqlDynamicColumn}) VALUES({sqlDynamicParam})";

            // Thêm mới nhân viên vào hệ thống 
            var res = _sqlConnection.Execute(sqlDynamicCommand, commandType: System.Data.CommandType.Text, param: dynamicParam);
            return res;
        }
    }
}

using Dapper;
using Microsoft.Extensions.Configuration;
using MISA.Fresher.Exam.Core.Entities;
using MISA.Fresher.Exam.Core.Interfaces.Repository;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.Fresher.Exam.Infrastructure.Repository
{
    public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
    {
        public EmployeeRepository(IConfiguration configuration) : base(configuration)
        {

        }

        public bool CheckDuplicateCode(string EmployeeCode, Guid EmployeeId)
        {
            var sqlCheckDuplicateEmployeeCode = "SELECT EmployeeCode FROM Employee WHERE EmployeeCode = @EmployeeCode AND EmployeeId != @EmployeeId";
            var paramCode = new DynamicParameters();
            paramCode.Add("@EmployeeId", EmployeeId);
            paramCode.Add("@EmployeeCode", EmployeeCode);
            var employeeCodeDuplicate = _sqlConnection.QueryFirstOrDefault<string>(sqlCheckDuplicateEmployeeCode, paramCode);
            if (employeeCodeDuplicate == null)
                return false;
            return true;
        }

        public bool CheckExistId(Guid EmployeeId)
        {
            var sqlCheckDuplicateEmployeeId = "SELECT * FROM Employee WHERE EmployeeId = @EmployeeId";
            var paramCode = new DynamicParameters();
            paramCode.Add("@EmployeeId", EmployeeId);
            var employeeIdDuplicate = _sqlConnection.QueryFirstOrDefault<object>(sqlCheckDuplicateEmployeeId, paramCode);
            if (employeeIdDuplicate == null)
                return false;
            return true;
        }

        public override int Delete(Guid employeeId)
        {
            using (_sqlConnection = new MySqlConnection(_connectionString))
            {
                _sqlConnection.Open();
                var sqlTransaction = _sqlConnection.BeginTransaction();
                // Xóa trong bảng Employee
                var sqlCommand = $"DELETE FROM Employee WHERE EmployeeId = @EmployeeId";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"@EmployeeId", employeeId);
                var res = _sqlConnection.Execute(sqlCommand, param: parameters, commandType: System.Data.CommandType.Text, transaction: sqlTransaction);
                sqlTransaction.Commit();
                return res;
            }
        }

        public override List<Employee> GetAll()
        {
            using (_sqlConnection = new MySqlConnection(_connectionString))
            {
                _sqlConnection.Open();
                var sqlTransaction = _sqlConnection.BeginTransaction();
                var sqlCommand = @" SELECT e.*, esm.SubjectListString, erm.RoomListString, d.DepartmentName FROM Employee e
                                      LEFT JOIN
                                    (SELECT sm.EmployeeId, GROUP_CONCAT(s.SubjectName SEPARATOR ', ' ) AS SubjectListString
                                    FROM SubjectManagement sm LEFT JOIN Subject s ON sm.SubjectId = s.SubjectId
                                    GROUP BY sm.EmployeeId) AS esm ON e.EmployeeId = esm.EmployeeId
                                      LEFT JOIN
                                    (SELECT rm.EmployeeId, GROUP_CONCAT(r.RoomName SEPARATOR ', ' ) AS RoomListString
                                    FROM  RoomManagement rm  LEFT JOIN Room r ON rm.RoomId = r.RoomId
                                    GROUP BY rm.EmployeeId) AS erm ON e.EmployeeId = erm.EmployeeId
                                      LEFT JOIN Department d ON d.DepartmentId = e.DepartmentId GROUP BY e.EmployeeCode ORDER BY e.EmployeeCode DESC";
                var entities = _sqlConnection.Query<Employee>(sqlCommand, transaction: sqlTransaction);
                sqlTransaction.Commit();
                return entities.ToList();
            }
        }

        public override Employee GetById(Guid employeeId)
        {
            using (_sqlConnection = new MySqlConnection(_connectionString))
            {
                _sqlConnection.Open();
                var sqlTransaction = _sqlConnection.BeginTransaction();
                var sqlCommand = $"SELECT * FROM Employee  WHERE EmployeeId = @EmployeeId";
                // Tạo dynamic parameters
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"@EmployeeId", employeeId);
                var employee = _sqlConnection.QueryFirstOrDefault<Employee>(sqlCommand, param: parameters, transaction: sqlTransaction);
                if (employee == null)
                {
                    return null;
                }

                // Láy list môn học
                var sqlGetSubjectList = $"SELECT s.SubjectId FROM Subject s LEFT JOIN SubjectManagement sm ON s.SubjectId = sm.SubjectId WHERE sm.EmployeeId = @EmployeeId";
                DynamicParameters paramSubject = new DynamicParameters();
                paramSubject.Add("@EmployeeId", employeeId);
                var subjectList = _sqlConnection.Query<Guid>(sqlGetSubjectList, param: paramSubject, transaction: sqlTransaction);


                // Láy list kho phòng
                var sqlGetRoomList = $"SELECT r.RoomId FROM Room r LEFT JOIN RoomManagement rm ON rm.RoomId = r.RoomId WHERE rm.EmployeeId = @EmployeeId";
                DynamicParameters paramRoom = new DynamicParameters();
                paramRoom.Add("@EmployeeId", employeeId);
                var roomList = _sqlConnection.Query<Guid>(sqlGetRoomList, param: paramRoom, transaction: sqlTransaction);

                // Gắn vào employee
                employee.SubjectList = subjectList.ToList();
                employee.RoomList = roomList.ToList();
                sqlTransaction.Commit();
                return employee;
            }
        }

        public override int Insert(Employee employee)
        {
            using (_sqlConnection = new MySqlConnection(_connectionString))
            {
                _sqlConnection.Open();
                var sqlTransaction = _sqlConnection.BeginTransaction();

                // Khai báo chuỗi SQL động
                var sqlDynamicParam = "";
                var sqlDynamicColumn = "";

                DynamicParameters dynamicParam = new DynamicParameters();
                // Lây ra các properties của đối tượng
                var props = employee.GetType().GetProperties();
                // Duyệt từng properties
                foreach (var prop in props)
                {
                    // Lây tên của property
                    var propName = prop.Name;
                    // Lấy giá trị của property
                    var propValue = prop.GetValue(employee);

                    // Lấy ra kiểu dữ liệu của property
                    var propType = prop.PropertyType;

                    if (propName == $"EmployeeId" && propType == typeof(Guid))
                    {
                        // Tạo id 
                        employee.EmployeeId = Guid.NewGuid();
                        propValue = employee.EmployeeId;
                    }

                    if (propName != "SubjectList" && propName != "RoomList" && propName != "SubjectListString" && propName != "RoomListString"  && propName != "DepartmentName")
                    {
                        // Bổ sung thông tin vào chuỗi 
                        sqlDynamicColumn += $"{propName},";
                        sqlDynamicParam += $"@{propName},";
                        dynamicParam.Add($"@{propName}", propValue);
                    }

                }
                // Cắt dấu  phẩy ở cuối
                sqlDynamicColumn = sqlDynamicColumn.Substring(0, sqlDynamicColumn.Length - 1);
                sqlDynamicParam = sqlDynamicParam.Substring(0, sqlDynamicParam.Length - 1);
                var sqlDynamicCommand = $"INSERT INTO Employee ({sqlDynamicColumn}) VALUES({sqlDynamicParam})";
                // Thêm mới nhân viên vào hệ thống 
                var res = _sqlConnection.Execute(sqlDynamicCommand, commandType: System.Data.CommandType.Text, param: dynamicParam, transaction: sqlTransaction);

                if (employee.SubjectList != null && employee.SubjectList.Count > 0)
                {
                    foreach (var subjectId in employee.SubjectList)
                    {
                        var sqlInsert = "INSERT INTO SubjectManagement (EmployeeId, SubjectId) VALUES(@EmployeeId, @SubjectId)";
                        DynamicParameters paramSubject = new DynamicParameters();
                        paramSubject.Add("@EmployeeId", employee.EmployeeId);
                        paramSubject.Add("@SubjectId", subjectId);
                        var resSubject = _sqlConnection.Execute(sqlInsert, commandType: System.Data.CommandType.Text, param: paramSubject, transaction: sqlTransaction);
                    }
                }

                if (employee.RoomList != null && employee.RoomList.Count > 0)
                {
                    foreach (var roomId in employee.RoomList)
                    {
                        var sqlInsert = "INSERT INTO RoomManagement (EmployeeId, RoomId) VALUES(@EmployeeId, @SubjectId)";
                        DynamicParameters paramRoom = new DynamicParameters();
                        paramRoom.Add("@EmployeeId", employee.EmployeeId);
                        paramRoom.Add("@SubjectId", roomId);
                        var resSubject = _sqlConnection.Execute(sqlInsert, commandType: System.Data.CommandType.Text, param: paramRoom, transaction: sqlTransaction);
                    }
                }


                sqlTransaction.Commit();
                return res;
            }
        }

        public override int Update(Employee employee, Guid employeeId)
        {
            using (_sqlConnection = new MySqlConnection(_connectionString))
            {
                _sqlConnection.Open();
                var sqlTransaction = _sqlConnection.BeginTransaction();

                // Xóa trong bảng RoomManagement
                var sqlDeleteRoom = $"DELETE FROM RoomManagement WHERE EmployeeId = @EmployeeId";
                DynamicParameters paramRoom = new DynamicParameters();
                paramRoom.Add($"@EmployeeId", employeeId);
                var resRoom = _sqlConnection.Execute(sqlDeleteRoom, param: paramRoom, commandType: System.Data.CommandType.Text, transaction: sqlTransaction);

                // Xóa trong bảng SubjectManagement
                var sqlDeleteSubject = $"DELETE FROM SubjectManagement WHERE EmployeeId = @EmployeeId";
                DynamicParameters paramSubject = new DynamicParameters();
                paramSubject.Add($"@EmployeeId", employeeId);
                var resSubject = _sqlConnection.Execute(sqlDeleteSubject, param: paramSubject, commandType: System.Data.CommandType.Text, transaction: sqlTransaction);

                // Xóa trong bảng Employee
                var sqlDelete = $"DELETE FROM Employee WHERE EmployeeId = @EmployeeId";
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add($"@EmployeeId", employeeId);
                var resDelete = _sqlConnection.Execute(sqlDelete, param: parameters, commandType: System.Data.CommandType.Text, transaction: sqlTransaction);

                // Khai báo chuỗi SQL động
                var sqlDynamicParam = "";
                var sqlDynamicColumn = "";

                DynamicParameters dynamicParam = new DynamicParameters();
                // Lây ra các properties của đối tượng
                var props = employee.GetType().GetProperties();
                // Duyệt từng properties
                foreach (var prop in props)
                {
                    // Lây tên của property
                    var propName = prop.Name;
                    // Lấy giá trị của property
                    var propValue = prop.GetValue(employee);

                    // Lấy ra kiểu dữ liệu của property
                    var propType = prop.PropertyType;

                    if (propName != "SubjectList" && propName != "RoomList" && propName != "SubjectListString" && propName != "RoomListString" && propName != "DepartmentName")
                    {
                        // Bổ sung thông tin vào chuỗi 
                        sqlDynamicColumn += $"{propName},";
                        sqlDynamicParam += $"@{propName},";
                        dynamicParam.Add($"@{propName}", propValue);
                    }

                }
                // Cắt dấu  phẩy ở cuối
                sqlDynamicColumn = sqlDynamicColumn.Substring(0, sqlDynamicColumn.Length - 1);
                sqlDynamicParam = sqlDynamicParam.Substring(0, sqlDynamicParam.Length - 1);
                var sqlDynamicCommand = $"INSERT INTO Employee ({sqlDynamicColumn}) VALUES({sqlDynamicParam})";
                // Thêm mới nhân viên vào hệ thống 
                var res = _sqlConnection.Execute(sqlDynamicCommand, commandType: System.Data.CommandType.Text, param: dynamicParam, transaction: sqlTransaction);

                if (employee.SubjectList != null && employee.SubjectList.Count > 0)
                {
                    foreach (var subjectId in employee.SubjectList)
                    {
                        var sqlInsert = "INSERT INTO SubjectManagement (EmployeeId, SubjectId) VALUES(@EmployeeId, @SubjectId)";
                        DynamicParameters paramSubject2 = new DynamicParameters();
                        paramSubject2.Add("@EmployeeId", employeeId);
                        paramSubject2.Add("@SubjectId", subjectId);
                        var resSubject2 = _sqlConnection.Execute(sqlInsert, commandType: System.Data.CommandType.Text, param: paramSubject2, transaction: sqlTransaction);
                    }
                }

                if (employee.RoomList != null && employee.RoomList.Count > 0)
                {
                    foreach (var roomId in employee.RoomList)
                    {
                        var sqlInsert = "INSERT INTO RoomManagement (EmployeeId, RoomId) VALUES(@EmployeeId, @SubjectId)";
                        DynamicParameters paramRoom2 = new DynamicParameters();
                        paramRoom2.Add("@EmployeeId", employeeId);
                        paramRoom2.Add("@SubjectId", roomId);
                        var resSubject2 = _sqlConnection.Execute(sqlInsert, commandType: System.Data.CommandType.Text, param: paramRoom2, transaction: sqlTransaction);
                    }
                }

                sqlTransaction.Commit();
                return res;
            }
        }

        public object GetEmployeePaging(string searchText, Guid? departmentId, int pageSize, int pageIndex)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                searchText = string.Empty;
            }
            
            var totalRecord = 0;
            var totalPage = 0;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@m_FullName", searchText);
            parameters.Add("@m_EmployeeCode", searchText);
            parameters.Add("@m_DepartmentId", departmentId );
            parameters.Add("@m_PageIndex", pageIndex);
            parameters.Add("@m_PageSize", pageSize);
            parameters.Add("@m_TotalRecord", direction: System.Data.ParameterDirection.Output, dbType: System.Data.DbType.Int32);
            parameters.Add("@m_TotalPage", direction: System.Data.ParameterDirection.Output, dbType: System.Data.DbType.Int32);
            var data = _sqlConnection.Query<Employee>("Proc_GetEmployeePaging", param: parameters, commandType: System.Data.CommandType.StoredProcedure).ToList();
            totalRecord = parameters.Get<int>("@m_TotalRecord");
            totalPage = parameters.Get<int>("@m_TotalPage");
            return new {
                Data = data,
                TotalRecord = totalRecord,
                TotalPage = totalPage,
            };
        }

        public string GetNewEmployeeCode()
        {
            
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@m_newCode", direction: System.Data.ParameterDirection.Output, dbType: System.Data.DbType.String);
            var res = _sqlConnection.Query<string>("Proc_GetNewEmployeeCode", param: parameters, commandType: System.Data.CommandType.StoredProcedure);
            var newCode = parameters.Get<string>("@m_newCode");
            return newCode;
        }

        public int DeleteMultiEmployee(string employeeListId)
        {
            using (_sqlConnection = new MySqlConnection(_connectionString))
            {
                _sqlConnection.Open();
                var sqlTransaction = _sqlConnection.BeginTransaction();
                // Xóa trong bảng Employee
                DynamicParameters parameters = new DynamicParameters();
                parameters.Add("@m_ListEmployeeId", employeeListId);
                var res = _sqlConnection.Execute("Proc_DeleteMultiEmployee", param: parameters, commandType: System.Data.CommandType.StoredProcedure, transaction: sqlTransaction);
                sqlTransaction.Commit();
                return res;
            }
        }

        public List<Employee> GetAllEmployeeToExport()
        {
            var employees = _sqlConnection.Query<Employee>("Proc_GetAllEmployeeToExport", commandType: System.Data.CommandType.StoredProcedure).ToList();
            return employees;
        }
    }

}

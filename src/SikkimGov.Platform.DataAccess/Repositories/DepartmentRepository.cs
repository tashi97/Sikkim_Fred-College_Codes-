﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SikkimGov.Platform.DataAccess.Core;
using SikkimGov.Platform.DataAccess.Repositories.Contracts;
using SikkimGov.Platform.Models.DomainModels;

namespace SikkimGov.Platform.DataAccess.Repositories
{
    public class DepartmentRepository : BaseRepository, IDepartmentRepository
    {
        private const string DEPT_READ_COMMAND = "P_READ_DEPT";
        public List<Department> GetAllDepartments()
        {
            var departments = new List<Department>();
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand(DEPT_READ_COMMAND, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var parameter = new SqlParameter("@DEPT_ID", DBNull.Value);
                    command.Parameters.Add(parameter);
                    connection.Open();
                    using (var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        while(reader.Read())
                        {
                            var dep = new Department();
                            dep.Id = reader.GetInt64(0);
                            dep.Name = reader.GetString(1);
                            departments.Add(dep);
                        }
                    }
                }
            }
            return departments;
        }

        public Department GetDepartmentById(long departmentId)
        {
            var departments = new List<Department>();
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand(DEPT_READ_COMMAND, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var parameter = new SqlParameter("@DEPT_ID", departmentId);
                    command.Parameters.Add(parameter);
                    connection.Open();

                    using (var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        if (reader.Read())
                        {
                            var department = new Department();
                            department.Id = reader.GetInt64(0);
                            department.Name = reader.GetString(1);
                            return department;
                        }
                    }
                }
            }
            return null;
        }
    }
}
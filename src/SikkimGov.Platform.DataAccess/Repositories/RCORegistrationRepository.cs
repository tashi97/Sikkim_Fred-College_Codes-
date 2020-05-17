﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using SikkimGov.Platform.DataAccess.Core;
using SikkimGov.Platform.DataAccess.Repositories.Contracts;
using SikkimGov.Platform.Models.DomainModels;

namespace SikkimGov.Platform.DataAccess.Repositories
{
    public class RCORegistrationRepository : BaseRepository, IRCORegistrationRepository
    {
        private const string RCO_REG_SAVE_COMMAND = "P_RCO_Registration_INS";
        private const string RCO_REG_DEL_COMMAND = "P_DEL_RCO_REGISTRATION";
        private const string RCO_REG_GET_BY_ID_COMMAND = "P_RCO_REG_READ_BY_ID";
        private const string RDO_REG_UPDATE_STATUS_COMMAND = "P_RCO_REG_UPDATE_STATUS";
        private const string RCO_REG_DETAILS_READ_BY_STATUS_COMMAND = "P_RCO_REG_DETAILS_READ_BY_STATUS";

        public RCORegistration SaveRCORegistration(RCORegistration rcoRegistration)
        {
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand(RCO_REG_SAVE_COMMAND, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var parameter = new SqlParameter("@RCO_Name_Of_Administrator", rcoRegistration.AdminName);
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@Type_Of_Reg", rcoRegistration.RegistrationType);
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@RCO_Department", rcoRegistration.Department);
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@RCO_Designation", rcoRegistration.Designation);
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@RCO_District", rcoRegistration.District);
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@RCO_Office_Add1", rcoRegistration.OfficeAddress1);
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@RCO_Office_Add2", string.IsNullOrEmpty(rcoRegistration.OfficeAddress2) ? "" : rcoRegistration.OfficeAddress2);
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@RCO_Tin_No", string.IsNullOrEmpty(rcoRegistration.TINNumber) ? "" : rcoRegistration.TINNumber);
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@RCO_Tan_No", rcoRegistration.TANNumber);
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@RCO_Email", rcoRegistration.EmailId);
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@RCO_Contact", rcoRegistration.ContactNumber);
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@RCO_Entry_Time", DBNull.Value);
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@RETURN_ID", System.Data.SqlDbType.BigInt);
                    parameter.Direction = System.Data.ParameterDirection.Output;
                    command.Parameters.Add(parameter);

                    connection.Open();

                    command.ExecuteNonQuery();

                    var newId = Convert.ToInt64(command.Parameters["@RETURN_ID"].Value);

                    rcoRegistration.Id = newId;
                }
            }
            return rcoRegistration;
        }

        public bool DeleteRCORegistration(long rcoRegistrationId)
        {
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand(RCO_REG_DEL_COMMAND, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var parameter = new SqlParameter("@RCO_REG_ID", rcoRegistrationId);
                    command.Parameters.Add(parameter);

                    connection.Open();
                    var rowCount = command.ExecuteNonQuery();
                    connection.Close();

                    return rowCount > 0;
                }
            }
        }

        public RCORegistration GetRCORegistrationById(long rcoRegistrationId)
        {
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand(RCO_REG_GET_BY_ID_COMMAND, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var parameter = new SqlParameter("@RCO_REG_ID", rcoRegistrationId);
                    command.Parameters.Add(parameter);

                    connection.Open();

                    using (var reader = command.ExecuteReader(System.Data.CommandBehavior.CloseConnection))
                    {
                        if (reader.Read())
                        {
                            var rcoRegistration = new RCORegistration();
                            rcoRegistration.Id = Convert.ToInt64(reader["REG_ID"]);
                            rcoRegistration.AdminName = reader["RCO_Name_Of_Administrator"].ToString();
                            rcoRegistration.RegistrationType = reader["Type_Of_Reg"].ToString();
                            rcoRegistration.Department = reader["RCO_Department"].ToString();
                            rcoRegistration.Designation = reader["RCO_Designation"].ToString();
                            rcoRegistration.District = reader["RCO_District"].ToString();
                            rcoRegistration.OfficeAddress1 = reader["RCO_Office_Add1"].ToString();
                            rcoRegistration.OfficeAddress2 = reader["RCO_Office_Add2"] == DBNull.Value ? "" : reader["RCO_Office_Add2"].ToString();
                            rcoRegistration.TINNumber = reader["RCO_Tin_No"] == DBNull.Value ? "" : reader["RCO_Tin_No"].ToString();
                            rcoRegistration.TANNumber = reader["RCO_Tan_No"].ToString();
                            rcoRegistration.EmailId = reader["RCO_Email"].ToString();
                            rcoRegistration.ContactNumber = reader["RCO_Contact"].ToString();
                            rcoRegistration.CreatedDate = Convert.ToDateTime(reader["RCO_Entry_Time"]);
                            rcoRegistration.Status = Convert.ToBoolean(reader["CUR_STATUS"]);
                            rcoRegistration.ApprovedBy = reader["PASSED_BY"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PASSED_BY"]);
                            rcoRegistration.ApprovedDate = null;
                            if (reader["PASSING_TIME"] != DBNull.Value)
                            {
                                rcoRegistration.ApprovedDate = Convert.ToDateTime(reader["PASSING_TIME"]);
                            }

                            return rcoRegistration;
                        }
                    }
                }
            }

            return null;
        }

        public List<RCORegistrationDetails> GetRCORegistrationsByStatus(bool? status)
        {
            var ddoRegistrations = new List<RCORegistrationDetails>();

            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand(RCO_REG_DETAILS_READ_BY_STATUS_COMMAND, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var parameter = new SqlParameter("@STATUS", status);
                    command.Parameters.Add(parameter);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var ddoRegistration = new RCORegistrationDetails();
                            ddoRegistration.Id = Convert.ToInt64(reader["REG_ID"]);
                            ddoRegistration.AdminName = reader["RCO_Name_Of_Administrator"] == DBNull.Value ? "" : reader["RCO_Name_Of_Administrator"].ToString();
                            ddoRegistration.Department = reader["RCO_Department"] == DBNull.Value ? "" : reader["RCO_Department"].ToString();
                            ddoRegistration.RegistrationType = reader["Type_Of_Reg"] == DBNull.Value ? "" : reader["Type_Of_Reg"].ToString();
                            ddoRegistration.Designation = reader["RCO_Designation"] == DBNull.Value ? "" : reader["RCO_Designation"].ToString();
                            ddoRegistration.District= reader["RCO_District"] == DBNull.Value ? "" : reader["RCO_District"].ToString();
                            ddoRegistration.OfficeAddress1 = reader["RCO_Office_Add1"] == DBNull.Value ? "" : reader["RCO_Office_Add1"].ToString();
                            ddoRegistration.OfficeAddress2 = reader["RCO_Office_Add2"] == DBNull.Value ? "" : reader["RCO_Office_Add2"].ToString();
                            ddoRegistration.TINNumber = reader["RCO_Tin_No"] == DBNull.Value ? "" : reader["RCO_Tin_No"].ToString();
                            ddoRegistration.TANNumber = reader["RCO_Tan_No"] == DBNull.Value ? "" : reader["RCO_Tan_No"].ToString();
                            ddoRegistration.EmailId = reader["RCO_Email"] == DBNull.Value ? "" : reader["RCO_Email"].ToString();
                            ddoRegistration.ContactNumber = reader["RCO_Contact"] == DBNull.Value ? "" : reader["RCO_Contact"].ToString();
                            ddoRegistration.Status = Convert.ToBoolean(reader["CUR_STATUS"]);
                            ddoRegistration.StatusName = reader["STATUS_NAME"].ToString();
                            ddoRegistration.CreatedDate = Convert.ToDateTime(reader["RCO_Entry_Time"]);
                            ddoRegistration.ApprovedBy = reader["PASSED_BY"] == DBNull.Value ? 0 : Convert.ToInt32(reader["PASSED_BY"]);
                            ddoRegistration.ApprovedDate = null;
                            if(reader["PASSING_TIME"] != DBNull.Value)
                            {
                                ddoRegistration.ApprovedDate = Convert.ToDateTime(reader["PASSING_TIME"]);
                            }

                            ddoRegistrations.Add(ddoRegistration);
                        }
                    }

                    return ddoRegistrations;
                }
            }
        }


        public bool UpdateDDORegistrationStatus(long rcoRegistrationId, bool status, int updatedBy)
        {
            using (var connection = GetConnection())
            {
                using (var command = new SqlCommand(RDO_REG_UPDATE_STATUS_COMMAND, connection))
                {
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var parameter = new SqlParameter("@RCO_REG_ID", rcoRegistrationId);
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@STATUS", status);
                    command.Parameters.Add(parameter);
                    parameter = new SqlParameter("@UPDATEDBY", updatedBy);
                    command.Parameters.Add(parameter);

                    connection.Open();
                    var rowCount = command.ExecuteNonQuery();
                    connection.Close();
                    return rowCount > 0;
                }
            }
        }
    }
}
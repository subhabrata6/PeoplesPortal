using System.Data;
using System.Data.SqlClient;
using System.Net;
using TestEmployeeWebApi.HelperClass;
using TestEmployeeWebApi.Model;

namespace TestEmployeeWebApi.DBAccess
{
    public class LeaveDbAccess
    {
        private readonly IConfiguration _configuration;
        public LeaveDbAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ResponseMessage SaveLeave(LeaveModule module)
        {
            ResponseMessage response = new ResponseMessage();

            string connStr = Helper.GetConnectionString(_configuration);

            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlTransaction transaction = null;

            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                transaction = conn.BeginTransaction();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "SAVE_LEAVE";
                cmd.Transaction = transaction;
                cmd.Parameters.AddWithValue("@FROM_DATE", module.FromDate);
                cmd.Parameters.AddWithValue("@TO_DATE", module.ToDate);
                cmd.Parameters.AddWithValue("@LEAVE_TYPE", module.LeaveType);
                cmd.Parameters.AddWithValue("@LEAVE_TIME", module.LeaveTime);
                cmd.Parameters.AddWithValue("@NO_OF_DAYS", module.NumberOfDays);
                cmd.Parameters.AddWithValue("@REASON", module.Reason);
                cmd.Parameters.AddWithValue("@CREATED_BY", module.CreatedBy);

                cmd.Parameters.Add("@MSG",SqlDbType.NVarChar,100).Direction = ParameterDirection.Output;

                int rows = cmd.ExecuteNonQuery();
                string msg = Helper.GetDBStringValue(cmd.Parameters["@MSG"].Value);

                if (rows > 0)
                {
                    transaction.Commit();
                    response.Status = HttpStatusCode.OK;
                    response.Message = "Saved successfully";
                }
                else
                {
                    transaction.Rollback();
                    response.Status = HttpStatusCode.BadRequest;
                    response.Message = "Failed to save new person";
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                response.Status = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
            }

            return response;
        }

        public ResponseMessage GetLeaveList(string userId)
        {
            ResponseMessage response = new ResponseMessage();

            string connStr = Helper.GetConnectionString(_configuration);

            SqlConnection conn = null;
            SqlDataAdapter da = null;
            SqlCommand cmd = null;
            DataTable peopleDt = new DataTable();
            List<LeaveModule> leaves = new List<LeaveModule>();

            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "GET_LEAVE_LIST";
                cmd.Parameters.AddWithValue("@USER_ID", userId);
                da = new SqlDataAdapter(cmd);
                da.Fill(peopleDt);

                if (peopleDt.Rows.Count > 0)
                {
                    foreach (DataRow row in peopleDt.Rows)
                    {
                        LeaveModule leave = new LeaveModule();
                        leave.FromDate = Helper.GetDBStringValue(row["FROM_DATE"]);
                        leave.ToDate = Helper.GetDBStringValue(row["TO_DATE"]);
                        leave.LeaveTypeName = Helper.GetDBStringValue(row["LEAVE_TYPE"]);
                        leave.Id = Helper.GetDBIntValue(row["LEAVE_ID"]);
                        leave.Reason = Helper.GetDBStringValue(row["REASON"]);
                        leave.Status = Helper.GetDBStringValue(row["LEAVE_STATUS"]);
                        leave.CreatedBy = Helper.GetDBStringValue(row["CREATED_BY"]);
                        leave.CreatedOn = Helper.GetDBStringValue(row["CREATED_ON"]);
                        leave.ApprovedBy = Helper.GetDBStringValue(row["APPROVED_BY"]);
                        leave.ApprovedOn = Helper.GetDBStringValue(row["APPROVED_ON"]);

                        leaves.Add(leave);
                    }

                    response.Status = HttpStatusCode.OK;
                    response.Response = leaves;
                    response.Message = "OK";
                }
                else
                {
                    response.Status = HttpStatusCode.NoContent;
                    response.Response = leaves;
                    response.Message = "No data found";
                }

            }
            catch (Exception ex)
            {
                response.Status = HttpStatusCode.InternalServerError;
                response.Message = ex.Message;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                conn.Dispose();
                da.Dispose();
            }

            return response;
        }
    }
}

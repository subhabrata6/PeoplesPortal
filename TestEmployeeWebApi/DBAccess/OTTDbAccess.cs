using System.Data.SqlClient;
using System.Data;
using System.Net;
using TestEmployeeWebApi.Model;
using TestEmployeeWebApi.HelperClass;

namespace TestEmployeeWebApi.DBAccess
{
    public class OTTDbAccess
    {
        private readonly IConfiguration _configuration;
        public OTTDbAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ResponseMessage SaveSubscriptionPlan(Plans plan)
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
                cmd.CommandText = "SAVE_OTT_PLAN";
                cmd.Transaction = transaction;
                cmd.Parameters.AddWithValue("@PLATFORM_ID", plan.PlatformId);
                cmd.Parameters.AddWithValue("@PLAN_ID", plan.PlanId);
                cmd.Parameters.AddWithValue("@DESC", plan.Description);
                cmd.Parameters.AddWithValue("@PRICE", plan.Price);
                cmd.Parameters.AddWithValue("@NAME", plan.PlanName);
                cmd.Parameters.AddWithValue("@DURATION", plan.Duration);
                cmd.Parameters.AddWithValue("@DURATION_TYPE", plan.DurationType);

                int rows = cmd.ExecuteNonQuery();

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
                    response.Message = "Failed to save OTT PLAN";
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
    }
}

using System.Data;
using System.Data.SqlClient;
using System.Net;
using TestEmployeeApp.HelperClass;
using TestEmployeeApp.Model;

namespace TestEmployeeApp.DBAccess
{
    public class OTTDBAccess
    {
        private readonly IConfiguration _configuration;
        public OTTDBAccess(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public ResponseMessage GetOttList()
        {
            ResponseMessage response = new ResponseMessage();

            string connStr = Helper.GetConnectionString(_configuration);

            SqlConnection conn = null;
            SqlDataAdapter da = null;
            SqlCommand cmd = null;
            DataTable ottDt = new DataTable();
            List<OttPlatforms> ottList = new List<OttPlatforms>();

            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "GET_OTT_LIST";
                da = new SqlDataAdapter(cmd);
                da.Fill(ottDt);

                if (ottDt.Rows.Count > 0)
                {
                    foreach (DataRow row in ottDt.Rows)
                    {
                        OttPlatforms ott = new OttPlatforms();
                        ott.PlatformId = Helper.GetDBIntValue(row["PLATFORM_ID"]);
                        ott.PlatformName = Helper.GetDBStringValue(row["PLATFORM_NAME"]);
                        ott.WebSiteUrl = Helper.GetDBStringValue(row["WEBSITE_URL"]);

                        ottList.Add(ott);
                    }

                    response.Status = HttpStatusCode.OK;
                    response.Response = ottList;
                    response.Message = "OK";
                }
                else
                {
                    response.Status = HttpStatusCode.NoContent;
                    response.Response = ottList;
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

        public ResponseMessage SaveOTT(OttPlatforms ott)
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
                cmd.CommandText = "SAVE_OTT";
                cmd.Transaction = transaction;
                cmd.Parameters.AddWithValue("@ID", ott.PlatformId);
                cmd.Parameters.AddWithValue("@NAME", ott.PlatformName);
                cmd.Parameters.AddWithValue("@URL", ott.WebSiteUrl);

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
                    response.Message = "Failed to save new country";
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

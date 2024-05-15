using System.Data.SqlClient;
using System.Data;
using System.Net;
using TestEmployeeApp.HelperClass;
using TestEmployeeApp.Model;

namespace TestEmployeeApp.DBAccess
{
    public class StateDbAccess
    {
        private readonly IConfiguration _configuration;
        public StateDbAccess(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public ResponseMessage GetStateList()
        {
            ResponseMessage response = new ResponseMessage();

            string connStr = Helper.GetConnectionString(_configuration);

            SqlConnection conn = null;
            SqlDataAdapter da = null;
            SqlCommand cmd = null;
            DataTable stateDt = new DataTable();
            List<State> stateList = new List<State>();

            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "GET_STATE_AND_COUNTRY_LIST";
                da = new SqlDataAdapter(cmd);
                da.Fill(stateDt);

                if (stateDt.Rows.Count > 0)
                {
                    foreach (DataRow row in stateDt.Rows)
                    {
                        State st = new State();
                        st.countryName = Helper.GetDBStringValue(row["COUNTRY_NAME"]);
                        st.state = Helper.GetDBStringValue(row["STATE"]);
                        st.createdOn = Helper.GetDBStringValue(row["CREATED_ON"]);
                        st.id = Helper.GetDBIntValue(row["ID"]);

                        stateList.Add(st);
                    }

                    response.Status = HttpStatusCode.OK;
                    response.Response = stateList;
                    response.Message = "OK";
                }
                else
                {
                    response.Status = HttpStatusCode.NoContent;
                    response.Response = stateList;
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

        public ResponseMessage SaveState(State newState)
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
                cmd.CommandText = "SAVE_STATE";
                cmd.Transaction = transaction;
                cmd.Parameters.AddWithValue("@ID", newState.id);
                cmd.Parameters.AddWithValue("@NAME", newState.state);
                cmd.Parameters.AddWithValue("@COUNTRY_ID", newState.countryId);

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
                    response.Message = "Failed to save new state";
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

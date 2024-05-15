using System.Data;
using System.Data.SqlClient;
using System.Net;
using TestEmployeeApp.HelperClass;
using TestEmployeeApp.Model;

namespace TestEmployeeApp.DBAccess
{
    public class PeopleDbAccess
    {
        private readonly IConfiguration _configuration;
        public PeopleDbAccess(IConfiguration configuration)
        {

            _configuration = configuration;

        }

        #region Methods
        public ResponseMessage GetPeopleList()
        {
            ResponseMessage response = new ResponseMessage();

            string connStr = Helper.GetConnectionString(_configuration);

            SqlConnection conn = null;
            SqlDataAdapter da = null;
            SqlCommand cmd = null;
            DataTable peopleDt = new DataTable();
            List<People> peoples = new List<People>();

            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "GET_PEOPLE_LIST";
                da = new SqlDataAdapter(cmd);
                da.Fill(peopleDt);

                if (peopleDt.Rows.Count > 0)
                {
                    foreach (DataRow row in peopleDt.Rows)
                    {
                        People people = new People();
                        people.description = Helper.GetDBStringValue(row["DESCRIPTION"]);
                        people.age = Helper.GetDBIntValue(row["AGE"]);
                        people.name = Helper.GetDBStringValue(row["NAME"]);
                        people.id = Helper.GetDBIntValue(row["ID"]);
                        people.country = Helper.GetDBStringValue(row["COUNTRY_NAME"]);
                        people.state = Helper.GetDBStringValue(row["STATE"]);
                        people.createdOn = Helper.GetDBStringValue(row["CREATED_ON"]);

                        peoples.Add(people);
                    }

                    response.Status = HttpStatusCode.OK;
                    response.Response = peoples;
                    response.Message = "OK";
                }
                else
                {
                    response.Status = HttpStatusCode.NoContent;
                    response.Response = peoples;
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

        public ResponseMessage SavePeople(People newPeople)
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
                cmd.CommandText = "SAVE_PEOPLE";
                cmd.Transaction = transaction;
                cmd.Parameters.AddWithValue("@ID", newPeople.id);
                cmd.Parameters.AddWithValue("@NAME", newPeople.name);
                cmd.Parameters.AddWithValue("@AGE", newPeople.age);
                cmd.Parameters.AddWithValue("@DESCRIPTION", newPeople.description);

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

        public ResponseMessage GetCountryList()
        {
            ResponseMessage response = new ResponseMessage();

            string connStr = Helper.GetConnectionString(_configuration);

            SqlConnection conn = null;
            SqlDataAdapter da = null;
            SqlCommand cmd = null;
            DataTable countryDt = new DataTable();
            List<Country> countryList = new List<Country>();

            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "GET_COUNTRY_LIST";
                da = new SqlDataAdapter(cmd);
                da.Fill(countryDt);

                if (countryDt.Rows.Count > 0)
                {
                    foreach (DataRow row in countryDt.Rows)
                    {
                        Country country = new Country();
                        country.countryName = Helper.GetDBStringValue(row["DESCRIPTION"]);
                        country.id = Helper.GetDBIntValue(row["ID"]);

                        countryList.Add(country);
                    }

                    response.Status = HttpStatusCode.OK;
                    response.Response = countryList;
                    response.Message = "OK";
                }
                else
                {
                    response.Status = HttpStatusCode.NoContent;
                    response.Response = countryList;
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

        public ResponseMessage GetStateList(int id)
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
                cmd.CommandText = "GET_STATE_LIST";
                cmd.Parameters.AddWithValue("@COUNTRY_ID", id);
                da = new SqlDataAdapter(cmd);
                da.Fill(stateDt);

                if (stateDt.Rows.Count > 0)
                {
                    foreach (DataRow row in stateDt.Rows)
                    {
                        State state = new State();
                        state.countryId = Helper.GetDBIntValue(row["DESCRIPTION"]);
                        state.id = Helper.GetDBIntValue(row["ID"]);
                        state.state = Helper.GetDBStringValue(row["ID"]);

                        stateList.Add(state);
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
        #endregion
    }
}

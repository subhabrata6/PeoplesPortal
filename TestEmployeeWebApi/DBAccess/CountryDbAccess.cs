using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using TestEmployeeWebApi.HelperClass;
using TestEmployeeWebApi.Model;

namespace TestEmployeeWebApi.DBAccess
{
    public class CountryDbAccess
    {
        private readonly IConfiguration _configuration;
        public CountryDbAccess(IConfiguration configuration)
        {

            _configuration = configuration;

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
                        country.countryName = Helper.GetDBStringValue(row["COUNTRY_NAME"]);
                        country.createdOn = Helper.GetDBStringValue(row["CREATED_ON"]);
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

        public ResponseMessage SaveCountry(Country newCountry)
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
                cmd.CommandText = "SAVE_COUNTRY";
                cmd.Transaction = transaction;
                cmd.Parameters.AddWithValue("@ID", newCountry.id);
                cmd.Parameters.AddWithValue("@NAME", newCountry.countryName);

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

        public ResponseMessage DeleteCountryById(int id)
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
                cmd.CommandText = "DELETE_COUNTRY_BY_ID";
                cmd.Transaction = transaction;
                cmd.Parameters.AddWithValue("@ID", id);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {
                    transaction.Commit();
                    response.Status = HttpStatusCode.OK;
                    response.Message = "Deleted successfully";
                }
                else
                {
                    transaction.Rollback();
                    response.Status = HttpStatusCode.BadRequest;
                    response.Message = "Failed to delete this country";
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

        public ResponseMessage GetCountryById(int id)
        {
            ResponseMessage response = new ResponseMessage();

            string connStr = Helper.GetConnectionString(_configuration);

            SqlConnection conn = null;
            SqlDataAdapter da = null;
            SqlCommand cmd = null;
            DataTable countryDt = new DataTable();
            Country country = new Country();

            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "GET_COUNTRY_BY_ID";
                cmd.Parameters.AddWithValue("@ID", id);
                da = new SqlDataAdapter(cmd);
                da.Fill(countryDt);

                if (countryDt.Rows.Count > 0)
                {
                    country.countryName = Helper.GetDBStringValue(countryDt.Rows[0]["COUNTRY_NAME"]);
                    country.createdOn = Helper.GetDBStringValue(countryDt.Rows[0]["CREATED_ON"]);
                    country.id = Helper.GetDBIntValue(countryDt.Rows[0]["ID"]);



                    response.Status = HttpStatusCode.OK;
                    response.Response = country;
                    response.Message = "OK";
                }
                else
                {
                    response.Status = HttpStatusCode.NoContent;
                    response.Response = country;
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

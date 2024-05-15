using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using TestEmployeeApp.HelperClass;
using TestEmployeeApp.Model;

namespace TestEmployeeApp.DBAccess
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
    }
}

using System.Data.SqlClient;
using System.Data;
using System.Net;
using TestEmployeeApp.HelperClass;
using TestEmployeeApp.Model;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace TestEmployeeApp.DBAccess
{
    public class AccountDbAccess
    {
        private readonly IConfiguration _configuration;
        private const string encryptionKey = "LEO_MESSI_10";
        public AccountDbAccess(IConfiguration configuration)
        {

            _configuration = configuration;
        }
        public ResponseMessage AuthenticateUser(AccountModel account)
        {
            ResponseMessage response = new ResponseMessage();

            string connStr = Helper.GetConnectionString(_configuration);

            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataTable dt = new DataTable();

            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "LOGIN_USER";
                cmd.Parameters.AddWithValue("@EMAIL", account.Email);
                cmd.Parameters.AddWithValue("@PASSWORD", account.Password);
                string hashPassword = PasswordHelper.EncryptString(encryptionKey, account.Password);
                cmd.Parameters.AddWithValue("@HASH_PASSWORD", hashPassword);

                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    string UserName = Helper.GetDBStringValue(dt.Rows[0]["USER_NAME"]);
                    string Msg = Helper.GetDBStringValue(dt.Rows[0]["MSG"]);

                    if (!string.IsNullOrEmpty(UserName))
                    {
                        response.Status = HttpStatusCode.OK;
                        response.Response = UserName;
                        response.Message = Msg;
                    }
                    else
                    {
                        response.Status = HttpStatusCode.NotFound;
                        response.Message = Msg;
                    }
                }
                else
                {
                    response.Status = HttpStatusCode.BadRequest;
                    response.Message = "Failed to login";
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
            }

            return response;
        }

        public ResponseMessage RegisterUser(AccountModel account)
        {
            ResponseMessage response = new ResponseMessage();

            string connStr = Helper.GetConnectionString(_configuration);

            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataTable dt = new DataTable();

            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "CREATE_USER";
                Guid userId = Guid.NewGuid();
                cmd.Parameters.AddWithValue("@ID", userId);
                cmd.Parameters.AddWithValue("@EMAIL", account.Email);
                cmd.Parameters.AddWithValue("@NAME", account.UserName);
                cmd.Parameters.AddWithValue("@PASSWORD", account.Password);

                string hashPassword = PasswordHelper.EncryptString(encryptionKey, account.Password);
                cmd.Parameters.AddWithValue("@HASH_PASSWORD", hashPassword);

                da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {

                    string UserName = Helper.GetDBStringValue(dt.Rows[0]["USER_NAME"]);
                    string Msg = Helper.GetDBStringValue(dt.Rows[0]["MSG"]);

                    if (!string.IsNullOrEmpty(UserName))
                    {
                        response.Status = HttpStatusCode.OK;
                        response.Response = UserName;
                        response.Message = Msg;
                    }
                    else
                    {
                        response.Status = HttpStatusCode.NotFound;
                        response.Message = Msg;
                    }
                }
                else
                {
                    response.Status = HttpStatusCode.BadRequest;
                    response.Message = "Failed to register";
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
            }

            return response;
        }

        public ResponseMessage GetUserRoleList()
        {
            ResponseMessage response = new ResponseMessage();

            string connStr = Helper.GetConnectionString(_configuration);

            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataTable dt = new DataTable();
            List<UserRoleModel> roleList = new List<UserRoleModel>();

            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "GET_USER_ROLE_LIST";

                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        UserRoleModel userRole = new UserRoleModel();
                        userRole.Id = Helper.GetDBIntValue(row["Id"]);
                        userRole.Name = Helper.GetDBStringValue(row["Name"]);

                        roleList.Add(userRole);
                    }

                    response.Response = roleList;
                    response.Status = HttpStatusCode.OK;
                    response.Message = "Ok";
                }
                else
                {
                    response.Response = roleList;
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = "No role found";
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
            }

            return response;
        }

        public ResponseMessage SaveUserRole(UserRoleModel role)
        {
            ResponseMessage response = new ResponseMessage();

            string connStr = Helper.GetConnectionString(_configuration);

            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataTable dt = new DataTable();

            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "CREATE_USER_ROLE";
                cmd.Parameters.AddWithValue("@ID", role.Id);
                cmd.Parameters.AddWithValue("@ROLE", role.Name);

                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    response.Status = HttpStatusCode.OK;
                    response.Message = "Role saved successfully";
                }
                else
                {
                    response.Status = HttpStatusCode.BadRequest;
                    response.Message = "Failed to save role";
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
            }

            return response;
        }

        public ResponseMessage GetUserRoleById(int id)
        {
            ResponseMessage response = new ResponseMessage();

            string connStr = Helper.GetConnectionString(_configuration);

            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataTable dt = new DataTable();

            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "GET_USER_ROLE_BY_ID";
                cmd.Parameters.AddWithValue("@ID", id);

                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {

                    UserRoleModel userRole = new UserRoleModel();
                    userRole.Id = Helper.GetDBIntValue(dt.Rows[0]["Id"]);
                    userRole.Name = Helper.GetDBStringValue(dt.Rows[0]["Name"]);

                    response.Response = userRole;
                    response.Status = HttpStatusCode.OK;
                    response.Message = "Ok";
                }
                else
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = "No role found";
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
            }

            return response;
        }

        public ResponseMessage DeleteUserRoleById(int id)
        {
            ResponseMessage response = new ResponseMessage();

            string connStr = Helper.GetConnectionString(_configuration);

            SqlConnection conn = null;
            SqlCommand cmd = null;

            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "DELETE_USER_ROLE_BY_ID";
                cmd.Parameters.AddWithValue("@ID", id);

                int rows = cmd.ExecuteNonQuery();

                if (rows > 0)
                {

                    response.Status = HttpStatusCode.OK;
                    response.Message = "Role deleted successfully";
                }
                else
                {
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = "No role found";
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
            }

            return response;
        }
    }
}

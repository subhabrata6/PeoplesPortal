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
using System.Dynamic;

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
                    string userRole = Helper.GetDBStringValue(dt.Rows[0]["ROLE"]);
                    Guid userId = Helper.GetDBGuidValue(dt.Rows[0]["USER_ID"]);

                    AssignRole responseObj = new AssignRole();
                    responseObj.UserName = UserName;
                    responseObj.RoleName = userRole;
                    responseObj.UserId = userId;

                    if (!string.IsNullOrEmpty(UserName))
                    {
                        response.Status = HttpStatusCode.OK;
                        response.Response = responseObj;
                        response.Message = Msg;
                    }
                    else
                    {
                        response.Status = HttpStatusCode.NotFound;
                        response.Message = Msg;
                        response.Response = responseObj;
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

        public ResponseMessage GetRoleList()
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
                cmd.Parameters.AddWithValue("@IS_ADMIN", role.IsAdmin);

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
                    userRole.IsAdmin = Helper.GetDBBoolValue(dt.Rows[0]["IsAdmin"]);

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

        public ResponseMessage GetUserList()
        {
            ResponseMessage response = new ResponseMessage();

            string connStr = Helper.GetConnectionString(_configuration);

            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataTable dt = new DataTable();
            List<User> userList = new List<User>();

            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "GET_USER_LIST";

                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        User user= new User();
                        user.Id = Helper.GetDBGuidValue(row["Id"]);
                        user.UserName = Helper.GetDBStringValue(row["UserName"]);

                        userList.Add(user);
                    }

                    response.Response = userList;
                    response.Status = HttpStatusCode.OK;
                    response.Message = "Ok";
                }
                else
                {
                    response.Response = userList;
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

        public ResponseMessage AssignRoleToUser(AssignRole assign)
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
                cmd.CommandText = "ASSIGN_USER_ROLE";
                cmd.Parameters.AddWithValue("@ID", assign.Id);
                cmd.Parameters.AddWithValue("@USER", assign.UserId);
                cmd.Parameters.AddWithValue("@ROLE", assign.RoleId);

                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    response.Status = HttpStatusCode.OK;
                    response.Message = "Role assigned successfully";
                }
                else
                {
                    response.Status = HttpStatusCode.BadRequest;
                    response.Message = "Failed to assign role";
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
            List<AssignRole> roleList = new List<AssignRole>();

            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "GET_USER_ROLE_DETAILS_LIST";

                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        AssignRole userRole = new AssignRole();
                        userRole.Id = Helper.GetDBIntValue(row["Id"]);
                        userRole.UserName = Helper.GetDBStringValue(row["UserName"]);
                        userRole.UserId = Helper.GetDBGuidValue(row["UserId"]);
                        userRole.RoleId = Helper.GetDBIntValue(row["RoleId"]);
                        userRole.RoleName = Helper.GetDBStringValue(row["RoleName"]);

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

        public ResponseMessage GetAssignedUserRoleById(int id)
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
                cmd.CommandText = "GET_ASSIGNED_USER_ROLE_BY_ID";
                cmd.Parameters.AddWithValue("@ID", id);

                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {

                    AssignRole userRole = new AssignRole();
                    userRole.Id = Helper.GetDBIntValue(dt.Rows[0]["Id"]);
                    userRole.UserName = Helper.GetDBStringValue(dt.Rows[0]["UserName"]);
                    userRole.UserId = Helper.GetDBGuidValue(dt.Rows[0]["UserId"]);
                    userRole.RoleId = Helper.GetDBIntValue(dt.Rows[0]["RoleId"]);
                    userRole.RoleName = Helper.GetDBStringValue(dt.Rows[0]["RoleName"]);

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

        public ResponseMessage DeleteAssignedRoleToUser(int id)
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
                cmd.CommandText = "DELETE_ASSIGNED_USER_ROLE";
                cmd.Parameters.AddWithValue("@ID", id);

                int rows = cmd.ExecuteNonQuery();
                if (rows > 0)
                {
                    response.Status = HttpStatusCode.OK;
                    response.Message = "Role deleted successfully";
                }
                else
                {
                    response.Status = HttpStatusCode.BadRequest;
                    response.Message = "Failed to delete role";
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

        public ResponseMessage GetUsersViewList()
        {
            ResponseMessage response = new ResponseMessage();

            string connStr = Helper.GetConnectionString(_configuration);

            SqlConnection conn = null;
            SqlCommand cmd = null;
            SqlDataAdapter da = null;
            DataTable dt = new DataTable();
            List<UserView> userList = new List<UserView>();

            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                cmd = new SqlCommand();
                cmd.Connection = conn;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.CommandText = "GET_USER_VIEW_LIST";

                da = new SqlDataAdapter(cmd);
                da.Fill(dt);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        UserView user = new UserView();
                        user.Password = Helper.GetDBStringValue(row["Password"]);
                        user.Username = Helper.GetDBStringValue(row["UserName"]);
                        user.Email = Helper.GetDBStringValue(row["Email"]);

                        userList.Add(user);
                    }

                    response.Response = userList;
                    response.Status = HttpStatusCode.OK;
                    response.Message = "Ok";
                }
                else
                {
                    response.Response = userList;
                    response.Status = HttpStatusCode.NotFound;
                    response.Message = "No users found";
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

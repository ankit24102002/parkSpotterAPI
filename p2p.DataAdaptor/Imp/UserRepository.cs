using Azure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;
using p2p.Common.Models;
using p2p.DataAdaptor.Contract;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static p2p.Common.Models.Space;
using static p2p.Common.Models.Users;

namespace p2p.DataAdaptor.Imp
{
    public class UserRepository : IUserRepository
    {

        private readonly ILogger<UserRepository> _logger;
        private readonly IConfiguration _configuration;
        public UserRepository(
         ILogger<UserRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _logger.LogDebug("NLog is integrated to UserRepository  ");
            _configuration = configuration;
        }

        public UserResponseData SignUp(User userdata)
        {
            UserResponseData response = new UserResponseData() { IsSaved = false, Message = "" };


            string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                try
                {
                    using (SqlCommand command = new SqlCommand("SignUpUser", connection))
                    {
                        connection.Open();
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Username", userdata.Username);
                        command.Parameters.AddWithValue("@Email", userdata.Email);
                        command.Parameters.AddWithValue("@Password", userdata.Password);
                        command.Parameters.AddWithValue("@Phone_Number", userdata.phoneno);
                        command.Parameters.AddWithValue("@rolename", userdata.roleName);

                        command.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                        command.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;


                        int i = command.ExecuteNonQuery();
                        connection.Close();
                        string message = (string)command.Parameters["@ErrorMessage"].Value;

                        if (i > 0)
                        {
                            response.Message = message;
                            response.IsSaved = true;
                        }
                        else
                        {
                            response.Message = message;
                            response.IsSaved = false;
                        }

                    }
                }
                catch (Exception ex)
                {
                    response.Message = ex.Message;
                    response.IsSaved = false;
                }
                return response;
            }

        }

        public AuthUser Login(AuthUser userdata)
        {
            string Username;
            string Password;

            string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                using (SqlCommand command = new SqlCommand("VerifyUser", connection))
                {
                    connection.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@Username", userdata.Username);
                    //     command.Parameters.AddWithValue("@Password", userdata.Password);


                    command.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                    command.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;


                    int i = command.ExecuteNonQuery();// non-query,scaller,readwer

                    //    string message = (string)command.Parameters["@ErrorMessage"].Value;

                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            AuthUser user = new AuthUser
                            {
                                Username = reader.GetString(0),
                                //   Password = reader.GetString(1)
                            };
                            return user;
                        }
                    }

                    connection.Close();
                }
                return null;

            }

        }

        public AuthUser VerifyUser(AuthUser userdata)
        {
            string connString = "server=ANKIT; database=BlogPostDatabase; trusted_connection=true; Encrypt=False;";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                string databaseName = "p2p";
                string sqlQuery = $"USE {databaseName}; select Username from User_Master where Username=@Username";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@Username", userdata.Username);
                    int i = command.ExecuteNonQuery();// non-query,scaller,readwer

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AuthUser user = new AuthUser
                            {
                                Username = reader.GetString(0),

                            };
                            return user;
                        }
                    }

                }
            }
            return null;
        }

        public LoginUser VerifyLoginUser(LoginUser userdata)
        {
            string connString = "server=ANKIT; database=BlogPostDatabase; trusted_connection=true; Encrypt=False;";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                string databaseName = "p2p";
                string sqlQuery = $"USE {databaseName}; select Username,Password from User_Master where Username=@Username and Password=@Password";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@Username", userdata.Username);
                    command.Parameters.AddWithValue("@Password", userdata.Password);
                    command.ExecuteNonQuery();// non-query,scaller,readwer

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LoginUser user = new LoginUser
                            {
                                Username = reader.GetString(0),
                                Password = reader.GetString(1),

                            };
                            return user;
                        }
                    }

                }
            }
            return null;
        }

        public UserData GetLoginUserData(LoginUser userdata)
        {
            string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
            string databaseName = "p2p";
            //   string qry = $"USE {databaseName};SELECT UserId,Username, Email,Password, Phone_Number, RoleID FROM User_Master where Username=@Username and Password=@Password";
            string qry = $"USE {databaseName};SELECT UserId,Username, Email,Password, Phone_Number, RoleID FROM User_Master where Username=@Username ";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                System.Diagnostics.Debug.WriteLine($"\nRunning Query : {qry};\n");

                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", userdata.Username);
                    //    cmd.Parameters.AddWithValue("@Password", userdata.Password);
                    //     cmd.CommandType = CommandType.Text;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            UserData item = new UserData
                            {
                                UserId = (int)reader["UserId"],
                                Username = reader["Username"].ToString(),
                                Email = reader["Email"].ToString(),
                                Password = reader["Password"].ToString(),
                                phoneno = reader["Phone_Number"].ToString(),
                                RoleID = (int)reader["RoleID"],
                            };
                            return item;
                        }
                        else
                        {
                            return null;
                        }

                    }

                }
                conn.Close();
            }

        }

        public UserData checkemail(string email)
        {
            string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
            string databaseName = "p2p";
            string qry = $"USE {databaseName};SELECT UserId,Username, Email,Password, Phone_Number, RoleID,ResetPasswordToken,ResetPasswordExpiry FROM User_Master where Email=@Email ";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();



                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            UserData item = new UserData
                            {
                                UserId = (int)reader["UserId"],
                                Username = reader["Username"].ToString(),
                                Email = reader["Email"].ToString(),
                                Password = reader["Password"].ToString(),
                                phoneno = reader["Phone_Number"].ToString(),
                                RoleID = (int)reader["RoleID"],
                                ResetPasswordToken =reader["ResetPasswordToken"].ToString(),
                                //  ResetPasswordExpiry = (DateTime)reader["ResetPasswordExpiry"],
                                ResetPasswordExpiry = (reader["ResetPasswordExpiry"] == DBNull.Value) ? DateTime.MinValue : (DateTime)reader["ResetPasswordExpiry"]

                        };
                            return item;
                        }
                        else
                        {
                            return null;
                        }

                    }

                }
                conn.Close();
            }

        }

        public UserResponseData updatetoken(UserData user)
        {
            string connString = "server=ANKIT; database=userdatabase; trusted_connection=true; Encrypt=False;";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                UserResponseData response = new UserResponseData() { IsSaved = false, Message = "" };

                connection.Open();
                string databaseName = "p2p";
                string sql = $@"USE {databaseName};UPDATE User_Master set ResetPasswordToken = '{user.ResetPasswordToken}',ResetPasswordExpiry='{user.ResetPasswordExpiry}' WHERE Username = '{user.Username}'";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        response.IsSaved = true;

                    }
                    return response;
                }
            }

        }

        public UserResponseData updatepassword(string password,string username){
                string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
                using (SqlConnection connection = new SqlConnection(connString))
                {
                UserResponseData response = new UserResponseData() { IsSaved = false, Message = "" };

                string databaseName = "p2p";
                    string sqlQuery = $"USE {databaseName};UPDATE User_Master SET Password = @new_password WHERE Username = @username ";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@username", username);
                        command.Parameters.AddWithValue("@new_password", password);
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        response.IsSaved = true;

                    }
                    return response;

                }
            }
            }

        public UserData Profiledata(AuthUser user)
        {

            string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
            string databaseName = "p2p";
            //   string qry = $"USE {databaseName};SELECT UserId,Username, Email,Password, Phone_Number, RoleID FROM User_Master where Username=@Username and Password=@Password";
            string qry = $"USE {databaseName};SELECT UserId,Username, Email,Password, Phone_Number, RoleID FROM User_Master where Username=@Username ";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                System.Diagnostics.Debug.WriteLine($"\nRunning Query : {qry};\n");

                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    //    cmd.Parameters.AddWithValue("@Password", userdata.Password);
                    //     cmd.CommandType = CommandType.Text;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            UserData item = new UserData
                            {
                                UserId = (int)reader["UserId"],
                                Username = reader["Username"].ToString(),
                                Email = reader["Email"].ToString(),
                                Password = reader["Password"].ToString(),
                                phoneno = reader["Phone_Number"].ToString(),
                                RoleID = (int)reader["RoleID"],
                            };
                            return item;
                        }
                        else
                        {
                            return null;
                        }

                    }

                }
                conn.Close();
            }
       }

        public UserResponseData UpdateProfiledata(UserProfileData userProfileData)
        {
            try
            {

                string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    UserResponseData response = new UserResponseData() { IsSaved = false, Message = "" };

                    connection.Open();            
                    using (SqlCommand command = new SqlCommand("UpdateUserProfile", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@Username", userProfileData.Detail.Username);
                        command.Parameters.AddWithValue("@Email", userProfileData.Detail.Email);
                        command.Parameters.AddWithValue("@Phone_Number", userProfileData.Detail.Phoneno);

                        command.Parameters.Add("@ErrorMessage", System.Data.SqlDbType.Char, 200);
                        command.Parameters["@ErrorMessage"].Direction = System.Data.ParameterDirection.Output;


                        int i = command.ExecuteNonQuery();
                        connection.Close();
                        string message = (string)command.Parameters["@ErrorMessage"].Value;

                        if (i > 0)
                        {
                            response.Message = message;
                            response.IsSaved = true;
                        }
                        else
                        {
                            response.Message = message;
                            response.IsSaved = false;
                        }
                        return response;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Updating profile: {ex.Message}");
                return null;
            }
        }

        public List<string> GetSuggestedUsernames(string input)
        {

            var similarUsernames = new List<string>();
            string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
            string databaseName = "p2p";
            string qry = $"USE {databaseName};SELECT Username FROM User_Master WHERE Username LIKE '%' + @Input + '%'";

            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(qry, conn))
                {
                    cmd.Parameters.AddWithValue("@Input", input);


                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            similarUsernames.Add(reader.GetString(0));
                        }
                    }
                }
                conn.Close();
            }
                   
                        return similarUsernames;
                    }


    }
}

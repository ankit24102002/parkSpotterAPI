using Microsoft.Extensions.Logging;
using p2p.Common.Models;
using p2p.DataAdaptor.Contract;
using p2p.DataAdaptor.SqlManager;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static p2p.Common.Models.Admin;
using static p2p.Common.Models.Space;

namespace p2p.DataAdaptor.Imp
{
    public class AdminRepository:IAdminRepository
    {

        private readonly ILogger<AdminRepository> _logger;
        public AdminRepository(ILogger<AdminRepository> logger)
        {
            _logger = logger;
            _logger.LogDebug("NLog is integrated to Customer repository Controller");
        }


        public List<Cus_and_own_detail> AllUserList(input_1 detail)
        {
            try
            {
                string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
                List<Cus_and_own_detail> spaces = new List<Cus_and_own_detail>();
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    string sql = $@"SELECT Username,Email,phone_number,Enable FROM User_Master where RoleID='{detail.Roleid}'";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Cus_and_own_detail item = new Cus_and_own_detail
                                {

                                    Username = reader["Username"].ToString(),
                                    Enable = (Boolean)reader["Enable"],
                                    Phoneno = reader["phone_number"].ToString(),
                                    Email = reader["Email"].ToString(),
                                };
                                spaces.Add(item);
                            }
                        }
                        return spaces;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                Console.WriteLine($"Error getting spaces: {ex.Message}");
                return null;
            }
        }

            public ResponseData EnableDisable(ongoning_booking detail)
            {
                ResponseData responce = new ResponseData() { IsSaved = false, Message = "" };

                string databaseName = "p2p";


            //able to solve after convertig it to toString()
                string qry = $"USE {databaseName};UPDATE User_Master set Enable = '{detail.Enable}' WHERE Username = '{detail.Username}'";

                responce.IsSaved = SqlConnectionManager.DeleteRecord(qry);
                if (responce.IsSaved)
                {
                    responce.Message = "Success";
                }
                return responce;

            }
        

        public ResponseData ContactUs(contactus info)
        {
            ResponseData response = new ResponseData() { IsSaved = false, Message = "" };


            string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();
                    string databaseName = "p2p";
                    string sql = @$"USE {databaseName};Insert into Query (Username,Q_Email,Message,Q_Username)
                                        values ('{info.Username}','{info.Q_Email}','{info.Q_Message}','{info.Q_Username}')";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {

                        int i = command.ExecuteNonQuery();
                        connection.Close();


                        if (i > 0)
                        {
                            response.Message = "added";
                            response.IsSaved = true;
                        }
                        else
                        {
                            response.Message = "not added";
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

    }
}

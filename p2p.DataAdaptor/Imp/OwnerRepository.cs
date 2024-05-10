using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using p2p.Common.Models;
using p2p.DataAdaptor.Contract;
using p2p.DataAdaptor.SqlManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static p2p.Common.Models.Space;
using static p2p.Common.Models.Users;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace p2p.DataAdaptor.Imp
{
    public class OwnerRepository: IOwnerRepository 
    {
        private readonly ILogger<OwnerRepository> _logger;
        private readonly IConfiguration _configuration;
        public OwnerRepository(ILogger<OwnerRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _logger.LogDebug("NLog is integrated to Customer repository Controller");
            _configuration = configuration;
        }

        public ResponseData DeleteSpace(int spaceid)
        {
            ResponseData responce = new ResponseData() { IsSaved = false, Message = "" };

            string databaseName = "p2p";


          
           // string qry = $"USE {databaseName};DELETE FROM Space_Owner_Master  WHERE SpaceID= {spaceid}";


            string qry = $@"USE {databaseName};DELETE Space_Owner_Master
WHERE SpaceID =  '{spaceid}'
AND NOT EXISTS (
    SELECT 1
    FROM BookingDetail
    WHERE BookingDetail.SpaceID = Space_Owner_Master.SpaceID
    AND BookingDetail.Enable = 1
)";

            responce.IsSaved = SqlConnectionManager.DeleteRecord(qry);
            if (responce.IsSaved)
            {
                responce.Message = "Item deleted Successfully";
            }
            return responce;

        }

        public List<cur_pasbooking> GetCurrentbooking(ongoning_booking detail)
        {
            try
            {
                string connString = _configuration["ConnectionStrings:dbcs"];
              //  string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
                List<cur_pasbooking> spaces = new List<cur_pasbooking>();
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    string sql = $@" 
 SELECT 
 som.SpaceID as SpaceID ,
    som.Address_Appartment_no as Address_Appartment_no,
	som.Address_District as Address_District,
    som.Address_street as Address_street,
    som.Space_Image_Path,
	som.Description as Description,
    bd.booking_amount AS bookingAmount,
    bd.StartBooking AS startbooking,
    bd.EndBooking AS endbooking,
    um.Username AS c_username,
    um.Email AS c_email,
    um.Phone_Number AS c_phoneno,
	rv.rating as rating,
bd.bookingID as bookingid
FROM 
    Space_Owner_Master AS som
JOIN 
    BookingDetail AS bd ON som.SpaceID = bd.SpaceID
JOIN 
    User_Master AS um ON bd.Username = um.Username
LEFT JOIN 
    Review AS rv ON bd.SpaceID = rv.SpaceID AND um.Username = rv.Username  And bd.bookingID=rv.bookingID
WHERE 
    som.Username ='{detail.Username}'AND bd.Enable = '{detail.Enable}'
GROUP BY 
    som.SpaceID,som.Address_Appartment_no,som.Address_District, som.Address_street,som.Description, bd.booking_amount, bd.StartBooking,
    bd.EndBooking, um.Username, um.Email, um.Phone_Number,rv.rating,som.Space_Image_Path,bd.bookingID ";




                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                cur_pasbooking item = new cur_pasbooking
                                {
                                    SpaceID = (int)reader["SpaceID"],
                              //      Username = reader["Username"].ToString(),
                               //     RoleID = (int)reader["RoleID"],
                              //      Longitude = reader["Longitude"].ToString(),
                              //      Latitude = reader["Latitude"].ToString(),
                                    Description = reader["Description"].ToString(),
                              //      Price = Convert.ToDecimal(reader["Price"]),
                                    Address_Appartment_no = reader["Address_Appartment_no"].ToString(),
                                    Address_street = reader["Address_street"].ToString(),
                                    Address_District = reader["Address_District"].ToString(),
                               //     Enable = (bool)reader["Enable"],
                                //    Created_By = reader["Created_By"].ToString(),
                               //     Created_Date = (DateTime)reader["Created_Date"],
                                  //  Modified_By = reader["Modified_By"].ToString(),
                                    Space_Image_Path = reader["Space_Image_Path"].ToString(),
                                    //    Modified_Date = (DateTime)reader["Modified_Date"],
                                    bookedamount= (decimal)reader["bookingAmount"],
                                    startdate= (DateTime)reader["startbooking"],
                                    c_email = reader["c_email"].ToString(),
                                    enddate=(DateTime)reader["endbooking"],
                                    c_phoneno = reader["c_phoneno"].ToString(),
                                    c_username = reader["c_username"].ToString(),
                                    //rating= (int)reader["rating"],
                                    rating = reader["rating"] is DBNull ? 0 : (int)reader["rating"],
                                    bookingid = (int)reader["bookingid"],

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

        public List<current_booking> GetBySpaceIDowner(int SpaceID)
        {
            string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
            List<current_booking> spaces = new List<current_booking>();
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();



                string sql = $@"  SELECT 
                                  So.Username as ownername,
So.Address_Appartment_no as Address_Appartment_no,
SO.Address_street as Address_street,
So.Address_District as Address_District,
                                  B.SpaceID,
                                  booking_amount,
                                  StartBooking,
                                  EndBooking,
                                  Space_Image_Path,  
                                  Description,
                                  B.Username AS Customer,
                                  Email,
                                  Phone_Number
                                  FROM  BookingDetail AS B
                                  INNER JOIN 
                                  Space_Owner_Master AS SO ON B.SpaceID = SO.SpaceID
                                  INNER JOIN 
                                  User_Master AS UM ON B.Username = UM.Username
                                  WHERE 
                                  B.SpaceID = {SpaceID} and B.Enable=1 ";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            //  DateTime dateTimeValue = (DateTime)reader["StartBooking"];
                            current_booking item = new current_booking
                            {
                                ownerUsername = reader["ownername"].ToString(),
                                Address_Appartment_no = reader["Address_Appartment_no"].ToString(),
                                Address_street = reader["Address_street"].ToString(),
                                Address_District = reader["Address_District"].ToString(),
                                SpaceID = (int)reader["SpaceID"],
                                booking_amount = Convert.ToInt32(reader["booking_amount"]),
                                Description = reader["Description"].ToString(),
                                StartBooking = (DateTime)reader["StartBooking"],
                                EndBooking = (DateTime)reader["EndBooking"],
                                Email = reader["Email"].ToString(),
                                Phone_Number = reader["Phone_Number"].ToString(),
                                customername = reader["customer"].ToString(),
                                Space_Image_Path = reader["Space_Image_Path"].ToString(),
                            };
                            spaces.Add(item);
                        }
                    }
                    return spaces;
                }
            }
        }

        public List<all_spaces> GetAllSpacesbyusername(string username)
        {
            string connString = _configuration["ConnectionStrings:dbcs"];
           // string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
            List<all_spaces> spaces = new List<all_spaces>();
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                //         string sql = $@"SELECT * FROM Space_Owner_Master WHERE username = '{username}' ";


                string sql = $@"SELECT 
    som.SpaceID as SpaceID,
    Price,
	Description,
Enable,
  Address_Appartment_no,
  Address_street,
  Address_District,
Space_Image_Path,
    COALESCE(AVG(rev.Rating), 0) AS AverageRating,
    COUNT(rev.SpaceID) AS NumberOfUsers
FROM 
    Space_Owner_Master AS som
LEFT JOIN 
    Review AS rev ON som.SpaceID = rev.SpaceID
WHERE 
    som.username = '{username}'
GROUP BY 
    som.SpaceID, Price,
	Description,
  Address_Appartment_no,
  Address_street,
  Address_District,
Enable,
Space_Image_Path";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    //  command.Parameters.AddWithValue("@Location", Location);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            all_spaces item = new all_spaces
                            {
                                SpaceID = (int)reader["SpaceID"],
                              //  Username = reader["Username"].ToString(),
                            //  RoleID = (int)reader["RoleID"],
                             //  Longitude = reader["Longitude"].ToString(),
                             //   Latitude = reader["Latitude"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = Convert.ToInt32(reader["Price"]),
                                Address_Appartment_no = reader["Address_Appartment_no"].ToString(),
                                Address_street = reader["Address_street"].ToString(),
                                Address_District = reader["Address_District"].ToString(),
                                    Enable = (bool)reader["Enable"],
                                //    Created_By = reader["Created_By"].ToString(),
                                //    Created_Date = (DateTime)reader["Created_Date"],
                                //     Modified_By = reader["Modified_By"].ToString(),
                                  Space_Image_Path = reader["Space_Image_Path"].ToString(),
                                //    Modified_Date = (DateTime)reader["Modified_Date"],
                                AverageRating = (int)reader["AverageRating"],
                                NumberOfUsers = (int)reader["NumberOfUsers"],


                            };
                            spaces.Add(item);
                        }
                    }
                    return spaces;
                }
            }
        }

        public ResponseData Enablespace(int spaceid)
        {
            try
            {
                string connString = _configuration["ConnectionStrings:dbcs"];
              //  string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    ResponseData response = new ResponseData() { IsSaved = false, Message = "" };

                    connection.Open();
                    string databaseName = "p2p";
                  //  string sql = $@"USE {databaseName};UPDATE Space_Owner_Master set Enable = 1 WHERE SpaceID = '{spaceid}'";

                    string sql = $@"USE {databaseName};UPDATE Space_Owner_Master
SET Enable = 1
WHERE SpaceID =  '{spaceid}'
AND NOT EXISTS (
    SELECT 1
    FROM BookingDetail
    WHERE BookingDetail.SpaceID = Space_Owner_Master.SpaceID
    AND BookingDetail.Enable = 1
)";
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
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting post: {ex.Message}");
                return null;
            }
        }

        public ResponseData Disablespace(int spaceid)
        {
            try {
                string connString = _configuration["ConnectionStrings:dbcs"];
               // string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    ResponseData response = new ResponseData() { IsSaved = false, Message = "" };

                    connection.Open();
                    string databaseName = "p2p";

                    string sql = $@"USE {databaseName};UPDATE Space_Owner_Master
SET Enable = 0
WHERE SpaceID =  '{spaceid}'
AND NOT EXISTS (
    SELECT 1
    FROM BookingDetail
    WHERE BookingDetail.SpaceID = Space_Owner_Master.SpaceID
    AND BookingDetail.Enable = 1)";

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
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting post: {ex.Message}");
                return null;
            }
        }

        public ResponseData Addspace(addspace space)
        {
            ResponseData response = new ResponseData() { IsSaved = false, Message = "" };

            string connString = _configuration["ConnectionStrings:dbcs"];
          //  string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();
                    string databaseName = "p2p";
                    string sql = @$"USE {databaseName};Insert into Space_Owner_Master (RoleID,Username,Longitude,Latitude,Description,Price,Address_Appartment_no,Address_street,Address_District,Space_Image_Path,Created_By,Created_Date,Modified_By,Modified_Date,Enable)
                                        values (2,'{space.Username}','{space.Longitude}','{space.Latitude}','{space.Description}','{space.Price}','{space.Address_Appartment_no}','{space.Address_street}','{space.Address_District}','{space.Space_Image_Path}','{space.Username}','{DateTime.Now}','','',1)";

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

        public List<detail_space> OwnerGetSpaceDetail(int spaceid)
        {
            string connString = _configuration["ConnectionStrings:dbcs"];
           // string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
            List<detail_space> spaces = new List<detail_space>();
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                  string sql = $@"SELECT Longitude,Latitude,Description,Price,Address_Appartment_no,Address_street,Address_District,Space_Image_Path
                  FROM Space_Owner_Master WHERE SpaceID = '{spaceid}' ";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            detail_space item = new detail_space
                            {
                             //   SpaceID = (int)reader["SpaceID"],
                             //   Username = reader["Username"].ToString(),
                              //  RoleID = (int)reader["RoleID"],
                                Longitude = reader["Longitude"].ToString(),
                                Latitude = reader["Latitude"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = Convert.ToInt32(reader["Price"]),
                                Address_Appartment_no = reader["Address_Appartment_no"].ToString(),
                                Address_street = reader["Address_street"].ToString(),
                                Address_District = reader["Address_District"].ToString(),
                              //  Enable = (bool)reader["Enable"],
                              //  Created_Date = (DateTime)reader["Created_Date"],
                                Space_Image_Path = reader["Space_Image_Path"].ToString(),
                             
                            };
                            spaces.Add(item);
                        }
                    }
                    return spaces;
                }



            }
        }
    
        public ResponseData UpdateSpace(update_space spacedata)
        {
            try
            {
                string connString = _configuration["ConnectionStrings:dbcs"];
              //  string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
                using (SqlConnection connection = new SqlConnection(connString))
                {
                   ResponseData response = new ResponseData() { IsSaved = false, Message = "" };

                    connection.Open();
              
                    string databaseName = "p2p";
                    string sql = @$"USE {databaseName};UPDATE  Space_Owner_Master set Longitude='{spacedata.Detail.Longitude}',Latitude='{spacedata.Detail.Latitude}',
                    Description='{spacedata.Detail.Description}',Price='{spacedata.Detail.Price}',Address_Appartment_no='{spacedata.Detail.Address_Appartment_no}',Address_street='{spacedata.Detail.Address_street}'
                    ,Address_District='{spacedata.Detail.Address_District}',Space_Image_Path='{spacedata.Detail.Space_Image_Path}'WHERE SpaceID = '{spacedata.SpaceID}';";
                
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                      
                        int i = command.ExecuteNonQuery();
                        connection.Close();
                     
                        if (i > 0)
                        {
                            response.Message = "Sucess";
                            response.IsSaved = true;
                        }
                        else
                        {
                            response.Message = "Failed";
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

    }
}

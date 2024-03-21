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

namespace p2p.DataAdaptor.Imp
{
    public class CustomerRepository : ICustomerRepository
    {

        private readonly ILogger<CustomerRepository> _logger;
        private readonly IConfiguration _configuration;
        public CustomerRepository(
         ILogger<CustomerRepository> logger, IConfiguration configuration)
        {
            _logger = logger;
            _logger.LogDebug("NLog is integrated to Customer repository Controller");
            _configuration = configuration;
        }

        //Returnig Info Of spaces by location and by avaliablity
        public List<Space_Owner_Master> GetByLocation(string Longitude, string Latitude)
        {/*
            string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
            List<Space_Owner_Master> spaces = new List<Space_Owner_Master>();
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                string sql = $@"SELECT * FROM Space_Owner_Master WHERE Location = '{Location}' and Enable=1";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                  //  command.Parameters.AddWithValue("@Location", Location);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Space_Owner_Master item = new Space_Owner_Master
                            {
                                SpaceID = (int)reader["SpaceID"],
                                Username = reader["Username"].ToString(),
                                RoleID = (int)reader["RoleID"],
                                Location = reader["Location"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = Convert.ToInt32(reader["Price"]),
                                Address_Appartment_no = reader["Address_Appartment_no"].ToString(),
                                Address_street = reader["Address_street"].ToString(),
                                Address_District = reader["Address_District"].ToString(),
                                Enable = (Boolean)reader["Enable"]
                            };
                            spaces.Add(item);
                        }
                    }
                    return spaces;
                }
            }
            */

            List<Space_Owner_Master> Spaces = new List<Space_Owner_Master>();
            string databaseName = "p2p";
            string sql = $@"USE {databaseName};SELECT * FROM Space_Owner_Master WHERE Longitude = '{Longitude}' and Latitude = '{Latitude}' ";

            Spaces = SqlConnectionManager.GetDataFromSql<Space_Owner_Master>(sql);
            return Spaces;
        }

        public List<Bookingdetail> GetBySpaceID(int SpaceID)
        {
            //    string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
            string connString = _configuration["ConnectionStrings:dbcs"];
            List<Bookingdetail> spaces = new List<Bookingdetail>();
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                string sql = $@"WITH cte AS (
    SELECT 
        so.*,                           
        um.Phone_Number AS ownerphoneno,
        um.email AS owneremail
    FROM  
        Space_Owner_Master AS so 
    LEFT JOIN 
        User_Master AS um ON so.username = um.username
    WHERE  
        so.SpaceID ='{SpaceID}' 
 ), 

cte2 AS (
    SELECT 
        CASE
            WHEN bd.EndBooking IS NULL THEN null
            ELSE bd.EndBooking 
        END AS bookingEnd,
        bd.SpaceID
    FROM 
        BookingDetail AS bd  
    WHERE 
        bd.SpaceID = '{SpaceID}'  AND bd.Enable = 1
)
SELECT 
	c.*,
    COALESCE(c2.bookingEnd, Null) AS bookingEnd
FROM 
    cte as c
LEFT JOIN 
    cte2 as c2 ON c.SpaceID = c2.SpaceID;
 ";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            // DateTime utcNow = DateTime.UtcNow;
                            // Console.WriteLine("Current UTC Time: " + utcNow);

                            //   DateTime bookingEnd = (reader["bookingEnd"] as DateTime?) ?? DateTime.Now;

                            // Subtract 5hr 30min when  bookingEnd is set to DateTime.Now
                            //     if (Math.Abs((bookingEnd - DateTime.Now).TotalMinutes) < 1)
                            //     {
                            //         bookingEnd = bookingEnd.AddHours(-5).AddMinutes(-30);
                            //     }
                            Bookingdetail item = new Bookingdetail
                            {
                                SpaceID = (int)reader["SpaceID"],
                                Username = reader["Username"].ToString(),
                                RoleID = (int)reader["RoleID"],
                                Longitude = reader["Longitude"].ToString(),
                                Latitude = reader["Latitude"].ToString(),
                                Description = reader["Description"].ToString(),
                                Price = Convert.ToInt32(reader["Price"]),
                                Address_Appartment_no = reader["Address_Appartment_no"].ToString(),
                                Address_street = reader["Address_street"].ToString(),
                                Address_District = reader["Address_District"].ToString(),
                                Enable = (Boolean)reader["Enable"],
                                Created_By = reader["Created_By"].ToString(),
                                Created_Date = (DateTime)reader["Created_Date"],
                                Modified_By = reader["Modified_By"].ToString(),
                                Space_Image_Path = reader["Space_Image_Path"].ToString(),
                                Modified_Date = (DateTime)reader["Modified_Date"],
                                phoneno = reader["ownerphoneno"].ToString(),
                                email = reader["owneremail"].ToString(),
                                //  enddate = (DateTime)reader["bookingEnd"],
                                enddate = (reader["bookingEnd"] as DateTime?) ?? DateTime.UtcNow,
                                //    enddate = bookingEnd



                            };
                            spaces.Add(item);
                        }
                    }
                    return spaces;
                }
            }
        }

        public List<Space_Owner_Master> GetAllSpaces()
        {
            try
            {
                // string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
                string connString = _configuration["ConnectionStrings:dbcs"];
                List<Space_Owner_Master> spaces = new List<Space_Owner_Master>();
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    //     string sql = " select   * from Space_Owner_Master as som join BookingDetail as bd  on som.SpaceID=bd.SpaceID where som.Username='aa'";
                    string sql = "SELECT * FROM Space_Owner_Master";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Space_Owner_Master item = new Space_Owner_Master
                                {
                                    SpaceID = (int)reader["SpaceID"],
                                    Username = reader["Username"].ToString(),
                                    RoleID = (int)reader["RoleID"],
                                    Longitude = reader["Longitude"].ToString(),
                                    Latitude = reader["Latitude"].ToString(),
                                    Description = reader["Description"].ToString(),
                                    Price = Convert.ToDecimal(reader["Price"]),
                                    Address_Appartment_no = reader["Address_Appartment_no"].ToString(),
                                    Address_street = reader["Address_street"].ToString(),
                                    Address_District = reader["Address_District"].ToString(),
                                    Enable = (Boolean)reader["Enable"],
                                    Created_By = reader["Created_By"].ToString(),
                                    Created_Date = (DateTime)reader["Created_Date"],
                                    Modified_By = reader["Modified_By"].ToString(),
                                    Space_Image_Path = reader["Space_Image_Path"].ToString(),
                                    //    Modified_Date = (DateTime)reader["Modified_Date"],

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

        public ResponseData UpdateSpace(Space_Owner_Master updatedSpace)
        {
            try
            {
                // string connString = "server=ANKIT; database=userdatabase; trusted_connection=true; Encrypt=False;";
                string connString = _configuration["ConnectionStrings:dbcs"];
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    ResponseData response = new ResponseData() { IsSaved = false, Message = "" };

                    connection.Open();
                    string databaseName = "p2p";
                    string sql = $@"USE {databaseName};UPDATE Space_Owner_Master SET Username = '{updatedSpace.Username}', Longitude = '{updatedSpace.Longitude}', Latitude = '{updatedSpace.Latitude}', Description ='{updatedSpace.Description}', 
                             Price = '{updatedSpace.Price}', Address_Appartment_no = '{updatedSpace.Address_Appartment_no}', Address_street = '{updatedSpace.Address_street}', 
                             Address_District = '{updatedSpace.Address_District}', Enable = '{updatedSpace.Enable}',Modified_Date= GETDATE(),Modified_By='{updatedSpace.Username}'
                             WHERE Username = '{updatedSpace.Username}'";
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

        public ResponseData DeleteSpace(int spaceid)
        {
            ResponseData responce = new ResponseData() { IsSaved = false, Message = "" };

            string databaseName = "p2p";

            //able to solve after convertig it to toString()
            string qry = $"USE {databaseName};DELETE FROM Space_Owner_Master  WHERE SpaceID= {spaceid}";

            responce.IsSaved = SqlConnectionManager.DeleteRecord(qry);
            if (responce.IsSaved)
            {
                responce.Message = "Item deleted Successfully";
            }
            return responce;

        }

        public ResponseData AddBookingDetail(Booking_Detail detail)
        {
            ResponseData response = new ResponseData() { IsSaved = false, Message = "" };


            //       string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
            string connString = _configuration["ConnectionStrings:dbcs"];
            using (SqlConnection connection = new SqlConnection(connString))
            {
                try
                {
                    string databaseName = "p2p";
                    string sql = $@"USE {databaseName};INSERT INTO BookingDetail (SpaceID, booking_amount, Username, StartBooking, EndBooking,Enable)
                                            VALUES (@SpaceID, @booking_amount, @Username, @StartBooking, @EndBooking,1)";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        connection.Open();

                        command.Parameters.AddWithValue("@SpaceID", detail.SpaceID);
                        command.Parameters.AddWithValue("@booking_amount", detail.booking_amount);
                        command.Parameters.AddWithValue("@Username", detail.Username);
                        command.Parameters.AddWithValue("@StartBooking", detail.StartBooking);
                        command.Parameters.AddWithValue("@EndBooking", detail.EndBooking);



                        int i = command.ExecuteNonQuery();
                        connection.Close();

                        if (i > 0)
                        {
                            response.Message = "booking Sucess";
                            response.IsSaved = true;
                        }
                        else
                        {
                            response.Message = "booking failed";
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

        public List<cur_pas_book> ongoingcustomerbooking(ongoning_booking detail)
        {
            try
            {
                //    string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
                string connString = _configuration["ConnectionStrings:dbcs"];
                List<cur_pas_book> spaces = new List<cur_pas_book>();
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();



                    string sql = $@"  SELECT 
    SO.Username AS ownername,
    booking_amount,
    B.SpaceID AS SpaceID,
    StartBooking,
    EndBooking,
    Address_Appartment_no,
    Address_street,
    Address_District,
    Space_Image_Path,  
    Description,
    UM.Email AS customer_email,
    UM.Phone_Number AS customer_phoneno,
    UM2.Email AS owner_email,
    UM2.Phone_Number AS owner_phoneno,
    B.Username AS Customer,
	B.BookingID as bookingid,
    COALESCE(re.Rating, 0) AS rating,
    COALESCE(re.RatingID, 0) AS ratingid
FROM  
    BookingDetail AS B
INNER JOIN 
    Space_Owner_Master AS SO ON B.SpaceID = SO.SpaceID
INNER JOIN 
    User_Master AS UM ON B.Username = UM.Username
INNER JOIN 
    User_Master AS UM2 ON SO.Username = UM2.Username
LEFT JOIN 
    Review AS re ON UM.Username = re.Username AND B.SpaceID = re.SpaceID AND re.bookingID = B.BookingID
WHERE 
      B.Enable = '{detail.Enable}' AND UM.Username = '{detail.Username}';
  ";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                //  DateTime dateTimeValue = (DateTime)reader["StartBooking"];
                                cur_pas_book item = new cur_pas_book
                                {
                                    ownerUsername = reader["ownername"].ToString(),
                                    SpaceID = (int)reader["SpaceID"],
                                    booking_amount = Convert.ToInt32(reader["booking_amount"]),
                                    Description = reader["Description"].ToString(),
                                    StartBooking = (DateTime)reader["StartBooking"],
                                    EndBooking = (DateTime)reader["EndBooking"],
                                    CEmail = reader["customer_email"].ToString(),
                                    CPhone_Number = reader["customer_phoneno"].ToString(),
                                    customername = reader["customer"].ToString(),
                                    Space_Image_Path = reader["Space_Image_Path"].ToString(),
                                    OEmail = reader["owner_email"].ToString(),
                                    OPhone_Number = reader["owner_phoneno"].ToString(),
                                    appartmant = reader["Address_Appartment_no"].ToString(),
                                    street = reader["Address_street"].ToString(),
                                    district = reader["Address_District"].ToString(),
                                    rating = (int)reader["rating"],
                                    ratingid = (int)reader["ratingid"],
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
                Console.WriteLine($" {ex.Message}");
                return null;
            }
        }

        public List<lat_long> GetLocationsWithin2km(Location_user detail)
        {
            List<lat_long> locations = new List<lat_long>();
            //  string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
            string connString = _configuration["ConnectionStrings:dbcs"];
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();



                string query = $@"WITH distanceCTE AS (
SELECT *, dbo.CalculateDistance(latitude, longitude,@latitude, @longitude) AS Distance
FROM Space_Owner_Master where Enable=1
)
SELECT 
    dc.SpaceID as spaceid,
	Price,
	dc.Username as ownername,
	Address_Appartment_no,
	Address_District,
	Address_street,
	Distance,
	Description,
    Space_Image_Path,
    COALESCE(AVG(r.rating), 0) as AverageRating,
    COALESCE(COUNT(r.rating), 0) as NumberOfReviews
FROM 
    distanceCTE dc
LEFT JOIN 
    review r ON dc.SpaceID = r.SpaceID
WHERE 
    dc.Distance <= '{detail.Distance}'
GROUP BY 
     dc.SpaceID,
	 dc.Username,
	 dc.Address_Appartment_no,
	 dc.Address_District,
	 dc.Address_street,
	 dc.Distance,
	 dc.Description,
     dc.Space_Image_Path,
	 dc.Price";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@latitude", detail.Latitude);
                    command.Parameters.AddWithValue("@longitude", detail.Longitude);


                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lat_long item = new lat_long
                            {
                                //  lat = Convert.ToDouble(reader["latitude"]),
                                //  Long = Convert.ToDouble(reader["longitude"]),
                                SpaceID = (int)reader["spaceid"],
                                Username = reader["ownername"].ToString(),
                                //   RoleID = (int)reader["RoleID"],
                                //  Longitude = reader["Longitude"].ToString(),
                                //   Latitude = reader["Latitude"].ToString(),
                                Description = reader["Description"].ToString(),


                                Price = Convert.ToDecimal(reader["Price"]),


                                Address_Appartment_no = reader["Address_Appartment_no"].ToString(),
                                Address_street = reader["Address_street"].ToString(),
                                Address_District = reader["Address_District"].ToString(),
                                //   Enable = (Boolean)reader["Enable"],
                                //   Created_By = reader["Created_By"].ToString(),
                                //    Created_Date = (DateTime)reader["Created_Date"],
                                //    Modified_By = reader["Modified_By"].ToString(),
                                Space_Image_Path = reader["Space_Image_Path"].ToString(),
                                //    Modified_Date = (DateTime)reader["Modified_Date"],
                                distance = Convert.ToDecimal(reader["Distance"]),

                                AverageRating = (int)reader["AverageRating"],

                                NumberOfReviews = (int)reader["NumberOfReviews"],
                            };

                            locations.Add(item);
                        }
                    }
                    return locations;
                }
            }
        }


    }


}

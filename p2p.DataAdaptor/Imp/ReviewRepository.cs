using Microsoft.Extensions.Logging;
using p2p.DataAdaptor.Contract;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static p2p.Common.Models.Review;
using static p2p.Common.Models.Space;

namespace p2p.DataAdaptor.Imp
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly ILogger<ReviewRepository> _logger;
        public ReviewRepository(ILogger<ReviewRepository> logger)
        {
            _logger = logger;
            _logger.LogDebug("NLog is integrated to Customer repository Controller");
        }

        public ResponseData Addrating(Rating value)
        {
            ResponseData response = new ResponseData() { IsSaved = false, Message = "" };


            string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                try
                {
                    connection.Open();
                    string databaseName = "p2p";


                    int count = 0;

                    if (value.ratingid == 0)
                    {
                        string checkQuery = @$"USE {databaseName};SELECT COUNT(*) FROM Review WHERE SpaceID = @SpaceID AND Username = @Username AND bookingID=@bookingID";
                        SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                        checkCommand.Parameters.AddWithValue("@SpaceID", value.SpaceID);
                        checkCommand.Parameters.AddWithValue("@Username", value.Username);
                        checkCommand.Parameters.AddWithValue("@bookingID", value.bookingID);
                        count = (int)checkCommand.ExecuteScalar();
                    }
                    else
                    {
                        string checkQuery = @$"USE {databaseName};SELECT COUNT(*) FROM Review WHERE SpaceID = @SpaceID AND Username = @Username AND RatingId=@RatingID";
                        SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                        checkCommand.Parameters.AddWithValue("@SpaceID", value.SpaceID);
                        checkCommand.Parameters.AddWithValue("@Username", value.Username);
                        checkCommand.Parameters.AddWithValue("@RatingID", value.ratingid);
                         count = (int)checkCommand.ExecuteScalar();
                    }
                    





              //      string checkQuery = @$"USE {databaseName};SELECT COUNT(*) FROM Review WHERE SpaceID = @SpaceID AND Username = @Username AND RatingId=@RatingID";
                //    SqlCommand checkCommand = new SqlCommand(checkQuery, connection);
                 //   checkCommand.Parameters.AddWithValue("@SpaceID", value.SpaceID);
                //    checkCommand.Parameters.AddWithValue("@Username", value.Username);
                //    checkCommand.Parameters.AddWithValue("@RatingID", value.ratingid);
               //     int count = (int)checkCommand.ExecuteScalar();

                    if (count > 0)
                    {
                        // User has already rated the place, update the existing rating
                        string updateQuery = @$"USE {databaseName};UPDATE Review SET Rating = @Rating WHERE SpaceID = @SpaceID AND Username = @Username AND RatingID=@RatingID AND bookingID=@bookingID";
                        SqlCommand updateCommand = new SqlCommand(updateQuery, connection);
                        updateCommand.Parameters.AddWithValue("@Rating", value.rating);
                        updateCommand.Parameters.AddWithValue("@RatingID", value.ratingid);
                        updateCommand.Parameters.AddWithValue("@SpaceID", value.SpaceID);
                        updateCommand.Parameters.AddWithValue("@bookingID", value.bookingID);

                        updateCommand.Parameters.AddWithValue("@Username", value.Username);
                        int i = updateCommand.ExecuteNonQuery();
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
                    else
                    {
                        string insertQuery = @$"USE {databaseName};INSERT INTO Review (SpaceID, Username, Rating,bookingID) VALUES (@SpaceID, @Username, @Rating,@bookingID )";
                        SqlCommand insertCommand = new SqlCommand(insertQuery, connection);
                        insertCommand.Parameters.AddWithValue("@SpaceID", value.SpaceID);
                        insertCommand.Parameters.AddWithValue("@Username", value.Username);
                        insertCommand.Parameters.AddWithValue("@Rating", value.rating);
                        insertCommand.Parameters.AddWithValue("@bookingID", value.bookingID);

                        int i = insertCommand.ExecuteNonQuery();
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

using Microsoft.ML.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p2p.Common.Models
{


    public class ValidationError
    {
        public string ColumnName { get; set; }
        public int RowNumber { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class Space
    {
        public class Space_Owner_Master
        {
            public int SpaceID { get; set; }
            public string Username { get; set; }
            public int RoleID { get; set; }
            public string Longitude { get; set; }
            public string Latitude { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public string Address_Appartment_no { get; set; }
            public string Address_street { get; set; }
            public string Address_District { get; set; }
            public Boolean Enable { get; set; }
            public string Created_By { get; set; }
            public DateTime Created_Date { get; set; }
            public string Modified_By { get; set; }
            public DateTime Modified_Date { get; set; }
            public string Space_Image_Path { get; set; }
        }

        public class detail_space
        {
           
          //  public string Username { get; set; }
            public string Longitude { get; set; }
            public string Latitude { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public string Address_Appartment_no { get; set; }
            public string Address_street { get; set; }
            public string Address_District { get; set; }
          //  public Boolean Enable { get; set; }
            public string Space_Image_Path { get; set; }
        }

        public class update_space
        {
            public int SpaceID { get; set; }
            public detail_space Detail { get; set; }
        }

     


        public class all_spaces
        {
            public int SpaceID { get; set; }
            public string Username { get; set; }
            public int RoleID { get; set; }
            public string Longitude { get; set; }
            public string Latitude { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public string Address_Appartment_no { get; set; }
            public string Address_street { get; set; }
            public string Address_District { get; set; }
            public Boolean Enable { get; set; }
            public string Created_By { get; set; }
            public DateTime Created_Date { get; set; }
            public string Modified_By { get; set; }
            public DateTime Modified_Date { get; set; }
            public string Space_Image_Path { get; set; }
            public int AverageRating { get; set;}
            public int NumberOfUsers { get; set;}
        }


        public class addspace
        {
           
            public string Username { get; set; }
            public float Longitude { get; set; }
            public float Latitude { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public string Address_Appartment_no { get; set; }
            public string Address_street { get; set; }
            public string Address_District { get; set; }
            public string Space_Image_Path { get; set; }

        }
        public class ResponseData
        {
            public bool IsSaved { get; set; }
            public string Message { get; set; }
        }

        public class Booking_Detail
        {
            public int SpaceID { get; set; }
            public decimal booking_amount { get; set; }
            public string Username { get; set; }
            public DateTime StartBooking { get; set; }
            public DateTime EndBooking { get; set; }
            public Boolean Enable { get; set; }
            public String paymentId {  get; set; }
        }



        public class current_booking
        {
            public string ownerUsername { get; set; }
            public int SpaceID { get; set; }
            public int booking_amount { get; set; }
            public DateTime StartBooking { get; set; }
            public DateTime EndBooking { get; set; }
            public string Space_Image_Path { get; set; }
           public string Description { get; set; }
            public string Email { get; set; }
            public string Phone_Number { get; set; }
            public string customername { get; set; }
            public string Address_Appartment_no { get; set; }
            public string Address_street { get; set; }
            public string Address_District { get; set; }


        }
        public class ongoning_booking
        {
            public string Username { get; set; }
            public Boolean Enable { get; set; }
        }

        public class CustomerBoooking
        {
            public string ownerUsername { get; set; }
            public int SpaceID { get; set; }
            public int booking_amount { get; set; }
            public DateTime StartBooking { get; set; }
            public DateTime EndBooking { get; set; }
            public string Space_Image_Path { get; set; }
            public string Description { get; set; }
            public string CEmail { get; set; }
            public string CPhone_Number { get; set; }
            public string customername { get; set; }
            public string OEmail { get; set; }
            public string OPhone_Number { get; set; }

        }



        public class Bookingdetail {
            public int SpaceID { get; set; }
            public string Username { get; set; }
            public int RoleID { get; set; }
            public string Longitude { get; set; }
            public string Latitude { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public string Address_Appartment_no { get; set; }
            public string Address_street { get; set; }
            public string Address_District { get; set; }
            public Boolean Enable { get; set; }
            public string Created_By { get; set; }
            public DateTime Created_Date { get; set; }
            public string Modified_By { get; set; }
            public DateTime Modified_Date { get; set; }
            public string Space_Image_Path { get; set; }
            public string phoneno { get; set; }
            public string email { get; set; }
            public DateTime enddate { get; set; }
        }

        public class cur_pasbooking
        {
            public int SpaceID { get; set; }
            public string Username { get; set; }
            public int RoleID { get; set; }
            public string Longitude { get; set; }
            public string Latitude { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public string Address_Appartment_no { get; set; }
            public string Address_street { get; set; }
            public string Address_District { get; set; }
            public Boolean Enable { get; set; }
            public string Created_By { get; set; }
            public DateTime Created_Date { get; set; }
            public string Modified_By { get; set; }
            public DateTime Modified_Date { get; set; }
            public string Space_Image_Path { get; set; }
            public decimal bookedamount { get; set; }
            public string c_email { get; set; }
            public DateTime startdate { get; set; }
            public string c_phoneno { get; set; }
            public DateTime enddate { get; set; }
            public string c_username { get; set; }
            public int rating { get; set;}
            public int bookingid { get; set;}
          

        }
        public class cur_pas_book
        {
            public string ownerUsername { get; set; }
            public int SpaceID { get; set; }
            public int booking_amount { get; set; }
            public DateTime StartBooking { get; set; }
            public DateTime EndBooking { get; set; }
            public string Space_Image_Path { get; set; }
            public string Description { get; set; }
            public string CEmail { get; set; }
            public string CPhone_Number { get; set; }
            public string customername { get; set; }
            public string OEmail { get; set; }
            public string OPhone_Number { get; set; }
            public string appartmant { get; set; }
            public string street { get; set; }
            public string district { get; set; }
            public int rating { get; set; }
            public int ratingid { get; set; }
            public int bookingid { get; set; }

        }

        public class Location
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }
        public class lat_long
        {
            public double lat { get; set; }
            public double Long { get; set; }
            public int SpaceID { get; set; }
            public string Username { get; set; }
            public int RoleID { get; set; }
            public string Longitude { get; set; }
            public string Latitude { get; set; }
            public string Description { get; set; }
            public decimal Price { get; set; }
            public string Address_Appartment_no { get; set; }
            public string Address_street { get; set; }
            public string Address_District { get; set; }
            public Boolean Enable { get; set; }
            public string Created_By { get; set; }
            public DateTime Created_Date { get; set; }
            public string Modified_By { get; set; }
            public DateTime Modified_Date { get; set; }
            public string Space_Image_Path { get; set; }
            public decimal distance { get; set;}
            public int AverageRating { get; set;}
            public int NumberOfReviews { get; set;}
        }


        public class Location_user
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public decimal Distance { get; set; }
        }

        public class ParkingDataInput
        {
            public float Hour { get; set; }
            public float DayOfWeek { get; set; }
            public float Demand { get; set; }

        }

     
    }
}

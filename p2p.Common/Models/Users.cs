using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace p2p.Common.Models
{
    public class Users
    {
        public class UserMaster
        {
            public List<UserData> Users { get; set; }
            public List<UserResponseData> UserResponse { get; set; }
        }
        public class UserData
        {
            public int UserId { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string phoneno { get; set; }
            public string roleName { get; set; }
            public bool Enable { get; set; }
            public int RoleID { get; set; }
            public string userImage { get; set; }
            public string Token { get; set; }
            public string? ResetPasswordToken { get; set; }
            public DateTime ResetPasswordExpiry { get; set; }
        }

        public class User
        {
            public int UserId { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string phoneno { get; set; }
            public string roleName { get; set; }
            public bool Enable { get; set; }
            public int RoleID { get; set; }
          
        }

        public class AuthUser
        {
            public string Username { get; set; }

        }

        public class LoginUser
        {
            public string Username { get; set; }
            public string Password { get; set; }

        }
        public class UserResponseData
        {
            public bool IsSaved { get; set; }
            public string Message { get; set; }
        }

        public class UserProfileData
        {
            public string Username { get; set; }
            public DetailModel Detail { get; set; }
        }

        public class DetailModel
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Phoneno { get; set; }
            public string UserImage { get; set; }

        }
        public class sugested_username
        {
            public string Username { get; set;}
        }
    }
}

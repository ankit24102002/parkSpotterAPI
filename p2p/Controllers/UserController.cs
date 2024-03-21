using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NETCore.MailKit.Core;
using p2p.Common.Models;
using p2p.Common.Models.Dto;
using p2p.Helper;
using p2p.Logic.Contract;
using p2p.Logic.Imp;
using p2p.UtilityServices;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using static p2p.Common.Models.Users;

namespace p2p.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    [AllowAnonymous]
    // [EnableCors("AllowAll")]
    public class UserController : ControllerBase
    {
        private readonly IUserManager _userManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserController> _logger;
        private readonly IEmailServices _emailServices;
        public UserController(IUserManager userManager,
        ILogger<UserController> logger,IConfiguration configuration,IEmailServices emailServices)
        {
            _userManager = userManager;
            _logger = logger;
            _logger.LogDebug("NLog is integrated to space Controller");
            _configuration = configuration;
            _emailServices = emailServices;
        }


        [HttpPost()]
        public IActionResult SignUp([FromBody] User userData)
        {
            try { 
            if(userData == null)
            {
                return BadRequest();
            }
            userData.Password= PasswordHasher.HashPassword(userData.Password);
          
            UserResponseData response = _userManager.SignUp(userData);

            if (response.IsSaved)
            {
                bool result = true;
                string message = response.Message;
                return Ok(new { result = result, message = message });
            }
            else
            {
                bool nresult = false;
                string message = response.Message;
                return Ok(new { result = nresult, message = message });
            }
        } 
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Error = "Internal Server Error" });
            }
        }


        [HttpPost()]
        public IActionResult Login([FromBody]LoginUser user)
        {
            try { 
            if(user == null)
            {
                return BadRequest();
            }
          
            var userdetail = _userManager.GetLoginUserData(user);

                if (userdetail == null)
                {
                    bool nresult = false;
                    return Ok(new { result = nresult, message = "Invalid  username" });
                }

                    if (!PasswordHasher.VerifyPassword(user.Password, userdetail.Password))
            {
                return Ok(new{message = "password incorect" });
            }
            userdetail.Token= CreateJwt(userdetail);
            if (userdetail != null)
            {
                string message = " Login Success!";
                bool result = true;
                int roleid = userdetail.RoleID;
                    string Username=userdetail.Username;
                    return Ok(new { result = result, message = message, roleid = roleid, Token = userdetail.Token, username = Username });
            }
            else
            {
                bool nresult = false;
                return Ok(new { result = nresult, message = "Invalid username or password" });
                //return BadRequest(new { Error = "User doesn`t match" });
            }
        } 
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Error = "Internal Server Error" });
            }
        }

        [HttpPost()]
        public IActionResult ForgotPassword(AuthUser userData, string new_password, string confirm_password)
        {
            var user = _userManager.VerifyUser(userData);
            if (user == null)
            {
                return BadRequest(new { Error = "User not found" });
            }
            if (new_password == confirm_password)
            {
                string connString = "server=ANKIT; database=BlogPostDatabase; trusted_connection=true; Encrypt=False;";
                using (SqlConnection connection = new SqlConnection(connString))
                {
                    string databaseName = "BlogPostDatabase";
                    string sqlQuery = $"USE {databaseName};UPDATE Users SET Password = @new_password WHERE Username = @username ";
                    using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@username", user.Username);
                        command.Parameters.AddWithValue("@new_password", new_password);
                        command.ExecuteNonQuery();
                    }

                    return Ok(new { Message = "password reset succesful" });

                }
            }
            else
            {
                return BadRequest(new { Error = "password doesn`t match" });
            }
        }


        [HttpPost()]
        public IActionResult Profiledata([FromBody] AuthUser user)
        {
            try
            {
               

                var userdetail = _userManager.Profiledata(user);

            
                if (userdetail != null)
                {
                    string message = "Success!";
                    bool result = true;
                 
                    return Ok(new { result = result, message = message, detail = userdetail });
                }
                else
                {
                    bool nresult = false;
                    return Ok(new { result = nresult, message = "Failed", detail = userdetail });
                   
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Error = "Internal Server Error" });
            }
        }


        [HttpPost()]
        public IActionResult UpdateProfiledata(UserProfileData userProfileData)
        {

            UserResponseData response = _userManager.UpdateProfiledata(userProfileData);
            try
            {
                if (response.IsSaved)
                {
                    response.Message = "Profile updated";
                    bool result = true;
                    return Ok(new { result = result, Message = response.Message });
                }
                else
                { 
                    bool result = false;
                    return Ok(new { result = result, Message = response.Message });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Error = "Internal Server Error" });
            }
        }


       [HttpPost()]
        public IActionResult GetSuggestedUsernames( string input)
        {
            try
            {


                var similarUsernames = _userManager.GetSuggestedUsernames(input);

                var suggestedUsernames = GenerateSuggestions(input, similarUsernames);

                if (suggestedUsernames != null)
                {
                    string message = "Success!";
                    bool result = true;

                    return Ok(new { result = result, message = message, detail = suggestedUsernames });
                }
                else
                {
                    bool nresult = false;
                    return Ok(new { result = nresult, message = "Failed", detail = suggestedUsernames });

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(500, new { Error = "Internal Server Error" });
            }
        }


        private string GenerateSuggestions(string input, List<string> existingUsernames)
        {
         
            if (input.Length < 4)
            {
                input = input.PadRight(5, '0');
            }
            if (!existingUsernames.Contains(input))
            {
               
                return input;
            }

            var suffix = 1;
            //  var suggestion = $"{input}{suffix}";
            var suggestion = $"{input}{GetSuffix(suffix)}";
            // Loop until a unique suggestion is found
            while (existingUsernames.Contains(suggestion))
            {
                suffix++;
             //   suggestion = $"{input}{suffix}";
                suggestion = $"{input}{GetSuffix(suffix)}";
            }

            return suggestion;
        }
        
        private string GetSuffix(int number)
        {
            const string alphabet = "abcdefghijklmnopqrstuvwxyz";
            string suffix = "";
            while (number > 0)
            {
                number--;
                suffix = alphabet[number % alphabet.Length] + suffix;
                number /= alphabet.Length;
            }
            return suffix;
        }

        private string CreateJwt(UserData user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretkey = _configuration["ConnectionStrings:key"];
            var key= Encoding.ASCII.GetBytes(secretkey);
            //    var secretkey = _configuration["EmailSettings:key"];
            //    var key = Encoding.ASCII.GetBytes("veryverysceret.....veryverysceret.....veryverysceret.....veryverysceret.....veryverysceret.....veryverysceret.....veryverysceret.....veryverysceret.....");

            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim("Roleid", $"{user.RoleID}"),
                new Claim("UserName",$"{user.Username}")

               // new Claim(ClaimTypes.Role, $"{user.RoleID}"),
               // new Claim(ClaimTypes.Name,$"{user.Username}")
               // new Claim(ClaimTypes.NameIdentifier,$"{user.Username}")

            });

            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.UtcNow.AddMinutes(1),
                //    Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);
        }

        [HttpPost("send-reset-email/{email}")]
        public  IActionResult SendEmail(string email) //remove task async
        {
            var user =  _userManager.checkemail(email);//removed await
            if(user is null)
            {
                return NotFound(new
                {
                    satusCode = 404,
                    message = "Email does't exist"
                });
            }
            var tokenBytes = RandomNumberGenerator.GetBytes(64);
            var emailToken = Convert.ToBase64String(tokenBytes);
            user.ResetPasswordToken=emailToken;
            user.ResetPasswordExpiry = DateTime.Now.AddMinutes(15);
            string from = _configuration["EmailSetting:From"];
            var emailModel= new EmailModel(email,"Reset Password!",EmailBody.EmailStringBody(email,emailToken));
            _emailServices.SendEmail(emailModel);
             var users = _userManager.updatetoken(user);

            return Ok(new
              {
                  StatusCode=200,
                  Message="Email Sent!"
              });
        }

        [HttpPost("reset-password")]
        public  IActionResult ResetPassword(ResetPasswordDto resetPasswordDto)
        {
            var newToken = resetPasswordDto.EmailToken.Replace(" ", "+");

            var user = _userManager.checkemail(resetPasswordDto.Email);//removed await
            if (user is null)
            {
                return NotFound(new
                {
                    satusCode = 404,
                    Messange = "user does't exist"
                });
            }

            var tokenCode = user.ResetPasswordToken;
            DateTime emailTokenExpiry = user.ResetPasswordExpiry;

            if(tokenCode!=resetPasswordDto.EmailToken)//||emailTokenExpiry<DateTime.Now
            {
                return BadRequest(new
                {
                    statuCode = 400,
                    Message = "Invalid reset link"
                });
            }
           user.Password = PasswordHasher.HashPassword(resetPasswordDto.NewPassword);

         
            // this method
            // var users = _userManager.updatepassword(user.Password,user.Username);




            // or this method
            string connString = "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;";
            using (SqlConnection connection = new SqlConnection(connString))
            {
                string databaseName = "p2p";
                string sqlQuery = $"USE {databaseName};UPDATE User_Master SET Password = @new_password WHERE Username = @username ";
                using (SqlCommand command = new SqlCommand(sqlQuery, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@username", user.Username);
                    command.Parameters.AddWithValue("@new_password", user.Password);
                    command.ExecuteNonQuery();
                }
            }
      
            return Ok(new
            {
                StatusCode = 200,
                Message = "reset success !"
            });
        }
      


    }
}

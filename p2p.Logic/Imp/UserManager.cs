using p2p.DataAdaptor.Contract;
using p2p.Logic.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static p2p.Common.Models.Space;
using static p2p.Common.Models.Users;

namespace p2p.Logic.Imp
{
    public class UserManager : IUserManager
    {
        protected readonly IUserRepository UserRepository;
        public UserManager(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        public UserResponseData SignUp(User userData)
        {
            return UserRepository.SignUp(userData);
        }


        public AuthUser VerifyUser(AuthUser userdata)
        {
            return UserRepository.VerifyUser(userdata);
        }

        /*        public string ExtractTokenFromHeader(HttpContext httpContext)
                {
                    return UserRepository.ExtractTokenFromHeader(httpContext);
                }
        */
        public AuthUser Login(AuthUser userdata)
        {
            return UserRepository.Login(userdata);
        }


        public LoginUser VerifyLoginUser(LoginUser userdata)
        {
            return UserRepository.VerifyLoginUser(userdata);
        }
        public UserData GetLoginUserData(LoginUser userdata)
        {
            return UserRepository.GetLoginUserData(userdata);
        }
        public UserData checkemail(string email)
        {
            return UserRepository.checkemail(email);
        }

        public UserResponseData updatetoken(UserData user)
        {
            return UserRepository.updatetoken(user);
        }

        public UserResponseData updatepassword(string password, string username)
        {
            return UserRepository.updatepassword(password, username);
        }
        public UserData Profiledata(AuthUser user)
        {
            return UserRepository.Profiledata(user);
        }
        public UserResponseData UpdateProfiledata(UserProfileData userProfileData)
        {
            return UserRepository.UpdateProfiledata( userProfileData);

        }
        public List<string> GetSuggestedUsernames(string input)
        {
            return UserRepository.GetSuggestedUsernames(input);
        }
    }
}

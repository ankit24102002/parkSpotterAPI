using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static p2p.Common.Models.Users;

namespace p2p.DataAdaptor.Contract
{
    public interface IUserRepository
    {
        public UserResponseData SignUp(User UserData);
        //  public UserResponseData Login(UserData userdata);
        public AuthUser VerifyUser(AuthUser userdata);
        //    public string ExtractTokenFromHeader(HttpContext httpContext);
        public AuthUser Login(AuthUser userdata);
        public LoginUser VerifyLoginUser(LoginUser userdata);
        public UserData GetLoginUserData(LoginUser userdata);
        public UserData checkemail(string email);
        UserResponseData updatetoken(UserData user);
        UserResponseData updatepassword(string password, string username);
        UserData Profiledata(AuthUser user);
        UserResponseData UpdateProfiledata(UserProfileData userProfileData);
        List<string> GetSuggestedUsernames(string input);
    }
}

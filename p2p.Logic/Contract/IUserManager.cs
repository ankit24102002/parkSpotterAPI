using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static p2p.Common.Models.Users;

namespace p2p.Logic.Contract
{
    public interface IUserManager
    {
        UserResponseData SignUp(User UserData);
        AuthUser VerifyUser(AuthUser userdata);
        AuthUser Login(AuthUser userdata);
        LoginUser VerifyLoginUser(LoginUser userdata);
        UserData GetLoginUserData(LoginUser userdata);
        UserData checkemail(string email);
        UserResponseData updatetoken(UserData user);
        UserResponseData updatepassword(string password, string username);
        UserData Profiledata(AuthUser user);
        UserResponseData UpdateProfiledata(UserProfileData userProfileData);
        List<string> GetSuggestedUsernames(string input);

    }
}

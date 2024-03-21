using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace p2p
{
    public class MyAuthorizationFilter : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var authorizationHeader) ||
                !context.HttpContext.Request.Headers.TryGetValue("Username", out var usernameHeader))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            string token = authorizationHeader.FirstOrDefault()?.Split(" ")[1];
            //string username = usernameHeader.FirstOrDefault()?.Split(" ")[0];
            string username = usernameHeader.FirstOrDefault();
            if (!IsAuthorized(token, username))
            {
                context.Result = new UnauthorizedResult(); 
            }
         
        }
        
        public bool IsAuthorized(string token, string username)
        {
                string SecretKey = "veryverysceret.....veryverysceret.....veryverysceret.....veryverysceret.....veryverysceret.....veryverysceret.....veryverysceret.....veryverysceret.....";
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken validatedToken;
                var validationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey)),
                    ValidateIssuer = false, 
                    ValidateAudience = false, 
                    ValidateLifetime = true
                };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
               var payload = validatedToken as JwtSecurityToken;
               var claims = payload.Claims;

              var usernameClaim = claims.FirstOrDefault(c => c.Type == "UserName" );

             // var usernameClaim = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (usernameClaim != null && usernameClaim.Value == username)
                {
                    return true;
                }
                else
                {
                    return false;
                }
             }
            catch (SecurityTokenExpiredException)
            {
                // Token is expired
                return false;
            }
            catch (Exception)
            {
                // Other token validation errors
                return false;
            }
        }
    }
}
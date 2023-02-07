using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace TodoApi.Helpers
{    
    public enum todo_status
    {
        NotStarted, OnGoing, Completed
    }

    public class Helper
    {
        //Check bearer token after login
        public bool CheckBearerToken(Microsoft.AspNetCore.Http.HttpRequest Request, JwtSecurityTokenHandler jwtHandler, AppSettings appSettings)
        {
            string bearerToken = string.Empty;
            if (Request.Headers.ContainsKey("Authorization"))
            {
                var header = Request.Headers["Authorization"];
                if (header.ToString().StartsWith("Bearer "))
                {
                    bearerToken = header.ToString().Substring(7);
                }
            }
            if (string.IsNullOrEmpty(bearerToken))
            {
                return false;
            }
            try
            {
                var jwt = jwtHandler.ReadJwtToken(bearerToken);
                if (jwt.Issuer != appSettings.Secret)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }

        }
    }
}

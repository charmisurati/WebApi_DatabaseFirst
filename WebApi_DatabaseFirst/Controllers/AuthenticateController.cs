using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebApi_DatabaseFirst.Controllers
{
    public class AuthenticateController : ApiController
    {
        [HttpGet]
        [ActionName("GetToken")]
        public string Token(string userName, string password)
        {
            string token = string.Empty;
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {

                // Database call

                token = TokenManager.GenerateToken(userName, 20);
            }

            return token;
        }
    }
}
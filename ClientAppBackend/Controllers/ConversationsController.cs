using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ClientAppBackend.Controllers
{
    public class ConversationsController : Controller
    {
        
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/token")]
        public string Token()
        {
            var userName = Request.Query["name"];
            
            var appId = System.Environment.GetEnvironmentVariable("APP_ID");
            var privateKeyPath = System.Environment.GetEnvironmentVariable("PRIVATE_KEY_PATH");
            var token = TokenGenerator.GenerateToken(appId: appId, privateKeyPath: privateKeyPath, subject: userName);
            //var ret = new TokenRet() { Token = token };
            return token;
        }

        public class TokenRet 
        {
            [JsonProperty("token")]
            public string Token { get; set; }
        }

        
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nexmo.Api.Voice.Nccos;

namespace ClientAppBackend.Controllers
{
    public class VoiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/webhook/answer")]
        public string Answer()
        {
            Ncco ncco;
            if(Request.Query.ContainsKey("to") && Request.Query["to"]== "12012971365")
            {
                var talkAction = new TalkAction { Text = "Please wait while you're connected to the call" };
                var connectAction = new ConnectAction
                {
                    Timeout = "20",
                    From = "12012854946",
                    Endpoint = new[] {new Nexmo.Api.Voice.Nccos.Endpoints.AppEndpoint
                {
                    User="steve"
                }}
                };
                ncco = new Ncco(talkAction, connectAction);
            }
            else
            {
                var talkAction = new TalkAction { Text = "Please wait while you're connected to the call" };
                var connectAction = new ConnectAction
                {
                    Timeout = "20",
                    From = "12012854946",
                    Endpoint = new[] {new Nexmo.Api.Voice.Nccos.Endpoints.PhoneEndpoint
                {
                    Number = "12018747427"
                }}
                };
                ncco = new Ncco(talkAction, connectAction);
            }            
            
            return ncco.ToString();
        }

        [HttpPost("/webhook/event")]
        public IActionResult Event()
        {
            return NoContent();
        }
    }
}
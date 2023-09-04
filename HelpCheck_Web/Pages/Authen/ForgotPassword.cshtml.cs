using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpCheck_Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace HelpCheck_Web.Pages.Authen
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public string Username { get; set; }

        public ForgotPasswordModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {

        }

        public IActionResult OnPost()
        {
            if (!string.IsNullOrWhiteSpace(Username))
            {

                var cli = new RestClient(_configuration["api"] + "/web-apps/Auth/ResetPassword")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.POST);
                req.AddHeader("Content-Type", "application/json");
                req.AddJsonBody(new ForgotPasswordDto()
                {
                    Username = Username
                });
                IRestResponse response = cli.Execute(req);
                var data = response.Content;
                if (data != null)
                {

                }

                ViewData["success"] = 1;
            }

            return Page();
        }
        public IActionResult OnPostForget()
        {
            if (!string.IsNullOrWhiteSpace(Username))
            {

                var cli = new RestClient(_configuration["api"] + "/web-apps/Auth/ResetPassword")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.POST);
                req.AddHeader("Content-Type", "application/json");
                req.AddJsonBody(new ForgotPasswordDto()
                {
                    Username = Username
                });
                IRestResponse response = cli.Execute(req);
                var data = response.Content;
                if (data != null)
                {

                }

                return new JsonResult("OK");
            }
            else
            {
                return new JsonResult("OK");
            }

        }
    }
}

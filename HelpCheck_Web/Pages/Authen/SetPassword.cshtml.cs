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
    public class SetPasswordModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public SetPasswordDto SetPasswordDto { get; set; }

        [BindProperty]
        public string Username { get; set; }
        [BindProperty]
        public string Code { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public SetPasswordModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (!string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Code) && !string.IsNullOrWhiteSpace(Password))
            {
                try
                {
                    var text = Username.Substring(Username.Length - 4);
                    var cli = new RestClient(_configuration["api"] + "/web-apps/Auth/SetPassword")
                    {
                        Timeout = -1
                    };
                    var req = new RestRequest(Method.POST);
                    req.AddHeader("Content-Type", "application/json");
                    req.AddJsonBody(new SetPasswordDto()
                    {
                        Email = Username,
                        Code = text,
                        NewPassword = Password
                    });
                    IRestResponse response = cli.Execute(req);
                    var data = response.Content;
                    if (data != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return RedirectToPage("/Index");
                    }
                    else
                    {
                        Console.WriteLine("Error : ข้อมูลไม่ตรง");
                        return Page();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error : " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Error : ข้อมูลไม่ครบ");
                return Page();
            }
            return Page();
        }
        public JsonResult OnPostCheckIDCard([FromBody] GetID data)
        {

            try
            {
                var cli = new RestClient("http://202.28.80.34:8080/ords/dev/patient/check/" + data.id)
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                IRestResponse response = cli.Execute(req);
                var dataz = JsonConvert.DeserializeObject<UserHostp>(response.Content);
                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    if (dataz.id_card ==data.id && dataz.hn == data.hn)
                    {
                        return new JsonResult("success");
                    }
                    return new JsonResult("error");
                }
                return new JsonResult("error");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new JsonResult("error");
            }
        }
    }
}

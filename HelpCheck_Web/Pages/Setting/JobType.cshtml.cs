using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HelpCheck_Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace HelpCheck_Web.Pages.Setting
{
    public class JobTypeModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<Models.WorkPlaceModel> MemberList = new List<Models.WorkPlaceModel>();
        public JobTypeModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
            var data = OnGetData();
            string tokenStr = HttpContext.Session.GetString("token");
            int? roleStr = Convert.ToInt32(HttpContext.Session.GetString("role"));

            if (string.IsNullOrWhiteSpace(tokenStr))
            {
                HttpContext.Session.Clear();
                return RedirectToPage("/Index");
            }

            if (roleStr != null)
            {
                ViewData["roleId"] = roleStr;
                if (roleStr == 1)
                {
                    return RedirectToPage("/Appointment/Index");
                }
                else if (roleStr == 2 || roleStr == 4)
                {
                    return RedirectToPage("/Doctor/Index");
                }
                else if (roleStr == 3)
                {
                    return RedirectToPage("/Patient/Index");
                }
                else if (roleStr == 5)
                {
                    return Page();
                }
                else
                {
                    return RedirectToPage("/Privacy");
                }
            }

            return Page();
        }
        public void OnGetRefleshToken()
        {
            string tokenStr = HttpContext.Session.GetString("token");
            string username = HttpContext.Session.GetString("userName");
            DateTime expDate = Convert.ToDateTime(HttpContext.Session.GetString("tokenExpDate")).ToLocalTime();
            DateTime dateNow = DateTime.Now.ToLocalTime();
            if (dateNow > expDate)
            {
                UserInfoModel user = new UserInfoModel();
                var cli = new RestClient(_configuration["api"] + "/web-apps/Auth/RefreshToken?userName=" + username)
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                var data = JsonConvert.DeserializeObject<LoginResponseDto>(response.Content);
                if (data != null)
                {
                    HttpContext.Session.SetString("token", data.AccessToken);
                    HttpContext.Session.SetString("role", data.RoleID.ToString());
                    HttpContext.Session.SetString("tokenExpDate", data.ExpireDate.ToString());
                    HttpContext.Session.SetString("userName", data.UserName);

                };
            }
        }
        public JsonResult OnGetData()
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");
            
            try
            {
                var cli = new RestClient(_configuration["api"] + "/web-apps/MasterJobType")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                var dataz = JsonConvert.DeserializeObject<List<Models.WorkPlaceModel>>(response.Content);
                this.MemberList = dataz;
                return new JsonResult(dataz);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new JsonResult(null);
            }
        }

        public JsonResult OnPost([FromBody] AddDropDownDto data)
        {
            OnGetRefleshToken();
            string token = HttpContext.Session.GetString("token");
            
            try
            {
                if (data != null && !string.IsNullOrWhiteSpace(data.Name))
                {
                    var cli = new RestClient(_configuration["api"] + "/web-apps/MasterJobType")
                    {
                        Timeout = -1
                    };
                    var req = new RestRequest(Method.POST);
                    req.AddHeader("Content-Type", "application/json");
                    req.AddHeader("Authorization", "Bearer " + token);
                    req.AddJsonBody(data);
                    IRestResponse response = cli.Execute(req);
                    if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return new JsonResult("success");
                    }
                }

                return new JsonResult("error");
            }
            catch (Exception)
            {
                return new JsonResult("error");
            }
        }
        public JsonResult OnPostEdit([FromBody] Models.WorkPlaceModel data)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");
            
            try
            {
                var cli = new RestClient(_configuration["api"] + "/web-apps/MasterJobType/" + data.id)
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.POST);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                req.Method = Method.PATCH;
                if (data != null)
                {
                    NameModel name = new NameModel()
                    {
                        name = data.name
                    };
                    req.AddJsonBody(name);
                }
                IRestResponse response = cli.Execute(req);
                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return new JsonResult("success");
                }
                return new JsonResult("error");
            }
            catch (Exception)
            {
                return new JsonResult(null);
            }
        }
        public JsonResult OnPostDelete([FromBody] Models.WorkPlaceModel data)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");
            
            try
            {
                var cli = new RestClient(_configuration["api"] + "/web-apps/MasterJobType/" + data.id)
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.POST);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                req.Method = Method.DELETE;
                IRestResponse response = cli.Execute(req);
                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return new JsonResult("success");
                }
                return new JsonResult("error");
            }
            catch (Exception)
            {
                return new JsonResult(null);
            }
        }
    }
}

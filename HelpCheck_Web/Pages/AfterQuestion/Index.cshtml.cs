using System;
using System.Collections.Generic;
using HelpCheck_Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace HelpCheck_Web.Pages.test
{
    [AllowAnonymous]
    public class IndexModel2 : PageModel
    {
        private readonly IConfiguration _configuration;
        public PatientInfoModel Info { get; set; }
        public List<MemberModel> MemberList = new List<MemberModel>();
        public List<DropdownModel> PhysicalList = new List<DropdownModel>();
        public List<PhysicaldetailModel> PhysicalDetail = new List<PhysicaldetailModel>();

        public IndexModel2(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult OnGet()
        {
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
            string password = HttpContext.Session.GetString("password");
            DateTime expDate = Convert.ToDateTime(HttpContext.Session.GetString("tokenExpDate")).AddMinutes(-10);
            DateTime dateNow = DateTime.Now.ToLocalTime();
            if (dateNow > expDate)
            {
                
                var cli = new RestClient(_configuration["api"] + "/web-apps/Auth/SignIn")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.POST);
                req.AddHeader("Content-Type", "application/json");
                req.AddJsonBody(new LoginRequestDto()
                {
                    Username = username,
                    Password = password
                });
                IRestResponse response = cli.Execute(req);
                var data = JsonConvert.DeserializeObject<LoginResponseDto>(response.Content);
                if (data != null)
                {
                    HttpContext.Session.SetString("token", data.AccessToken);
                    HttpContext.Session.SetString("role", data.RoleID.ToString());
                    HttpContext.Session.SetString("tokenExpDate", data.ExpireDate.ToString());
                    HttpContext.Session.SetString("userName", data.UserName);
                    HttpContext.Session.SetString("password", password);
                    
                }
            }
        }
        public JsonResult OnGetProfile()
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");

            try
            {
                var cli = new RestClient(_configuration["api"] + "/web-apps/User/Profile")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                var data = JsonConvert.DeserializeObject<UserInfoModel>(response.Content);
                var item = new MemberModel();
                if (data != null)
                {
                    item = new MemberModel()
                    {
                        memberId = data.ID,
                        fullName = data.TitleName + " " + data.FirstName + " " + data.LastName,
                        agencyName = data.Agency,
                        jobTypeName = data.JobTypeName,
                        workPlaceName = data.WorkPlaceName,
                        profileImg = ""
                    };
                }
                return new JsonResult(item);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new JsonResult(null);
            }
        }

        public JsonResult OnGetPatientAppointment(string search)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");
             
            try
            {
                var cli = new RestClient(_configuration["api"] + "/web-apps/Patient?search=" + search)
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                var data = JsonConvert.DeserializeObject<List<GetPatientAppointmentDto>>(response.Content);
                if (data != null)
                {
                    foreach (var m in data)
                    {
                        var item = new MemberModel()
                        {
                            memberId = m.ID,
                            fullName = m.TitleName + " " + m.FirstName + " " + m.LastName,
                            agencyName = m.Agency,
                            jobTypeName = m.JobTypeName,
                            workPlaceName = m.WorkPlaceName,
                            profileImg = ""
                        };
                        this.MemberList.Add(item);
                    }
                }
                return new JsonResult(this.MemberList);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new JsonResult(null);
            }
        }

        public JsonResult OnPost([FromBody] PatientInfoModel data)
        {
            OnGetRefleshToken();
            string token = HttpContext.Session.GetString("token");
            
            try
            {
                if (data != null)
                {
                    var cli = new RestClient(_configuration["api"] + "/web-apps/Physical")
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

        public List<DropdownModel> GetDropdownPhysicalList(string token)
        {
            try
            {
                List<DropdownModel> models = new List<DropdownModel>();
                var cli = new RestClient(_configuration["api"] + "/web-apps/Physical/DropdownList")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + token);
                IRestResponse response = cli.Execute(req);
                var data = JsonConvert.DeserializeObject<List<DropdownModel>>(response.Content);
                if (data != null)
                {
                    models = data;
                }
                return models;
            }
            catch (Exception)
            {
                return new List<DropdownModel>();
            }
        }

        public IActionResult OnGetPhysicalDetail(int id)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");
             
            try
            {
                List<PhysicaldetailModel> list = new List<PhysicaldetailModel>();
                var cli = new RestClient(_configuration["api"] + "/web-apps/Physical/" + id)
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                var data = JsonConvert.DeserializeObject<List<PhysicaldetailModel>>(response.Content);
                if (data != null)
                {
                    list = data;
                }
                return new JsonResult(list);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public IActionResult OnGetPatientInfo(int id)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");
            
            try
            {
                UserInfoModel user = new UserInfoModel();
                var cli = new RestClient(_configuration["api"] + "/web-apps/User/Profile/" + id)
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                var data = JsonConvert.DeserializeObject<UserInfoModel>(response.Content);
                if (data != null)
                {
                    user = data;
                }
                return new JsonResult(user);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}

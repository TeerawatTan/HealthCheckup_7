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
    public class UserSettingModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public List<DropdownModel> Titles { get; set; }
        public List<DropdownModel> WorkPlaces { get; set; }
        public List<DropdownModel> JobTypes { get; set; }
        public List<DropdownModel> Roles { get; set; }
        public List<UserShowModel> Userd { get; set; }

        public UserSettingModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult OnGet()
        {
            this.Titles = GetDropdownMasterTitle();
            this.WorkPlaces = GetDropdownMasterWorkPlace();
            this.JobTypes = GetDropdownMasterJobType();
            this.Roles = GetDropdownRole();
            this.Userd = GetUserInfo();
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
                    return Page();
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
                    return RedirectToPage("/Question/Question");
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
        public JsonResult OnPostEdit([FromBody] Models.UserEditSendMOdel data)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");
            
            try
            {
                if (data != null)
                {
                    UserEditModel editUser = new UserEditModel()
                    {
                        Agency = data.Agency,
                        BirthDate = data.BirthDate,
                        TitleID = data.TitleID,
                        FirstName = data.FirstName,
                        LastName = data.LastName,
                        Gender = data.Gender,
                        JobTypeID = data.JobTypeID,
                        WorkPlaceID = data.WorkPlaceID,
                        PhoneNo = data.PhoneNo,
                        HN = data.HN
                    };
                    var cli = new RestClient(_configuration["api"] + "/web-apps/User/" + data.UserID)
                    {
                        Timeout = -1
                    };
                    var req = new RestRequest(Method.POST);
                    req.AddHeader("Content-Type", "application/json");
                    req.AddHeader("Authorization", "Bearer " + tokenStr);
                    req.Method = Method.PATCH;
                    req.AddJsonBody(editUser);
                    IRestResponse response = cli.Execute(req);
                    if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return new JsonResult("success");
                    }
                    else
                    {
                        return new JsonResult("error");
                    }

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
                var cli = new RestClient(_configuration["api"] + "/web-apps/User/" + data.id)
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
        public List<UserShowModel> GetUserInfo()
        {
            OnGetRefleshToken();
            string token = HttpContext.Session.GetString("token");
            try
            {
                List<UserShowModel> models = new List<UserShowModel>();
                var cli = new RestClient(_configuration["api"] + "/web-apps/User")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + token);
                IRestResponse response = cli.Execute(req);
                var data = JsonConvert.DeserializeObject<List<UserShowModel>>(response.Content);
                if (data != null)
                {
                    
                    foreach (var item in data)
                    {
                        var subModel = new UserShowModel();
                        subModel.ID = item.ID;
                        subModel.UserID = item.UserID;
                        subModel.TitleID = item.TitleID;
                        subModel.TitleName = item.TitleName;
                        subModel.FirstName = item.FirstName;
                        subModel.LastName = item.LastName;
                        subModel.IDCard = item.IDCard;
                        subModel.BirthDate = item.BirthDate;
                        System.Globalization.CultureInfo _cultureInfoTh = new System.Globalization.CultureInfo("th-TH");
                        DateTime dThai = Convert.ToDateTime(subModel.BirthDate, _cultureInfoTh);
                        subModel.BirthDateStr = dThai.ToString("dd-MM-yyyy");
                        subModel.Age = item.Age;
                        subModel.Gender = item.Gender;
                        //if (subModel.Gender == 0)
                        //{
                        //    subModel.GenderName = "ªÒÂ";
                        //}
                        //else
                        //{
                        //    subModel.GenderName = "Ë­Ô§";
                        //}
                        subModel.Agency = item.Agency;
                        subModel.WorkPlaceID = item.WorkPlaceID;
                        subModel.WorkPlaceName = item.WorkPlaceName;
                        subModel.JobTypeID = item.JobTypeID;
                        subModel.JobTypeName = item.JobTypeName;
                        subModel.PhoneNo = item.PhoneNo;
                        models.Add(subModel);
                    }
                }
                return models;
            }
            catch (Exception)
            {
                return new List<UserShowModel>();
            }

        }
        public List<DropdownModel> GetDropdownMasterTitle()
        {
            OnGetRefleshToken();
            string token = HttpContext.Session.GetString("token");
            try
            {
                List<DropdownModel> models = new List<DropdownModel>();
                var cli = new RestClient(_configuration["api"] + "/web-apps/MasterTitle")
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

        public List<DropdownModel> GetDropdownMasterWorkPlace()
        {
            OnGetRefleshToken();
            string token = HttpContext.Session.GetString("token");
            try
            {
                List<DropdownModel> models = new List<DropdownModel>();
                var cli = new RestClient(_configuration["api"] + "/web-apps/MasterWorkPlace")
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

        public List<DropdownModel> GetDropdownMasterJobType()
        {
            OnGetRefleshToken();
            string token = HttpContext.Session.GetString("token");
            try
            {
                List<DropdownModel> models = new List<DropdownModel>();
                var cli = new RestClient(_configuration["api"] + "/web-apps/MasterJobType")
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

        public List<DropdownModel> GetDropdownRole()
        {
            OnGetRefleshToken();
            string token = HttpContext.Session.GetString("token");
            
            try
            {
                List<DropdownModel> models = new List<DropdownModel>();
                var cli = new RestClient(_configuration["api"] + "/web-apps/MasterRole")
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
    }
}

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
    public class UserProfileModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public UserInfoModel UserInfo { get; set; }

        public List<DropdownModel> Titles { get; set; }
        public List<DropdownModel> WorkPlaces { get; set; }
        public List<DropdownModel> JobTypes { get; set; }
        public List<DropdownModel> Roles { get; set; }
        public List<DropdownModel> Agency { get; set; }


        public UserProfileModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnGet()
        {
            this.WorkPlaces = GetDropdownMasterWorkPlace();
            this.JobTypes = GetDropdownMasterJobType();
            this.Roles = GetDropdownRole();
            this.Agency = GetDropdownAgency();
        }

        public JsonResult OnPost([FromBody] UserInfoModel data)
        {
            string token = HttpContext.Session.GetString("token");

            try
            {
                if (data != null)
                {
                    AddUserDto addUser = new AddUserDto()
                    {
                        Username = data.Username,
                        Password = data.Password,
                        Agency = data.Agency,
                        BirthDate = data.BirthDate,
                        Email = data.Username,
                        TitleName = data.TitleName,
                        FirstName = data.FirstName,
                        LastName = data.LastName,
                        Gender = data.Gender,
                        IDCard = data.IDCard,
                        JobTypeID = data.JobTypeID,
                        WorkPlaceID = data.WorkPlaceID,
                        PhoneNo = data.PhoneNo,
                        RoleID = data.RoleID
                    };

                    var cli = new RestClient(_configuration["api"] + "/web-apps/User/SignUp")
                    {
                        Timeout = -1
                    };
                    var req = new RestRequest(Method.POST);
                    req.AddHeader("Content-Type", "application/json");
                    req.AddHeader("Authorization", "Bearer " + token);
                    req.AddJsonBody(addUser);
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
                return new JsonResult("error");
            }
        }
        public List<DropdownModel> GetDropdownAgency()
        {
            string token = HttpContext.Session.GetString("token");
            try
            {
                List<DropdownModel> models = new List<DropdownModel>();
                var cli = new RestClient(_configuration["api"] + "/web-apps/MasterAgency")
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
        public List<DropdownModel> GetDropdownMasterTitle()
        {
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
                    foreach(var item in data)
                    {
                        if(item.ID == 14)
                        {
                            models.Add(item);
                        }
                    }
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

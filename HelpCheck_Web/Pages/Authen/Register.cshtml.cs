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

namespace HelpCheck_Web.Pages.Authen
{
    public class RegisterModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public RegisterModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public IActionResult OnGetUser(string idCard)
        {
            try
            {
                UserInfoRegisterModel user = new UserInfoRegisterModel();
                var cli = new RestClient(_configuration["api"] + "/mobile-apps/User/"+ idCard)
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                IRestResponse response = cli.Execute(req);
                var dataz = JsonConvert.DeserializeObject<UserInfoRegisterModel>(response.Content);
                if (dataz.IDCard == null)
                {
                    return new JsonResult(null);
                }
                else {
                    return new JsonResult(dataz);
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new JsonResult(null);
            }
        }
        
        public IActionResult OnGetTreatment()
        {
            string tokenStr = HttpContext.Session.GetString("token");
            try
            {
                Console.WriteLine("t12356");
                var cli = new RestClient(_configuration["api"] + "/web-apps/MasterTreatment")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                IRestResponse response = cli.Execute(req);
                var dataz = JsonConvert.DeserializeObject<List<Models.TreatmentModel>>(response.Content);
                return new JsonResult(dataz);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new JsonResult(null);
            }
        }
        public IActionResult OnGetWorkPlace()
        {
            string tokenStr = HttpContext.Session.GetString("token");
            try
            {
                Console.WriteLine("t12356");
                var cli = new RestClient(_configuration["api"] + "/mobile-apps/MasterWorkPlace")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                IRestResponse response = cli.Execute(req);
                var dataz = JsonConvert.DeserializeObject<List<Models.WorkPlaceModel>>(response.Content);
                return new JsonResult(dataz);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new JsonResult(null);
            }
        }
        public IActionResult OnGetJobType()
        {
            string tokenStr = HttpContext.Session.GetString("token");
            try
            {
                Console.WriteLine("t12356");
                var cli = new RestClient(_configuration["api"] + "/mobile-apps/MasterJobType")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                var dataz = JsonConvert.DeserializeObject<List<Models.WorkPlaceModel>>(response.Content);
                return new JsonResult(dataz);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new JsonResult(null);
            }
        }
        public IActionResult OnGetAgencyType()
        {
            string tokenStr = HttpContext.Session.GetString("token");
            try
            {
                Console.WriteLine("t12356");
                var cli = new RestClient(_configuration["api"] + "/mobile-apps/MasterAgency")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                var dataz = JsonConvert.DeserializeObject<List<Models.WorkPlaceModel>>(response.Content);
                return new JsonResult(dataz);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new JsonResult(null);
            }
        }
        public JsonResult OnGetRegister(AddUserDto data)
        {
            string token = HttpContext.Session.GetString("token");
            try
            {
                var cli = new RestClient(_configuration["api"] + "/web-apps/User/SignUp")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.POST);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + token);
                if (data != null)
                {
                    AddUserDto model = new AddUserDto()
                    {
                        Username = data.Username,
                        Password = data.Password,
                        TitleName = data.TitleName,
                        FirstName = data.FirstName,
                        LastName = data.LastName,
                        IDCard = data.IDCard,
                        BirthDate = data.BirthDate,
                        Gender = data.Gender,
                        Email = data.Email,
                        Agency = data.Agency,
                        WorkPlaceID = data.WorkPlaceID,
                        JobTypeID = data.JobTypeID,
                        PhoneNo = data.PhoneNo,
                        Hn = data.Hn,
                        RoleID = data.RoleID,
                        TreatmentID = data.TreatmentID
                    };
                    req.AddJsonBody(model);
                }
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
            catch (Exception)
            {
                return new JsonResult(null);
            }
        }
    }
}

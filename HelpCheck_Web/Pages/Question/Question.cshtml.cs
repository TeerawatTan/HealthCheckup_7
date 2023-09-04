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
    public class QuestionModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public QuestionModel(IConfiguration configuration)
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
        public IActionResult OnGetQuestion()
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");
            
            try
            {
                var cli = new RestClient(_configuration["api"] + "/mobile-apps/QuestionAndChoice/List")
                {
                    Timeout = -1
                };
                List<AnswerModel> list = new List<AnswerModel>();
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                var data = JsonConvert.DeserializeObject<List<QuestionAnsModel>>(response.Content);
                return new JsonResult(data);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new JsonResult(null);
            }
        }
        public IActionResult OnGetMyAnswer(int Year)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");
            
            try
            {
                List<AnswerModel> list = new List<AnswerModel>();
                var cli = new RestClient(_configuration["api"] + "/mobile-apps/QuestionAndChoice/MyAnswer/" + Year)
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                var data = JsonConvert.DeserializeObject<List<AnswerModel>>(response.Content);
                if (data != null)
                {
                    list = data;
                }
                return new JsonResult(list);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new JsonResult(null);
            }
        }
        public JsonResult OnGetSave(object dto)
        {
            try
            {
                OnGetRefleshToken();
                string tokenStr = HttpContext.Session.GetString("token");
                

                var cli = new RestClient(_configuration["api"] + "/mobile-apps/QuestionAndChoice/Answer")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.POST);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                if (dto != null)
                {
                    req.AddJsonBody(dto);
                }
                IRestResponse response = cli.Execute(req);
                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return new JsonResult("success");
                }
            }
            catch (Exception)
            {
                //Console.WriteLine("Error : " + ex.Message);
                //return Page();
                return new JsonResult("error");
            }

            return new JsonResult("error");
        }

        public JsonResult OnPostBooking([FromBody] List<AwnserModel> dto)
        {
            try
            {
                OnGetRefleshToken();
                string tokenStr = HttpContext.Session.GetString("token");
               
                var cli = new RestClient(_configuration["api"] + "/mobile-apps/QuestionAndChoice/Answer")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.POST);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                if (dto != null)
                {
                    req.AddJsonBody(dto);
                }
                else
                {
                    return new JsonResult("error");
                }
                IRestResponse response = cli.Execute(req);
                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return new JsonResult("success");
                }
            }
            catch (Exception)
            {
                //Console.WriteLine("Error : " + ex.Message);
                //return Page();
                return new JsonResult("error");
            }

            return new JsonResult("error");
        }
    }
}

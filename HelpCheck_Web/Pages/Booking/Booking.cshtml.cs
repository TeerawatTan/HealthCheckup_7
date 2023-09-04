using HelpCheck_Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;

namespace HelpCheck_Web.Pages.Authen
{
    public class BookingModel : PageModel
    {
        private readonly IConfiguration _configuration;
        public BookingModel(IConfiguration configuration)
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
        public void OnGetRefleshToken(string token)
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
        public JsonResult OnGetMyBooking()
        {
            try
            {
                string tokenStr = HttpContext.Session.GetString("token");
                OnGetRefleshToken(tokenStr);
                //string tokenStr = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJvaTF2TTZYUkRySmhNUmJYc0E3ZG00RllTODduWGJYZTFaTVFyajNFSmg2ZVZaalRsY1IwT1E9PSIsImp0aSI6IjBhODI3NGZjLWMxYTMtNDg0NS1hMzE4LWE2YjIxNTRmZjQ0YSIsImV4cCI6MTY1MzM2MjU0NCwiaXNzIjoiSGVscCBDaGVjayIsImF1ZCI6IkhlbHAgQ2hlY2sifQ.d8M37UNKpM1DhUvhjHPzP-nL_tLHAPPF0kHSs54Py7s";
                MyAppointmentModel user = new MyAppointmentModel();
                var cli = new RestClient(_configuration["api"] + "/mobile-apps/Appointment/MyAppointment")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                var dataz = JsonConvert.DeserializeObject<MyAppointmentModel>(response.Content);
                user = dataz;
                return new JsonResult(user);

            }
            catch (Exception)
            {
                return new JsonResult("error");
            }
        }
        public JsonResult OnGetMyProfile()
        {
            string tokenStr = HttpContext.Session.GetString("token");
            OnGetRefleshToken(tokenStr);
            //string tokenStr = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJvaTF2TTZYUkRySmhNUmJYc0E3ZG00RllTODduWGJYZTFaTVFyajNFSmg2ZVZaalRsY1IwT1E9PSIsImp0aSI6IjBhODI3NGZjLWMxYTMtNDg0NS1hMzE4LWE2YjIxNTRmZjQ0YSIsImV4cCI6MTY1MzM2MjU0NCwiaXNzIjoiSGVscCBDaGVjayIsImF1ZCI6IkhlbHAgQ2hlY2sifQ.d8M37UNKpM1DhUvhjHPzP-nL_tLHAPPF0kHSs54Py7s";
            try
            {
                UserInfoModel user = new UserInfoModel();
                var cli = new RestClient(_configuration["api"] + "/web-apps/User/Profile")
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
                    data.BirthDateStr = data.BirthDate?.ToString("dd mm yyyy");
                    user = data;
                }
                return new JsonResult(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new JsonResult(null);
            }
        }
        public JsonResult OnPostCancelBooking([FromBody] int dto)
        {
            try
            {
                string tokenStr = HttpContext.Session.GetString("token");
                OnGetRefleshToken(tokenStr);
                var cli = new RestClient(_configuration["api"] + "/mobile-apps/Appointment/CancelBooked/"+ dto)
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.DELETE);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
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

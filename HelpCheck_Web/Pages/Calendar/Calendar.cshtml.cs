using HelpCheck_Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using HelpCheck_Web.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace HelpCheck_Web.Pages.Authen
{
    public class CalendarModel : PageModel
    {
        private readonly IHubContext<MyHub> _hubContext;

        private readonly IConfiguration _configuration;
        public CalendarModel(IConfiguration configuration,IHubContext<MyHub> hubContext)
        {
            _configuration = configuration;
            _hubContext = hubContext;

        }
        public IActionResult OnGet()
        {
            string tokenStr = HttpContext.Session.GetString("token");
            int? roleStr = Convert.ToInt32(HttpContext.Session.GetString("role"));

            if (string.IsNullOrWhiteSpace(tokenStr))
            {
                HttpContext.Session.Clear();
                return Page();
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
        public IActionResult OnGetAppointment()
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");
            
            List<AppointmentModel> list = new List<AppointmentModel>();
            try
            {
                var cli = new RestClient(_configuration["api"] + "/mobile-apps/Appointment")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                var data = JsonConvert.DeserializeObject<List<AppointmentModel>>(response.Content);
                if (data != null)
                {
                    foreach (var app in data)
                    {
                        DateTimeOffset? dateStart = app.AppointmentDateTimeStart.HasValue ? app.AppointmentDateTimeStart.Value.AddHours(7) : app.AppointmentDateTimeStart;
                        DateTimeOffset? dateEnd = app.AppointmentDateTimeEnd.HasValue ? app.AppointmentDateTimeEnd.Value.AddHours(7) : app.AppointmentDateTimeEnd;

                        AppointmentModel model = new AppointmentModel();
                        model.ID = app.ID;
                        model.DoctorID = app.DoctorID;
                        model.DoctorFullName = app.DoctorFullName;
                        model.AppointmentDate = app.AppointmentDate.LocalDateTime;
                        model.AppointmentDateTimeStart = dateStart;
                        model.AppointmentDateTimeEnd = dateEnd;
                        model.MaximunBooked = app.MaximunBooked;
                        model.DateStr = app.AppointmentDate.LocalDateTime.ToString("yyy-MM-dd", new CultureInfo("en-US"));
                        model.Title = (dateStart.HasValue ? dateStart.Value.ToString("HH:mm") : "00:00") +
                            " - " + (dateEnd.HasValue ? dateEnd.Value.ToString("HH:mm") : "00:00") + " " + Convert.ToString(app.Booked + "/" + app.MaximunBooked);
                        if (app.Booked == app.MaximunBooked)
                        {
                            model.Title = (dateStart.HasValue ? dateStart.Value.ToString("HH:mm") : "00:00") +
                            " - " + (dateEnd.HasValue ? dateEnd.Value.ToString("HH:mm") : "00:00") + " " + "เต็ม";
                        }
                        
                        model.Booked = app.Booked;
                        model.MaximunBooked = app.MaximunBooked;
                        list.Add(model);
                    }

                }
                return new JsonResult(list);
            }
            catch (Exception)
            {
                return new JsonResult(list);
            }
        }
        public JsonResult OnGetMyToken()
        {
            try
            {
                string tokenStr = HttpContext.Session.GetString("token");
                return new JsonResult(tokenStr);

            }
            catch (Exception)
            {
                //Console.WriteLine("Error : " + ex.Message);
                //return Page();
                return new JsonResult("error");
            }
        }
        public JsonResult OnPostBooking([FromBody] BookingDto dto)
        {
            try
            {
                OnGetRefleshToken();
                string tokenStr = HttpContext.Session.GetString("token");
                var cli = new RestClient(_configuration["api"] + "/mobile-apps/Appointment/Booking")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.POST);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                if (dto != null)
                {
                    BookingDto model = new BookingDto()
                    {
                        DocterID = dto.DocterID,
                        AppointmentID = dto.AppointmentID,
                    };
                    req.AddJsonBody(model);
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
        public JsonResult OnGetMyBooking()
        {
            try
            {
                OnGetRefleshToken();
                string tokenStr = HttpContext.Session.GetString("token");
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

        public JsonResult OnPostReserve([FromBody] BookingDto data)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");
            //string tokenStr = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJVTXdVSDhQODdVWFY1TUZhTnBBUGszR0svalJydEgrMmN1Z0pzeU9rcjNxNUk2YVZZMjdKOWc9PSIsImp0aSI6IjBhODI3NGZjLWMxYTMtNDg0NS1hMzE4LWE2YjIxNTRmZjQ0YSIsImV4cCI6MTY1Mzg5NjI4NSwiaXNzIjoiSGVscCBDaGVjayIsImF1ZCI6IkhlbHAgQ2hlY2sifQ.TSac1adZbBoaf4Hiv9UYvmqwimWE_LHg5Vq33NYLUrQ";
            try
            {
                var cli = new RestClient(_configuration["api"] + "/mobile-apps/Appointment/Reserve")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.POST);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                if (data != null)
                {
                    BookingDto model = new BookingDto()
                    {
                        DocterID = data.DocterID,
                        AppointmentID = data.AppointmentID,
                    };
                    req.AddJsonBody(model);
                }
                IRestResponse response = cli.Execute(req);
                _hubContext.Clients.All.SendAsync("BCNewData");
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

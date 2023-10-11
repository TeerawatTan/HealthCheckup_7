using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ClosedXML.Excel;
using HelpCheck_Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace HelpCheck_Web.Pages.Appointment
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public CreateAppointmentDto AppointmentDto { get; set; }
        public List<AllPatientModel> AllPatient { get; set; }
        public int currentpage { get; set; }
        public int totalpage { get; set; }


        public IndexModel(IConfiguration configuration)
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
        public void OnGetRefleshToken2()
        {
            string tokenStr = HttpContext.Session.GetString("token");
            string username = HttpContext.Session.GetString("userName");
            string password = HttpContext.Session.GetString("password");
            DateTime expDate = Convert.ToDateTime(HttpContext.Session.GetString("tokenExpDate")).AddMinutes(-10);
            DateTime dateNow = DateTime.Now.ToLocalTime();
            var cli = new RestClient(_configuration["api"] + "/web-apps/Auth/RefreshToken?userName="+ username)
            {
                Timeout = -1
            };
            var req = new RestRequest(Method.POST);
            req.AddHeader("Content-Type", "application/json");
            req.AddHeader("Authorization", "Bearer " + tokenStr);
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
        public IActionResult OnGetAppointment()
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");
            
            List<AppointmentModel> list = new List<AppointmentModel>();
            try
            {
                var cli = new RestClient(_configuration["api"] + "/web-apps/Appointment")
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
                            " - " + (dateEnd.HasValue ? dateEnd.Value.ToString("HH:mm") : "00:00") + " " + Convert.ToString(app.Booked+"/"+app.MaximunBooked);
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

        public JsonResult OnPost([FromBody] CreateAppointmentDto data)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");
            
            try
            {
                if (data != null)
                {
                    data.AppointmentDate = data.AppointmentDate.LocalDateTime;
                    data.AppointmentTimeStart = data.AppointmentTimeStart.HasValue ? data.AppointmentTimeStart.Value.LocalDateTime : data.AppointmentTimeStart;
                    data.AppointmentTimeEnd = data.AppointmentTimeEnd.HasValue ? data.AppointmentTimeEnd.Value.LocalDateTime : data.AppointmentTimeEnd;
                }

                var cli = new RestClient(_configuration["api"] + "/web-apps/Appointment")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.POST);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                req.AddJsonBody(data);
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

        public JsonResult OnPostEdit([FromBody] SubEditAppointmentDto data)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");
           
            try
            {
                if (data != null)
                {
                    data.AppointmentDate = data.AppointmentDate.LocalDateTime;
                    data.AppointmentTimeStart = data.AppointmentTimeStart.HasValue ? data.AppointmentTimeStart.Value.LocalDateTime : data.AppointmentTimeStart;
                    data.AppointmentTimeEnd = data.AppointmentTimeEnd.HasValue ? data.AppointmentTimeEnd.Value.LocalDateTime : data.AppointmentTimeEnd;
                }

                var cli = new RestClient(_configuration["api"] + "/web-apps/Appointment/" + data.ID)
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.POST);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                req.Method = Method.PATCH;
                if (data != null)
                {
                    EditAppointmentDto edit = new EditAppointmentDto()
                    {
                        doctorID = data.DoctorID,
                        appointmentDateTimeStart = data.AppointmentTimeStart,
                        appointmentDateTimeEnd = data.AppointmentTimeEnd,
                        maximumPatient = data.MaximumPatient
                    };
                    req.AddJsonBody(edit);
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

        public JsonResult OnPostDelete([FromBody] SubEditAppointmentDto id)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");
            
            try
            {
                var cli = new RestClient(_configuration["api"] + "/web-apps/Appointment/" + id)
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

        public async Task<ActionResult> OnPostDownload([FromBody] Models.DownloadAwsAppointmentDto data)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");
            
            try
            {
                var cli = new RestClient(_configuration["api"] + "/web-apps/Answer/DownloadFile?year=" + data.Year)
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                if (response != null && response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var spit = response.Headers[5].Value.ToString().Split(';');
                    var filename = spit[1].Split('=');
                    var filePath = filename[1];
                    var provider = new FileExtensionContentTypeProvider();
                    if (!provider.TryGetContentType(filePath, out var contentType))
                    {
                        contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    }

                    var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
                    return File(bytes, contentType, Path.GetFileName(filePath));
                }
                return new JsonResult("error");
            }
            catch (Exception ex)
            {
                return new JsonResult(null);
            }
        }

        public JsonResult OnGetDownloadFileAnswer(int year)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");
            
            try
            {
                var cli = new RestClient(_configuration["api"] + "/web-apps/Answer/DownloadFile?year=" + year)
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                var data = JsonConvert.DeserializeObject<List<GetAnswerAllUserDto>>(response.Content);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return new JsonResult(data);
                }
                return new JsonResult("error");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        public JsonResult OnGetDownloadApp(string date)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");
            try
            {
                var cli = new RestClient(_configuration["api"] + "/web-apps/Reports/Appointment?date=" + date)
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                var data = JsonConvert.DeserializeObject<List<GetAllApp>>(response.Content);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return new JsonResult(data);
                }
                return new JsonResult("error");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }

        private List<ReportDailyStrModel> GetReportDailyFromAPIs(string date)
        {
            string tokenStr = HttpContext.Session.GetString("token");
            int totalPages = 1;
            int currentPage = 1;
            bool hasNextPage = true;
            List<ReportDailyStrModel> result = new List<ReportDailyStrModel>();

            while (hasNextPage)
            {
                var cli = new RestClient(_configuration["api"] + "/web-apps/Reports/Daily/HealthCheck?date=" + date + "&pageNumber=" + (currentPage) + "&pageSize=100")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                if (response.StatusCode != System.Net.HttpStatusCode.OK)
                {
                    break;
                }

                var resultObj = JObject.Parse(response.Content);
                if (resultObj != null && resultObj["items"] != null)
                {
                    if (int.TryParse(resultObj["totalPages"].ToString(), out totalPages))
                    {
                        hasNextPage = (totalPages - currentPage) > 0;
                        currentPage++;

                        var items = resultObj["items"];

                        result.AddRange(items.Select(item => new ReportDailyStrModel
                        {
                            date = item["date"].ToString(),
                            registerAmount = item["amount"].ToString(),
                            amount = item["registerAmount"].ToString(),
                            titleName = item["titleName"].ToString(),
                            firstName = item["firstName"].ToString(),
                            lastName = item["lastName"].ToString(),
                            idCard = item["idCard"].ToString(),
                            workPlaceName = item["workPlaceName"].ToString(),
                            jobTypeName = item["jobTypeName"].ToString(),
                            age = item["age"].ToString(),
                            gender = item["gender"].ToString(),
                            isHealthCheckStr = Boolean.Parse(item["isHealthCheck"].ToString()) == true ? "1" : "0",
                            isScreeningStr = Boolean.Parse(item["isScreening"].ToString()) == true ? "1" : "0",
                            isWeightStr = Boolean.Parse(item["isWeight"].ToString()) == true ? "1" : "0",
                            isHeightStr = Boolean.Parse(item["isHeight"].ToString()) == true ? "1" : "0",
                            isBmiStr = Boolean.Parse(item["isBmi"].ToString()) == true ? "1" : "0",
                            isWaistlineStr = Boolean.Parse(item["isWaistline"].ToString()) == true ? "1" : "0",
                            isBloodPressureStr = Boolean.Parse(item["isBloodPressure"].ToString()) == true ? "1" : "0",
                            isDentalCheckStr = Boolean.Parse(item["isDentalCheck"].ToString()) == true ? "1" : "0",
                            isDoctorCheckStr = Boolean.Parse(item["isDoctorCheck"].ToString()) == true ? "1" : "0",
                            isBloodCheckStr = Boolean.Parse(item["isBloodCheck"].ToString()) == true ? "1" : "0",
                            isPapSmearStr = Boolean.Parse(item["isPapSmear"].ToString()) == true ? "1" : "0",
                            isXrayStr = Boolean.Parse(item["isXray"].ToString()) == true ? "1" : "0"
                        }));
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return result;
        }

        public JsonResult OnGetDownloadAppDailyAsync(string date)
        {
            OnGetRefleshToken();
            
            try
            {
                var listData = GetReportDailyFromAPIs(date);
                if (listData != null && listData.Count > 0)
                {
                    return new JsonResult(listData);
                }
                return new JsonResult("error");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        public JsonResult OnGetDownloadPsych(string date,string pagest)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");
            try
            {
                var cli = new RestClient(_configuration["api"] + "/web-apps/Reports/Daily/PsychiatristCheck?date=" +date+ "&pageNumber="+ pagest + "&pageSize=40")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                var data = JsonConvert.DeserializeObject<PageginationResultResponse<List<ReportPsychModel>>>(response.Content);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    return new JsonResult(data);
                }
                return new JsonResult("error");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        public JsonResult OnGetDownloadDentist(string date)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");
            try
            {
                var cli = new RestClient(_configuration["api"] + "/web-apps/Reports/DentistCheck")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                var data = JsonConvert.DeserializeObject<List<DentistReportModel>>(response.Content);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {

                    return new JsonResult(data);
                }
                return new JsonResult("error");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
        public JsonResult OnGetDownloadShowPage(int date)
        {
            OnGetRefleshToken2();
            string tokenStr = HttpContext.Session.GetString("token");
            try
            {
                var cli = new RestClient(_configuration["api"] + "/web-apps/Reports/AllPatient?pageNumber="+ date + "&pageSize=10")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                var data = JsonConvert.DeserializeObject<PageginationResultResponse<List<AllPatientModel>>>(response.Content);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    foreach (var items in data.Items)
                    {
                        foreach (var model in items)
                        {
                            var sub = model.systolic.Split('/');
                            if (sub[0] != "")
                            {
                                model.systolic = sub[0];
                                model.diastolic = sub[1];
                            }
                        }
                    }
                    return new JsonResult(data);

                }
                return new JsonResult("error");
            }
            catch (Exception ex)
            {
                return new JsonResult(ex.Message);
            }
        }
    }
}

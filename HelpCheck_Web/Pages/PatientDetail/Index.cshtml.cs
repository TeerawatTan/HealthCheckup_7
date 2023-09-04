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

namespace HelpCheck_Web.Pages.PatientDetail
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public List<AnswerModel> AnswerDtos { get; set; }
        public string patientIdCard { get; set; }
        public string patientHN { get; set; }
        public string agen { get; set; }
        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public List<MemberModel> MemberList = new List<MemberModel>();
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
        public JsonResult OnGetPhychCheck(int id)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");

            try
            {
                var cli = new RestClient(_configuration["api"] + "/web-apps/Psychiatrist/" + id)
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                var data = JsonConvert.DeserializeObject<PhychModel>(response.Content);
                return new JsonResult(data);
            }
            catch (Exception)
            {
                return new JsonResult(null);
            }
        }
        public JsonResult OnGetMemberID()
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");

            try
            {
                var cli = new RestClient(_configuration["api"] + "/mobile-apps/User/Profile")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                var data = JsonConvert.DeserializeObject<GetPatientAppointmentDto>(response.Content);
                if (data != null)
                {
                    return new JsonResult(data);
                }
                return new JsonResult(null);
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

        public IActionResult OnGetAnswer(int id)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");


            try
            {
                List<AnswerModel> list = new List<AnswerModel>();
                var cli = new RestClient(_configuration["api"] + "/web-apps/QuestionAndChoice/MyAnswer/" + id)
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
            catch (Exception)
            {
                return null;
            }
        }

        public IActionResult OnGetPatientInfos(int id)
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
                    data.BirthDateStr = data.BirthDate?.ToString("dd MMMM yyyy");
                    OnGetAgencyType(data.Agency);
                    data.AgencyText = agen;
                    
                    user = data;
                }
                patientIdCard = user.IDCard;
                patientHN = user.Hn;
                user.imageBase64 = GetImageFromURlAsync(user.imageUrl);
                return new JsonResult(user);
            }
            catch (Exception)
            {
                return new JsonResult(null);
            }
        }
        public void OnGetAgencyType(string id)
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
                var dataz = JsonConvert.DeserializeObject<List<WorkPlaceModel>>(response.Content);
                foreach (var item in dataz)
                {
                    if (item.id == id)
                    {
                        agen = item.name;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw ex;
            }
        }
        public string GetImageFromURlAsync(string imageUrl)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");

            try
            {
                UserInfoModel user = new UserInfoModel();
                var cli = new RestClient(_configuration["api"] + "/api/Attachment/DownloadImage/" + imageUrl)
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                var response = cli.DownloadData(req);
                if (response.Length != 0)
                {
                    var ImageBase64 = Convert.ToBase64String(response);

                    return "data:image/png;base64," + ImageBase64;
                }
                else
                {
                    return "/assets/images/icon-personal.png";
                }

            }
            catch (Exception)
            {
                return "/assets/images/icon-personal.png";
            }
        }

        public JsonResult OnGetPatientPhysical(int memberId)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");

            try
            {
                var cli = new RestClient(_configuration["api"] + "/web-apps/Patient/Physical/" + memberId)
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                var data = JsonConvert.DeserializeObject<List<GetPhysicalExaminationMasterDto>>(response.Content);
                return new JsonResult(data);
            }
            catch (Exception)
            {
                return new JsonResult(null);
            }
        }

        public JsonResult OnGetDoctorCheck(int id)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");

            try
            {
                var cli = new RestClient(_configuration["api"] + "/web-apps/Doctor/" + id)
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                var data = JsonConvert.DeserializeObject<GetDoctorCheckDto>(response.Content);
                return new JsonResult(data);
            }
            catch (Exception)
            {
                return new JsonResult(null);
            }
        }

        public JsonResult OnGetDentistCheck(int id)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");

            try
            {
                var cli = new RestClient(_configuration["api"] + "/web-apps/Dentist/" + id)
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                IRestResponse response = cli.Execute(req);
                var data = JsonConvert.DeserializeObject<GetDentistCheckDto>(response.Content);
                return new JsonResult(data);
            }
            catch (Exception)
            {
                return new JsonResult(null);
            }
        }

        public JsonResult OnPostDoctor([FromBody] DoctorDto data)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");

            try
            {
                var cli = new RestClient(_configuration["api"] + "/web-apps/Doctor")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.POST);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + tokenStr);
                if (data != null)
                {
                    AddDoctorModel model = new AddDoctorModel()
                    {
                        DoctorID = data.DoctorId,
                        MemberID = data.MemberId,
                        IsResult = data.IsResult,
                        Detail = data.Detail
                    };
                    req.AddJsonBody(model);
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

        public JsonResult OnPostDentist([FromBody] DentistDto data)
        {
            OnGetRefleshToken();
            string tokenStr = HttpContext.Session.GetString("token");

            try
            {
                var cli = new RestClient(_configuration["api"] + "/web-apps/Dentist")
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
        public JsonResult OnGetXrayDetail(string id)
        {
            try
            {
                var cli = new RestClient("http://202.28.80.34:8080/ords/dev/patient/xrayresult/" + id)
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                IRestResponse response = cli.Execute(req);
                if (response != null)
                {
                    var data = JsonConvert.DeserializeObject<List<object>>(response.Content);
                    return new JsonResult(data);
                }
                else
                {
                    return new JsonResult(null);
                }
            }
            catch (Exception)
            {
                return new JsonResult(null);
            }
        }
    }
    
}

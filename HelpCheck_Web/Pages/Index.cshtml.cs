using HelpCheck_Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestSharp;
using System;

namespace HelpCheck_Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        //[BindProperty]
        //public string Username { get; set; }

        //[BindProperty]
        //public string Password { get; set; }

        private readonly IConfiguration _configuration;
        public IndexModel(IConfiguration configuration, ILogger<IndexModel> logger)
        {
            _configuration = configuration;
            _logger = logger;
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
                else if (roleStr == 2 || roleStr == 4 || roleStr == 6)
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

        public JsonResult OnPost([FromBody] LoginRequestDto dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.Username) || !string.IsNullOrWhiteSpace(dto.Password))
            {
                try
                {
                    HttpContext.Session.Clear();

                    var cli = new RestClient(_configuration["api"] + "/web-apps/Auth/SignIn")
                    {
                        Timeout = -1
                    };
                    var req = new RestRequest(Method.POST);
                    req.AddHeader("Content-Type", "application/json");
                    req.AddJsonBody(new LoginRequestDto()
                    {
                        Username = dto.Username,
                        Password = dto.Password
                    });
                    IRestResponse response = cli.Execute(req);
                    var data = JsonConvert.DeserializeObject<LoginResponseDto>(response.Content);
                    if (data != null)
                    {
                        HttpContext.Session.SetString("token", data.AccessToken);
                        HttpContext.Session.SetString("role", data.RoleID.ToString());
                        HttpContext.Session.SetString("tokenExpDate", data.ExpireDate.ToString());
                        HttpContext.Session.SetString("userName", data.UserName);
                        HttpContext.Session.SetString("password", dto.Password);
                        if (data.RoleID != null)
                        {
                            ViewData["roleId"] = data.RoleID.ToString();
                            if (data.RoleID == 1)
                            {
                                return new JsonResult("/Appointment/Index");
                                //return RedirectToPage("/Appointment/Index");
                            }
                            else if (data.RoleID == 2 || data.RoleID == 4 || data.RoleID == 6)
                            {
                                return new JsonResult("/Doctor/Index");
                                //return RedirectToPage("/Doctor/Index");
                            }
                            else if (data.RoleID == 3)
                            {
                                return new JsonResult("/Patient/Index");
                                //return RedirectToPage("/Patient/Index");
                            }
                            else if (data.RoleID == 5)
                            {
                                return new JsonResult("/Question/Question");
                            }
                            else
                            {
                                return new JsonResult("/Privacy");
                                //return RedirectToPage("/Privacy");
                            }

                            //if (data.RoleID == 5)
                            //{
                            //    return new JsonResult("/Question/Question");
                            //}
                            //else
                            //{
                            //    return new JsonResult("/Privacy");
                            //    //return RedirectToPage("/Privacy");
                            //}
                        }
                    }
                }
                catch (Exception)
                {
                    //Console.WriteLine("Error : " + ex.Message);
                    //return Page();
                    return new JsonResult("error");
                }
            }

            return new JsonResult("error");
        }


        public JsonResult OnGetUserInfo()
        {
            string token = HttpContext.Session.GetString("token");
            try
            {
                UserInfoModel user = new UserInfoModel();
                var cli = new RestClient(_configuration["api"] + "/web-apps/User/Profile")
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                req.AddHeader("Authorization", "Bearer " + token);
                IRestResponse response = cli.Execute(req);
                var data = JsonConvert.DeserializeObject<UserInfoModel>(response.Content);
                if (data != null)
                {
                    data.BirthDateStr = data.BirthDate?.ToString("dd mm yyyy");
                    user = data;
                    user.imageBase64 = GetImageFromURlAsync(user.imageUrl);
                }
                return new JsonResult(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new JsonResult(null);
            }
        }
        public string GetImageFromURlAsync(string imageUrl)
        {
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
                if (response.Length !=0)
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
    }
}
    

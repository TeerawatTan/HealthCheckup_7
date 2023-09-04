using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.Patients;
using HelpCheck_API.Dtos.Reports;
using HelpCheck_API.Repositories.DentistChecks;
using HelpCheck_API.Repositories.DoctorChecks;
using HelpCheck_API.Repositories.Patients;
using HelpCheck_API.Repositories.PhysicalExaminationMasters;
using HelpCheck_API.Repositories.QuestionAndChoices;
using HelpCheck_API.Services.Patients;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HelpCheck_API.Services.Reports
{
    public class ReportService : IReportService
    {
        private readonly IPatientRepository _patientRepository;
        private readonly IPhysicalRepository _physicalRepository;
        private readonly IPatientService _patientService;
        private readonly IQuestionAndChoiceRepository _questionAndChoiceRepository;
        private readonly IDentistCheckRepository _dentistCheckRepository;
        private readonly IDoctorCheckRepository _doctorCheckRepository;
        private readonly IMemoryCache _memoryCache;

        public ReportService(IPatientRepository patientRepository, IPhysicalRepository physicalRepository, IPatientService patientService, IQuestionAndChoiceRepository questionAndChoiceRepository, IDentistCheckRepository dentistCheckRepository, IDoctorCheckRepository doctorCheckRepository, IMemoryCache memoryCache)
        {
            _patientRepository = patientRepository;
            _physicalRepository = physicalRepository;
            _patientService = patientService;
            _questionAndChoiceRepository = questionAndChoiceRepository;
            _dentistCheckRepository = dentistCheckRepository;
            _doctorCheckRepository = doctorCheckRepository;
            _memoryCache = memoryCache;
        }

        private async Task<CheckResultDetailFromPMKDto> GetCheckResultAsync(string idcard)
        {
            try
            {
                var cli = new RestClient("http://dev34.pmk.ac.th:8080/ords/dev/HEALTH_CHECK/Check_result/" + idcard.Trim().Replace("-", ""))
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                IRestResponse response = await cli.ExecuteAsync(req);
                var data = JsonConvert.DeserializeObject<CheckResultFromPMKDto>(response.Content);
                if (data is not null && data.Data is not null && data.Data.Count > 0)
                {
                    return data.Data.LastOrDefault();
                }
                return null;
            }
            catch (System.Exception ex)
            {
                throw new Exception("Error from api pmk : " + ex.Message);
            }
        }

        private async Task<CheckResultToDayDetailFromPMKDto> GetCheckResultTodayAsync(string idcard) //, DateTime date) /* น่าจะมีในอนาคต  */
        {
            try
            {
                var cli = new RestClient("http://dev34.pmk.ac.th:8080/ords/dev/HEALTH_CHECK/Check_result_to_day/" + idcard.Trim().Replace("-", ""))
                {
                    Timeout = -1
                };
                var req = new RestRequest(Method.GET);
                req.AddHeader("Content-Type", "application/json");
                IRestResponse response = await cli.ExecuteAsync(req);
                var data = JsonConvert.DeserializeObject<CheckResultToDayFromPMKDto>(response.Content);
                if (data is not null && data.Data is not null && data.Data.Count > 0)
                {
                    return data.Data.LastOrDefault();
                }
                return null;
            }
            catch (System.Exception ex)
            {
                throw new Exception("Error from api pmk : " + ex.Message);
            }
        }

        public async Task<ResultResponse> GetReportAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            pageNumber = (pageNumber < 0) ? 1 : pageNumber;
            pageSize = (pageSize < 0) ? 10 : pageSize;

            try
            {
                PageginationResultResponse<List<GetReportDto>> result = new PageginationResultResponse<List<GetReportDto>>();
                List<GetReportDto> response = new List<GetReportDto>();
                CheckResultDetailFromPMKDto checkResultDetailDto = new CheckResultDetailFromPMKDto();

                var patientAppointments = await _patientRepository.GetAllPatientAppointmnetAsync(new PageRequestModel() { PageNumber = pageNumber, PageSize = pageSize }, cancellationToken);

                if (patientAppointments is not null && patientAppointments.Items is not null && patientAppointments.Items.Count > 0)
                {
                    foreach (var p in patientAppointments.Items.ToList())
                    {
                        var physicals = await _physicalRepository.GetPhysicalAnswerListByMemberIDAsync(p.ID);

                        GetReportDto reportDto = new GetReportDto();

                        // Infomation
                        reportDto.TitleName = p.TitleName;
                        reportDto.FirstName = p.FirstName;
                        reportDto.LastName = p.LastName;
                        reportDto.Hn = p.Hn;
                        reportDto.IdCard = p.IDCard;
                        reportDto.WorkPlaceName = p.WorkPlaceName;
                        reportDto.JobTypeName = p.JobTypeName;
                        switch (p.Agency)
                        {
                            case "1":
                                reportDto.Agency = "อื่น ๆ";
                                break;
                            case "2":
                                reportDto.Agency = "กพย.รพ.รร.๖";
                                break;
                            case "3":
                                reportDto.Agency = "กวฟ.รพ.รร.๖";
                                break;
                            case "4":
                                reportDto.Agency = "กภก.รพ.รร.๖";
                                break;
                            case "5":
                                reportDto.Agency = "กจก.รพ.รร.๖";
                                break;
                            case "6":
                                reportDto.Agency = "กสศ.รพ.รร.๖";
                                break;
                            case "7":
                                reportDto.Agency = "กอย.รพ.รร.๖";
                                break;
                            case "8":
                                reportDto.Agency = "กศก.รพ.รร.๖";
                                break;
                            case "9":
                                reportDto.Agency = "กรส.รพ.รร.๖";
                                break;
                            case "10":
                                reportDto.Agency = "กสน.รพ.รร.๖";
                                break;
                            case "11":
                                reportDto.Agency = "กทต.รพ.รร.๖";
                                break;
                            case "12":
                                reportDto.Agency = "กตร.รพ.รร.๖";
                                break;
                            case "13":
                                reportDto.Agency = "กอธ.รพ.รร.๖";
                                break;
                            case "14":
                                reportDto.Agency = "กจว.รพ.รร.๖";
                                break;
                            case "15":
                                reportDto.Agency = "กกว.รพ.รร.๖";
                                break;
                            case "16":
                                reportDto.Agency = "กวญ.รพ.รร.๖";
                                break;
                            case "17":
                                reportDto.Agency = "กพธ.รพ.รร.๖";
                                break;
                            case "18":
                                reportDto.Agency = "กอบ.รพ.รร.๖";
                                break;
                            case "19":
                                reportDto.Agency = "ผวภ.รพ.รร.๖";
                                break;
                            case "20":
                                reportDto.Agency = "ร้อย.พล.สร.รพ.รร.๖";
                                break;
                            case "21":
                                reportDto.Agency = "สง.ผบช.รพ.รร.๖";
                                break;
                            case "22":
                                reportDto.Agency = "ศูนย์รับรองข้าราชการ";
                                break;
                            case "23":
                                reportDto.Agency = "ศูนย์โรคหัวใจสิรินธร";
                                break;
                            case "24":
                                reportDto.Agency = "ศูนย์บริหารงานวิชาการ";
                                break;
                            case "25":
                                reportDto.Agency = "มูลนิธิ";
                                break;
                            case "26":
                                reportDto.Agency = "ศูนย์บริหารการเงิน";
                                break;
                            case "27":
                                reportDto.Agency = "คลินิกนอกเวลา";
                                break;
                            case "28":
                                reportDto.Agency = "ศบบ.รพ.รร.๖";
                                break;
                            case "29":
                                reportDto.Agency = "ศคม.";
                                break;
                            case "30":
                                reportDto.Agency = "ศูนย์บริหารงานสิ่งแวดล้อม";
                                break;
                            case "31":
                                reportDto.Agency = "ผธก.กพ.รพ.รร.๖";
                                break;
                            case "32":
                                reportDto.Agency = "ผกง.รพ.รร.๖";
                                break;
                            case "33":
                                reportDto.Agency = "งป.รพ.รร.๖";
                                break;
                            case "34":
                                reportDto.Agency = "หน่วยระบาด";
                                break;
                            case "35":
                                reportDto.Agency = "หน่วยป้องกันและควบคุมโรคติดเชื้อ";
                                break;
                            case "36":
                                reportDto.Agency = "ศูนย์คอมพิวเตอร์";
                                break;
                            case "37":
                                reportDto.Agency = "นธน.รพ.รร.๖";
                                break;
                            case "38":
                                reportDto.Agency = "ฝสก.รพ.รร.๖";
                                break;
                            case "39":
                                reportDto.Agency = "ศปชส.รพ.รร.๖";
                                break;
                            case "40":
                                reportDto.Agency = "ผวส.รพ.รร.๖";
                                break;
                            case "41":
                                reportDto.Agency = "สงร.รพ.รร.๖";
                                break;
                            case "42":
                                reportDto.Agency = "ศูนย์จำหน่ายผู้ป่วย";
                                break;
                            case "43":
                                reportDto.Agency = "ผสค.รพ.รร.๖";
                                break;
                            case "44":
                                reportDto.Agency = "ศูนย์บริหารงานยุทธศาสตร์";
                                break;
                            case "45":
                                reportDto.Agency = "ศูนย์โทรศัพท์";
                                break;
                            case "46":
                                reportDto.Agency = "ผขส.รพ.รร.๖";
                                break;
                            case "47":
                                reportDto.Agency = "ผยย.รพ.รร.๖";
                                break;
                            case "48":
                                reportDto.Agency = "ผพธ.รพ.รร.6";
                                break;
                            case "49":
                                reportDto.Agency = "ฝกก.รพ.รร.6";
                                break;
                            case "50":
                                reportDto.Agency = "ฝซร.รพ.รร.๖";
                                break;
                            case "51":
                                reportDto.Agency = "ฝยบ.รพ.รร.๖";
                                break;
                            case "52":
                                reportDto.Agency = "ศวภ.รพ.รร.๖";
                                break;
                            case "53":
                                reportDto.Agency = "ศกบ.รพ.รร.๖";
                                break;
                            case "54":
                                reportDto.Agency = "ศูนย์เคลื่อนย้ายผู้ป่วย";
                                break;
                            case "55":
                                reportDto.Agency = "ศูนย์พัฒนางานวิจัยและวิจัยทางคลินิก";
                                break;
                            case "56":
                                reportDto.Agency = "สง.ส่งเสริมและพัฒนางานวิจัยวพม./รพ.รร.6";
                                break;
                        }

                        reportDto.BirthDate = p.BirthDate;
                        reportDto.Sex = p.Sex;
                        reportDto.Privilege = p.TreatmentName;
                        // BMI
                        reportDto.Weight = physicals.Count > 0 ? Math.Round(Convert.ToDecimal(physicals.LastOrDefault(f => "น้ำหนัก".Contains(f.DescriptTh))?.Value ?? "0"), 2) : 0;
                        reportDto.Height = physicals.Count > 0 ? Math.Round(Convert.ToDecimal(physicals.LastOrDefault(f => "ส่วนสูง".Contains(f.DescriptTh))?.Value ?? "0"), 2) : 0;
                        reportDto.Waistline = physicals.Count > 0 ? Math.Round(Convert.ToDecimal(physicals.LastOrDefault(f => "รอบเอว".Contains(f.DescriptTh))?.Value ?? "0"), 2) : 0;
                        reportDto.Systolic = physicals.Count > 0 ? physicals.LastOrDefault(f => "ความดันโลหิต 1".Contains(f.DescriptTh))?.Value : "";
                        reportDto.Diastolic = physicals.Count > 0 ? Convert.ToInt32(physicals.LastOrDefault(f => "ชีพจร 1".Contains(f.DescriptTh))?.Value ?? "0") : 0;

                        if (!string.IsNullOrWhiteSpace(p.IDCard) && p.IDCard.Length >= 13)
                        {
                            checkResultDetailDto = await GetCheckResultAsync(p.IDCard);
                            if (checkResultDetailDto is not null)
                            {
                                reportDto.TitleName = checkResultDetailDto.prename;
                                reportDto.FirstName = checkResultDetailDto.name;
                                reportDto.LastName = checkResultDetailDto.surname;
                                reportDto.Hn = checkResultDetailDto.hn;
                                // X-Ray
                                reportDto.XRayResult = checkResultDetailDto.xray;
                                reportDto.XRayResultOther = default;
                                // Urine Examination
                                reportDto.Result = checkResultDetailDto.ua;
                                reportDto.Proteinurea = checkResultDetailDto.protrinurea;
                                reportDto.Hematuria = checkResultDetailDto.hematoria;
                                reportDto.ResultOther = default;
                                // ผลการตรวจเลือด
                                reportDto.CbcResult = checkResultDetailDto.cbc;
                                reportDto.CbcOther = default;
                                reportDto.Glu = checkResultDetailDto.GLU;
                                reportDto.Chol = checkResultDetailDto.CHOL;
                                reportDto.TG = checkResultDetailDto.TG;
                                reportDto.HDLC = default;
                                reportDto.LDLC = default;
                                reportDto.BUN = checkResultDetailDto.BUN;
                                reportDto.Cr = checkResultDetailDto.CR;
                                reportDto.Uric = checkResultDetailDto.URIN;
                                reportDto.AST = checkResultDetailDto.AST;
                                reportDto.ALT = checkResultDetailDto.ALT;
                                // Pap smear
                                reportDto.PapSmearResult = checkResultDetailDto.PAP_SMEAR;
                                reportDto.PapSmearResultOther = default;

                            }

                            // ประวัติโรคประจำตัว
                            reportDto.CongenitalDiseaseResult = await _questionAndChoiceRepository.GetHasAnswerCongenitalDiseaseResultAsync(p.ID);
                            reportDto.CongenitalDiseaseResultOther = default;
                            // พฤติกรรมการดำเนินชีวิตที่มีผลต่อความเสี่ยงเป็นโรค
                            GetReportAllPatientDto c = await _questionAndChoiceRepository.GetSmokingBehaviorAsync(p.ID);
                            reportDto.SmokingBehavior = c.SmokingBehavior;
                            reportDto.AlcoholBehavior = c.AlcoholBehavior;
                            reportDto.ExerciseBehavior = c.ExerciseBehavior;

                            response.Add(reportDto);
                        }
                    }
                }

                result.CurrentPage = patientAppointments.CurrentPage;
                result.PageSize = patientAppointments.PageSize;
                result.TotalItems = patientAppointments.TotalItems;
                result.TotalPages = patientAppointments.TotalPages;
                result.Items.Add(response);

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = ex.Message
                };
            }
        }

        public async Task<ResultResponse> GetReportDailyBccCheck()
        {
            List<GetReportDailyDoctorCheckDto> response = new List<GetReportDailyDoctorCheckDto>();

            var patientAppointments = await _patientRepository.GetAllPatientAppointmnetAsync(new FilterPatientDto());

            if (patientAppointments is not null && patientAppointments.Count > 0)
            {
                foreach (var p in patientAppointments)
                {
                    GetReportDailyDoctorCheckDto reportDto = new GetReportDailyDoctorCheckDto();
                    // reportDto.Date = DateTime.Now;
                    reportDto.TitleName = p.TitleName;
                    reportDto.FirstName = p.FirstName;
                    reportDto.LastName = p.LastName;
                    reportDto.BirthDate = p.BirthDate;
                    reportDto.Province = default;
                    reportDto.IsBcc = default;
                    reportDto.IsPopulation = default;
                    reportDto.IsOfficer = default;
                    reportDto.IsSoldier = default;

                    response.Add(reportDto);
                }
            }

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = response
            };
        }

        private async Task<GetAllPatientAppointmentDto> GetDataPatientAppointmentAsync(GetAllPatientAppointmentDto data, DateTime? dateTime, int patientCount, int count)
        {
            try
            {
                bool checkBlood = false, checkXray = false;

                if (!string.IsNullOrWhiteSpace(data.IDCard))
                {
                    string cacheKey = $"getCheckResultToday_{data.IDCard}";

                    if (!_memoryCache.TryGetValue(cacheKey, out CheckResultToDayDetailFromPMKDto cachedValue))
                    {
                        CheckResultToDayDetailFromPMKDto resultCheckToday = await GetCheckResultTodayAsync(data.IDCard);
                        if (resultCheckToday != null)
                        {
                            cachedValue = resultCheckToday;
                            var cacheEntryOptions = new MemoryCacheEntryOptions
                            {
                                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                            };
                            _memoryCache.Set(cacheKey, cachedValue, cacheEntryOptions);
                        }
                    }
                    checkBlood = cachedValue != null && cachedValue.LAB == "1";
                    checkXray = cachedValue != null && cachedValue.XRAY == "1";
                }

                data.Date = dateTime ?? DateTime.Now;
                data.RegisterAmount = patientCount;
                data.Amount = count;
                data.IsBloodCheck = checkBlood;
                data.IsPapSmear = data.Sex.HasValue && data.Sex.Value == 1 && data.Age != null && data.Age >= 35;
                data.IsXray = checkXray;

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResultResponse> GetReportDailyHealthCheck(FilterPatientDto filter, int pageNumber, int pageSize)
        {
            try
            {
                pageNumber = (pageNumber < 0) ? 1 : pageNumber;
                pageSize = (pageSize < 0) ? 10 : pageSize;

                PageginationResultResponse<GetAllPatientAppointmentDto> response = new PageginationResultResponse<GetAllPatientAppointmentDto>();

                if (filter != null && filter.AppointmentDate.HasValue)
                {
                    PageginationResultResponse<GetAllPatientAppointmentDto> patientAppointments = await _patientRepository.GetAllPatientAppointmnetPaginationAsync(filter, new PageRequestModel() { PageNumber = pageNumber, PageSize = pageSize });

                    int patientCount = await _patientRepository.CountPhysicalWithPatientAsync(filter.AppointmentDate.Value.Date);
                    if (patientAppointments is not null && patientAppointments.Items is not null && patientAppointments.Items.Count > 0)
                    {
                        foreach (var item in patientAppointments.Items)
                        {
                            response.Items.Add(await GetDataPatientAppointmentAsync(item, filter.AppointmentDate, patientCount, patientAppointments.TotalItems));
                        }
                    }

                    response.CurrentPage = patientAppointments.CurrentPage;
                    response.PageSize = patientAppointments.PageSize;
                    response.TotalItems = patientAppointments.TotalItems;
                    response.TotalPages = patientAppointments.TotalPages;
                }

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = response
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ResultResponse> GetReportDailyPsychiatristCheck(DateTime dt, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            try
            {
                PageginationResultResponse<List<GetReportDailyPsychiatristCheckDto>> response = new PageginationResultResponse<List<GetReportDailyPsychiatristCheckDto>>();
                CheckResultDetailFromPMKDto checkResultDetailDto = new CheckResultDetailFromPMKDto();
                List<GetReportDailyPsychiatristCheckDto> result = new List<GetReportDailyPsychiatristCheckDto>();

                var appointments = await _patientRepository.GetReportAllPatientWithLastAppointmentAsync(dt, new PageRequestModel() { PageNumber = pageNumber, PageSize = pageSize }, cancellationToken);

                if (appointments is not null && appointments.Items is not null && appointments.Items.Count > 0)
                {
                    foreach (var p in appointments.Items.ToList())
                    {
                        GetReportDailyPsychiatristCheckDto reportDto = new GetReportDailyPsychiatristCheckDto();
                        var ans = await _patientRepository.GetPatientWithAnswerDetailAsync(p.ID);
                        if (ans is not null && ans.Count > 0)
                        {
                            reportDto.MemberID = p.ID;
                            reportDto.FirstName = p.FirstName;
                            reportDto.LastName = p.LastName;
                            reportDto.Hn = p.Hn;
                            reportDto.IdCard = p.IDCard;
                            reportDto.WorkPlaceName = p.WorkPlaceName;
                            reportDto.JobTypeName = p.JobTypeName;
                            reportDto.BirthDate = p.BirthDate;
                            reportDto.Sex = p.Sex;
                            reportDto.TreatmentName = p.TreatmentName;
                            reportDto.Question2QOne = ans.FirstOrDefault(answer => answer.Question2QOne.HasValue && answer.Question2QOne.Value) is not null;
                            reportDto.Question2QTwo = ans.FirstOrDefault(answer => answer.Question2QTwo.HasValue && answer.Question2QTwo.Value) is not null;
                            reportDto.Question9QOne = ans.FirstOrDefault(answer => answer.Question9QOne > 0) is not null ? ans.FirstOrDefault(answer => answer.Question9QOne > 0).Question9QOne : 0;
                            reportDto.Question9QTwo = ans.FirstOrDefault(answer => answer.Question9QTwo > 0) is not null ? ans.FirstOrDefault(answer => answer.Question9QTwo > 0).Question9QTwo : 0;
                            reportDto.Question9QThree = ans.FirstOrDefault(answer => answer.Question9QThree > 0) is not null ? ans.FirstOrDefault(answer => answer.Question9QThree > 0).Question9QThree : 0;
                            reportDto.Question9QFour = ans.FirstOrDefault(answer => answer.Question9QFour > 0) is not null ? ans.FirstOrDefault(answer => answer.Question9QFour > 0).Question9QFour : 0;
                            reportDto.Question9QFive = ans.FirstOrDefault(answer => answer.Question9QFive > 0) is not null ? ans.FirstOrDefault(answer => answer.Question9QFive > 0).Question9QFive : 0;
                            reportDto.Question9QSix = ans.FirstOrDefault(answer => answer.Question9QSix > 0) is not null ? ans.FirstOrDefault(answer => answer.Question9QSix > 0).Question9QSix : 0;
                            reportDto.Question9QSeven = ans.FirstOrDefault(answer => answer.Question9QSeven > 0) is not null ? ans.FirstOrDefault(answer => answer.Question9QSeven > 0).Question9QSeven : 0;
                            reportDto.Question9QEight = ans.FirstOrDefault(answer => answer.Question9QEight > 0) is not null ? ans.FirstOrDefault(answer => answer.Question9QEight > 0).Question9QEight : 0;
                            reportDto.Question9QNine = ans.FirstOrDefault(answer => answer.Question9QNine > 0) is not null ? ans.FirstOrDefault(answer => answer.Question9QNine > 0).Question9QNine : 0;

                        }
                        if (!string.IsNullOrWhiteSpace(p.IDCard) && p.IDCard.Length >= 13)
                        {
                            checkResultDetailDto = await GetCheckResultAsync(p.IDCard);
                            if (checkResultDetailDto is not null)
                            {
                                reportDto.TitleName = checkResultDetailDto.prename;
                            }
                        }
                        result.Add(reportDto);
                    }
                }

                response.CurrentPage = appointments.CurrentPage;
                response.PageSize = appointments.PageSize;
                response.TotalItems = appointments.TotalItems;
                response.TotalPages = appointments.TotalPages;
                response.Items.Add(result);

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = ex.Message
                };
            }
        }

        public async Task<ResultResponse> GetReportDentistCheck()
        {
            try
            {
                var data = await _patientRepository.GetReportAllPatientWithDentistCheckAsync();

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = ex.Message
                };
            }
        }
    }
}

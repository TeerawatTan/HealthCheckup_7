using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.Patients;
using HelpCheck_API.Dtos.Reports;
using HelpCheck_API.Repositories.DentistChecks;
using HelpCheck_API.Repositories.DoctorChecks;
using HelpCheck_API.Repositories.OtherInterfaceHospitals;
using HelpCheck_API.Repositories.OtherInterfaces;
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
        private readonly IOtherInterfaceHospital _otherInterfaceHospital;

        public ReportService(IPatientRepository patientRepository, IPhysicalRepository physicalRepository, IPatientService patientService, IQuestionAndChoiceRepository questionAndChoiceRepository, IDentistCheckRepository dentistCheckRepository, IDoctorCheckRepository doctorCheckRepository, IMemoryCache memoryCache,IOtherInterfaceHospital otherInterfaceHospital)
        {
            _patientRepository = patientRepository;
            _physicalRepository = physicalRepository;
            _patientService = patientService;
            _questionAndChoiceRepository = questionAndChoiceRepository;
            _dentistCheckRepository = dentistCheckRepository;
            _doctorCheckRepository = doctorCheckRepository;
            _memoryCache = memoryCache;
            _otherInterfaceHospital = otherInterfaceHospital;
        }

        private async Task<CheckResultDetailFromPMKDto> GetCheckResultAsync(string idcard)
        {
            try
            {
                //var cli = new RestClient("http://dev34.pmk.ac.th:8080/ords/dev/HEALTH_CHECK/Check_result/" + idcard.Trim().Replace("-", ""))
                //{
                //    Timeout = -1
                //};
                //var req = new RestRequest(Method.GET);
                //req.AddHeader("Content-Type", "application/json");
                //IRestResponse response = await cli.ExecuteAsync(req);
                //var data = JsonConvert.DeserializeObject<CheckResultFromPMKDto>(response.Content);
                //if (data is not null && data.Data is not null && data.Data.Count > 0)
                //{
                //    return data.Data.LastOrDefault();
                //}
                

                return _otherInterfaceHospital.GetDetailResultFromHospitals(idcard).FirstOrDefault();

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
                //var cli = new RestClient("http://dev34.pmk.ac.th:8080/ords/dev/HEALTH_CHECK/Check_result_to_day/" + idcard.Trim().Replace("-", ""))
                //{
                //    Timeout = -1
                //};
                //var req = new RestRequest(Method.GET);
                //req.AddHeader("Content-Type", "application/json");
                //IRestResponse response = await cli.ExecuteAsync(req);
                //var data = JsonConvert.DeserializeObject<CheckResultToDayFromPMKDto>(response.Content);
                //if (data is not null && data.Data is not null && data.Data.Count > 0)
                //{
                //    return data.Data.LastOrDefault();
                //}
                return _otherInterfaceHospital.GetResultToDayDetailFromPMKs(idcard).FirstOrDefault();
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

                        reportDto.MemberID = p.ID;
                        reportDto.TitleName = p.TitleName;
                        reportDto.FirstName = p.FirstName;
                        reportDto.LastName = p.LastName;
                        reportDto.Hn = p.Hn;
                        reportDto.IdCard = p.IDCard;
                        reportDto.WorkPlaceName = p.WorkPlaceName;
                        reportDto.JobTypeName = p.JobTypeName;
                        reportDto.BirthDate = p.BirthDate;
                        reportDto.Sex = p.Sex;
                        reportDto.TreatmentName = p.TreatmentName;

                        var ans = await _patientRepository.GetPatientWithAnswerDetailAsync(p.ID);
                        if (ans is not null)
                        {
                            reportDto.Question9Q_1 = ans.Question9Q_1;
                            reportDto.Question9Q_2 = ans.Question9Q_2;
                            reportDto.Question9Q_3 = ans.Question9Q_3;
                            reportDto.Question9Q_4 = ans.Question9Q_4;
                            reportDto.Question9Q_5 = ans.Question9Q_5;
                            reportDto.Question9Q_6 = ans.Question9Q_6;
                            reportDto.Question9Q_7 = ans.Question9Q_7;
                            reportDto.Question9Q_8 = ans.Question9Q_8;
                            reportDto.Question9Q_9 = ans.Question9Q_9;

                            reportDto.Question8Q_1 = ans.Question8Q_1;
                            reportDto.Question8Q_2 = ans.Question8Q_2;
                            reportDto.Question8Q_3 = ans.Question8Q_3;
                            reportDto.Question8Q_4 = ans.Question8Q_4;
                            reportDto.Question8Q_5 = ans.Question8Q_5;
                            reportDto.Question8Q_6 = ans.Question8Q_6;
                            reportDto.Question8Q_7 = ans.Question8Q_7;
                            reportDto.Question8Q_8 = ans.Question8Q_8;
                            reportDto.Question8Q_9 = ans.Question8Q_9;

                            reportDto.QuestionGHQ28_1 = ans.QuestionGHQ28_1;
                            reportDto.QuestionGHQ28_2 = ans.QuestionGHQ28_2;
                            reportDto.QuestionGHQ28_3 = ans.QuestionGHQ28_3;
                            reportDto.QuestionGHQ28_4 = ans.QuestionGHQ28_4;
                            reportDto.QuestionGHQ28_5 = ans.QuestionGHQ28_5;
                            reportDto.QuestionGHQ28_6 = ans.QuestionGHQ28_6;
                            reportDto.QuestionGHQ28_7 = ans.QuestionGHQ28_7;
                            reportDto.QuestionGHQ28_8 = ans.QuestionGHQ28_8;
                            reportDto.QuestionGHQ28_9 = ans.QuestionGHQ28_9;
                            reportDto.QuestionGHQ28_10 = ans.QuestionGHQ28_10;
                            reportDto.QuestionGHQ28_11 = ans.QuestionGHQ28_11;
                            reportDto.QuestionGHQ28_12 = ans.QuestionGHQ28_12;
                            reportDto.QuestionGHQ28_13 = ans.QuestionGHQ28_13;
                            reportDto.QuestionGHQ28_14 = ans.QuestionGHQ28_14;
                            reportDto.QuestionGHQ28_15 = ans.QuestionGHQ28_15;
                            reportDto.QuestionGHQ28_16 = ans.QuestionGHQ28_16;
                            reportDto.QuestionGHQ28_17 = ans.QuestionGHQ28_17;
                            reportDto.QuestionGHQ28_18 = ans.QuestionGHQ28_18;
                            reportDto.QuestionGHQ28_19 = ans.QuestionGHQ28_19;
                            reportDto.QuestionGHQ28_20 = ans.QuestionGHQ28_20;
                            reportDto.QuestionGHQ28_21 = ans.QuestionGHQ28_21;
                            reportDto.QuestionGHQ28_22 = ans.QuestionGHQ28_22;
                            reportDto.QuestionGHQ28_23 = ans.QuestionGHQ28_23;
                            reportDto.QuestionGHQ28_24 = ans.QuestionGHQ28_24;
                            reportDto.QuestionGHQ28_25 = ans.QuestionGHQ28_25;
                            reportDto.QuestionGHQ28_26 = ans.QuestionGHQ28_26;
                            reportDto.QuestionGHQ28_27 = ans.QuestionGHQ28_27;
                            reportDto.QuestionGHQ28_28 = ans.QuestionGHQ28_28;
                            reportDto.AnswerGHQ28_1 = ans.AnswerGHQ28_1;
                            reportDto.AnswerGHQ28_2 = ans.AnswerGHQ28_2;
                            reportDto.AnswerGHQ28_3 = ans.AnswerGHQ28_3;
                            reportDto.AnswerGHQ28_4 = ans.AnswerGHQ28_4;
                            reportDto.AnswerGHQ28_5 = ans.AnswerGHQ28_5;
                            reportDto.AnswerGHQ28_6 = ans.AnswerGHQ28_6;
                            reportDto.AnswerGHQ28_7 = ans.AnswerGHQ28_7;
                            reportDto.AnswerGHQ28_8 = ans.AnswerGHQ28_8;
                            reportDto.AnswerGHQ28_9 = ans.AnswerGHQ28_9;
                            reportDto.AnswerGHQ28_10 = ans.AnswerGHQ28_10;
                            reportDto.AnswerGHQ28_11 = ans.AnswerGHQ28_11;
                            reportDto.AnswerGHQ28_12 = ans.AnswerGHQ28_12;
                            reportDto.AnswerGHQ28_13 = ans.AnswerGHQ28_13;
                            reportDto.AnswerGHQ28_14 = ans.AnswerGHQ28_14;
                            reportDto.AnswerGHQ28_15 = ans.AnswerGHQ28_15;
                            reportDto.AnswerGHQ28_16 = ans.AnswerGHQ28_16;
                            reportDto.AnswerGHQ28_17 = ans.AnswerGHQ28_17;
                            reportDto.AnswerGHQ28_18 = ans.AnswerGHQ28_18;
                            reportDto.AnswerGHQ28_19 = ans.AnswerGHQ28_19;
                            reportDto.AnswerGHQ28_20 = ans.AnswerGHQ28_20;
                            reportDto.AnswerGHQ28_21 = ans.AnswerGHQ28_21;
                            reportDto.AnswerGHQ28_22 = ans.AnswerGHQ28_22;
                            reportDto.AnswerGHQ28_23 = ans.AnswerGHQ28_23;
                            reportDto.AnswerGHQ28_24 = ans.AnswerGHQ28_24;
                            reportDto.AnswerGHQ28_25 = ans.AnswerGHQ28_25;
                            reportDto.AnswerGHQ28_26 = ans.AnswerGHQ28_26;
                            reportDto.AnswerGHQ28_27 = ans.AnswerGHQ28_27;
                            reportDto.AnswerGHQ28_28 = ans.AnswerGHQ28_28;
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

using HelpCheck_API.Constants;
using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.Patients;
using HelpCheck_API.Dtos.Physicals;
using HelpCheck_API.Models;
using HelpCheck_API.Repositories.Appointments;
using HelpCheck_API.Repositories.AppointmentSettings;
using HelpCheck_API.Repositories.OtherInterfaces;
using HelpCheck_API.Repositories.Patients;
using HelpCheck_API.Repositories.PhysicalExaminationMasters;
using HelpCheck_API.Repositories.Users;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Services.Patients
{
    public class PatientService : IPatientService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPatientRepository _patientRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IAppointmentSettingRepository _appointmentSettingRepository;
        private readonly IPhysicalRepository _physicalRepository;
        private readonly IOtherInterfaceRepository _otherInterfaceRepository;

        public PatientService(
            IUserRepository userRepository,
            IPatientRepository patientRepository,
            IAppointmentRepository appointmentRepository,
            IAppointmentSettingRepository appointmentSettingRepository,
            IPhysicalRepository physicalRepository,
            IOtherInterfaceRepository otherInterfaceRepository)
        {
            _userRepository = userRepository;
            _patientRepository = patientRepository;
            _appointmentRepository = appointmentRepository;
            _appointmentSettingRepository = appointmentSettingRepository;
            _physicalRepository = physicalRepository;
            _otherInterfaceRepository = otherInterfaceRepository;
        }

        public async Task<ResultResponse> CreatePhysicalPatientAsync(AddPhysicalDto addPhysicalDto)
        {
            var user = await _userRepository.GetUserByAccessTokenAsync(addPhysicalDto.AccessToken);
            string userFullName = user.TitleName + " " + user.FirstName + " " + user.LastName;

            try
            {
                foreach (var i in addPhysicalDto.PhysicalDetails)
                {
                    AnswerPhysical model = new AnswerPhysical();
                    model.MemberID = addPhysicalDto.MemberID;
                    model.PhysicalChoiceID = i.PhysicalChoiceID;
                    model.AnsphyKeyIn = i.Value;
                    model.PhysicalSetID = addPhysicalDto.PhysicalSetID;
                    model.CreateBy = userFullName;
                    await _patientRepository.CreatePhysicalPatientAsync(model);
                }

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = Constant.STATUS_SUCCESS
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

        public async Task<ResultResponse> UpdatePhysicalPatientAsync(EditPhysicalDto editPhysicalDto)
        {
            var user = await _userRepository.GetUserByAccessTokenAsync(editPhysicalDto.AccessToken);
            string userFullName = user.TitleName + " " + user.FirstName + " " + user.LastName;

            var physical = await _patientRepository.GetLastAnswerPhysicalByMemberIDAndPhysicalSetIDAsync(editPhysicalDto.MemberID, editPhysicalDto.PhysicalSetID);
            if (physical is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }

            try
            {
                physical.PhysicalChoiceID = editPhysicalDto.PhysicalChoiceID;
                physical.AnsphyKeyIn = editPhysicalDto.Value;
                physical.UpdateBy = userFullName;

                string status = await _patientRepository.UpdatePhysicalPatientAsync(physical);

                if (status != Constant.STATUS_SUCCESS)
                {
                    return new ResultResponse()
                    {
                        IsSuccess = false,
                        Data = status
                    };
                }

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = Constant.STATUS_SUCCESS
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

        public async Task<ResultResponse> GetAppointmentDateByMemberIDAsync(int memberId)
        {
            var appointments = await _appointmentRepository.GetAppointmentByMemberIDAsync(memberId);

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = appointments
            };
        }

        public async Task<ResultResponse> GetPhysicalByTokenAsync(string accessToken)
        {
            User user = await _userRepository.GetUserByAccessTokenAsync(accessToken);
            var physicals = await _physicalRepository.GetPhysicalAnswerListByMemberIDAsync(user.ID);

            var data = physicals.Select(s => new GetPhysicalExaminationMasterDto()
            {
                ID = s.ID,
                DescriptTh = s.DescriptTh,
                DescriptEn = s.DescriptEn,
                UnitTh = s.UnitTh,
                UnitEn = s.UnitEn,
                Image = s.Image,
                BgColor = s.BgColor,
                Value = s.Value
            });

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = data
            };
        }

        public async Task<ResultResponse> GetPhysicalByMemberIDAsync(int memberId)
        {
            var physicals = await _physicalRepository.GetPhysicalAnswerListByMemberIDAsync(memberId);

            var data = physicals.Select(s => new GetPhysicalExaminationMasterDto()
            {
                ID = s.ID,
                DescriptTh = s.DescriptTh,
                DescriptEn = s.DescriptEn,
                UnitTh = s.UnitTh,
                UnitEn = s.UnitEn,
                Image = s.Image,
                BgColor = s.BgColor,
                Value = s.Value
            });

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = data
            };
        }

        public async Task<ResultResponse> GetPatientAppointmentAsync(FilterPatientAppointmentDto filterPatientAppointmentDto)
        {
            var patientAppointments = await _patientRepository.GetPatientAppointmentAsync(filterPatientAppointmentDto);

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = patientAppointments
            };
        }

        public async Task<ResultResponse> GetAllPatientAppointmentAsync(FilterPatientDto filterDto)
        {
            var patientAppointments = await _patientRepository.GetAllPatientAppointmnetAsync(filterDto);
            
            return new ResultResponse()
            {
                IsSuccess = true,
                Data = patientAppointments
            };
        }

        public async Task<ResultResponse> GetXRayResultByIdCard(string idcard)
        {
            if (string.IsNullOrWhiteSpace(idcard))
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_INVALID_REQUEST_DATA
                };
            }

            try
            {
                //var cli = new RestClient("http://202.28.80.34:8080/ords/dev/HEALTH_CHECK/XRAY_CHECK/" + idcard.Trim().Replace("-", ""))
                //{
                //    Timeout = -1
                //};
                //var req = new RestRequest(Method.GET);
                //req.AddHeader("Content-Type", "application/json");
                //IRestResponse response = await cli.ExecuteAsync(req);
                //var data = JsonConvert.DeserializeObject<DataXRayResult>(response.Content);

                //return new ResultResponse()
                //{
                //    IsSuccess = true,
                //    Data = data
                //};

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = _otherInterfaceRepository.GetXRayResults()
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = ex.Data
                };
            }
        }

        public async Task<ResultResponse> GetBloodResultByHn(string hn)
        {
            if (string.IsNullOrWhiteSpace(hn))
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_INVALID_REQUEST_DATA
                };
            }

            try
            {
            //    var cli = new RestClient("http://202.28.80.34:8080/ords/dev/HEALTH_CHECK/LAB_CHECK/" + hn)
            //    {
            //        Timeout = -1
            //    };
            //    var req = new RestRequest(Method.GET);
            //    req.AddHeader("Content-Type", "application/json");
            //    IRestResponse response = await cli.ExecuteAsync(req);
            //    var data = JsonConvert.DeserializeObject<DataBloodResult>(response.Content);

            //return new ResultResponse()
            //{
            //    IsSuccess = true,
            //    Data = data
            //};

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = _otherInterfaceRepository.GetBloodResults()
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = ex.Data
                };
            }
        }

        public async Task<ResultResponse> GetLabSmearByHn(string hn)
        {
            if (string.IsNullOrWhiteSpace(hn))
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_INVALID_REQUEST_DATA
                };
            }

            try
            {
                //var cli = new RestClient("http://dev34.pmk.ac.th:8080/ords/dev/HEALTH_CHECK/LAB_SMEAR/" + hn)
                //{
                //    Timeout = -1
                //};
                //var req = new RestRequest(Method.GET);
                //req.AddHeader("Content-Type", "application/json");
                //IRestResponse response = await cli.ExecuteAsync(req);
                //var data = JsonConvert.DeserializeObject<GetLabSmearDto>(response.Content);

                //return new ResultResponse()
                //{
                //    IsSuccess = true,
                //    Data = data
                //};

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = _otherInterfaceRepository.GetLabSmearDetails()
                };
            }
            catch (Exception ex)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = ex.Data
                };
            }
        }
    }
}

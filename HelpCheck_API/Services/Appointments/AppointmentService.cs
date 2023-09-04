using HelpCheck_API.Constants;
using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.Appointments;
using HelpCheck_API.Dtos.Reports;
using HelpCheck_API.Models;
using HelpCheck_API.Repositories.Appointments;
using HelpCheck_API.Repositories.AppointmentSettings;
using HelpCheck_API.Repositories.MasterAgencies;
using HelpCheck_API.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Services.Appointments
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentSettingRepository _appointmentSettingRepository;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMasterAgencyRepository _masterAgencyRepository;

        public AppointmentService(IAppointmentSettingRepository appointmentSettingRepository, IAppointmentRepository appointmentRepository, IUserRepository userRepository, IMasterAgencyRepository masterAgencyRepository)
        {
            _appointmentSettingRepository = appointmentSettingRepository;
            _appointmentRepository = appointmentRepository;
            _userRepository = userRepository;
            _masterAgencyRepository = masterAgencyRepository;
        }

        public async Task<ResultResponse> CreateAppointmentSettingAsync(AddAppointmentSettingDto addDto)
        {
            var user = await _userRepository.GetUserByAccessTokenAsync(addDto.AccessToken);
            string userFullName = user.TitleName + " " + user.FirstName + " " + user.LastName;

            try
            {
                AppointmentSetting model = new AppointmentSetting()
                {
                    DoctorID = addDto.DoctorID,
                    AppointmentDate = addDto.AppointmentDate.Date,
                    AppointmentDateTimeStart = addDto.AppointmentTimeStart,
                    AppointmentDateTimeEnd = addDto.AppointmentTimeEnd,
                    MaximumPatient = addDto.MaximumPatient ?? 0,
                    CreateBy = userFullName
                };

                string status = await _appointmentSettingRepository.CreateAppointmentSettingAsync(model);

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

        public async Task<ResultResponse> GetAllAppointmentSettingAsync(FilterAppointmentDto filterDto)
        {
            var data = await _appointmentSettingRepository.GetAppointmentSettingsAsync();

            if (data != null)
            {
                if (filterDto.AppointmnentDate != null)
                {
                    data = data.Where(w => w.AppointmentDate.Date == filterDto.AppointmnentDate.Value.Date).ToList();
                }
            }

            var Appointments = new List<GetAppointmentSettingDto>();
            foreach (var s in data)
            {
                int countReserved = await _appointmentRepository.GetCountAppointmentAsync(s.ID);
                var item = new GetAppointmentSettingDto();
                item.ID = s.ID;
                item.DoctorID = s.DoctorID;
                item.DoctorFullName = "";
                item.AppointmentDate = s.AppointmentDate.Date;
                item.AppointmentDateTimeStart = s.AppointmentDateTimeStart;
                item.AppointmentDateTimeEnd = s.AppointmentDateTimeEnd;
                item.Booked = countReserved;
                item.MaximunBooked = s.MaximumPatient;
                Appointments.Add(item);
            }

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = Appointments
            };
        }
        public async Task<ResultResponse> GetAllAppointmentSettingMobileAsync(FilterAppointmentDto filterDto)
        {
            var data = await _appointmentSettingRepository.GetAppointmentSettingsAsync();
            var enddate = filterDto.AppointmnentDate.Value.AddMonths(2);

            if (data != null)
            {
                if (filterDto.AppointmnentDate != null)
                {
                    data = data.Where(w => w.AppointmentDate.Date >= filterDto.AppointmnentDate.Value.Date && w.AppointmentDate.Date <= enddate).OrderBy(d => d.AppointmentDate).ToList();
                }
            }

            var Appointments = new List<GetAppointmentSettingDto>();
            foreach (var s in data)
            {
                int countReserved = await _appointmentRepository.GetCountAppointmentAsync(s.ID);
                var item = new GetAppointmentSettingDto();
                item.ID = s.ID;
                item.DoctorID = s.DoctorID;
                item.DoctorFullName = "";
                item.AppointmentDate = s.AppointmentDate.Date;
                item.AppointmentDateTimeStart = s.AppointmentDateTimeStart;
                item.AppointmentDateTimeEnd = s.AppointmentDateTimeEnd;
                item.Booked = countReserved;
                item.MaximunBooked = s.MaximumPatient;
                Appointments.Add(item);
            }

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = Appointments
            };
        }
        public async Task<ResultResponse> GetAppointmentSettingByAccessTokenAsync(string accessToken)
        {
            User user = await _userRepository.GetUserByAccessTokenAsync(accessToken);
            var appointments = await _appointmentRepository.GetAppointmentByMemberIDAsync(user.ID);

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = appointments
            };
        }

        public async Task<ResultResponse> GetAppointmentSettingByIDAsync(int id)
        {
            var data = await _appointmentSettingRepository.GetAppointmentSettingByIDAsync(id);

            GetAppointmentSettingDto getDto = new GetAppointmentSettingDto();

            if (data != null)
            {
                int countReserved = await _appointmentRepository.GetCountAppointmentAsync(data.ID);
                getDto.ID = data.ID;
                getDto.DoctorID = data.DoctorID;
                getDto.AppointmentDate = data.AppointmentDate.Date;
                getDto.AppointmentDateTimeStart = data.AppointmentDateTimeStart;
                getDto.AppointmentDateTimeEnd = data.AppointmentDateTimeEnd;
                getDto.Booked = countReserved;
                getDto.MaximunBooked = data.MaximumPatient;
            }

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = getDto
            };
        }

        public async Task<ResultResponse> UpdateAppointmentSettingAsync(EditAppointmentSettingDto editDto)
        {
            var user = await _userRepository.GetUserByAccessTokenAsync(editDto.AccessToken);
            string userFullName = user.TitleName + " " + user.FirstName + " " + user.LastName;
            var data = await _appointmentSettingRepository.GetAppointmentSettingByIDAsync(editDto.ID);

            if (data is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }

            try
            {
                data.DoctorID = editDto.DoctorID;
                data.AppointmentDateTimeStart = editDto.AppointmentDateTimeStart;
                data.AppointmentDateTimeEnd = editDto.AppointmentDateTimeEnd;
                data.MaximumPatient = editDto.MaximumPatient ?? 0;
                data.UpdateBy = userFullName;
                string status = await _appointmentSettingRepository.UpdateAppointmentSettingAsync(data);

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

        public async Task<ResultResponse> DeleteAppointmentSettingAsync(int id)
        {
            var data = await _appointmentSettingRepository.GetAppointmentSettingByIDAsync(id);

            if (data is null)
            {
                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
                };
            }

            string status = await _appointmentSettingRepository.DeleteAppointmentSettingAsync(data);

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

        public async Task<ResultResponse> GetCountAppointmentSettingByIDAsync(int id)
        {
            var data = await _appointmentSettingRepository.GetAppointmentSettingByIDAsync(id);

            GetCountAppointmentDto getDto = new GetCountAppointmentDto();

            if (data != null)
            {
                int countBooked = await _appointmentRepository.GetCountAppointmentAsync(id);

                if (countBooked > data.MaximumPatient)
                {
                    return new ResultResponse()
                    {
                        IsSuccess = false,
                        Data = "Maximum Booking"
                    };
                }

                getDto.ID = data.ID;
                getDto.AppointmentDate = data.AppointmentDate.Date;
                getDto.Booked = countBooked;
                getDto.MaximumBooking = data.MaximumPatient;
            }

            return new ResultResponse()
            {
                IsSuccess = true,
                Data = getDto
            };
        }

        public async Task<ResultResponse> BookingAppointmentAsync(AddBookingAppointmentDto addDto)
        {
            var user = await _userRepository.GetUserByAccessTokenAsync(addDto.AccessToken);
            string userFullName = user.TitleName + " " + user.FirstName + " " + user.LastName;

            try
            {
                if (addDto.IsBooked)
                {
                    int maximumPatient = await _appointmentRepository.GetMaximumBookedAppointmentByIDAsync(addDto.AppointmentID);
                    int countBooked = await _appointmentRepository.GetCountBookedAppointmentAsync(addDto.AppointmentID);
                    if (countBooked >= maximumPatient)
                    {
                        return new ResultResponse()
                        {
                            IsSuccess = false,
                            Data = "Maximum Booking"
                        };
                    }

                    int resultQueueNo = 0;
                    int lastQueue = await _appointmentRepository.GetLastQueueAppointmentAsync(addDto.AppointmentID) + 1;

                    var app = await _appointmentRepository.GetAppointmentByIDAndNotBookAsync(addDto.AppointmentID);
                    app.QueueNo = lastQueue;
                    app.MemberID = user.ID;
                    app.AppointmentID = addDto.AppointmentID;
                    app.DoctorID = addDto.DoctorID;
                    app.UpdateBy = userFullName;
                    app.IsBooked = true;

                    resultQueueNo = await _appointmentRepository.UpdateAppointmentAsync(app);
                    if (resultQueueNo == 0)
                    {
                        return new ResultResponse()
                        {
                            IsSuccess = false,
                            Data = Constant.STATUS_ERROR
                        };
                    }

                    return new ResultResponse()
                    {
                        IsSuccess = true,
                        Data = resultQueueNo
                    };
                }
                else
                {
                    var data = await _appointmentSettingRepository.GetAppointmentSettingByIDAsync(addDto.AppointmentID);
                    int countBooked = await _appointmentRepository.GetCountAppointmentAsync(addDto.AppointmentID);
                    if (data == null)
                    {
                        return new ResultResponse()
                        {
                            IsSuccess = false,
                            Data = Constant.STATUS_ERROR
                        };
                    }

                    if (countBooked >= data.MaximumPatient)
                    {
                        return new ResultResponse()
                        {
                            IsSuccess = false,
                            Data = "Maximum Booking"
                        };
                    }

                    var appointReserve = await _appointmentRepository.GetReserveAppointmentByIDAsync(addDto.AppointmentID);
                    if (appointReserve != null)
                    {
                        appointReserve.IsReserve = true;
                        appointReserve.UpdateBy = userFullName;
                        await _appointmentRepository.UpdateAppointmentAsync(appointReserve);
                        return new ResultResponse()
                        {
                            IsSuccess = true,
                            Data = Constant.STATUS_SUCCESS
                        };
                    }

                    Appointment model = new Appointment()
                    {
                        MemberID = user.ID,
                        AppointmentID = addDto.AppointmentID,
                        DoctorID = addDto.DoctorID,
                        CreateBy = userFullName,
                        IsBooked = false,
                        IsReserve = true,
                        QueueNo = 0
                    };

                    string status = await _appointmentRepository.CreateAppointmentAsync(model);
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

        public async Task<ResultResponse> UnReserveAppointmentAsync(int id, string accessToken)
        {
            var user = await _userRepository.GetUserByAccessTokenAsync(accessToken);
            string userFullName = user.TitleName + " " + user.FirstName + " " + user.LastName;

            try
            {
                var appointReserve = await _appointmentRepository.GetAppointmentByIDAndNotBookAsync(id);
                if (appointReserve != null)
                {
                    if (appointReserve.MemberID == user.ID)
                    {
                        appointReserve.IsReserve = false;
                        appointReserve.UpdateBy = userFullName;

                        await _appointmentRepository.UpdateAppointmentAsync(appointReserve);
                    }

                    return new ResultResponse()
                    {
                        IsSuccess = true,
                        Data = Constant.STATUS_SUCCESS
                    };
                }

                return new ResultResponse()
                {
                    IsSuccess = false,
                    Data = Constant.STATUS_DATA_NOT_FOUND
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

        public async Task<ResultResponse> GetAppointmentByDateAsync(DateTime date)
        {
            try
            {
                List<GetAppointmentReportDto> list = new List<GetAppointmentReportDto>();

                var data = await _appointmentRepository.GetAppointmentsByDateAsync(date);
                if (data.Count > 0)
                {
                    var ct = new CultureInfo("en-Us");
                    foreach (var d in data)
                    {
                        int agencyId = 0;
                        try
                        {
                            agencyId = Convert.ToInt32(d.Agency);
                        }
                        catch (Exception)
                        {
                            agencyId = 0;
                        }

                        GetAppointmentReportDto item = new GetAppointmentReportDto()
                        {
                            MemberHn = d.MemberHn,
                            MemberIdCard = d.MemberIdCard,
                            MemberName = d.MemberName,
                            Date = d.AppointmentDate.ToString("yyyy-MM-dd", ct),
                            StartTime = d.AppointmentDateTimeStart.ToString("HH:mm", new CultureInfo("th-TH")),
                            EndTime = d.AppointmentDateTimeEnd.ToString("HH:mm", new CultureInfo("th-TH")),
                            TreatmentName = d.TreatmentName,
                            Agency = _masterAgencyRepository.GetMasterAgencyNameByID(agencyId)
                        };
                        list.Add(item);
                    }
                }

                return new ResultResponse()
                {
                    IsSuccess = true,
                    Data = list
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

        public async Task<ResultResponse> CancelBookedByAppointmentIDAsync(int id, string token)
        {
            try
            {
                var user = await _userRepository.GetUserByAccessTokenAsync(token);

                var app = await _appointmentRepository.GetAppointmentByAppointIDAndMemberIdAsync(id, user.ID);
                if (app is null || !app.AppointmentID.HasValue)
                {
                    return new ResultResponse()
                    {
                        IsSuccess = false,
                        Data = Constant.STATUS_DATA_NOT_FOUND + " from appointment."
                    };
                }

                var appointmentSetting = await _appointmentSettingRepository.GetAppointmentSettingByIDAsync(app.AppointmentID.Value);
                if (appointmentSetting is null)
                {
                    return new ResultResponse()
                    {
                        IsSuccess = false,
                        Data = Constant.STATUS_DATA_NOT_FOUND + " from appointment setting."
                    };
                }

                if ((appointmentSetting.AppointmentDate - DateTime.Now).TotalDays <= 3)
                {
                    return new ResultResponse()
                    {
                        IsSuccess = false,
                        Data = "Can not cancel appointment"
                    };
                }

                string status = await _appointmentRepository.DeleteAppointmentAsync(app);

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
    }
}
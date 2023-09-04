using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using HelpCheck_API.Constants;
using HelpCheck_API.Data;
using HelpCheck_API.Dtos;
using HelpCheck_API.Dtos.Patients;
using HelpCheck_API.Dtos.Physicals;
using HelpCheck_API.Dtos.Reports;
using HelpCheck_API.Models;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;

namespace HelpCheck_API.Repositories.Patients
{
    public class PatientRepository : IPatientRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IDbConnection _dbConnection;
        public PatientRepository(ApplicationDbContext context, IDbConnection dbConnection)
        {
            _context = context;
            _dbConnection = dbConnection;
        }

        public async Task<List<AnswerPhysical>> GetLastAnswerPhysicalByMemberIDAsync(int memberId)
        {
            return await _context.AnswerPhysicals.Where(w => w.MemberID == memberId)?.OrderByDescending(o => o.CreateDate).ToListAsync();
        }

        public async Task<AnswerPhysical> GetAnswerPhysicalByIDAsync(int ansphyId, int memberId)
        {
            return await _context.AnswerPhysicals.Where(w => w.AnsphyID == ansphyId && w.MemberID == memberId)?.FirstOrDefaultAsync();
        }

        public async Task<AnswerPhysical> GetLastAnswerPhysicalByMemberIDAndPhysicalSetIDAsync(int memberId, int phySetId)
        {
            return await _context.AnswerPhysicals.Where(w => w.MemberID == memberId && w.PhysicalSetID == phySetId)?.OrderByDescending(o => o.CreateDate).FirstOrDefaultAsync();
        }

        public async Task<string> CreatePhysicalPatientAsync(AnswerPhysical data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                data.CreateDate = DateTime.Now;
                await _context.AnswerPhysicals.AddAsync(data);
                await _context.SaveChangesAsync();
                await trans.CommitAsync();
                return Constant.STATUS_SUCCESS;
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return ex.Message;
            }
        }

        public async Task<string> UpdatePhysicalPatientAsync(AnswerPhysical data)
        {
            var trans = _context.Database.BeginTransaction();
            try
            {
                data.UpdateDate = DateTime.Now;

                _context.AnswerPhysicals.Update(data);
                await _context.SaveChangesAsync();
                await trans.CommitAsync();

                return Constant.STATUS_SUCCESS;
            }
            catch (Exception ex)
            {
                await trans.RollbackAsync();
                return ex.Message;
            }
        }

        public async Task<List<GetPatientAppointmentDto>> GetPatientAppointmentAsync(FilterPatientAppointmentDto filterPatientAppointmentDto)
        {
            var list = await (from u in _context.UserEntities
                              join wp in _context.MasterWorkPlaces on u.WorkPlaceID equals wp.ID into lwp
                              from wp in lwp.DefaultIfEmpty()
                              join jt in _context.MasterJobTypes on u.JobTypeID equals jt.ID into ljt
                              from jt in ljt.DefaultIfEmpty()
                              join a in _context.Appointments on u.ID equals a.MemberID
                              join ast in _context.AppointmentSettings on a.AppointmentID equals ast.ID
                              join d in _context.UserEntities on a.DoctorID equals d.ID into ld
                              from d in ld.DefaultIfEmpty()
                              where !string.IsNullOrWhiteSpace(u.FirstName) && u.FirstName.Contains(filterPatientAppointmentDto.search)
                                || !string.IsNullOrWhiteSpace(u.LastName) && u.LastName.Contains(filterPatientAppointmentDto.search)
                                || !string.IsNullOrWhiteSpace(u.IDCard) && u.IDCard.Contains(filterPatientAppointmentDto.search)
                                || !string.IsNullOrWhiteSpace(u.Hn) && u.Hn.Contains(filterPatientAppointmentDto.search)
                                && a.IsBooked && ast.AppointmentDate.Year == DateTime.Now.Year
                              select new GetPatientAppointmentDto
                              {
                                  ID = u.ID,
                                  TitleName = u.TitleName,
                                  FirstName = u.FirstName,
                                  LastName = u.LastName,
                                  IDCard = u.IDCard,
                                  PhoneNo = u.PhoneNo,
                                  Agency = u.Agency,
                                  WorkPlaceID = u.WorkPlaceID,
                                  WorkPlaceName = wp.Name,
                                  JobTypeID = u.JobTypeID,
                                  JobTypeName = jt.Name,
                                  DoctorID = a.DoctorID,
                                  DoctorName = a.DoctorID == 0 ? "" : d.TitleName + " " + d.FirstName + " " + d.LastName,
                                  Hn = u.Hn
                              }).Distinct().ToListAsync();
            //var ansPhys = await _context.AnswerPhysicals.ToListAsync();
            //list = list.Where(w1 => !ansPhys.Any(w2 => w2.MemberID == w1.ID)).ToList();
            return list;
        }

        public async Task<List<GetAllPatientAppointmentDto>> GetAllPatientAppointmnetAsync(FilterPatientDto filterDto)
        {
            var list = await (from a in _context.Appointments
                              join ast in _context.AppointmentSettings on a.AppointmentID equals ast.ID
                              join u in _context.UserEntities on a.MemberID equals u.ID
                              join wp in _context.MasterWorkPlaces on u.WorkPlaceID equals wp.ID into lwp
                              from wp in lwp.DefaultIfEmpty()
                              join jt in _context.MasterJobTypes on u.JobTypeID equals jt.ID into ljt
                              from jt in ljt.DefaultIfEmpty()
                              join d in _context.UserEntities on a.DoctorID equals d.ID into ld
                              from d in ld.DefaultIfEmpty()
                              join tm in _context.MasterTreatments on u.TreatmentID equals tm.ID into ltm
                              from tm in ltm.DefaultIfEmpty()
                              where ast.AppointmentDate.Year == DateTime.Now.Year
                              select new GetAllPatientAppointmentDto
                              {
                                  ID = u.ID,
                                  TitleName = u.TitleName,
                                  FirstName = u.FirstName,
                                  LastName = u.LastName,
                                  IDCard = u.IDCard,
                                  PhoneNo = u.PhoneNo,
                                  Agency = u.Agency,
                                  WorkPlaceID = u.WorkPlaceID,
                                  WorkPlaceName = wp.Name,
                                  JobTypeID = u.JobTypeID,
                                  JobTypeName = jt.Name,
                                  AppointmentID = a.AppointmentID,
                                  DoctorID = a.DoctorID,
                                  DoctorName = d.TitleName + " " + d.FirstName + " " + d.LastName,
                                  AppointmentDate = ast.AppointmentDate,
                                  Hn = u.Hn,
                                  BirthDate = u.BirthDate,
                                  Sex = u.Gender,
                                  TreatmentID = u.TreatmentID,
                                  TreatmentName = tm.Name
                              }).Distinct().AsNoTracking().ToListAsync();

            if (filterDto != null)
            {
                if (filterDto.AppointmentDate != null)
                {
                    list = list.Where(w => w.AppointmentDate.Date == filterDto.AppointmentDate.Value.Date).ToList();
                }

                if (!string.IsNullOrWhiteSpace(filterDto.FullName))
                {
                    list = list.Where(w => (w.TitleName + " " + w.FirstName + " " + w.LastName).Contains(filterDto.FullName)).ToList();
                }
            }

            return list.ToList();
        }

        public async Task<PageginationResultResponse<GetAllPatientAppointmentDto>> GetAllPatientAppointmnetAsync(PageRequestModel filter, CancellationToken cancellationToken)
        {
            PageginationResultResponse<GetAllPatientAppointmentDto> result = new();

            var startRow = (filter.PageNumber - 1) * filter.PageSize;

            IQueryable<GetAllPatientAppointmentDto> data = (from a in _context.Appointments
                                                            join ast in _context.AppointmentSettings on a.AppointmentID equals ast.ID
                                                            join u in _context.UserEntities on a.MemberID equals u.ID
                                                            join wp in _context.MasterWorkPlaces on u.WorkPlaceID equals wp.ID into lwp
                                                            from wp in lwp.DefaultIfEmpty()
                                                            join jt in _context.MasterJobTypes on u.JobTypeID equals jt.ID into ljt
                                                            from jt in ljt.DefaultIfEmpty()
                                                            join d in _context.UserEntities on a.DoctorID equals d.ID into ld
                                                            from d in ld.DefaultIfEmpty()
                                                            join tm in _context.MasterTreatments on u.TreatmentID equals tm.ID into ltm
                                                            from tm in ltm.DefaultIfEmpty()
                                                            where ast.AppointmentDate.Year == DateTime.Now.Year && a.IsBooked

                                                            select new GetAllPatientAppointmentDto
                                                            {
                                                                ID = u.ID,
                                                                TitleName = u.TitleName,
                                                                FirstName = u.FirstName,
                                                                LastName = u.LastName,
                                                                IDCard = u.IDCard,
                                                                PhoneNo = u.PhoneNo,
                                                                Agency = u.Agency,
                                                                WorkPlaceID = u.WorkPlaceID,
                                                                WorkPlaceName = wp.Name,
                                                                JobTypeID = u.JobTypeID,
                                                                JobTypeName = jt.Name,
                                                                AppointmentID = a.AppointmentID,
                                                                DoctorID = a.DoctorID,
                                                                DoctorName = d.TitleName + " " + d.FirstName + " " + d.LastName,
                                                                AppointmentDate = ast.AppointmentDate,
                                                                Hn = u.Hn,
                                                                BirthDate = u.BirthDate,
                                                                Sex = u.Gender,
                                                                TreatmentID = u.TreatmentID,
                                                                TreatmentName = tm.Name
                                                            }).Distinct().AsNoTracking();

            var totalItemsCountTask = data.CountAsync(cancellationToken);

            result.CurrentPage = filter.PageNumber;
            result.PageSize = filter.PageSize;
            result.TotalItems = await totalItemsCountTask;
            result.TotalPages = (int)Math.Ceiling(result.TotalItems / (double)result.PageSize);
            var resultData = data.OrderBy(o => o.ID).Skip(startRow).Take(filter.PageSize).ToList();
            result.Items = resultData.GroupBy(g => g.ID).Select(s => s.LastOrDefault()).ToList();
            return result;
        }

        public async Task<PageginationResultResponse<GetAllPatientAppointmentDto>> GetReportAllPatientWithLastAppointmentAsync(DateTime date, PageRequestModel filter, CancellationToken cancellationToken)
        {
            PageginationResultResponse<GetAllPatientAppointmentDto> response = new();

            var startRow = (filter.PageNumber - 1) * filter.PageSize;

            IQueryable<GetAllPatientAppointmentDto> data = (from a in _context.Appointments
                                                            join ast in _context.AppointmentSettings on a.AppointmentID equals ast.ID
                                                            join u in _context.UserEntities on a.MemberID equals u.ID
                                                            join wp in _context.MasterWorkPlaces on u.WorkPlaceID equals wp.ID into lwp
                                                            from wp in lwp.DefaultIfEmpty()
                                                            join jt in _context.MasterJobTypes on u.JobTypeID equals jt.ID into ljt
                                                            from jt in ljt.DefaultIfEmpty()
                                                            join d in _context.UserEntities on a.DoctorID equals d.ID into ld
                                                            from d in ld.DefaultIfEmpty()
                                                            join tm in _context.MasterTreatments on u.TreatmentID equals tm.ID into ltm
                                                            from tm in ltm.DefaultIfEmpty()
                                                            where ast.AppointmentDate == date && a.IsBooked
                                                            select new GetAllPatientAppointmentDto
                                                            {
                                                                ID = u.ID,
                                                                TitleName = u.TitleName,
                                                                FirstName = u.FirstName,
                                                                LastName = u.LastName,
                                                                IDCard = u.IDCard,
                                                                PhoneNo = u.PhoneNo,
                                                                Agency = u.Agency,
                                                                WorkPlaceID = u.WorkPlaceID,
                                                                WorkPlaceName = wp.Name,
                                                                JobTypeID = u.JobTypeID,
                                                                JobTypeName = jt.Name,
                                                                AppointmentID = a.AppointmentID,
                                                                DoctorID = a.DoctorID,
                                                                DoctorName = d.TitleName + " " + d.FirstName + " " + d.LastName,
                                                                AppointmentDate = ast.AppointmentDate,
                                                                Hn = u.Hn,
                                                                BirthDate = u.BirthDate,
                                                                Sex = u.Gender,
                                                                TreatmentID = u.TreatmentID,
                                                                TreatmentName = tm.Name
                                                            }).Distinct().AsNoTracking();

            var totalItemsCountTask = data.CountAsync(cancellationToken);

            response.CurrentPage = filter.PageNumber;
            response.PageSize = filter.PageSize;
            response.TotalItems = await totalItemsCountTask;
            response.TotalPages = (int)Math.Ceiling(response.TotalItems / (double)response.PageSize);
            var resultData = data.OrderBy(o => o.ID).Skip(startRow).Take(filter.PageSize).ToList();
            response.Items = resultData.GroupBy(g => g.ID).Select(s => s.LastOrDefault()).ToList();

            return response;
        }

        public async Task<List<GetReportDailyPsychiatristCheckDto>> GetPatientWithAnswerDetailAsync(int memberId)
        {
            int[] q = { 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51 };

            return await (from a in _context.AmedAnswerDetails
                          join c in _context.AmedChoiceMasters on a.ChoiceID equals c.ChoiceID
                          where a.MemberID == memberId && q.Contains(a.QuestionID) && a.CreatedDate.Year == DateTime.Now.Year
                          select new GetReportDailyPsychiatristCheckDto
                          {
                              Question2QOne = a.QuestionID == 41 && a.ChoiceID == 35,
                              Question2QTwo = a.QuestionID == 42 && a.ChoiceID == 35,
                              Question9QOne = a.QuestionID == 43 && c.Score.HasValue ? c.Score.Value : 0,
                              Question9QTwo = a.QuestionID == 44 && c.Score.HasValue ? c.Score.Value : 0,
                              Question9QThree = a.QuestionID == 45 && c.Score.HasValue ? c.Score.Value : 0,
                              Question9QFour = a.QuestionID == 46 && c.Score.HasValue ? c.Score.Value : 0,
                              Question9QFive = a.QuestionID == 47 && c.Score.HasValue ? c.Score.Value : 0,
                              Question9QSix = a.QuestionID == 48 && c.Score.HasValue ? c.Score.Value : 0,
                              Question9QSeven = a.QuestionID == 49 && c.Score.HasValue ? c.Score.Value : 0,
                              Question9QEight = a.QuestionID == 50 && c.Score.HasValue ? c.Score.Value : 0,
                              Question9QNine = a.QuestionID == 51 && c.Score.HasValue ? c.Score.Value : 0
                          }).AsQueryable().Distinct().ToListAsync();

        }

        private async Task<IEnumerable<GetPersonalOfWorkPlaceDto>> GetCountUserWithWorkPlaceAsync()
        {
            return await (from u in _context.UserEntities
                          join a in _context.MasterAgencies on u.Agency equals a.ID.ToString()
                          where u.IsActive && u.RoleID == 5
                          group u.ID by new { u.Agency, a.Name } into g
                          select new GetPersonalOfWorkPlaceDto
                          {
                              PersonalOfAgency = g.Count(),
                              AgencyID = g.Key.Agency,
                              AgencyName = g.Key.Name
                          }).ToListAsync();
        }

        private async Task<int> CountMemberOfDentistCheckWithAgencyAsync(string agencyId)
        {
            if (string.IsNullOrWhiteSpace(agencyId)) return 0;

            return await (from d in _context.DentistChecks
                          join u in _context.UserEntities on d.MemberID equals u.ID
                          where u.Agency == agencyId && d.CreateDate.HasValue && d.CreateDate.Value.Year == DateTime.Now.Year
                          group d.MemberID by d.MemberID into g
                          select g.Count()).CountAsync();
        }

        private async Task<int> CountMemberOfLevelCheckByLevelNumberAsync(int levelNum, string agencyId)
        {
            return await (from d in _context.DentistChecks
                          join u in _context.UserEntities on d.MemberID equals u.ID
                          where u.Agency == agencyId && d.Level.HasValue && d.Level.Value == levelNum &&
                            d.CreateDate.HasValue && d.CreateDate.Value.Year == DateTime.Now.Year
                          group d.MemberID by d.MemberID into g
                          select g.Count()).CountAsync();
        }
        private async Task<int> CountOralOfMemberByAgencyNumAsync(string agencyId, string oralCode)
        {
            return await (from d in _context.DentistChecks
                          join u in _context.UserEntities on d.MemberID equals u.ID
                          join doh in _context.DentistCheckOralHealths on d.ID equals doh.DentistCheckID
                          join moh in _context.MasterOralHealths on doh.OralID equals moh.ID
                          where u.Agency == agencyId && moh.Code == oralCode && d.CreateDate.HasValue && d.CreateDate.Value.Year == DateTime.Now.Year
                          group d.MemberID by d.MemberID into g
                          select g.Count()).CountAsync();
        }
        public async Task<List<GetReportDentistCheckDto>> GetReportAllPatientWithDentistCheckAsync()
        {
            var resultUser = await GetCountUserWithWorkPlaceAsync();
            List<GetReportDentistCheckDto> list = new List<GetReportDentistCheckDto>();
            int i = 0;
            foreach (var item in resultUser.ToList())
            {
                GetReportDentistCheckDto dto = new GetReportDentistCheckDto();
                dto.No = ++i;
                dto.Agency = item.AgencyName;
                dto.PersonalOfAgency = item.PersonalOfAgency;
                dto.OralCheck = await CountMemberOfDentistCheckWithAgencyAsync(item.AgencyID);
                dto.Percent = Math.Round(Convert.ToDecimal(dto.OralCheck) / Convert.ToDecimal(dto.PersonalOfAgency) * 100, 2);
                dto.Level1 = await CountMemberOfLevelCheckByLevelNumberAsync(1, item.AgencyID);
                dto.Level2 = await CountMemberOfLevelCheckByLevelNumberAsync(2, item.AgencyID);
                dto.Level3 = await CountMemberOfLevelCheckByLevelNumberAsync(3, item.AgencyID);
                dto.Level4 = await CountMemberOfLevelCheckByLevelNumberAsync(4, item.AgencyID);
                dto.A = await CountOralOfMemberByAgencyNumAsync(item.AgencyID, "A");
                dto.B = await CountOralOfMemberByAgencyNumAsync(item.AgencyID, "B");
                dto.C = await CountOralOfMemberByAgencyNumAsync(item.AgencyID, "C");
                dto.D = await CountOralOfMemberByAgencyNumAsync(item.AgencyID, "D");
                dto.E = await CountOralOfMemberByAgencyNumAsync(item.AgencyID, "E");
                dto.F = await CountOralOfMemberByAgencyNumAsync(item.AgencyID, "F");
                dto.G = await CountOralOfMemberByAgencyNumAsync(item.AgencyID, "G");
                dto.H = await CountOralOfMemberByAgencyNumAsync(item.AgencyID, "H");
                dto.I = await CountOralOfMemberByAgencyNumAsync(item.AgencyID, "I");
                dto.J = await CountOralOfMemberByAgencyNumAsync(item.AgencyID, "J");
                dto.K = await CountOralOfMemberByAgencyNumAsync(item.AgencyID, "K");
                dto.L = await CountOralOfMemberByAgencyNumAsync(item.AgencyID, "L");
                dto.M = await CountOralOfMemberByAgencyNumAsync(item.AgencyID, "M");
                dto.N = await CountOralOfMemberByAgencyNumAsync(item.AgencyID, "N");

                list.Add(dto);
            }

            return list;
        }

        public async Task<int> CountPhysicalWithPatientAsync(DateTime date)
        {
            return await _context.AnswerPhysicals.Where(w => w.CreateDate.HasValue && w.CreateDate.Value.Date == date).Select(s => s.MemberID).GroupBy(g => g.Value).CountAsync();
        }

        public async Task<PageginationResultResponse<GetAllPatientAppointmentDto>> GetAllPatientAppointmnetPaginationAsync(FilterPatientDto filterDto, PageRequestModel pageModel)
        {
            PageginationResultResponse<GetAllPatientAppointmentDto> result = new();

            var startRow = (pageModel.PageNumber - 1) * pageModel.PageSize;

            string date = filterDto.AppointmentDate.Value.Date.ToString("yyyy-MM-dd");
            IEnumerable<GetAllPatientAppointmentDto> data = _dbConnection.Query<GetAllPatientAppointmentDto>("Report_Daily_HealthCheck", 
                new { date }, 
                commandType: CommandType.StoredProcedure);

            result.CurrentPage = pageModel.PageNumber;
            result.PageSize = pageModel.PageSize;
            result.TotalItems = data.Count();
            result.TotalPages = (int)Math.Ceiling(result.TotalItems / (double)result.PageSize);
            var resultData = data.OrderBy(o => o.ID).Skip(startRow).Take(pageModel.PageSize).ToList();
            result.Items = resultData.GroupBy(g => g.ID).Select(s => s.LastOrDefault()).ToList();

            return result;
        }
    }
}
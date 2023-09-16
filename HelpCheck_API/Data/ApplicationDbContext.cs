using HelpCheck_API.Dtos.Patients;
using HelpCheck_API.Dtos.Reports;
using HelpCheck_API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HelpCheck_API.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("user");
            builder.Entity<MasterAgency>().ToTable("master_agency");
            builder.Entity<MasterJobType>().ToTable("master_job_type");
            builder.Entity<MasterWorkPlace>().ToTable("master_work_place");
            builder.Entity<MasterTitle>().ToTable("master_title");
            builder.Entity<AmedAnswerHeader>().ToTable("amed_answer_header");
            builder.Entity<AmedAnswerDetail>().ToTable("amed_answer_detail");
            builder.Entity<AmedQuestionMaster>().ToTable("amed_question_master");
            builder.Entity<AmedChoiceMaster>().ToTable("amed_choice_master");
            builder.Entity<Module>().ToTable("module");
            builder.Entity<Role>().ToTable("role");
            builder.Entity<UserRolePermission>().ToTable("user_role_permission");
            builder.Entity<PhysicalSet>().ToTable("physical_set");
            builder.Entity<PhysicalChoice>().ToTable("physical_choice");
            builder.Entity<PhysicalExaminationMaster>().ToTable("physical_examination_master");
            builder.Entity<AnswerPhysical>().ToTable("answer_physical");
            builder.Entity<Appointment>().ToTable("appointment");
            builder.Entity<AppointmentSetting>().ToTable("appointment_setting");
            builder.Entity<DentistCheck>().ToTable("dentist_check");
            builder.Entity<DoctorCheck>().ToTable("doctor_check");
            builder.Entity<MasterOralHealth>().ToTable("master_oral_health");
            builder.Entity<DentistCheckOralHealth>().ToTable("dentist_check_oral_health").HasKey(t => new { t.DentistCheckID, t.OralID });
            builder.Entity<ViewAnswerPhysical>().ToView("v_answer_physical");
            builder.Entity<PsychiatristCheck>().ToTable("psychiatrist_check");
            builder.Entity<MasterTreatment>().ToTable("master_treatment");
            builder.Entity<ViewReportPsychiatristCheck>().ToView("v_answer_psychiatrist_check");
            builder.Entity<CheckResultDetailFromPMKDto>().ToView("check_result");
            builder.Entity<GetXRayResultDto>().ToView("xray_result");
            builder.Entity<GetBloodResultDto>().ToView("blood_result");
            builder.Entity<GetLabSmearDetailDto>().ToView("labsmear_detail");
            builder.Entity<CheckResultToDayDetailFromPMKDto>().ToView("result_today");
        }

        public DbSet<User> UserEntities { get; set; }
        public DbSet<MasterAgency> MasterAgencies { get; set; }
        public DbSet<MasterJobType> MasterJobTypes { get; set; }
        public DbSet<MasterWorkPlace> MasterWorkPlaces { get; set; }
        public DbSet<MasterTitle> MasterTitles { get; set; }
        public DbSet<AmedAnswerHeader> AmedAnswerHeaders { get; set; }
        public DbSet<AmedAnswerDetail> AmedAnswerDetails { get; set; }
        public DbSet<AmedQuestionMaster> AmedQuestionMasters { get; set; }
        public DbSet<AmedChoiceMaster> AmedChoiceMasters { get; set; }
        public DbSet<Module> Modules { get; set; }
        public DbSet<Role> RolePermissions { get; set; }
        public DbSet<UserRolePermission> UserRolePermissions { get; set; }
        public DbSet<PhysicalSet> PhysicalSets { get; set; }
        public DbSet<PhysicalChoice> PhysicalChoices { get; set; }
        public DbSet<PhysicalExaminationMaster> PhysicalExaminationMasters { get; set; }
        public DbSet<AnswerPhysical> AnswerPhysicals { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentSetting> AppointmentSettings { get; set; }
        public DbSet<DentistCheck> DentistChecks { get; set; }
        public DbSet<DoctorCheck> DoctorChecks { get; set; }
        public DbSet<MasterOralHealth> MasterOralHealths { get; set; }
        public DbSet<DentistCheckOralHealth> DentistCheckOralHealths { get; set; }
        public DbSet<ViewAnswerPhysical> ViewAnswerPhysicals { get; set; }
        public DbSet<PsychiatristCheck> PsychiatristChecks { get; set; }
        public DbSet<MasterTreatment> MasterTreatments { get; set; }
        public DbSet<ViewReportPsychiatristCheck> ViewReportPsychiatristChecks { get; set; }
        public DbSet<CheckResultDetailFromPMKDto> DetailResultFromHospitals { get; set; }
        public DbSet<GetXRayResultDto> XRayResults { get; set; }
        public DbSet<GetBloodResultDto> BloodResults { get; set; }
        public DbSet<GetLabSmearDetailDto> LabSmearDetails { get; set; }
        public DbSet<CheckResultToDayDetailFromPMKDto> ResultToDayDetails { get; set;}
    }
}

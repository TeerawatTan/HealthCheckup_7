using HelpCheck_API.Data;
using HelpCheck_API.Repositories.AmedAnswerDetails;
using HelpCheck_API.Repositories.AmedAnswerHeaders;
using HelpCheck_API.Repositories.AmedChoiceMasters;
using HelpCheck_API.Repositories.AmedQuestionMasters;
using HelpCheck_API.Repositories.Appointments;
using HelpCheck_API.Repositories.AppointmentSettings;
using HelpCheck_API.Repositories.DentistChecks;
using HelpCheck_API.Repositories.DoctorChecks;
using HelpCheck_API.Repositories.MasterAgencies;
using HelpCheck_API.Repositories.MasterJobTypes;
using HelpCheck_API.Repositories.MasterOralHealths;
using HelpCheck_API.Repositories.MasterTreatments;
using HelpCheck_API.Repositories.MasterWorkPlaces;
using HelpCheck_API.Repositories.Modules;
using HelpCheck_API.Repositories.OtherInterfaceHospitals;
using HelpCheck_API.Repositories.OtherInterfaces;
using HelpCheck_API.Repositories.Patients;
using HelpCheck_API.Repositories.PhysicalExaminationMasters;
using HelpCheck_API.Repositories.PsychiatristChecks;
using HelpCheck_API.Repositories.QuestionAndChoices;
using HelpCheck_API.Repositories.Roles;
using HelpCheck_API.Repositories.UserRolePermissions;
using HelpCheck_API.Repositories.Users;
using HelpCheck_API.Services.AmedChoiceMasters;
using HelpCheck_API.Services.AmedQuestionMasters;
using HelpCheck_API.Services.Appointments;
using HelpCheck_API.Services.Attachments;
using HelpCheck_API.Services.Authentications;
using HelpCheck_API.Services.Dentists;
using HelpCheck_API.Services.Doctors;
using HelpCheck_API.Services.MasterAgencies;
using HelpCheck_API.Services.MasterJobTypes;
using HelpCheck_API.Services.MasterOralHealths;
using HelpCheck_API.Services.MasterTreatments;
using HelpCheck_API.Services.MasterWorkPlaces;
using HelpCheck_API.Services.Patients;
using HelpCheck_API.Services.Physicals;
using HelpCheck_API.Services.PsychiatristChecks;
using HelpCheck_API.Services.QuestionAndChoices;
using HelpCheck_API.Services.Reports;
using HelpCheck_API.Services.Roles;
using HelpCheck_API.Services.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MySqlConnector;
using System;
using System.Data;
using System.Text;

namespace HelpCheck_API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            //Allow Cors
            services.AddCors(o => o.AddPolicy("CorsAPI", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HelpCheck_API", Version = "v1" });
                c.TagActionsBy(api =>
                {
                    if (api.GroupName != null)
                    {
                        return new[] { api.GroupName };
                    }

                    var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
                    if (controllerActionDescriptor != null)
                    {
                        return new[] { controllerActionDescriptor.ControllerName };
                    }

                    throw new InvalidOperationException("Unable to determine tag for endpoint.");
                });
                c.DocInclusionPredicate((name, api) => true);
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                c.CustomSchemaIds(a => a.FullName);
            });

            try
            {
                string mySqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");
                services.AddDbContextPool<ApplicationDbContext>(options => options.UseMySql(mySqlConnectionStr, 
                    ServerVersion.AutoDetect(mySqlConnectionStr)));

                services.AddTransient<IDbConnection>(sp => new MySqlConnection(mySqlConnectionStr));
            }
            catch (Exception ex)
            {
                throw new ArgumentException(ex.Message);
            }

            #region Scoped Service
            // Add Scoped
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IMasterAgencyService, MasterAgencyService>();
            services.AddScoped<IMasterJobTypeService, MasterJobTypeService>();
            services.AddScoped<IMasterWorkPlaceService, MasterWorkPlaceService>();
            services.AddScoped<IAmedQuestionMasterService, AmedQuestionMasterService>();
            services.AddScoped<IQuestionAndChoiceService, QuestionAndChoiceService>();
            services.AddScoped<IPatientService, PatientService>();
            services.AddScoped<IPhysicalService, PhysicalService>();
            services.AddScoped<IDentistService, DentistService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IMasterOralHealthService, MasterOralHealthService>();
            services.AddScoped<IReportService, ReportService>();
            services.AddScoped<IPsychiatristCheckService, PsychiatristCheckService>();
            services.AddScoped<IMasterTreatmentService, MasterTreatmentService>();
            services.AddScoped<IAttachmentService, AttachmentService>();
            services.AddScoped<IAmedChoiceMasterService, AmedChoiceMasterService>();

            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IMasterAgencyRepository, MasterAgencyRepository>();
            services.AddScoped<IMasterWorkPlaceRepository, MasterWorkPlaceRepository>();
            services.AddScoped<IMasterJobTypeRepository, MasterJobTypeRepository>();
            services.AddScoped<IAmedAnswerDetailRepository, AmedAnswerDetailRepository>();
            services.AddScoped<IAmedAnswerHeaderRepository, AmedAnswerHeaderRepository>();
            services.AddScoped<IAmedChoiceMasterRepository, AmedChoiceMasterRepository>();
            services.AddScoped<IAmedQuestionMasterRepository, AmedQuestionMasterRepository>();
            services.AddScoped<IModuleRepository, ModuleRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUserRolePermissionRepository, UserRolePermissionRepository>();
            services.AddScoped<IQuestionAndChoiceRepository, QuestionAndChoiceRepository>();
            services.AddScoped<IPatientRepository, PatientRepository>();
            services.AddScoped<IPhysicalRepository, PhysicalRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IDentistCheckRepository, DentistCheckRepository>();
            services.AddScoped<IDoctorCheckRepository, DoctorCheckRepository>();
            services.AddScoped<IAppointmentSettingRepository, AppointmentSettingRepository>();
            services.AddScoped<IMasterOralHealthRepository, MasterOralHealthRepository>();
            services.AddScoped<IPsychiatristCheckRepository, PsychiatristCheckRepository>();
            services.AddScoped<IMasterTreatmentRepository, MasterTreatmentRepository>();
            services.AddScoped<IOtherInterfaceRepository, OtherInterfaceRepository>();
            services.AddScoped<IOtherInterfaceHospital, OtherInterfaceHospital>();


            #endregion

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

                })
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = Configuration["JwtIssuer"],
                        ValidAudience = Configuration["JwtAudience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JwtKey"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddMemoryCache();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "HelpCheck_API v1");
                //c.RoutePrefix = string.Empty;
            });

            app.Use((context, next) =>
            {
                if (context.Request.Headers["x-forwarded-proto"] == "https")
                {
                    context.Request.Scheme = "https";
                }
                return next();
            });
            

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseCors("CorsAPI");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
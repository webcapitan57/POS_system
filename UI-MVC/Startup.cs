using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using POC.BL.Domain.delivery;
using POC.BL.Domain.user;
using POC.BL.logic;
using POC.BL.logic.InterFaces;
using POC.DAL;
using POC.DAL.EF.SC.DAL.EF;
using POC.DAL.repo;
using POC.DAL.repo.InterFaces;
using StackExchange.Redis;
using UI.MVC.Services;
using UI_MVC.Gcloud;
using UI_MVC.Services;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using POC.DAL.repo.InterFaces;
using POC.Sockets;
using WebSocketOptions = Microsoft.AspNetCore.Builder.WebSocketOptions;

namespace UI_MVC
{
    public class Startup
    {
        private IHostingEnvironment currentEnvironment { get; set; }

        public Startup(IHostingEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
            currentEnvironment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );


            //SignalR
            services.AddSignalR();

            services.AddMvc()
                .AddRazorPagesOptions(options =>
                {
                    options.Conventions.AuthorizeAreaFolder("Identity", "/Account/Manage");
                    options.Conventions.AuthorizeAreaPage("Identity", "/Account/Logout");
                });


            string envName = currentEnvironment.EnvironmentName;


            if (envName == "Staging")
            {
                services.AddDbContext<PhotoAppDbContext>(options => options.UseMySql(
                    Configuration.GetConnectionString("Production")));
            }
            else if (envName == "Production")
            {
                services.AddDbContext<PhotoAppDbContext>(options => options.UseMySql(
                    Configuration.GetConnectionString("Production")));

                //Store keys in redis
                var redis = ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis"));
                services.AddDataProtection().PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");
                services.AddOptions();
            }
            else
            {
                services.AddDbContext<PhotoAppDbContext>(options =>
                    options.UseSqlite(Configuration.GetConnectionString("Development")));
            }

            services.Configure<GcloudOptions>(options =>
                Configuration.GetSection("GoogleCloud").Bind(options));

            services.AddControllers();

            #region ScopedRepositories

            services.AddScoped<IAdminRepository, AdminRepository>();
            services.AddScoped<IDeliveryRepository, DeliveryRepository>();
            services.AddScoped<IGroupRepository, GroupRepository>();
            services.AddScoped<IGroupTaskRepository, GroupTaskRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IProfileQuestionRepository, ProfileQuestionRepository>();
            services.AddScoped<IProfileRepository, ProfileRepository>();
            services.AddScoped<ISetupRepository, SetupRepository>();
            services.AddScoped<IStudentRepository, StudentRepository>();
            services.AddScoped<ITaskRepository, TaskRepository>();
            services.AddScoped<ITeacherRepository, TeacherRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IFilterProfileRepository, FilterProfileRepository>();
            services.AddScoped<ILocationRepository, LocationRepository>();

            #endregion


            #region Scoped Services

            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IDeliveryService, DeliveryService>();
            services.AddScoped<IGroupService, GroupService>();
            services.AddScoped<ISetupService, SetupService>();
            services.AddScoped<IStudentService, StudentService>();
            services.AddScoped<ITaskService, TaskService>();
            services.AddScoped<ITeacherService, TeacherService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<GcloudStorage>();

            #endregion

            services.AddIdentity<UserAccount, IdentityRole>(
                    options => options.SignIn.RequireConfirmedAccount = true)
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<PhotoAppDbContext>();
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection =
                        Configuration.GetSection("Authentication:Google");

                    options.ClientId = googleAuthNSection["ClientId"];
                    options.ClientSecret = googleAuthNSection["ClientSecret"];
                })
                .AddFacebook(options =>
                {
                    IConfigurationSection facebookAuthNSection =
                        Configuration.GetSection("Authentication:Facebook");

                    options.ClientId = facebookAuthNSection["ClientId"];
                    options.ClientSecret = facebookAuthNSection["ClientSecret"];
                })
                .AddTwitter(options =>
                {
                    IConfigurationSection twitterAuthNSection =
                        Configuration.GetSection("Authentication:Twitter");

                    options.ConsumerKey = twitterAuthNSection["ConsumerKey"];
                    options.ConsumerSecret = twitterAuthNSection["ConsumerSecret"];
                    options.RetrieveUserDetails = true;
                });


            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                    policy.RequireRole("Admin"));
                options.AddPolicy("TeacherOrHigher", policy =>
                    policy.RequireRole("Teacher", "Admin"));
                options.AddPolicy("GuestTeacherOrHigher", policy =>
                    policy.RequireRole("GuestTeacher", "Teacher", "Admin"));
            });

            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<AuthMessageSenderOptions>(options =>
                Configuration.GetSection("AuthMessageSenderOptions").Bind(options));


            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(10);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+#";
                options.User.RequireUniqueEmail = false;
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedProto
            });
            var webSocketOptions = new WebSocketOptions()
            {
                KeepAliveInterval = TimeSpan.FromSeconds(120),
            };
            if (env.IsDevelopment())
            {
                webSocketOptions.AllowedOrigins.Add("http://localhost:5001");
            }else
            {
                webSocketOptions.AllowedOrigins.Add("https://driesdehouwer.xyz");
                webSocketOptions.AllowedOrigins.Add("https://www.driesdehouwer.xyz");
                webSocketOptions.AllowedOrigins.Add("http://www.driesdehouwer.xyz");
                webSocketOptions.AllowedOrigins.Add("https://driesdehouwer.xyz");
            }

            app.UseWebSockets(webSocketOptions);

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<ModerateHub>("/moderateDeliveries", options =>
                {
                    options.Transports =
                        HttpTransportType.WebSockets |
                        HttpTransportType.LongPolling;
                });
                endpoints.MapRazorPages();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Group}/{action=Join}");
            });
        }
    }
}
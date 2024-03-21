using p2p.DataAdaptor.Contract;
using p2p.DataAdaptor.Imp;
using p2p.Logic.Contract;
using p2p.Logic.Imp;
using NLog.Web;
using NLog;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.Extensions.Options;
using p2p;
using NETCore.MailKit.Core;
using p2p.UtilityServices;
using Microsoft.ML;
using static p2p.Common.Models.Space;


var logger = NLog.Web.NLogBuilder.ConfigureNLog("nlog.config").GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);
    logger.Debug("init main");


    //Nlog setup of dependency injection
    builder.Host.ConfigureLogging(logging =>
    {
        logging.ClearProviders();
        logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    });
    builder.Host.UseNLog();

    // Add services to the container.
    builder.Services.AddControllers();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //Cors
    var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
    builder.Services.AddCors(options =>
    {
        options.AddPolicy(MyAllowSpecificOrigins,
                              policy =>
                              {
                                  policy.WithOrigins("http://localhost:4200")
                                                      .AllowAnyHeader()
                                                      .AllowAnyMethod();
                              });
    });


    var key = builder.Configuration["ConnectionStrings:key"];

    //JWT Authentication
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            // IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("veryverysceret.....veryverysceret.....veryverysceret.....veryverysceret.....veryverysceret.....veryverysceret.....veryverysceret.....veryverysceret.....")),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),


            ValidateAudience = false,
            ValidateIssuer = false,
            ClockSkew = TimeSpan.Zero
        };
    });



    builder.Services.AddSingleton(typeof(string), "server=ANKIT; database=p2p; trusted_connection=true; Encrypt=False;");
    builder.Services.AddScoped<ICustomerManager, CustomerManager>();
    builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
    builder.Services.AddScoped<IUserManager, UserManager>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddScoped<IAdminManager, AdminManager>();
    builder.Services.AddScoped<IAdminRepository, AdminRepository>();
    builder.Services.AddScoped<IOwnerManager, OwnerManager>();
    builder.Services.AddScoped<IOwnerRepository, OwnerRepository>();
    builder.Services.AddHostedService<ScheduleApiService>();
    builder.Services.AddScoped<IReviewManager, ReviewManager>();
    builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
    builder.Services.AddScoped<IEmailServices, EmailServices>();
    builder.Services.AddScoped<ParkingPredictionService>();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseCors(MyAllowSpecificOrigins);

    app.UseAuthentication();

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch(Exception ex)
{
    logger.Error(ex);
    throw;
}
finally
{
    LogManager.Shutdown();
}

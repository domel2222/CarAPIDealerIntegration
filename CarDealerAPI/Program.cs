using CarDealerAPI;
using CarDealerAPI.Authentication;
using CarDealerAPI.Authorization;
using CarDealerAPI.Contexts;
using CarDealerAPI.DTOS;
using CarDealerAPI.Extensions;
using CarDealerAPI.Extensions.Validators;
using CarDealerAPI.Middlewere;
using CarDealerAPI.Models;
using CarDealerAPI.Services;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NLog.Web;
using System.Reflection;
using System.Text;

//[assembly: InternalsVisibleTo("CarDealerAPI.IntegrationTests")]

var builder = WebApplication.CreateBuilder(args);

//NLog config
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(LogLevel.Trace);
builder.Host.UseNLog();

//config builder.Services
var authSettings = new AuthenticationSettings();

builder.Configuration.GetSection("AuthenticationRysiek").Bind(authSettings);
builder.Services.AddSingleton(authSettings);
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = "Bearer";
    option.DefaultChallengeScheme = "Bearer";
    option.DefaultScheme = "Bearer";
}).AddJwtBearer(configureOptions =>
{
    configureOptions.RequireHttpsMetadata = false;
    configureOptions.SaveToken = true;
    configureOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = authSettings.JwtIssuer,
        ValidAudience = authSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.JwtKey)),
    };
});

builder.Services.AddAuthorization(policy =>
{
    policy.AddPolicy("HasNation", b => b.RequireClaim("Nationality", "Poland"));
    policy.AddPolicy("ColorEyes", b => b.RequireClaim("ColorEye", "blue", "green", "grey"));
    policy.AddPolicy("OnlyForEagles", b => b.AddRequirements(new CheckAge(int.Parse(builder.Configuration["MyVariables:OnlyForEagles"]))));
    //policy.AddPolicy("OnlyForEagles", b => b.AddRequirements(new CheckAge(18)));
    //policy.AddPolicy("DealerMinimum", b => b.AddRequirements(new MultiDealerRequiment(2)));
    policy.AddPolicy("DealerMinimum", b => b.AddRequirements(new MultiDealerRequiment(int.Parse(builder.Configuration["MyVariables:MultiDealer"]))));
});

builder.Services.AddControllers().AddFluentValidation();
//builder.Services.AddDbContext<DealerDbContext>();
builder.Services.AddScoped<DealerSeeder>();
builder.Services.AddScoped<IAuthorizationHandler, CheckAgeHandler>();
builder.Services.AddScoped<IAuthorizationHandler, ResouceOperationRequirementHandler>();
builder.Services.AddScoped<IAuthorizationHandler, MultiDealerRequimentHandler>();
//builder.Services.AddAutoMapper(typeof(DealerProfile).GetTypeInfo().Assembly);
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddScoped<IDealerService, DealerService>();
builder.Services.AddScoped<ICarService, CarService>();
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IUserContextService, UserContextService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<RequestTimeMiddle>();
builder.Services.AddScoped<IValidator<UserCreateDTO>, RegisterDtoValidator>();
builder.Services.AddScoped<IValidator<DealerQuerySearch>, DealerQuerySearchValidator>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "CarDEaler", Version = "v1" });
});

builder.Services.AddCors(option =>
{
    option.AddPolicy("Dealer", policybuilder =>
        policybuilder.AllowAnyMethod()
            .AllowAnyHeader()
            .WithOrigins(builder.Configuration["AllowClient"]));
});

builder.Services.AddDbContext<DealerDbContext>(
    option =>
        option.UseSqlServer(builder.Configuration.GetConnectionString("DealersCar"))
    );

var app = builder.Build();
//configure

var scope = app.Services.CreateScope();
var seeder = scope.ServiceProvider.GetRequiredService<DealerSeeder>();

app.UseResponseCaching();
app.UseStaticFiles();
app.UseCors("Dealer");
seeder.Seed();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestTimeMiddle>();
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "CarDealerAPI");
});
app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

public partial class Program
{ }
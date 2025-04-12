using AppAngular;
using AppAngular.Authenticacion;
using AppAngular.Data.Repository;
using AppAngular.Domain.Interfaces;
using AppAngular.Domain.Models;
using AppAngular.Repository;
using AppAngular.Service.Servicios;
using AppAngular.Servicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

builder.Services.Configure<AuthConfiguration>(builder.Configuration.GetSection("AuthConfiguration"));

// Configurar DbContext
builder.Services.AddDbContext<AplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configurar Identity para usar AspNetUsers
builder.Services.AddIdentity<AspNetUsers, IdentityRole>()
    .AddEntityFrameworkStores<AplicationDbContext>()
.AddDefaultTokenProviders();



// Registros Repository
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IPublicacionRepository, PublicacionRepository>();

// Registros Servicios
builder.Services.AddScoped<IAuthenticacionService, AuthenticacionService>();
builder.Services.AddScoped<IAspNetUserService, AspNetUserService>();
builder.Services.AddScoped<IEnviarEmailService, EnviarEmailService>();
builder.Services.AddScoped<IPublicacionService, PublicacionService>();

// clases extensiones son distintas a los servicios
var authConfigSection = builder.Configuration.GetSection("AuthConfiguration");
builder.Services.Configure<AuthConfiguration>(authConfigSection);
builder.Services.AddSingleton<AuthConfiguration>(
    authConfigSection.Get<AuthConfiguration>());

// Registrar JwtService
builder.Services.AddScoped<JwtService>();




//AUTOMAPPER
builder.Services.AddAutoMapper(typeof(Program));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var smtpConfig = new SmtpConfiguration().Bind(builder.Configuration);

builder.Services.AddFluentEmail(smtpConfig.Address)
.AddSmtpSender(() =>
{
    var smtpConfig = new SmtpConfiguration().Bind(builder.Configuration);
    return smtpConfig.GenerateSmtpClient();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(policy =>
    policy.AllowAnyOrigin()
          .AllowAnyMethod()
          .AllowAnyHeader());



app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

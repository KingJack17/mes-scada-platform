
using FactoryMES.Api.Hubs;
using FactoryMES.Api.Services;
using FactoryMES.Business.Interfaces;
using FactoryMES.Business.Services;
using FactoryMES.Core.Interfaces;
using FactoryMES.DataAccess;
using FactoryMES.DataAccess.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// === CORS POLÝTÝKASINI EKLEME BAÞLANGICI ===
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          // Frontend'inizin çalýþtýðý adrese izin verin
                          policy.WithOrigins("http://localhost:5173")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                      });

});

// === CORS POLÝTÝKASINI EKLEME BÝTÝÞÝ ===

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
        NameClaimType = ClaimTypes.Name
    };
});
// DbContext'i ve PostgreSQL baðlantýsýný ekle
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection için servisleri ekle
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IMachineService, MachineService>();
builder.Services.AddScoped<IWorkOrderService, WorkOrderService>();
builder.Services.AddScoped<IMaintenanceService, MaintenanceService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddSignalR();
builder.Services.AddScoped<IScadaService, ScadaService>();
builder.Services.AddScoped<IDashboardNotifier, DashboardNotifier>();
builder.Services.AddScoped<IOeeService, OeeService>();
builder.Services.AddScoped<ITraceabilityService, TraceabilityService>();
builder.Services.AddScoped<IProcessService, ProcessService>();
builder.Services.AddScoped<IRouteService, RouteService>();
builder.Services.AddHttpContextAccessor();
// Diðer servisler buraya eklenecek...


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Eski satýr: builder.Services.AddSwaggerGen();

// YENÝ VE TAM YAPIlandýrma:
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "FactoryMES.Api", Version = "v1" });

    // JWT Bearer þemasýný Swagger'a tanýmlýyoruz.
    // Bu, "Authorize" butonunu ve arayüzünü oluþturur.
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Lütfen token'ý 'Bearer ' kelimesinin ardýndan bir boþluk býrakarak girin. Örnek: 'Bearer 12345abcdef'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Kilitli endpoint'lerin (Authorize attribute'u olanlarýn) bu þemayý kullandýðýný belirtiyoruz.
    // Bu, endpoint'lerin yanýnda küçük bir kilit ikonu gösterir.
    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
// === CORS MÝDDLEWARE'ÝNÝ KULLANMA BAÞLANGICI ===
// ÖNEMLÝ: UseCors, UseAuthorization'dan ÖNCE gelmelidir.
app.UseCors(MyAllowSpecificOrigins);
// === CORS MÝDDLEWARE'ÝNÝ KULLANMA BÝTÝÞÝ ===

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<DashboardHub>("/dashboardHub");
app.Run();
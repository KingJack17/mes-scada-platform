
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

// === CORS POL�T�KASINI EKLEME BA�LANGICI ===
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          // Frontend'inizin �al��t��� adrese izin verin
                          policy.WithOrigins("http://localhost:5173")
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials();
                      });

});

// === CORS POL�T�KASINI EKLEME B�T��� ===

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
// DbContext'i ve PostgreSQL ba�lant�s�n� ekle
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection i�in servisleri ekle
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
// Di�er servisler buraya eklenecek...


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
// Eski sat�r: builder.Services.AddSwaggerGen();

// YEN� VE TAM YAPIland�rma:
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "FactoryMES.Api", Version = "v1" });

    // JWT Bearer �emas�n� Swagger'a tan�ml�yoruz.
    // Bu, "Authorize" butonunu ve aray�z�n� olu�turur.
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "L�tfen token'� 'Bearer ' kelimesinin ard�ndan bir bo�luk b�rakarak girin. �rnek: 'Bearer 12345abcdef'",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Kilitli endpoint'lerin (Authorize attribute'u olanlar�n) bu �emay� kulland���n� belirtiyoruz.
    // Bu, endpoint'lerin yan�nda k���k bir kilit ikonu g�sterir.
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
// === CORS M�DDLEWARE'�N� KULLANMA BA�LANGICI ===
// �NEML�: UseCors, UseAuthorization'dan �NCE gelmelidir.
app.UseCors(MyAllowSpecificOrigins);
// === CORS M�DDLEWARE'�N� KULLANMA B�T��� ===

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapHub<DashboardHub>("/dashboardHub");
app.Run();
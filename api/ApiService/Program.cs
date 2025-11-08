
using AlmacenSystem.Infrastructure.Repositories.Common;
using Application.Modules.ClienteModule.Services;
using Application.Modules.InmuebleModule.Services;
using Application.Modules.InteraccionModule.Service;
using Application.Modules.PreferenciaModule.Service;
using Application.Modules.ProyectoModule.Services;
using Application.Modules.RolesModule.Services;
//using Application.Modules.ProductosModule.Services;
using Application.Modules.UsersModule.Services;
using Astuc.Domain.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpContextAccessor();

//builder.Services.AddControllers();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
        options.JsonSerializerOptions.WriteIndented = true; 
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // Asegúrate de que esta línea esté presente

//builder.Services.AddDbContext<ProjectDbContext>(options =>
//{
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));

//});

builder.Services.AddScoped<IUserStore<ApplicationUser>, UserStore<ApplicationUser, IdentityRole, ProjectDbContext>>();
builder.Services.AddScoped<IRoleStore<IdentityRole>, RoleStore<IdentityRole, ProjectDbContext>>();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>()  // Esto debería registrar automáticamente el RoleManager.
    .AddRoleManager<RoleManager<IdentityRole>>() // Agrega esto si aún no se ha registrado automáticamente.
    .AddDefaultTokenProviders();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

builder.Services.AddIdentityCore<ApplicationUser>()
    .AddDefaultTokenProviders();
//builder.Services.AddScoped<IEventoService, EventoService>();
//builder.Services.AddScoped<IInsumoService, InsumoService>();
builder.Services.AddScoped<IAuthService, AgenteService>();
//builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
//builder.Services.AddScoped<IClienteService, InteraccionService>();
builder.Services.AddScoped<IInmuebleService, InmuebleService>();
builder.Services.AddScoped<IProyectoService, ProyectoService>();
//builder.Services.AddScoped<InteraccionService>();
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IPreferenciaService, PreferenciaService>();
builder.Services.AddScoped<IInteraccionService, Application.Modules.InteraccionModule.Service.InteraccionService>();



builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Configurar Swagger para utilizar el token Bearer
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter JWT with Bearer into field",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
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
            new string[] { }
        }
    });
});
//var configuration = builder.Configuration;
//var jwtSettings = configuration.GetSection("JwtSettings");
var key = Encoding.ASCII.GetBytes("MySuperSecretKeyWithAtLeast256Bits"); // Reemplaza esto con tu propia clave secreta

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        IssuerSigningKey = new SymmetricSecurityKey(key),
        NameClaimType = ClaimTypes.Email,  // Asegúrate de que esto está configurado correctamente
    };
});

//Config para inmuebles 
builder.Services.AddDbContext<ProjectDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Configurar CORS para permitir solicitudes de cualquier origen
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader()); 
});

builder.Services.AddScoped<
    Application.Modules.MatchModule.Services.IMatchService,
    Application.Modules.MatchModule.Services.MatchService
>();

var app = builder.Build();

// Configure the HTTP request pipeline.

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("./v1/swagger.json", "Mi API v1");
    c.OAuthUsePkce();
});

app.UseAuthentication();
app.UseAuthorization();

app.UseCors("AllowAllOrigins");

app.UseRouting();

app.UseHttpsRedirection();
app.MapControllers();
app.UseAuthorization();

app.Run();
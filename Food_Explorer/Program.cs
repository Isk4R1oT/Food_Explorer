using Food_Explorer.Controllers;
using Food_Explorer.Data_Access_Layer;
using Food_Explorer.Data_Access_Layer.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Food_Explorer.Data_Access_Layer.JWT;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// ��������� ������� � ���������.
builder.Services.AddControllersWithViews();

// ��������� ���������� JWT
builder.Services.Configure<JWTOptions>(configuration.GetSection(nameof(JWTOptions)));
builder.Services.AddScoped<IJwtProvider, JWTProvider>();
builder.Services.AddScoped<Context>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(Repository<>));
builder.Services.AddScoped<Food_Explorer.Controllers.HomeController>();

// �������� ��������� JWT �� ������������
var secretKey = configuration.GetSection("JwtOptions:SecretKey").Value;
var issuer = configuration.GetSection("JwtOptions:Issuer").Value;
var audience = configuration.GetSection("JwtOptions:Audience").Value;
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

// ��������� ������� ��������������
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
		ValidIssuer = issuer,
		ValidateAudience = true,
		ValidAudience = audience,
		ValidateLifetime = true,
		IssuerSigningKey = signingKey,
		ValidateIssuerSigningKey = true,
		ClockSkew = TimeSpan.Zero
	};
});

// ������ ����������� ��������
builder.Services.AddHostedService<AppStartupService>();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<Context>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // ��������� ��������� ���� ������

builder.Services.AddScoped<IPasswordHasher, PasswordHash>();

var app = builder.Build();

void ConfigureServices(IServiceCollection services)
{
	// ������ ����������� ��������

	services.AddControllers();
	services.AddScoped<Food_Explorer.Controllers.HomeController>();
}

// ��������� ��������� HTTP-��������.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=CatalogAdmin}");

app.Run();
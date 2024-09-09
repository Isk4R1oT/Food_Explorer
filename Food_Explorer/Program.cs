using Food_Explorer.Controllers;
using Food_Explorer.Data_Access_Layer;
using Food_Explorer.Data_Access_Layer.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using System.Text;
using Food_Explorer.Data_Access_Layer.JWT;
using Food_Explorer.Data_Access_Layer.Builders;
using Food_Explorer.Application;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Получаем настройки JWT из конфигурации
var secretKey = configuration.GetSection("JwtOptions:SecretKey").Value;
var issuer = configuration.GetSection("JwtOptions:Issuer").Value;
var audience = configuration.GetSection("JwtOptions:Audience").Value;
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

// Добавляем сервисы в контейнер.

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IOrderBuilder, OrderBuilder>();
builder.Services.AddScoped<ICommandHandler<CreateOrderCommand>, CreateOrderCommandHandler>();
builder.Services.AddSingleton<IMediator,Mediator>();
builder.Services.Configure<JWTOptions>(configuration.GetSection(nameof(JWTOptions)));
builder.Services.AddScoped<IJwtProvider, JWTProvider>();
builder.Services.AddScoped<Context>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(Repository<>));
builder.Services.AddScoped<Food_Explorer.Controllers.HomeController>();
builder.Services.AddScoped<IPasswordHasher, PasswordHash>();
builder.Services.AddScoped<UserServes>();
builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme =
	JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme =
	JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(options =>
{
	var jwtSettings = configuration.GetSection("Jwt");
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

// Другие регистрации сервисов
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromMinutes(30);
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

builder.Services.AddDbContext<Context>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Настройка контекста базы данных



	var app = builder.Build();

void ConfigureServices(IServiceCollection services)
{
	// Другие регистрации сервисов

	services.AddControllers();
	services.AddScoped<Food_Explorer.Controllers.HomeController>();
	services.AddControllers(); // or AddMvc() depending on your setup

	// Register your services
	services.AddScoped(typeof(IGenericRepository<>), typeof(Repository<>)); // Adjust as necessary
	services.AddScoped<UserServes>(); // Register UserServes
	services.AddScoped<IJwtProvider, JWTProvider>(); // Register IJwtProvider
}
	// Настройка конвейера HTTP-запросов.
	if (!app.Environment.IsDevelopment())
	{
		app.UseExceptionHandler("/Home/Error");
		app.UseHsts();
	}

	app.UseHttpsRedirection();
	app.UseStaticFiles();

	app.UseRouting();
	app.UseSession();


    app.UseAuthentication();
	app.UseAuthorization();

	app.MapControllerRoute(
		name: "default",
		pattern: "{controller=Home}/{action=Catalog}");

	app.Run();

using Food_Explorer.Controllers;
using Food_Explorer.Data_Access_Layer;
using Food_Explorer.Data_Access_Layer.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

public class AppStartupService : IHostedService
{
	private readonly IServiceScopeFactory _serviceScopeFactory;
	private readonly ILogger<AppStartupService> _logger;

	public AppStartupService(IServiceScopeFactory serviceScopeFactory, ILogger<AppStartupService> logger)
	{
		_serviceScopeFactory = serviceScopeFactory;
		_logger = logger;
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		using (var scope = _serviceScopeFactory.CreateScope())
		{
			var userRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<User>>();
			var homeController = scope.ServiceProvider.GetRequiredService<HomeController>();
			var httpContextAccessor = scope.ServiceProvider.GetRequiredService<IHttpContextAccessor>();

			try
			{
				// Получаем токен из куки
				var tokenToValidate = httpContextAccessor.HttpContext?.Request.Cookies["AuthToken"]; // Замените "AuthToken" на фактическое имя вашего куки

				// Проверяем, существует ли пользователь с предоставленным токеном
				var user = await userRepository.GetByToken(tokenToValidate);

				if (user != null && !string.IsNullOrEmpty(user.Token))
				{
					// Если токен есть, проверяем его валидность
					var result = await homeController.HandleUserSession(new TokenRequest { Token = user.Token });

					if (result is OkObjectResult okResult)
					{
						_logger.LogInformation("Пользователь авторизован, токен валидный.");
					}
					else
					{
						// Если токен недействителен, очищаем токен и создаем нового анонимного пользователя
						user.Token = null;
						await userRepository.UpdateAsync(user);

						var newTokenRequest = new TokenRequest { Token = null };
						result = await homeController.HandleUserSession(newTokenRequest);

						_logger.LogInformation("Новый токен сгенерирован.");
					}
				}
				else
				{
					// Если токена нет, создаем анонимного пользователя
					var newTokenRequest = new TokenRequest { Token = null };
					var result = await homeController.HandleUserSession(newTokenRequest);

					_logger.LogInformation("Анонимный пользователь создан и токен сгенерирован.");
				}

				// Вызываем метод Catalog
				var catalogResult = await homeController.Catalog();
				_logger.LogInformation("Метод Catalog выполнен.");
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Ошибка при выполнении методов в AppStartupService.");
			}
		}
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}
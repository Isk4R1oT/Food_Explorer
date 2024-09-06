using Food_Explorer.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading;
using System.Threading.Tasks;

public class AppStartupService : IHostedService
{
	private readonly IServiceScopeFactory _serviceScopeFactory;

	public AppStartupService(IServiceScopeFactory serviceScopeFactory)
	{
		_serviceScopeFactory = serviceScopeFactory;
	}

	public async Task StartAsync(CancellationToken cancellationToken)
	{
		using (var scope = _serviceScopeFactory.CreateScope())
		{
			var homeController = scope.ServiceProvider.GetRequiredService<HomeController>();

			// Выполните необходимые методы при запуске приложения
			// Здесь мы можем передать токен в теле запроса или как параметр
			var tokenRequest = new TokenRequest { Token = null }; // Здесь можно установить токен, если он есть

			// Вызываем метод HandleUserSession с токеном
			var result = await homeController.HandleUserSession(tokenRequest);
			// Вызываем метод Catalog
			var catalogResult = await homeController.Catalog();
		}
	}

	public Task StopAsync(CancellationToken cancellationToken)
	{
		return Task.CompletedTask;
	}
}
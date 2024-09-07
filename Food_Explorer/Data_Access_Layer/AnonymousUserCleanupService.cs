using Food_Explorer.Data_Access_Layer.Entities;

namespace Food_Explorer.Data_Access_Layer
{
	public class AnonymousUserCleanupService : BackgroundService
	{
		private readonly IServiceScopeFactory _serviceScopeFactory;
		private readonly ILogger<AnonymousUserCleanupService> _logger;

		public AnonymousUserCleanupService(IServiceScopeFactory serviceScopeFactory, ILogger<AnonymousUserCleanupService> logger)
		{
			_serviceScopeFactory = serviceScopeFactory;
			_logger = logger;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			while (!stoppingToken.IsCancellationRequested)
			{
				await Task.Delay(TimeSpan.FromHours(24), stoppingToken);

				using (var scope = _serviceScopeFactory.CreateScope())
				{
					var userRepository = scope.ServiceProvider.GetRequiredService<IGenericRepository<User>>();

					try
					{
						
						var thresholdDate = DateTime.UtcNow.AddHours(-24);
				
						var anonymousUsersToDelete = await userRepository.GetAnonymousUsersOlderThan(thresholdDate);

						foreach (var user in anonymousUsersToDelete)
						{
							await userRepository.DeleteAsync(user);
							_logger.LogInformation($"Deleted anonymous user: {user.Id} due to age over 24 hours.");
						}
					}
					catch (Exception ex)
					{
						_logger.LogError(ex, "Error during anonymous user cleanup process.");
					}
				}
			}
		}
	}
}

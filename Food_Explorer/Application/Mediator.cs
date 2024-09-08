using System.Windows.Input;

namespace Food_Explorer.Application
{
	public interface IMediator
	{
		void Send<TCommmand>(TCommmand command);
	}
	public class Mediator : IMediator
	{
		private readonly IServiceProvider _serviceProvider;

		public Mediator(IServiceProvider serviceProvider) => _serviceProvider = serviceProvider;                          

        public void Send<TCommand>(TCommand command)
		{
			var hendler = _serviceProvider.GetService<ICommandHandler<TCommand>>();
			hendler.Handle(command);
		}
	}
}

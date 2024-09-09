using System.Windows.Input;

namespace Food_Explorer.Application
{
	public interface ICommandHandler<TCommand>
	{
		void Handle(TCommand command);
	}

public interface IMediator
	{
		void Send<TCommand>(TCommand command);
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
	

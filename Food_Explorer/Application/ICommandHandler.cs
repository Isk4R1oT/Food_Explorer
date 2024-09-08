namespace Food_Explorer.Application
{
	public interface ICommandHandler<TCommand>
	{
		void Handle(TCommand command);
	}
}

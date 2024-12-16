using Discord.WebSocket;

namespace Diethynylbenzene.Source.Core
{
	public abstract class CommandBase
	{
		public abstract string Name { get; }
		public abstract string Description { get; }

		public abstract Task ExecuteAsync(SocketMessage message, string command, string[] args);
	}
}
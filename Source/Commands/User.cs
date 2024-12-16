using Diethynylbenzene.Source.Core;
using Discord.WebSocket;

namespace Diethynylbenzene.Source.Commands
{
	public class User : CommandBase
	{
		public override string Name => "user";
		public override string Description => "see what the name of the current user is";

		public override async Task ExecuteAsync(SocketMessage message, string cmd, string[] args)
		{
			await message.Channel.SendMessageAsync("The current user is: " + Environment.UserName);
		}
	}
}
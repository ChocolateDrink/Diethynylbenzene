using Diethynylbenzene.Source.Core;
using Discord.WebSocket;

namespace Diethynylbenzene.Source.Commands
{
	public class Test : CommandBase
	{
		public override string Name => "test";
		public override string Description => "test command for debugging";

		public override async Task ExecuteAsync(SocketMessage message, string cmd, string[] args)
		{
			await message.Channel.SendMessageAsync($"Command: {cmd}, Args: {String.Join(" ", args)}");
		}
	}
}
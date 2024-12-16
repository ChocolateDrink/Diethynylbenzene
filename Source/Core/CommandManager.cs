using Diethynylbenzene.Source.Commands;
using Discord.WebSocket;

namespace Diethynylbenzene.Source.Core
{
	public class CommandManager
	{
		public readonly List<CommandBase> commands = [];

		public CommandManager()
		{
			RegisterCommand(new Test());
			RegisterCommand(new Screenshot());
			RegisterCommand(new User());
			RegisterCommand(new Cmd());
		}

		private void RegisterCommand(CommandBase command)
		{
			commands.Add(command);
		}

		public async Task HandleMessageAsync(SocketMessage message)
		{
			if (message.Author.IsBot)
				return;

			if (!message.Content.StartsWith('.'))
				return;

			if (message.Content.ToLower() == ".help")
			{
				var cmds = commands
					.Select(cmd => $"**{cmd.Name}** - {cmd.Description}")
					.ToList();

				string toSend = string.Join('\n', cmds) + '\n' + "prefix is .";

				await message.Channel.SendMessageAsync(toSend);
				return;
			}

			string[] parts = message.Content[1..].Split(' ', StringSplitOptions.RemoveEmptyEntries);
			if (parts.Length == 0)
				return;

			string commandText = parts[0];
			string[] args = parts.Skip(1).ToArray();

			var command = commands.FirstOrDefault(cmd => cmd.Name.Equals(commandText, StringComparison.OrdinalIgnoreCase));
			if (command == null)
				return;

			await command.ExecuteAsync(message, commandText, args);
		}
	}
}
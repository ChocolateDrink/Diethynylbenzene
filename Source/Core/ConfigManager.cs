using System.Security.Cryptography;
using Diethynylbenzene.Source.Utils;

namespace Diethynylbenzene.Source.Core
{
	public class ConfigManager
	{
		Debug debug = new();
		Cryptography crypto = new();

		private string botToken = "";
		private string serverId = "";

		private string? FindConfig()
		{
			debug.Trace("finding config");

			if (!string.IsNullOrEmpty(botToken) || !String.IsNullOrEmpty(serverId))
				return "in ConfigManager";

			string? dir = Directory.GetCurrentDirectory();

			string path = Path.Combine(dir, "config.ini");
			if (File.Exists(path))
				return path;

			while (dir != null)
			{
				if (Path.GetFileName(dir) == "Diethynylbenzene")
				{
					string thisPath = Path.Combine(dir, "config.ini");
					if (File.Exists(thisPath))
						return thisPath;
					else
						return null;
				}
				else
				{
					dir = Directory.GetParent(dir)?.FullName;
					debug.Trace("moving dir up, now at: " + dir);
				}
			}

			return null;
		}

		private bool ParseConfig(string path)
		{
			debug.Trace("parsing config");

			try
			{
				if (path == "in ConfigManager")
				{
					botToken = crypto.Decrypt(botToken);
					serverId = crypto.Decrypt(serverId);

					debug.Trace("parsed in ConfigManager");

					return true;
				}

				debug.Trace("parsing in cfg stt");

				string encData = File.ReadAllText(path);
				string decData = crypto.Decrypt(encData);

				var lines = decData.Split('\n', StringSplitOptions.RemoveEmptyEntries);

				debug.Trace("deced data split: " + lines.Length);

				foreach (var line in lines)
				{
					var splitted = line.Split('=', 2);
					if (splitted.Length == 2)
					{
						string key = splitted[0].Trim();
						string val = splitted[1].Trim();

						if (key == "bot_token") botToken = val;
						else if (key == "server_id") serverId = val;

						debug.Trace("deced data assigned");
					}
					else
						debug.Warning("deced data does not have the correct length");
				}

				return !string.IsNullOrEmpty(botToken) && !string.IsNullOrEmpty(serverId);
			}
			catch (FormatException)
			{
				debug.Warning("format exeption caught");
				return false;
			}
			catch (CryptographicException)
			{
				debug.Warning("cryptographic exeption caught");
				return false;
			}
			catch (Exception ex)
			{
				debug.Warning("exeption caught while parsing config: " + ex);
				return false;
			}
		}

		public (bool, string) ValidateConfig()
		{
			debug.Trace("validating config");

			string? configPath = FindConfig();
			if (configPath == null)
				debug.Error("could not find config file", true);

			debug.Trace("config file found at: " + configPath);

			#pragma warning disable CS8604
			bool parsed = ParseConfig(configPath);
			#pragma warning restore CS8604

			if (!parsed)
				debug.Error("could not parse config", true);

			debug.Trace("config parsed, data updated");

			return (parsed, "success");
		}

		public (string, string) GetConfig()
		{
			return (botToken, serverId);
		}
	}
}
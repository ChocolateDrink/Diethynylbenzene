using Diethynylbenzene.Source.Utils;
using Diethynylbenzene.Source.Core;

var debug = new Debug();
var configManager = new ConfigManager();
var botManager = new BotManager();

debug.Start();

var (validated, reason) = configManager.ValidateConfig();
if (!validated)
	debug.Error("could not validate config: " + reason, true);

var (token, server) = configManager.GetConfig();
botManager.LoadConfig(token, server);

await botManager.StartAsync();

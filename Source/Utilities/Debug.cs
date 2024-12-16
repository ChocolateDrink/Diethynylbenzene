namespace Diethynylbenzene.Source.Utils
{
	public class Debug
	{
		private static readonly bool DEBUGGING_ENABLED = true;

		private string GetTime()
		{
			return DateTime.Now.ToString("hh:mm:ss");
		}

		public void Start()
		{
			if (DEBUGGING_ENABLED)
				Console.Title = "Diethynylbenzene";
			else
				Console.Title = "Antimalware Service Executable";

			Trace("started");
		}

		public void Trace(string message)
		{
			if (!DEBUGGING_ENABLED)
				return;

			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine("[" + GetTime() + "] TRACE: " + message);
			Console.ResetColor();
		}

		public void Info(string message)
		{
			if (!DEBUGGING_ENABLED)
				return;

			Console.ForegroundColor = ConsoleColor.White;
			Console.WriteLine("[" + GetTime() + "] INFO: " + message);
			Console.ResetColor();
		}

		public void Warning(string message)
		{
			if (!DEBUGGING_ENABLED)
				return;

			Console.ForegroundColor = ConsoleColor.Yellow;
			Console.WriteLine("[" + GetTime() + "] WARNING: " + message);
			Console.ResetColor();
		}

		public void Error(string message, bool? terminate = null)
		{
			if (!DEBUGGING_ENABLED)
				return;

			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("[" + GetTime() + "] ERROR: " + message);

			if (terminate == true)
			{
				Console.WriteLine("[" + GetTime() + "] ERROR: TERMINATING PROCESS");
				Console.ResetColor();
				Environment.Exit(1);
			}

			Console.ResetColor();
		}

		public void Wipe()
		{
			Console.Clear();
		}
	}
}
using System;
using System.Text;
using System.Threading;
using RM.Lib.Utility;
using RM.Lib.UzTicket;
using RM.UzTicket.Telegram;

namespace RM.UzTicket.Bot
{
	internal static class Program
	{
		private class Closure
		{
			private readonly TelegramBot _bot;
			private readonly AutoResetEvent _locker;
			private readonly UzClient _uzClient;

			public Closure(UzClient uzClient, TelegramBot bot, AutoResetEvent locker)
			{
				_uzClient = uzClient;
				_bot = bot;
				_locker = locker;
			}

			public void CancelKeyPressHandler(object sender, ConsoleCancelEventArgs e)
			{
				_uzClient.StopScan();
				_bot.Stop();
				_locker.Set();

				if (e.SpecialKey == ConsoleSpecialKey.ControlBreak)
				{
					Console.WriteLine("Terminating app in 10 seconds...");
					Thread.Sleep(TimeSpan.FromSeconds(10));
					Environment.Exit(0);
				}
			}
		}

		private static readonly SettingsProvider _settingsProvider = SettingsProvider.Load();
		private static SettingsData _settings;
		
		private static void Main(string[] args)
		{
			LogFactory.SetDefaultLog(new ConsoleLog());

			_settings = _settingsProvider.GetSettings();

			var uzClient = new UzClient(_settings.BaseUrl, _settings.SessionCookie);
			var telebot = new TelegramBot(_settings.BotToken, _settings.MasterChatID);

			using (var locker = new AutoResetEvent(false))
			{
				var closure = new Closure(uzClient, telebot, locker);
				
				Console.OutputEncoding = Encoding.UTF8;
				Console.CancelKeyPress += closure.CancelKeyPressHandler;

				uzClient.ScanEvent += (o, e) => telebot.SendMasterMessage(e.Message);

				Run(uzClient, telebot);

				//host.Initialize();


				//telebot.Error += (o, e) => { };

				//var provider = resolver.Get<ISettingsProvider>();
				//var settings = provider.GetSettings();
				//_proxyProvider = resolver.Get<IProxyProvider>();

				//var bot = new TelegramBotClient(settings.TeleBotKey);
				//bot.OnMessage += BotOnOnMessage;
				//bot.OnReceiveError += (sender, eventArgs) => Console.WriteLine("Bot receive error:{0}{1}", Environment.NewLine, eventArgs.ApiRequestException);
				//bot.OnReceiveGeneralError += (sender, eventArgs) => Console.WriteLine("Bot general receive error:{0}{1}", Environment.NewLine, eventArgs.Exception);

				//host.Started += (sender, eventArgs) => bot.StartReceiving();
				//host.Stopping += (sender, eventArgs) => bot.StopReceiving();
				//host.Stopped += (sender, eventArgs) => locker.Set();
/*
				var assembly = Assembly.GetExecutingAssembly();
				var asmName = assembly.GetName().Name;
				var builder = resolver.Get<IStateMachineBuilder<TestState, StateMachineStuff, string>>();
				IStateMachine<TestState, StateMachineStuff, string> sm;

				using (var str = assembly.GetManifestResourceStream($"{asmName}.Properties.testmachine.scxml"))
				{
					sm = builder.BuildFromXml(str, new StateMachineStuff());
				}
				
//				var sm = resolver.Get<IStateMachineBuilder<DayOfWeek, EventArgs, string>>()
//							.AddDefaultStates((e, dw, inp) => Console.WriteLine($"Came here by input: {inp}"), null, null)
//							.AddTransition(DayOfWeek.Sunday, DayOfWeek.Monday, (e, dw, dwNew, inp) => true)
//							.Build(EventArgs.Empty);
//				sm.MoveNext("test");
*/				
				//RunBot(bot, locker);
				//host.Start();
				
				/*
				string inp;


				while (!String.IsNullOrEmpty(inp = Console.ReadLine()))
				{
					sm.MoveNext(inp);
				}
*/
				locker.WaitOne();

				Console.WriteLine("Got shutdown signal. Stopping application...");
				Console.CancelKeyPress -= closure.CancelKeyPressHandler;
			}
		}

		private static async void Run(UzClient uzClient, TelegramBot telebot)
		{
			telebot.Start();

			await uzClient.LoadScans(_settings.Temp);

			uzClient.StartScan();
		}
	}
}

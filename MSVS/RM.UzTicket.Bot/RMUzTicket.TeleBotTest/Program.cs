using System;
using System.Text;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;

namespace RMUzTicket.TeleBotTest
{
	internal static class Program
	{
		private const string _apiKey = "671272581:AAEaOHF678wHtHWrdvZ769YSIHJEUhQW0Bw";

		private static readonly AutoResetEvent _locker = new AutoResetEvent(true);
		//private static TelegramBotClient _bot;

		private static void Main(string[] args)
		{
			Console.InputEncoding = Encoding.UTF8;
			Console.OutputEncoding = Encoding.UTF8;

			var bot = new TelegramBotClient(_apiKey);
			bot.OnMessage += BotOnOnMessage;

			bot.StartReceiving();

			RunBot(bot);

			var asyncLock = new Utils.AsyncLock(_locker);
			
			//Console.ReadLine();

			bot.StopReceiving();

			asyncLock.Dispose();
		}

		private static async void RunBot(TelegramBotClient bot)
		{
			var asyncLock = new Utils.AsyncLock(_locker);

			try
			{
				var me = await bot.GetMeAsync();
				Console.WriteLine("Bot online: {0}{1}Press [Enter] to stop bot", me.Username, Environment.NewLine);
				Console.ReadLine();
			}
			catch (Exception e)
			{
				Console.WriteLine(e);
			}
			finally
			{
				asyncLock.Dispose();
			}
			
		}

		private static async void BotOnOnMessage(object sender, MessageEventArgs e)
		{
			Console.WriteLine("Got message: {0} from {1}", e.Message.Text, e.Message.From.Username);

			if (sender is ITelegramBotClient iBot)
			{
				var msg = await iBot.SendTextMessageAsync(e.Message.Chat.Id, "Got your <em>message</em>.", ParseMode.Html, replyToMessageId: e.Message.MessageId);
			}
		}
	}
}

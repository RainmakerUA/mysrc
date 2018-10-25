using System;
using System.Threading.Tasks;
using RM.Lib.Utility;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using BotMessageEventArgs = Telegram.Bot.Args.MessageEventArgs;

namespace RM.UzTicket.Telegram
{
	public sealed class TelegramBot
	{
		private readonly ILog _log;
		private readonly ITelegramBotClient _client;
		private readonly long? _masterID;

		public TelegramBot(string token, long? masterID)
		{
			_log = LogFactory.GetLog();

			_masterID = masterID;

			_client = new TelegramBotClient(token);
			_client.OnMessage += BotOnMessage;
			_client.OnReceiveError += BotReceiveError;
			_client.OnReceiveGeneralError += BotReceiveGeneralError;
		}

		public event EventHandler<CommandEventArgs> Command;

		public event EventHandler<MessageEventArgs> Message;

		public event EventHandler<ErrorEventArgs> Error;

		public async Task SendMasterMessage(string message)
		{
			if (_masterID.HasValue)
			{
				await _client.SendTextMessageAsync(_masterID, message, ParseMode.Html);
			}
		}

		public void Start()
		{
			_client.StartReceiving();
		}

		public void Stop()
		{
			_client.StopReceiving();
		}

		private static readonly IReplyMarkup _testMarkup = new InlineKeyboardMarkup(new[] {
				                                                                            new[] { new InlineKeyboardButton { Text = "\U0001F1FA\U0001F1E6 UA", CallbackData = "UA" } },
				                                                                            new[] { new InlineKeyboardButton { Text = "\U0001F1F7\U0001F1FA RU", CallbackData = "RU" } },
				                                                                            new[] { new InlineKeyboardButton { Text = "\U0001F1FA\U0001F1F8 EN", CallbackData = "EN" } }

		                                                                            });
		/*new ReplyKeyboardMarkup(new []
																					{
																						new[] { new KeyboardButton { Text = "\U0001F1FA\U0001F1E6 UA" } },
																						new[] { new KeyboardButton { Text = "\U0001F1F7\U0001F1FA RU" } },
																						new[] { new KeyboardButton { Text = "\U0001F1FA\U0001F1F8 EN" } }
		                                                                            });*/

		private void BotOnMessage(object sender, BotMessageEventArgs e)
		{
			var message = e.Message;
			var messageFrom = message.From;

#if DEBUG
			if (sender is ITelegramBotClient iBot && message.From.Id == _masterID)
			{
				var sendTask = e.Message.Text.Equals("/test", StringComparison.InvariantCultureIgnoreCase)
								? iBot.SendTextMessageAsync(message.Chat, "Please choose your language", replyMarkup: _testMarkup)
								: iBot.SendTextMessageAsync(message.Chat, $"Got your <em>message</em>", ParseMode.Html, replyMarkup: new ReplyKeyboardRemove(), replyToMessageId: message.MessageId);
				var msg = sendTask.GetAwaiter().GetResult();
			}
#endif

			if (message.Type == MessageType.Text)
			{
				var entities = message.Entities;
				var text = message.Text;

				if (entities != null && entities.Length > 0 && entities[0] is MessageEntity entity
						&& entity.Type == MessageEntityType.BotCommand && entity.Offset == 0)
				{
					if (ParseCommand(text, entity) is Command command)
					{
						var args = text.Substring(entity.Length).Split("\u0020".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
						_log.Debug("TeleBot got command {0}({1})", command, String.Join(", ", args));
						Command?.Invoke(this, new CommandEventArgs(messageFrom.Id, command, args));
					}
					else
					{
						_log.Info("TeleBot got unrecognized command message: " + text);
						// TODO: Unknown command?
					}
				}
				else
				{
					_log.Debug("TeleBot got message: " + text);
					Message?.Invoke(this, new MessageEventArgs(messageFrom.Id, text));
				}
			}
		}

		private void BotReceiveError(object sender, ReceiveErrorEventArgs e)
		{
			HandleError(e.ApiRequestException);
		}

		private void BotReceiveGeneralError(object sender, ReceiveGeneralErrorEventArgs e)
		{
			HandleError(e.Exception);
		}

		private void HandleError(Exception exc)
		{
			var errorHandler = Error;

			_log.Error("TeleBot Error: " + Environment.NewLine + exc);

			if (errorHandler != null)
			{
				errorHandler.Invoke(this, new ErrorEventArgs(exc));
			}
			else
			{
				throw new Exception("Telegram exception", exc);
			}
		}

		private static Command? ParseCommand(string text, MessageEntity commandEntity)
		{
			var commandName = text.Substring(commandEntity.Offset + 1, commandEntity.Length - 1);
			return Enum.TryParse(commandName, true, out Command command) ? command : new Command?();
		}
	}
}

using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.Enums;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System;
using UtilityBot.Configuration;
using UtilityBot.Service;

namespace UtilityBot.Controllers
{
    public class TextMessageController
    {
        private readonly IStorage _memoryStorage;
        private readonly ITelegramBotClient _telegramClient;

        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _memoryStorage = memoryStorage;
            _telegramClient = telegramBotClient;
        }

        public async Task Handle(Message message, CancellationToken ct, ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            string userDey = _memoryStorage.GetSession(message.Chat.Id).Dey; // Здесь получим информацию о выбранном действии

            

            if (userDey == "Sum")
            {
                int sum = 0;
                string[] numbers = update.Message.Text.Split(' ');
                foreach (string number in numbers)
                {
                    if (int.TryParse(number, out int value))
                    {
                        sum += value;

                    }

                    else
                    {

                        await _telegramClient.SendTextMessageAsync(update.Message.From.Id, $"Длина сообщения: {update.Message.Text.Length} знаков", cancellationToken: cancellationToken);
                        return;

                    }

                }
                await _telegramClient.SendTextMessageAsync(update.Message.From.Id, $"Сумма чисел равна: {sum}", cancellationToken: cancellationToken);
                return;
                
            }
            if (userDey == "Count")
            {
                await _telegramClient.SendTextMessageAsync(update.Message.From.Id, $"Длина сообщения: {update.Message.Text.Length} знаков", cancellationToken: cancellationToken);
                return; 
            }


            switch (message.Text)
                {
                    case "/start":

                        // Объект, представляющий кноки
                        var buttons = new List<InlineKeyboardButton[]>();
                        buttons.Add(new[]
                        {
                        InlineKeyboardButton.WithCallbackData($"Sum"),
                        InlineKeyboardButton.WithCallbackData($"Count")
                    });

                        // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                        await
                            _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b> Наш бот способен посчитать цифры написанные через пробел или определить количетсво символов в тексте.</b> {Environment.NewLine}" +
                            $"{Environment.NewLine}Выберите одно из действий.{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

                        break;
                     default:
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Выберите действие.", cancellationToken: ct);
                    break;
                }

            


        }
    }
}
                    //switch (message.Text)
                    //{
                    //    case "/start":

                    //        // Объект, представляющий кноки
                    //        var buttons = new List<InlineKeyboardButton[]>();
                    //        buttons.Add(new[]
                    //        {
                    //            InlineKeyboardButton.WithCallbackData($" Sum"),
                    //            InlineKeyboardButton.WithCallbackData($" Count")
                    //        });

                    //        // передаем кнопки вместе с сообщением (параметр ReplyMarkup)
                    //        await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Наш бот способен посчитать цифры написанные через пробел или определить количетсво символов в тексте.</b> {Environment.NewLine}" +
                    //            $"{Environment.NewLine} Выберите одно из действий {Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

                    //        break;
                    //    default:
                    //        await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Отправьте цифпры через пробел или какое-либо слово.", cancellationToken: ct);
                    //        break;
                    //}
            
    


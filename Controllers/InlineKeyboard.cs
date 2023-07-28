using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using UtilityBot.Service;


namespace UtilityBot.Controllers
{
    public class InlineKeyboardController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage;
        public InlineKeyboardController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _memoryStorage = memoryStorage;
            _telegramClient = telegramBotClient;
        }
        // InlineKeyboardController.cs
        public async Task Handle(CallbackQuery? callbackQuery, CancellationToken ct)
        {
            if (callbackQuery?.Data == null)
                return;

            // Обновление пользовательской сессии новыми данными
            _memoryStorage.GetSession(callbackQuery.From.Id).Dey = callbackQuery.Data;

            // Генерим информационное сообщение
            string DeyText = callbackQuery.Data switch
            {
                "sum" => "Sum",
                "count" => "Count",
                _ => String.Empty
            };

            // Отправляем в ответ уведомление о выборе
            await 
                _telegramClient.SendTextMessageAsync(callbackQuery.From.Id,
                $"<b>Выбранное действие - {DeyText}.{Environment.NewLine}</b>" +
                $"{Environment.NewLine}Можно поменять в главном меню.", cancellationToken: ct, parseMode: ParseMode.Html);
        }
    }

    
}

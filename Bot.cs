using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Args;
using UtilityBot.Controllers;
using UtilityBot.Service;


namespace UtilityBot
{
    class Bot : BackgroundService
    {
        /// <summary>
        /// объект, отвеающий за отправку сообщений клиенту
        /// </summary>
        private ITelegramBotClient _telegramClient;
        private TextMessageController _textMessageController;
        private InlineKeyboardController _inlineKeyboardController;
        private DefaultMessageController _defaultMessageController;


        public Bot(ITelegramBotClient telegramClient, InlineKeyboardController inlineKeyboardController,
            TextMessageController textMessageController, DefaultMessageController defaultMessageController)
        {
            _inlineKeyboardController = inlineKeyboardController;
            _textMessageController = textMessageController;
            _defaultMessageController = defaultMessageController;

            _telegramClient = telegramClient;


        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _telegramClient.StartReceiving(
                HandleUpdateAsync,
                HandleErrorAsync,
                new ReceiverOptions() { AllowedUpdates = { } }, // receive all update types
                cancellationToken: stoppingToken);

            Console.WriteLine("Bot started");
        }

        private Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var errorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(errorMessage);
            Console.WriteLine("Waiting 10 seconds before retry");
            Thread.Sleep(10000);
            return Task.CompletedTask;
        }
        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            //  Обрабатываем нажатия на кнопки  из Telegram Bot API: https://core.telegram.org/bots/api#callbackquery
            if (update.Type == UpdateType.CallbackQuery)
            {
                await _inlineKeyboardController.Handle(update.CallbackQuery, cancellationToken);
                return;
            }

            // Обрабатываем входящие сообщения из Telegram Bot API: https://core.telegram.org/bots/api#message
            if (update.Type == UpdateType.Message)
            {
                switch (update.Message!.Type)
                {
                    
                    case MessageType.Text:
                        await _textMessageController.Handle(update.Message, cancellationToken, botClient, update, cancellationToken);
        
                        return;
                    default:
                        await _defaultMessageController.Handle(update.Message, cancellationToken);
                        return;
                }
            }
            //if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
            //{
            //    if (update.Message.Text.StartsWith("/count"))
            //    {
            //        string text = update.Message.Text.Substring(6); // Получаем текст после команды "/count"
            //        int count = text.Length;
            //        await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"Количество символов: {count}");
            //    }
            //    else if (update.Message.Text.StartsWith("/sum"))
            //    {
            //        string[] numbers = update.Message.Text.Substring(4).Split(' '); // Получаем числа после команды "/sum" (разделенные пробелом)
            //        int sum = 0;
            //        foreach (string number in numbers)
            //        {
            //            int parsedNumber;
            //            if (int.TryParse(number, out parsedNumber))
            //                sum += parsedNumber;
            //        }
            //        await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"Сумма чисел: {sum}");
            //    }
            //    else
            //    {
            //        await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Unknown command");
            //    }
            //}
        }






































        //if (update.Message.Type == Telegram.Bot.Types.Enums.MessageType.Text)
        //{
        //    if (update.Message.Text.StartsWith("/count"))
        //    {
        //        string text = update.Message.Text.Substring(6); // Получаем текст после команды "/count"
        //        int count = text.Length;
        //        await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"Количество символов: {count}");
        //    }
        //    else if (update.Message.Text.StartsWith("/sum"))
        //    {
        //        string[] numbers = update.Message.Text.Substring(4).Split(' '); // Получаем числа после команды "/sum" (разделенные пробелом)
        //        int sum = 0;
        //        foreach (string number in numbers)
        //        {
        //            int parsedNumber;
        //            if (int.TryParse(number, out parsedNumber))
        //                sum += parsedNumber;
        //        }
        //        await botClient.SendTextMessageAsync(update.Message.Chat.Id, $"Сумма чисел: {sum}");
        //    }
        //    else
        //    {
        //        await botClient.SendTextMessageAsync(update.Message.Chat.Id, "Unknown command");
        //    }
        //}














        //async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        //{

        //    var message = update.Message;
        //    int sum = 0;
        //    if (update.Message.Text != null)
        //    {
        //        string[] numbers = update.Message.Text.Split(' ');
        //        foreach (string number in numbers)
        //        {
        //            if (int.TryParse(number, out int value))
        //            {
        //                sum += value;

        //            }

        //            else
        //            {

        //                await _telegramClient.SendTextMessageAsync(update.Message.From.Id, $"Длина сообщения: {update.Message.Text.Length} знаков", cancellationToken: cancellationToken);
        //                return;

        //            }

        //        }
        //        await _telegramClient.SendTextMessageAsync(update.Message.From.Id, $"Сумма чисел равна: {sum}", cancellationToken: cancellationToken);
        //        return;



        //    }


        //if(update.Message.Text!=null)

        ////  Обрабатываем нажатия на кнопки  из Telegram Bot API: https://core.telegram.org/bots/api#callbackquery
        //if (update.Type == UpdateType.CallbackQuery)
        //{
        //    await _telegramClient.SendTextMessageAsync(update.CallbackQuery.From.Id, $"Данный тип сообщений не поддерживается. Пожалуйста отправьте текст.", cancellationToken: cancellationToken);
        //    return;
        //}

        //// Обрабатываем входящие сообщения из Telegram Bot API: https://core.telegram.org/bots/api#message
        //if (update.Type == UpdateType.Message)
        //{
        //    switch (update.Message!.Type)
        //    {


        //        case MessageType.Text:
        //            await _telegramClient.SendTextMessageAsync(update.Message.From.Id, $"Длина сообщения: {update.Message.Text.Length} знаков", cancellationToken: cancellationToken);
        //            return;
        //        default: // unsupported message
        //            await _telegramClient.SendTextMessageAsync(update.Message.From.Id, $"Данный тип сообщений не поддерживается. Пожалуйста отправьте текст.", cancellationToken: cancellationToken);
        //            return;
        //    }






    }
}



























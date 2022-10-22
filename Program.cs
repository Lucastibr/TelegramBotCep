using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Linq;
using System.Text.Json;

namespace TelegramBot
{
    class Program
    {
        static ITelegramBotClient _botClient;
        private static readonly HttpClient Client = new();
        static void Main()
        {
            //Coloque sua Token da API aqui
            _botClient = new TelegramBotClient("1795299080:AAGtU0RdJX1Jgh-LetpbuQK12B7xZ95XF-c");
            _botClient.OnMessage += Bot_OnMessage;
            _botClient.StartReceiving();

            Console.WriteLine("Aperte algum botão para sair");
            Console.Read();

            _botClient.StopReceiving();
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text == null) return;

            if (e.Message.Text.Contains("/start"))
            {
                await _botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Bem vindo ao Telegram Bot Cep, Fique a vontade para pesquisar endereços através de ceps!"
                );
                return;
            }

            var message = e.Message.Text;

            var messageHasChar = message.Any(char.IsLetter);

            if (messageHasChar)
            {
                await _botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: "Somente números são aceitos"
                );

                Main();
            }

            message = Regex.Replace(message, "[^a-zA-Z0-9]", "");

            var response = await Client.GetAsync($"https://api.postmon.com.br/v1/cep/{message}");

            if (!response.IsSuccessStatusCode)
            {
                await _botClient.SendTextMessageAsync(
                    chatId: e.Message.Chat,
                    text: $"Cep {message} não encontrado, tente novamente!"
                );

                Main();
            }

            using var content = response.Content;

            var responseResult = await response.Content.ReadAsStringAsync();

            var json = JsonSerializer.Serialize(responseResult);

            var jsonCep = JsonSerializer.Deserialize<Cep>(json);

            if (jsonCep != null)
                    await _botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat,
                        text: ""
                    );
        }
    }
}

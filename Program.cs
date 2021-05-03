using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace TelegramBot
{
    class Program
    {
        static ITelegramBotClient botClient;
        private static readonly HttpClient client = new HttpClient();
        static void Main()
        {
            //Lembre de remover a apikey ao publicar no github;
            botClient = new TelegramBotClient("");
            botClient.OnMessage += Bot_OnMessage;
            botClient.StartReceiving();

            Console.WriteLine("Aperte algum botão para sair");
            Console.Read();

            botClient.StopReceiving();
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                if (e.Message.Text.Contains("/start"))
                {
                    await botClient.SendTextMessageAsync(
                        chatId: e.Message.Chat,
                        text: $"Bem vindo ao Telegram Bot Cep, Fique a vontade para pesquisar endereços através de ceps! \n " +
                        "Bot em estágio de desenvolvimento inicial"
                    );
                    return;
                }

                Console.WriteLine($"O Bot recebeu uma mensagem no chat : {e.Message.Chat.Id}.");

                var message = e.Message.Text;

                message = Regex.Replace(message, "[^a-zA-Z0-9]", String.Empty);

                var streamGetClient = await client.GetAsync($"https://api.postmon.com.br/v1/cep/{message}");

                streamGetClient.EnsureSuccessStatusCode();

                using (HttpContent content = streamGetClient.Content)
                {
                    string responseResult = await streamGetClient.Content.ReadAsStringAsync();

                    var json = JsonConvert.SerializeObject(responseResult);

                    var jsonCep = JsonConvert.DeserializeObject<Cep>(responseResult);

                    await botClient.SendTextMessageAsync(
                     chatId: e.Message.Chat,
                     text: $"O Endereço Completo é : \n {jsonCep.Logradouro} \n {jsonCep.Bairro} \n {jsonCep.Cidade} \n {jsonCep.Estado}"
                    );
                }
            }
        }
    }
}

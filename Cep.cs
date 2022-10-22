using System.Text.Json.Serialization;

namespace TelegramBot
{
    public class Cep
    {
        [JsonPropertyName("bairro")]
        public string Bairro {get; set;}

      
    }
}
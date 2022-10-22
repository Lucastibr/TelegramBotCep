using System.Text.Json.Serialization;

namespace TelegramBot
{
    public class PostalCode
    {
        public string Bairro {get; set;}

        public string Cidade {get; set;}

        public string Logradouro {get; set;}

        public string Estado {get; set;}

        public string Cep {get; set;}

        public string GetFullAddress()
        {
            string fullAddress;

            var estado = EnumHelper.GetValueFromDescription<States>(Estado);

            fullAddress = $"O Endereço completo do Cep {Cep} é: \n" +
                $"Rua : {Logradouro} \n Bairro : {Bairro} \n Cidade : {Cidade} \n Estado : {estado}";

            return fullAddress;
        }


    }
}
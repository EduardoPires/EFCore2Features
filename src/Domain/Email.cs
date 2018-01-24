using System.Text.RegularExpressions;

namespace Domain
{
    public class Email
    {
        public string Endereco { get; private set; }

        public Email(string endereco)
        {
            Endereco = endereco;
        }

        // Para o EF
        protected Email() { }

        public bool Validar()
        {
            return Regex.IsMatch(Endereco, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);
        }
    }
}

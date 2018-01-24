using System;

namespace Domain
{
    public class Cliente
    {
        public Guid Id { get; private set; }
        public string Nome { get; private set; }
        public Email Email { get; private set; }
        public CPF CPF { get; private set; }

        public Cliente(string nome, string email, string cpfNumero, DateTime cpfDataEmissao)
        {
            Id = Guid.NewGuid();
            Nome = nome;

            if(string.IsNullOrEmpty(Nome))
                throw new Exception("Nome em branco! Não é possível criar instância de Cliente");

            if (!AtribuirCpf(cpfNumero, cpfDataEmissao)) 
                throw new Exception("CPF Inválido! Não é possível criar instância de Cliente");

            if(!AtribuirEmail(email))
                throw new Exception("E-mail Inválido! Não é possível criar instância de Cliente");
        }

        // Para o EF
        protected Cliente() { }

        private bool AtribuirCpf(string cpfNumero, DateTime cpfDataEmissao)
        {
            var cpf = new CPF(cpfNumero, cpfDataEmissao);
            if (!cpf.Validar()) return false;

            CPF = cpf;
            return true;
        }

        private bool AtribuirEmail(string enderecoEmail)
        {
            var email = new Email(enderecoEmail);
            if (!email.Validar()) return false;

            Email = email;
            return true;
        }

        public bool EhValido()
        {
            if (string.IsNullOrEmpty(Nome))
                return false;

            if (!Email.Validar())
                return false;

            if (!CPF.Validar())
                return false;

            return true;
        }
    }
}
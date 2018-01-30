using System;
using System.Linq;
using Bogus;
using Bogus.DataSets;
using Bogus.Extensions.Brazil;
using Data;
using Domain;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests
{
    public class EFCore2Tests
    {
        [Fact(DisplayName = "Nova instância válida")]
        [Trait("Category","Testes de Cliente")]
        public void Cliente_NovaInstancia_DeveRetornarComSucesso()
        {
            var cliente = GerarClienteValido();
            Assert.True(cliente.EhValido());
        }

        [Fact(DisplayName = "Nova instância com CPF inválido")]
        [Trait("Category", "Testes de Cliente")]
        public void Cliente_NovaInstanciaCPFInvalido_DeveRetornarComErro()
        {
            var exception = Assert.Throws<Exception>(() => new Cliente("Nome", "email@teste.com", "12121212121", DateTime.Now));
            Assert.Equal("CPF Inválido! Não é possível criar instância de Cliente", exception.Message);
        }

        [Fact(DisplayName = "Nova instância com E-mail inválido")]
        [Trait("Category", "Testes de Cliente")]
        public void Cliente_NovaInstanciaEmailInvalido_DeveRetornarComErro()
        {
            var exception = Assert.Throws<Exception>(() => new Cliente("Nome", "email2.com", "90446896730", DateTime.Now));
            Assert.Equal("E-mail Inválido! Não é possível criar instância de Cliente", exception.Message);
        }

        [Fact(DisplayName = "Nova instância com Nome inválido")]
        [Trait("Category", "Testes de Cliente")]
        public void Cliente_NovaInstanciaNomeInvalido_DeveRetornarComErro()
        {
            var exception = Assert.Throws<Exception>(() => new Cliente("", "email@teste.com", "90446896730", DateTime.Now));
            Assert.Equal("Nome em branco! Não é possível criar instância de Cliente", exception.Message);
        }

        [Fact(DisplayName = "Adicionar novo Cliente no banco")]
        [Trait("Category", "Testes de Cliente")]
        public void Cliente_AdicionarNovo_DeveRetornarComSucesso()
        {
            var db = new EFCore2FeaturesContext();
            var cliente = db.Clientes.Add(GerarClienteValido());

            Assert.True(cliente.Entity.EhValido());
            Assert.Equal(1, db.SaveChanges());
        }

        [Fact(DisplayName = "Obter Cliente do banco")]
        [Trait("Category", "Testes de Cliente")]
        public void Cliente_ObterClienteBanco_DeveRetornarComSucesso()
        {
            var db = new EFCore2FeaturesContext();
            var cliente = db.Clientes.FirstOrDefault();

            Assert.NotNull(cliente);
            Assert.True(cliente.EhValido());
        }

        [Fact(DisplayName = "Atualizar CPF Cliente")]
        [Trait("Category", "Testes de Cliente")]
        public void Cliente_AtualizarCPFCliente_DeveRetornarNovoCPF()
        {
            Cliente cliente;
            using (var db = new EFCore2FeaturesContext())
            {
                cliente = db.Clientes.Add(GerarClienteValido()).Entity;
                db.SaveChanges();
            }

            Cliente clienteAlterado;
            var novoCpf = new CPF("44961624403", DateTime.Now.AddYears(-18));

            // Simulando novo request (e evitando o problema de tracking)
            using (var db = new EFCore2FeaturesContext())
            {
                cliente.AtribuirCpf(novoCpf.Numero, novoCpf.DataEmissao);
                db.Clientes.Update(cliente);
                db.SaveChanges();

                clienteAlterado = db.Clientes.Find(cliente.Id);
            }

            Assert.True(clienteAlterado.EhValido());
            Assert.Equal(novoCpf.Numero, clienteAlterado.CPF.Numero);
            Assert.Equal(novoCpf.DataEmissao, clienteAlterado.CPF.DataEmissao);
        }

        private static Cliente GerarClienteValido()
        {
            var cliente = new Faker<Cliente>("pt_BR")
                .CustomInstantiator(f => new Cliente(
                    f.Name.FullName(Name.Gender.Male),
                    f.Internet.Email().ToLower(),
                    f.Person.Cpf().Replace(".", "").Replace("-", ""),
                    DateTime.Now.AddYears(-18)));

            return cliente;
        }
    }
}

using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data
{
    public class ClienteMap : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.OwnsOne(c => c.CPF, cpf =>
            {
                cpf.Property(c => c.Numero)
                    .IsRequired()
                    .HasColumnName("CPF")
                    .HasColumnType("varchar(11)");

                cpf.Property(c => c.DataEmissao)
                    .IsRequired()
                    .HasColumnName("CPFDataEmissao");
            });

            builder.OwnsOne(c => c.Email, email =>
            {
                email.Property(c => c.Endereco)
                    .IsRequired()
                    .HasColumnName("Email")
                    .HasColumnType("varchar(150)");
            });

            builder.ToTable("Clientes");
        }
    }
}
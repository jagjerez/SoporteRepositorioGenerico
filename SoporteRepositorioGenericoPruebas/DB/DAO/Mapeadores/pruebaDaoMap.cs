using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace SoporteRepositorioGenericoPruebas.DB.DAO.Mapeadores
{
    public class pruebaDaoMap : IEntityTypeConfiguration<pruebaDao>
    {
        public void Configure(EntityTypeBuilder<pruebaDao> builder)
        {
            builder.HasKey(d => d.id);
            builder.Property(d => d.id)
                .HasColumnName("id")
                .IsRequired();
            builder.Property(d => d.valor)
                .HasColumnName("valor")
                .IsRequired();
            builder.ToTable("pruebas");
        }
    }
}

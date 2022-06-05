﻿// <auto-generated />
using MateMachine.CurrencyConverter.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MateMachine.CurrencyConverter.Data.Migrations
{
    [DbContext(typeof(CurrencyConverterDbContext))]
    partial class CurrencyConverterDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("MateMachine.CurrencyConverter.Data.Entities.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("MateMachine.CurrencyConverter.Data.Entities.CurrenyExchangeRate", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<double>("ExchangeRate")
                        .HasColumnType("float");

                    b.Property<int>("FromCurrencyId")
                        .HasColumnType("int");

                    b.Property<int>("ToCurrencyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FromCurrencyId");

                    b.HasIndex("ToCurrencyId");

                    b.ToTable("ExchangeRates");
                });

            modelBuilder.Entity("MateMachine.CurrencyConverter.Data.Entities.CurrenyExchangeRate", b =>
                {
                    b.HasOne("MateMachine.CurrencyConverter.Data.Entities.Currency", "FromCurrency")
                        .WithMany()
                        .HasForeignKey("FromCurrencyId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("MateMachine.CurrencyConverter.Data.Entities.Currency", "ToCurrency")
                        .WithMany()
                        .HasForeignKey("ToCurrencyId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("FromCurrency");

                    b.Navigation("ToCurrency");
                });
#pragma warning restore 612, 618
        }
    }
}

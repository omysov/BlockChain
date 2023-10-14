﻿// <auto-generated />
using BlockChainTest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BlockChainTest.Migrations
{
    [DbContext(typeof(AppBlockChainContext))]
    [Migration("20231012081134_AddTableBCModel")]
    partial class AddTableBCModel
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.12");

            modelBuilder.Entity("BlockChainTest.ModelDb.BlockchainModel", b =>
                {
                    b.Property<byte[]>("key")
                        .HasColumnType("BLOB");

                    b.Property<byte[]>("value")
                        .IsRequired()
                        .HasColumnType("BLOB");

                    b.HasKey("key");

                    b.ToTable("BlockchainModel");
                });
#pragma warning restore 612, 618
        }
    }
}
﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Infrastructure.Data.Migrations
{
    [DbContext(typeof(ContractContext))]
    partial class ContractContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ContractBC.ContractAggregate.Contract", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("Completed")
                        .HasColumnType("bit");

                    b.Property<DateOnly>("CompletedDate")
                        .HasColumnType("date");

                    b.Property<string>("ContractNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CurrentVersionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("DateInitiated")
                        .HasColumnType("date");

                    b.Property<Guid>("FinalVersionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("Fullfilled")
                        .HasColumnType("date");

                    b.HasKey("Id");

                    b.ToTable("Contracts");
                });

            modelBuilder.Entity("ContractBC.ContractAggregate.ContractVersion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateOnly>("AcceptanceDeadline")
                        .HasColumnType("date");

                    b.Property<bool>("Accepted")
                        .HasColumnType("bit");

                    b.Property<Guid>("ContractId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateSentToAuthors")
                        .HasColumnType("datetime2");

                    b.Property<string>("ModificationDetails")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ModificationReason")
                        .HasColumnType("int");

                    b.Property<string>("WorkingTitle")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("_hasRevisedSpecSet")
                        .HasColumnType("bit");

                    b.ComplexProperty<Dictionary<string, object>>("Specs", "ContractBC.ContractAggregate.ContractVersion.Specs#SpecificationSet", b1 =>
                        {
                            b1.Property<int>("AdvanceAmountUSD")
                                .HasColumnType("int");

                            b1.Property<bool>("AuthorAvailableForPR")
                                .HasColumnType("bit");

                            b1.Property<int>("DigitalRoyaltyPct")
                                .HasColumnType("int");

                            b1.Property<int>("HardCoverRoyaltyPct")
                                .HasColumnType("int");

                            b1.Property<decimal>("PriceForAddlAuthorCopiesUSD")
                                .HasColumnType("decimal(18,2)");

                            b1.Property<int>("PromoCopiesForAuthor")
                                .HasColumnType("int");

                            b1.Property<bool>("PublicityProvided")
                                .HasColumnType("bit");

                            b1.Property<int>("SoftCoverRoyaltyPct")
                                .HasColumnType("int");

                            b1.Property<int>("TranslationRoyaltyUSD")
                                .HasColumnType("int");
                        });

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.ToTable("ContractVersions", (string)null);
                });

            modelBuilder.Entity("ContractBC.ContractAggregate.ContractVersion", b =>
                {
                    b.HasOne("ContractBC.ContractAggregate.Contract", null)
                        .WithMany("Versions")
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsMany("ContractBC.ValueObjects.Author", "Authors", b1 =>
                        {
                            b1.Property<Guid>("ContractVersionId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<string>("Email")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Phone")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<bool>("Signed")
                                .HasColumnType("bit");

                            b1.Property<Guid>("SignedAuthorId")
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("ContractVersionId", "Id");

                            b1.ToTable("ContractVersion_Authors", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ContractVersionId");

                            b1.OwnsOne("PublisherSystem.SharedKernel.ValueObjects.PersonName", "Name", b2 =>
                                {
                                    b2.Property<Guid>("AuthorContractVersionId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<int>("AuthorId")
                                        .HasColumnType("int");

                                    b2.Property<string>("FirstName")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)");

                                    b2.Property<string>("LastName")
                                        .IsRequired()
                                        .HasColumnType("nvarchar(max)");

                                    b2.HasKey("AuthorContractVersionId", "AuthorId");

                                    b2.ToTable("ContractVersion_Authors");

                                    b2.WithOwner()
                                        .HasForeignKey("AuthorContractVersionId", "AuthorId");
                                });

                            b1.Navigation("Name")
                                .IsRequired();
                        });

                    b.Navigation("Authors");
                });

            modelBuilder.Entity("ContractBC.ContractAggregate.Contract", b =>
                {
                    b.Navigation("Versions");
                });
#pragma warning restore 612, 618
        }
    }
}
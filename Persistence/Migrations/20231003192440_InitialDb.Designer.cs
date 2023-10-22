﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Persistence.Context;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231003192440_InitialDb")]
    partial class InitialDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.YtChannel", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(26)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Handle")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Name")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<string>("Url")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("YtId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("YtId");

                    b.ToTable("YtChannels", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.YtVideo", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(26)");

                    b.Property<string>("ChannelId")
                        .HasColumnType("nvarchar(26)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<TimeSpan?>("Duration")
                        .HasColumnType("time");

                    b.Property<string>("LanguageCulture")
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Name")
                        .HasMaxLength(150)
                        .HasColumnType("nvarchar(150)");

                    b.Property<bool>("Process")
                        .HasColumnType("bit");

                    b.Property<string>("Url")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("YtId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("ChannelId");

                    b.HasIndex("Name");

                    b.ToTable("YtVideos", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.YtVideoDescription", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(26)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(26)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Process")
                        .HasColumnType("bit");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("YtVideoId")
                        .HasColumnType("nvarchar(26)");

                    b.Property<string>("YtVideoTranscriptionId")
                        .HasColumnType("nvarchar(26)");

                    b.HasKey("Id");

                    b.HasIndex("YtVideoId")
                        .IsUnique()
                        .HasFilter("[YtVideoId] IS NOT NULL");

                    b.HasIndex("YtVideoTranscriptionId")
                        .IsUnique()
                        .HasFilter("[YtVideoTranscriptionId] IS NOT NULL");

                    b.ToTable("YtVideoDescriptions", null, t =>
                        {
                            t.Property("CreatedAt")
                                .HasColumnName("YtVideoDescription_CreatedAt");

                            t.Property("CreatedBy")
                                .HasColumnName("YtVideoDescription_CreatedBy");

                            t.Property("CreatedById")
                                .HasColumnName("YtVideoDescription_CreatedById");
                        });
                });

            modelBuilder.Entity("Domain.Entities.YtVideoFile", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(26)");

                    b.Property<long>("Bytes")
                        .HasColumnType("bigint");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<bool>("Process")
                        .HasColumnType("bit");

                    b.Property<int>("Retries")
                        .HasColumnType("int");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("VideoId")
                        .HasColumnType("nvarchar(26)");

                    b.HasKey("Id");

                    b.HasIndex("VideoId");

                    b.ToTable("YtVideoFiles", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.YtVideoFileWav", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(26)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<bool>("Process")
                        .HasColumnType("bit");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("YtVideoFileId")
                        .HasColumnType("nvarchar(26)");

                    b.HasKey("Id");

                    b.HasIndex("YtVideoFileId")
                        .IsUnique()
                        .HasFilter("[YtVideoFileId] IS NOT NULL");

                    b.ToTable("YtVideoFileWavs", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.YtVideoTag", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(26)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<bool>("Process")
                        .HasColumnType("bit");

                    b.Property<string>("Tags")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("YtVideoId")
                        .HasColumnType("nvarchar(26)");

                    b.Property<string>("YtVideoTranscriptionId")
                        .HasColumnType("nvarchar(26)");

                    b.HasKey("Id");

                    b.HasIndex("YtVideoId")
                        .IsUnique()
                        .HasFilter("[YtVideoId] IS NOT NULL");

                    b.HasIndex("YtVideoTranscriptionId")
                        .IsUnique()
                        .HasFilter("[YtVideoTranscriptionId] IS NOT NULL");

                    b.ToTable("YtVideoTags", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.YtVideoTranscription", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(26)");

                    b.Property<bool>("Deleted")
                        .HasColumnType("bit");

                    b.Property<bool>("Process")
                        .HasColumnType("bit");

                    b.Property<byte[]>("Version")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("rowversion");

                    b.Property<string>("WavFileId")
                        .HasColumnType("nvarchar(26)");

                    b.HasKey("Id");

                    b.HasIndex("WavFileId")
                        .IsUnique()
                        .HasFilter("[WavFileId] IS NOT NULL");

                    b.ToTable("YtVideoTranscriptions", (string)null);
                });

            modelBuilder.Entity("Domain.Entities.YtChannel", b =>
                {
                    b.OwnsOne("Domain.ValueObjects.CreationInfo", "CreationInfo", b1 =>
                        {
                            b1.Property<string>("YtChannelId")
                                .HasColumnType("nvarchar(26)");

                            b1.Property<DateTimeOffset>("CreatedAt")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("CreatedAt");

                            b1.Property<string>("CreatedBy")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("CreatedBy");

                            b1.Property<string>("CreatedById")
                                .IsRequired()
                                .HasColumnType("nvarchar(26)")
                                .HasColumnName("CreatedById");

                            b1.HasKey("YtChannelId");

                            b1.ToTable("YtChannels");

                            b1.WithOwner()
                                .HasForeignKey("YtChannelId");
                        });

                    b.OwnsOne("Domain.ValueObjects.UpdateInfo", "UpdateInfo", b1 =>
                        {
                            b1.Property<string>("YtChannelId")
                                .HasColumnType("nvarchar(26)");

                            b1.Property<DateTimeOffset?>("UpdatedAt")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("UpdatedAt");

                            b1.Property<string>("UpdatedBy")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("UpdatedBy");

                            b1.Property<string>("UpdatedById")
                                .IsRequired()
                                .HasColumnType("nvarchar(26)")
                                .HasColumnName("UpdatedById");

                            b1.HasKey("YtChannelId");

                            b1.ToTable("YtChannels");

                            b1.WithOwner()
                                .HasForeignKey("YtChannelId");
                        });

                    b.Navigation("CreationInfo");

                    b.Navigation("UpdateInfo");
                });

            modelBuilder.Entity("Domain.Entities.YtVideo", b =>
                {
                    b.HasOne("Domain.Entities.YtChannel", "Channel")
                        .WithMany("Videos")
                        .HasForeignKey("ChannelId");

                    b.OwnsOne("Domain.ValueObjects.CreationInfo", "CreationInfo", b1 =>
                        {
                            b1.Property<string>("YtVideoId")
                                .HasColumnType("nvarchar(26)");

                            b1.Property<DateTimeOffset>("CreatedAt")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("CreatedAt");

                            b1.Property<string>("CreatedBy")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("CreatedBy");

                            b1.Property<string>("CreatedById")
                                .IsRequired()
                                .HasColumnType("nvarchar(26)")
                                .HasColumnName("CreatedById");

                            b1.HasKey("YtVideoId");

                            b1.ToTable("YtVideos");

                            b1.WithOwner()
                                .HasForeignKey("YtVideoId");
                        });

                    b.OwnsOne("Domain.ValueObjects.UpdateInfo", "UpdateInfo", b1 =>
                        {
                            b1.Property<string>("YtVideoId")
                                .HasColumnType("nvarchar(26)");

                            b1.Property<DateTimeOffset?>("UpdatedAt")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("UpdatedAt");

                            b1.Property<string>("UpdatedBy")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("UpdatedBy");

                            b1.Property<string>("UpdatedById")
                                .IsRequired()
                                .HasColumnType("nvarchar(26)")
                                .HasColumnName("UpdatedById");

                            b1.HasKey("YtVideoId");

                            b1.ToTable("YtVideos");

                            b1.WithOwner()
                                .HasForeignKey("YtVideoId");
                        });

                    b.Navigation("Channel");

                    b.Navigation("CreationInfo");

                    b.Navigation("UpdateInfo");
                });

            modelBuilder.Entity("Domain.Entities.YtVideoDescription", b =>
                {
                    b.HasOne("Domain.Entities.YtVideo", "YtVideo")
                        .WithOne("Description")
                        .HasForeignKey("Domain.Entities.YtVideoDescription", "YtVideoId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Domain.Entities.YtVideoTranscription", "YtVideoTranscription")
                        .WithOne("Description")
                        .HasForeignKey("Domain.Entities.YtVideoDescription", "YtVideoTranscriptionId");

                    b.OwnsOne("Domain.ValueObjects.CreationInfo", "CreationInfo", b1 =>
                        {
                            b1.Property<string>("YtVideoDescriptionId")
                                .HasColumnType("nvarchar(26)");

                            b1.Property<DateTimeOffset>("CreatedAt")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("CreatedAt");

                            b1.Property<string>("CreatedBy")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("CreatedBy");

                            b1.Property<string>("CreatedById")
                                .IsRequired()
                                .HasColumnType("nvarchar(26)")
                                .HasColumnName("CreatedById");

                            b1.HasKey("YtVideoDescriptionId");

                            b1.ToTable("YtVideoDescriptions");

                            b1.WithOwner()
                                .HasForeignKey("YtVideoDescriptionId");
                        });

                    b.OwnsOne("Domain.ValueObjects.UpdateInfo", "UpdateInfo", b1 =>
                        {
                            b1.Property<string>("YtVideoDescriptionId")
                                .HasColumnType("nvarchar(26)");

                            b1.Property<DateTimeOffset?>("UpdatedAt")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("UpdatedAt");

                            b1.Property<string>("UpdatedBy")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("UpdatedBy");

                            b1.Property<string>("UpdatedById")
                                .IsRequired()
                                .HasColumnType("nvarchar(26)")
                                .HasColumnName("UpdatedById");

                            b1.HasKey("YtVideoDescriptionId");

                            b1.ToTable("YtVideoDescriptions");

                            b1.WithOwner()
                                .HasForeignKey("YtVideoDescriptionId");
                        });

                    b.Navigation("CreationInfo");

                    b.Navigation("UpdateInfo");

                    b.Navigation("YtVideo");

                    b.Navigation("YtVideoTranscription");
                });

            modelBuilder.Entity("Domain.Entities.YtVideoFile", b =>
                {
                    b.HasOne("Domain.Entities.YtVideo", "Video")
                        .WithMany("Files")
                        .HasForeignKey("VideoId");

                    b.OwnsOne("Domain.ValueObjects.CreationInfo", "CreationInfo", b1 =>
                        {
                            b1.Property<string>("YtVideoFileId")
                                .HasColumnType("nvarchar(26)");

                            b1.Property<DateTimeOffset>("CreatedAt")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("CreatedAt");

                            b1.Property<string>("CreatedBy")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("CreatedBy");

                            b1.Property<string>("CreatedById")
                                .IsRequired()
                                .HasColumnType("nvarchar(26)")
                                .HasColumnName("CreatedById");

                            b1.HasKey("YtVideoFileId");

                            b1.ToTable("YtVideoFiles");

                            b1.WithOwner()
                                .HasForeignKey("YtVideoFileId");
                        });

                    b.OwnsOne("Domain.ValueObjects.PathData", "PathData", b1 =>
                        {
                            b1.Property<string>("YtVideoFileId")
                                .HasColumnType("nvarchar(26)");

                            b1.Property<string>("FileExtension")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("nvarchar(10)")
                                .HasColumnName("Path_FileExtension");

                            b1.Property<string>("FileName")
                                .IsRequired()
                                .HasMaxLength(150)
                                .HasColumnType("nvarchar(150)")
                                .HasColumnName("Path_FileName");

                            b1.Property<string>("FullValue")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("MainPath")
                                .IsRequired()
                                .HasMaxLength(150)
                                .HasColumnType("nvarchar(150)")
                                .HasColumnName("Path_MainPath");

                            b1.HasKey("YtVideoFileId");

                            b1.ToTable("YtVideoFiles");

                            b1.WithOwner()
                                .HasForeignKey("YtVideoFileId");
                        });

                    b.OwnsOne("Domain.ValueObjects.UpdateInfo", "UpdateInfo", b1 =>
                        {
                            b1.Property<string>("YtVideoFileId")
                                .HasColumnType("nvarchar(26)");

                            b1.Property<DateTimeOffset?>("UpdatedAt")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("UpdatedAt");

                            b1.Property<string>("UpdatedBy")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("UpdatedBy");

                            b1.Property<string>("UpdatedById")
                                .IsRequired()
                                .HasColumnType("nvarchar(26)")
                                .HasColumnName("UpdatedById");

                            b1.HasKey("YtVideoFileId");

                            b1.ToTable("YtVideoFiles");

                            b1.WithOwner()
                                .HasForeignKey("YtVideoFileId");
                        });

                    b.OwnsOne("Domain.ValueObjects.Quality", "Quality", b1 =>
                        {
                            b1.Property<string>("YtVideoFileId")
                                .HasColumnType("nvarchar(26)");

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("nvarchar(10)")
                                .HasColumnName("Quality");

                            b1.HasKey("YtVideoFileId");

                            b1.ToTable("YtVideoFiles");

                            b1.WithOwner()
                                .HasForeignKey("YtVideoFileId");
                        });

                    b.Navigation("CreationInfo");

                    b.Navigation("PathData");

                    b.Navigation("Quality");

                    b.Navigation("UpdateInfo");

                    b.Navigation("Video");
                });

            modelBuilder.Entity("Domain.Entities.YtVideoFileWav", b =>
                {
                    b.HasOne("Domain.Entities.YtVideoFile", "YtVideoFile")
                        .WithOne("WavFile")
                        .HasForeignKey("Domain.Entities.YtVideoFileWav", "YtVideoFileId");

                    b.OwnsOne("Domain.ValueObjects.CreationInfo", "CreationInfo", b1 =>
                        {
                            b1.Property<string>("YtVideoFileWavId")
                                .HasColumnType("nvarchar(26)");

                            b1.Property<DateTimeOffset>("CreatedAt")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("CreatedAt");

                            b1.Property<string>("CreatedBy")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("CreatedBy");

                            b1.Property<string>("CreatedById")
                                .IsRequired()
                                .HasColumnType("nvarchar(26)")
                                .HasColumnName("CreatedById");

                            b1.HasKey("YtVideoFileWavId");

                            b1.ToTable("YtVideoFileWavs");

                            b1.WithOwner()
                                .HasForeignKey("YtVideoFileWavId");
                        });

                    b.OwnsOne("Domain.ValueObjects.PathData", "PathData", b1 =>
                        {
                            b1.Property<string>("YtVideoFileWavId")
                                .HasColumnType("nvarchar(26)");

                            b1.Property<string>("FileExtension")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("nvarchar(10)")
                                .HasColumnName("Path_FileExtension");

                            b1.Property<string>("FileName")
                                .IsRequired()
                                .HasMaxLength(150)
                                .HasColumnType("nvarchar(150)")
                                .HasColumnName("Path_FileName");

                            b1.Property<string>("FullValue")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("MainPath")
                                .IsRequired()
                                .HasMaxLength(150)
                                .HasColumnType("nvarchar(150)")
                                .HasColumnName("Path_MainPath");

                            b1.HasKey("YtVideoFileWavId");

                            b1.ToTable("YtVideoFileWavs");

                            b1.WithOwner()
                                .HasForeignKey("YtVideoFileWavId");
                        });

                    b.OwnsOne("Domain.ValueObjects.UpdateInfo", "UpdateInfo", b1 =>
                        {
                            b1.Property<string>("YtVideoFileWavId")
                                .HasColumnType("nvarchar(26)");

                            b1.Property<DateTimeOffset?>("UpdatedAt")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("UpdatedAt");

                            b1.Property<string>("UpdatedBy")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("UpdatedBy");

                            b1.Property<string>("UpdatedById")
                                .IsRequired()
                                .HasColumnType("nvarchar(26)")
                                .HasColumnName("UpdatedById");

                            b1.HasKey("YtVideoFileWavId");

                            b1.ToTable("YtVideoFileWavs");

                            b1.WithOwner()
                                .HasForeignKey("YtVideoFileWavId");
                        });

                    b.OwnsOne("Domain.ValueObjects.Language", "Language", b1 =>
                        {
                            b1.Property<string>("YtVideoFileWavId")
                                .HasColumnType("nvarchar(26)");

                            b1.Property<string>("CultureValue")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("nvarchar(10)")
                                .HasColumnName("Language_CultureValue");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("Language_Name");

                            b1.HasKey("YtVideoFileWavId");

                            b1.ToTable("YtVideoFileWavs");

                            b1.WithOwner()
                                .HasForeignKey("YtVideoFileWavId");
                        });

                    b.Navigation("CreationInfo");

                    b.Navigation("Language");

                    b.Navigation("PathData");

                    b.Navigation("UpdateInfo");

                    b.Navigation("YtVideoFile");
                });

            modelBuilder.Entity("Domain.Entities.YtVideoTag", b =>
                {
                    b.HasOne("Domain.Entities.YtVideo", "YtVideo")
                        .WithOne("Tag")
                        .HasForeignKey("Domain.Entities.YtVideoTag", "YtVideoId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("Domain.Entities.YtVideoTranscription", "YtVideoTranscription")
                        .WithOne("Tag")
                        .HasForeignKey("Domain.Entities.YtVideoTag", "YtVideoTranscriptionId");

                    b.OwnsOne("Domain.ValueObjects.CreationInfo", "CreationInfo", b1 =>
                        {
                            b1.Property<string>("YtVideoTagId")
                                .HasColumnType("nvarchar(26)");

                            b1.Property<DateTimeOffset>("CreatedAt")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("CreatedAt");

                            b1.Property<string>("CreatedBy")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("CreatedBy");

                            b1.Property<string>("CreatedById")
                                .IsRequired()
                                .HasColumnType("nvarchar(26)")
                                .HasColumnName("CreatedById");

                            b1.HasKey("YtVideoTagId");

                            b1.ToTable("YtVideoTags");

                            b1.WithOwner()
                                .HasForeignKey("YtVideoTagId");
                        });

                    b.OwnsOne("Domain.ValueObjects.UpdateInfo", "UpdateInfo", b1 =>
                        {
                            b1.Property<string>("YtVideoTagId")
                                .HasColumnType("nvarchar(26)");

                            b1.Property<DateTimeOffset?>("UpdatedAt")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("UpdatedAt");

                            b1.Property<string>("UpdatedBy")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("UpdatedBy");

                            b1.Property<string>("UpdatedById")
                                .IsRequired()
                                .HasColumnType("nvarchar(26)")
                                .HasColumnName("UpdatedById");

                            b1.HasKey("YtVideoTagId");

                            b1.ToTable("YtVideoTags");

                            b1.WithOwner()
                                .HasForeignKey("YtVideoTagId");
                        });

                    b.Navigation("CreationInfo");

                    b.Navigation("UpdateInfo");

                    b.Navigation("YtVideo");

                    b.Navigation("YtVideoTranscription");
                });

            modelBuilder.Entity("Domain.Entities.YtVideoTranscription", b =>
                {
                    b.HasOne("Domain.Entities.YtVideoFileWav", "WavFile")
                        .WithOne("YtVideoTranscription")
                        .HasForeignKey("Domain.Entities.YtVideoTranscription", "WavFileId");

                    b.OwnsOne("Domain.ValueObjects.CreationInfo", "CreationInfo", b1 =>
                        {
                            b1.Property<string>("YtVideoTranscriptionId")
                                .HasColumnType("nvarchar(26)");

                            b1.Property<DateTimeOffset>("CreatedAt")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("CreatedAt");

                            b1.Property<string>("CreatedBy")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("CreatedBy");

                            b1.Property<string>("CreatedById")
                                .IsRequired()
                                .HasColumnType("nvarchar(26)")
                                .HasColumnName("CreatedById");

                            b1.HasKey("YtVideoTranscriptionId");

                            b1.ToTable("YtVideoTranscriptions");

                            b1.WithOwner()
                                .HasForeignKey("YtVideoTranscriptionId");
                        });

                    b.OwnsOne("Domain.ValueObjects.PathData", "PathData", b1 =>
                        {
                            b1.Property<string>("YtVideoTranscriptionId")
                                .HasColumnType("nvarchar(26)");

                            b1.Property<string>("FileExtension")
                                .IsRequired()
                                .HasMaxLength(10)
                                .HasColumnType("nvarchar(10)")
                                .HasColumnName("Path_FileExtension");

                            b1.Property<string>("FileName")
                                .IsRequired()
                                .HasMaxLength(150)
                                .HasColumnType("nvarchar(150)")
                                .HasColumnName("Path_FileName");

                            b1.Property<string>("FullValue")
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("MainPath")
                                .IsRequired()
                                .HasMaxLength(150)
                                .HasColumnType("nvarchar(150)")
                                .HasColumnName("Path_MainPath");

                            b1.HasKey("YtVideoTranscriptionId");

                            b1.ToTable("YtVideoTranscriptions");

                            b1.WithOwner()
                                .HasForeignKey("YtVideoTranscriptionId");
                        });

                    b.OwnsOne("Domain.ValueObjects.UpdateInfo", "UpdateInfo", b1 =>
                        {
                            b1.Property<string>("YtVideoTranscriptionId")
                                .HasColumnType("nvarchar(26)");

                            b1.Property<DateTimeOffset?>("UpdatedAt")
                                .HasColumnType("datetimeoffset")
                                .HasColumnName("UpdatedAt");

                            b1.Property<string>("UpdatedBy")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("nvarchar(100)")
                                .HasColumnName("UpdatedBy");

                            b1.Property<string>("UpdatedById")
                                .IsRequired()
                                .HasColumnType("nvarchar(26)")
                                .HasColumnName("UpdatedById");

                            b1.HasKey("YtVideoTranscriptionId");

                            b1.ToTable("YtVideoTranscriptions");

                            b1.WithOwner()
                                .HasForeignKey("YtVideoTranscriptionId");
                        });

                    b.Navigation("CreationInfo");

                    b.Navigation("PathData");

                    b.Navigation("UpdateInfo");

                    b.Navigation("WavFile");
                });

            modelBuilder.Entity("Domain.Entities.YtChannel", b =>
                {
                    b.Navigation("Videos");
                });

            modelBuilder.Entity("Domain.Entities.YtVideo", b =>
                {
                    b.Navigation("Description");

                    b.Navigation("Files");

                    b.Navigation("Tag");
                });

            modelBuilder.Entity("Domain.Entities.YtVideoFile", b =>
                {
                    b.Navigation("WavFile");
                });

            modelBuilder.Entity("Domain.Entities.YtVideoFileWav", b =>
                {
                    b.Navigation("YtVideoTranscription");
                });

            modelBuilder.Entity("Domain.Entities.YtVideoTranscription", b =>
                {
                    b.Navigation("Description");

                    b.Navigation("Tag");
                });
#pragma warning restore 612, 618
        }
    }
}

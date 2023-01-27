﻿#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion

// <auto-generated />
using System;

using Duende.IdentityServer.EntityFramework.DbContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Aegis.Persistence.Migrations.IdentityServer.Configuration
{
	[DbContext(typeof(ConfigurationDbContext))]
	partial class ConfigurationDbContextModelSnapshot : ModelSnapshot
	{
		protected override void BuildModel(ModelBuilder modelBuilder)
		{
#pragma warning disable 612, 618
			modelBuilder
				.HasDefaultSchema("aegis-ids")
				.HasAnnotation("ProductVersion", "7.0.2")
				.HasAnnotation("Relational:MaxIdentifierLength", 63);

			NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResource", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("integer");

					NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

					b.Property<string>("AllowedAccessTokenSigningAlgorithms")
						.HasMaxLength(100)
						.HasColumnType("character varying(100)");

					b.Property<DateTime>("Created")
						.HasColumnType("timestamp with time zone");

					b.Property<string>("Description")
						.HasMaxLength(1000)
						.HasColumnType("character varying(1000)");

					b.Property<string>("DisplayName")
						.HasMaxLength(200)
						.HasColumnType("character varying(200)");

					b.Property<bool>("Enabled")
						.HasColumnType("boolean");

					b.Property<DateTime?>("LastAccessed")
						.HasColumnType("timestamp with time zone");

					b.Property<string>("Name")
						.IsRequired()
						.HasMaxLength(200)
						.HasColumnType("character varying(200)");

					b.Property<bool>("NonEditable")
						.HasColumnType("boolean");

					b.Property<bool>("RequireResourceIndicator")
						.HasColumnType("boolean");

					b.Property<bool>("ShowInDiscoveryDocument")
						.HasColumnType("boolean");

					b.Property<DateTime?>("Updated")
						.HasColumnType("timestamp with time zone");

					b.HasKey("Id");

					b.HasIndex("Name")
						.IsUnique();

					b.ToTable("ApiResources", "aegis-ids");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceClaim", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("integer");

					NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

					b.Property<int>("ApiResourceId")
						.HasColumnType("integer");

					b.Property<string>("Type")
						.IsRequired()
						.HasMaxLength(200)
						.HasColumnType("character varying(200)");

					b.HasKey("Id");

					b.HasIndex("ApiResourceId", "Type")
						.IsUnique();

					b.ToTable("ApiResourceClaims", "aegis-ids");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceProperty", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("integer");

					NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

					b.Property<int>("ApiResourceId")
						.HasColumnType("integer");

					b.Property<string>("Key")
						.IsRequired()
						.HasMaxLength(250)
						.HasColumnType("character varying(250)");

					b.Property<string>("Value")
						.IsRequired()
						.HasMaxLength(2000)
						.HasColumnType("character varying(2000)");

					b.HasKey("Id");

					b.HasIndex("ApiResourceId", "Key")
						.IsUnique();

					b.ToTable("ApiResourceProperties", "aegis-ids");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceScope", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("integer");

					NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

					b.Property<int>("ApiResourceId")
						.HasColumnType("integer");

					b.Property<string>("Scope")
						.IsRequired()
						.HasMaxLength(200)
						.HasColumnType("character varying(200)");

					b.HasKey("Id");

					b.HasIndex("ApiResourceId", "Scope")
						.IsUnique();

					b.ToTable("ApiResourceScopes", "aegis-ids");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceSecret", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("integer");

					NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

					b.Property<int>("ApiResourceId")
						.HasColumnType("integer");

					b.Property<DateTime>("Created")
						.HasColumnType("timestamp with time zone");

					b.Property<string>("Description")
						.HasMaxLength(1000)
						.HasColumnType("character varying(1000)");

					b.Property<DateTime?>("Expiration")
						.HasColumnType("timestamp with time zone");

					b.Property<string>("Type")
						.IsRequired()
						.HasMaxLength(250)
						.HasColumnType("character varying(250)");

					b.Property<string>("Value")
						.IsRequired()
						.HasMaxLength(4000)
						.HasColumnType("character varying(4000)");

					b.HasKey("Id");

					b.HasIndex("ApiResourceId");

					b.ToTable("ApiResourceSecrets", "aegis-ids");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiScope", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("integer");

					NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

					b.Property<DateTime>("Created")
						.HasColumnType("timestamp with time zone");

					b.Property<string>("Description")
						.HasMaxLength(1000)
						.HasColumnType("character varying(1000)");

					b.Property<string>("DisplayName")
						.HasMaxLength(200)
						.HasColumnType("character varying(200)");

					b.Property<bool>("Emphasize")
						.HasColumnType("boolean");

					b.Property<bool>("Enabled")
						.HasColumnType("boolean");

					b.Property<DateTime?>("LastAccessed")
						.HasColumnType("timestamp with time zone");

					b.Property<string>("Name")
						.IsRequired()
						.HasMaxLength(200)
						.HasColumnType("character varying(200)");

					b.Property<bool>("NonEditable")
						.HasColumnType("boolean");

					b.Property<bool>("Required")
						.HasColumnType("boolean");

					b.Property<bool>("ShowInDiscoveryDocument")
						.HasColumnType("boolean");

					b.Property<DateTime?>("Updated")
						.HasColumnType("timestamp with time zone");

					b.HasKey("Id");

					b.HasIndex("Name")
						.IsUnique();

					b.ToTable("ApiScopes", "aegis-ids");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiScopeClaim", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("integer");

					NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

					b.Property<int>("ScopeId")
						.HasColumnType("integer");

					b.Property<string>("Type")
						.IsRequired()
						.HasMaxLength(200)
						.HasColumnType("character varying(200)");

					b.HasKey("Id");

					b.HasIndex("ScopeId", "Type")
						.IsUnique();

					b.ToTable("ApiScopeClaims", "aegis-ids");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiScopeProperty", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("integer");

					NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

					b.Property<string>("Key")
						.IsRequired()
						.HasMaxLength(250)
						.HasColumnType("character varying(250)");

					b.Property<int>("ScopeId")
						.HasColumnType("integer");

					b.Property<string>("Value")
						.IsRequired()
						.HasMaxLength(2000)
						.HasColumnType("character varying(2000)");

					b.HasKey("Id");

					b.HasIndex("ScopeId", "Key")
						.IsUnique();

					b.ToTable("ApiScopeProperties", "aegis-ids");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.Client", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("integer");

					NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

					b.Property<int>("AbsoluteRefreshTokenLifetime")
						.HasColumnType("integer");

					b.Property<int>("AccessTokenLifetime")
						.HasColumnType("integer");

					b.Property<int>("AccessTokenType")
						.HasColumnType("integer");

					b.Property<bool>("AllowAccessTokensViaBrowser")
						.HasColumnType("boolean");

					b.Property<bool>("AllowOfflineAccess")
						.HasColumnType("boolean");

					b.Property<bool>("AllowPlainTextPkce")
						.HasColumnType("boolean");

					b.Property<bool>("AllowRememberConsent")
						.HasColumnType("boolean");

					b.Property<string>("AllowedIdentityTokenSigningAlgorithms")
						.HasMaxLength(100)
						.HasColumnType("character varying(100)");

					b.Property<bool>("AlwaysIncludeUserClaimsInIdToken")
						.HasColumnType("boolean");

					b.Property<bool>("AlwaysSendClientClaims")
						.HasColumnType("boolean");

					b.Property<int>("AuthorizationCodeLifetime")
						.HasColumnType("integer");

					b.Property<bool>("BackChannelLogoutSessionRequired")
						.HasColumnType("boolean");

					b.Property<string>("BackChannelLogoutUri")
						.HasMaxLength(2000)
						.HasColumnType("character varying(2000)");

					b.Property<int?>("CibaLifetime")
						.HasColumnType("integer");

					b.Property<string>("ClientClaimsPrefix")
						.HasMaxLength(200)
						.HasColumnType("character varying(200)");

					b.Property<string>("ClientId")
						.IsRequired()
						.HasMaxLength(200)
						.HasColumnType("character varying(200)");

					b.Property<string>("ClientName")
						.HasMaxLength(200)
						.HasColumnType("character varying(200)");

					b.Property<string>("ClientUri")
						.HasMaxLength(2000)
						.HasColumnType("character varying(2000)");

					b.Property<int?>("ConsentLifetime")
						.HasColumnType("integer");

					b.Property<bool?>("CoordinateLifetimeWithUserSession")
						.HasColumnType("boolean");

					b.Property<DateTime>("Created")
						.HasColumnType("timestamp with time zone");

					b.Property<string>("Description")
						.HasMaxLength(1000)
						.HasColumnType("character varying(1000)");

					b.Property<int>("DeviceCodeLifetime")
						.HasColumnType("integer");

					b.Property<bool>("EnableLocalLogin")
						.HasColumnType("boolean");

					b.Property<bool>("Enabled")
						.HasColumnType("boolean");

					b.Property<bool>("FrontChannelLogoutSessionRequired")
						.HasColumnType("boolean");

					b.Property<string>("FrontChannelLogoutUri")
						.HasMaxLength(2000)
						.HasColumnType("character varying(2000)");

					b.Property<int>("IdentityTokenLifetime")
						.HasColumnType("integer");

					b.Property<bool>("IncludeJwtId")
						.HasColumnType("boolean");

					b.Property<DateTime?>("LastAccessed")
						.HasColumnType("timestamp with time zone");

					b.Property<string>("LogoUri")
						.HasMaxLength(2000)
						.HasColumnType("character varying(2000)");

					b.Property<bool>("NonEditable")
						.HasColumnType("boolean");

					b.Property<string>("PairWiseSubjectSalt")
						.HasMaxLength(200)
						.HasColumnType("character varying(200)");

					b.Property<int?>("PollingInterval")
						.HasColumnType("integer");

					b.Property<string>("ProtocolType")
						.IsRequired()
						.HasMaxLength(200)
						.HasColumnType("character varying(200)");

					b.Property<int>("RefreshTokenExpiration")
						.HasColumnType("integer");

					b.Property<int>("RefreshTokenUsage")
						.HasColumnType("integer");

					b.Property<bool>("RequireClientSecret")
						.HasColumnType("boolean");

					b.Property<bool>("RequireConsent")
						.HasColumnType("boolean");

					b.Property<bool>("RequirePkce")
						.HasColumnType("boolean");

					b.Property<bool>("RequireRequestObject")
						.HasColumnType("boolean");

					b.Property<int>("SlidingRefreshTokenLifetime")
						.HasColumnType("integer");

					b.Property<bool>("UpdateAccessTokenClaimsOnRefresh")
						.HasColumnType("boolean");

					b.Property<DateTime?>("Updated")
						.HasColumnType("timestamp with time zone");

					b.Property<string>("UserCodeType")
						.HasMaxLength(100)
						.HasColumnType("character varying(100)");

					b.Property<int?>("UserSsoLifetime")
						.HasColumnType("integer");

					b.HasKey("Id");

					b.HasIndex("ClientId")
						.IsUnique();

					b.ToTable("Clients", "aegis-ids");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientClaim", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("integer");

					NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

					b.Property<int>("ClientId")
						.HasColumnType("integer");

					b.Property<string>("Type")
						.IsRequired()
						.HasMaxLength(250)
						.HasColumnType("character varying(250)");

					b.Property<string>("Value")
						.IsRequired()
						.HasMaxLength(250)
						.HasColumnType("character varying(250)");

					b.HasKey("Id");

					b.HasIndex("ClientId", "Type", "Value")
						.IsUnique();

					b.ToTable("ClientClaims", "aegis-ids");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientCorsOrigin", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("integer");

					NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

					b.Property<int>("ClientId")
						.HasColumnType("integer");

					b.Property<string>("Origin")
						.IsRequired()
						.HasMaxLength(150)
						.HasColumnType("character varying(150)");

					b.HasKey("Id");

					b.HasIndex("ClientId", "Origin")
						.IsUnique();

					b.ToTable("ClientCorsOrigins", "aegis-ids");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientGrantType", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("integer");

					NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

					b.Property<int>("ClientId")
						.HasColumnType("integer");

					b.Property<string>("GrantType")
						.IsRequired()
						.HasMaxLength(250)
						.HasColumnType("character varying(250)");

					b.HasKey("Id");

					b.HasIndex("ClientId", "GrantType")
						.IsUnique();

					b.ToTable("ClientGrantTypes", "aegis-ids");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientIdPRestriction", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("integer");

					NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

					b.Property<int>("ClientId")
						.HasColumnType("integer");

					b.Property<string>("Provider")
						.IsRequired()
						.HasMaxLength(200)
						.HasColumnType("character varying(200)");

					b.HasKey("Id");

					b.HasIndex("ClientId", "Provider")
						.IsUnique();

					b.ToTable("ClientIdPRestrictions", "aegis-ids");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientPostLogoutRedirectUri", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("integer");

					NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

					b.Property<int>("ClientId")
						.HasColumnType("integer");

					b.Property<string>("PostLogoutRedirectUri")
						.IsRequired()
						.HasMaxLength(400)
						.HasColumnType("character varying(400)");

					b.HasKey("Id");

					b.HasIndex("ClientId", "PostLogoutRedirectUri")
						.IsUnique();

					b.ToTable("ClientPostLogoutRedirectUris", "aegis-ids");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientProperty", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("integer");

					NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

					b.Property<int>("ClientId")
						.HasColumnType("integer");

					b.Property<string>("Key")
						.IsRequired()
						.HasMaxLength(250)
						.HasColumnType("character varying(250)");

					b.Property<string>("Value")
						.IsRequired()
						.HasMaxLength(2000)
						.HasColumnType("character varying(2000)");

					b.HasKey("Id");

					b.HasIndex("ClientId", "Key")
						.IsUnique();

					b.ToTable("ClientProperties", "aegis-ids");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientRedirectUri", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("integer");

					NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

					b.Property<int>("ClientId")
						.HasColumnType("integer");

					b.Property<string>("RedirectUri")
						.IsRequired()
						.HasMaxLength(400)
						.HasColumnType("character varying(400)");

					b.HasKey("Id");

					b.HasIndex("ClientId", "RedirectUri")
						.IsUnique();

					b.ToTable("ClientRedirectUris", "aegis-ids");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientScope", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("integer");

					NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

					b.Property<int>("ClientId")
						.HasColumnType("integer");

					b.Property<string>("Scope")
						.IsRequired()
						.HasMaxLength(200)
						.HasColumnType("character varying(200)");

					b.HasKey("Id");

					b.HasIndex("ClientId", "Scope")
						.IsUnique();

					b.ToTable("ClientScopes", "aegis-ids");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientSecret", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("integer");

					NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

					b.Property<int>("ClientId")
						.HasColumnType("integer");

					b.Property<DateTime>("Created")
						.HasColumnType("timestamp with time zone");

					b.Property<string>("Description")
						.HasMaxLength(2000)
						.HasColumnType("character varying(2000)");

					b.Property<DateTime?>("Expiration")
						.HasColumnType("timestamp with time zone");

					b.Property<string>("Type")
						.IsRequired()
						.HasMaxLength(250)
						.HasColumnType("character varying(250)");

					b.Property<string>("Value")
						.IsRequired()
						.HasMaxLength(4000)
						.HasColumnType("character varying(4000)");

					b.HasKey("Id");

					b.HasIndex("ClientId");

					b.ToTable("ClientSecrets", "aegis-ids");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityProvider", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("integer");

					NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

					b.Property<DateTime>("Created")
						.HasColumnType("timestamp with time zone");

					b.Property<string>("DisplayName")
						.HasMaxLength(200)
						.HasColumnType("character varying(200)");

					b.Property<bool>("Enabled")
						.HasColumnType("boolean");

					b.Property<DateTime?>("LastAccessed")
						.HasColumnType("timestamp with time zone");

					b.Property<bool>("NonEditable")
						.HasColumnType("boolean");

					b.Property<string>("Properties")
						.HasColumnType("text");

					b.Property<string>("Scheme")
						.IsRequired()
						.HasMaxLength(200)
						.HasColumnType("character varying(200)");

					b.Property<string>("Type")
						.IsRequired()
						.HasMaxLength(20)
						.HasColumnType("character varying(20)");

					b.Property<DateTime?>("Updated")
						.HasColumnType("timestamp with time zone");

					b.HasKey("Id");

					b.HasIndex("Scheme")
						.IsUnique();

					b.ToTable("IdentityProviders", "aegis-ids");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityResource", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("integer");

					NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

					b.Property<DateTime>("Created")
						.HasColumnType("timestamp with time zone");

					b.Property<string>("Description")
						.HasMaxLength(1000)
						.HasColumnType("character varying(1000)");

					b.Property<string>("DisplayName")
						.HasMaxLength(200)
						.HasColumnType("character varying(200)");

					b.Property<bool>("Emphasize")
						.HasColumnType("boolean");

					b.Property<bool>("Enabled")
						.HasColumnType("boolean");

					b.Property<string>("Name")
						.IsRequired()
						.HasMaxLength(200)
						.HasColumnType("character varying(200)");

					b.Property<bool>("NonEditable")
						.HasColumnType("boolean");

					b.Property<bool>("Required")
						.HasColumnType("boolean");

					b.Property<bool>("ShowInDiscoveryDocument")
						.HasColumnType("boolean");

					b.Property<DateTime?>("Updated")
						.HasColumnType("timestamp with time zone");

					b.HasKey("Id");

					b.HasIndex("Name")
						.IsUnique();

					b.ToTable("IdentityResources", "aegis-ids");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityResourceClaim", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("integer");

					NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

					b.Property<int>("IdentityResourceId")
						.HasColumnType("integer");

					b.Property<string>("Type")
						.IsRequired()
						.HasMaxLength(200)
						.HasColumnType("character varying(200)");

					b.HasKey("Id");

					b.HasIndex("IdentityResourceId", "Type")
						.IsUnique();

					b.ToTable("IdentityResourceClaims", "aegis-ids");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityResourceProperty", b =>
				{
					b.Property<int>("Id")
						.ValueGeneratedOnAdd()
						.HasColumnType("integer");

					NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

					b.Property<int>("IdentityResourceId")
						.HasColumnType("integer");

					b.Property<string>("Key")
						.IsRequired()
						.HasMaxLength(250)
						.HasColumnType("character varying(250)");

					b.Property<string>("Value")
						.IsRequired()
						.HasMaxLength(2000)
						.HasColumnType("character varying(2000)");

					b.HasKey("Id");

					b.HasIndex("IdentityResourceId", "Key")
						.IsUnique();

					b.ToTable("IdentityResourceProperties", "aegis-ids");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceClaim", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiResource", "ApiResource")
						.WithMany("UserClaims")
						.HasForeignKey("ApiResourceId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("ApiResource");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceProperty", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiResource", "ApiResource")
						.WithMany("Properties")
						.HasForeignKey("ApiResourceId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("ApiResource");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceScope", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiResource", "ApiResource")
						.WithMany("Scopes")
						.HasForeignKey("ApiResourceId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("ApiResource");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResourceSecret", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiResource", "ApiResource")
						.WithMany("Secrets")
						.HasForeignKey("ApiResourceId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("ApiResource");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiScopeClaim", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiScope", "Scope")
						.WithMany("UserClaims")
						.HasForeignKey("ScopeId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Scope");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiScopeProperty", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.ApiScope", "Scope")
						.WithMany("Properties")
						.HasForeignKey("ScopeId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Scope");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientClaim", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client")
						.WithMany("Claims")
						.HasForeignKey("ClientId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Client");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientCorsOrigin", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client")
						.WithMany("AllowedCorsOrigins")
						.HasForeignKey("ClientId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Client");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientGrantType", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client")
						.WithMany("AllowedGrantTypes")
						.HasForeignKey("ClientId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Client");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientIdPRestriction", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client")
						.WithMany("IdentityProviderRestrictions")
						.HasForeignKey("ClientId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Client");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientPostLogoutRedirectUri", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client")
						.WithMany("PostLogoutRedirectUris")
						.HasForeignKey("ClientId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Client");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientProperty", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client")
						.WithMany("Properties")
						.HasForeignKey("ClientId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Client");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientRedirectUri", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client")
						.WithMany("RedirectUris")
						.HasForeignKey("ClientId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Client");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientScope", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client")
						.WithMany("AllowedScopes")
						.HasForeignKey("ClientId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Client");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ClientSecret", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.Client", "Client")
						.WithMany("ClientSecrets")
						.HasForeignKey("ClientId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("Client");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityResourceClaim", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.IdentityResource", "IdentityResource")
						.WithMany("UserClaims")
						.HasForeignKey("IdentityResourceId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("IdentityResource");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityResourceProperty", b =>
				{
					b.HasOne("Duende.IdentityServer.EntityFramework.Entities.IdentityResource", "IdentityResource")
						.WithMany("Properties")
						.HasForeignKey("IdentityResourceId")
						.OnDelete(DeleteBehavior.Cascade)
						.IsRequired();

					b.Navigation("IdentityResource");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiResource", b =>
				{
					b.Navigation("Properties");

					b.Navigation("Scopes");

					b.Navigation("Secrets");

					b.Navigation("UserClaims");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.ApiScope", b =>
				{
					b.Navigation("Properties");

					b.Navigation("UserClaims");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.Client", b =>
				{
					b.Navigation("AllowedCorsOrigins");

					b.Navigation("AllowedGrantTypes");

					b.Navigation("AllowedScopes");

					b.Navigation("Claims");

					b.Navigation("ClientSecrets");

					b.Navigation("IdentityProviderRestrictions");

					b.Navigation("PostLogoutRedirectUris");

					b.Navigation("Properties");

					b.Navigation("RedirectUris");
				});

			modelBuilder.Entity("Duende.IdentityServer.EntityFramework.Entities.IdentityResource", b =>
				{
					b.Navigation("Properties");

					b.Navigation("UserClaims");
				});
#pragma warning restore 612, 618
		}
	}
}

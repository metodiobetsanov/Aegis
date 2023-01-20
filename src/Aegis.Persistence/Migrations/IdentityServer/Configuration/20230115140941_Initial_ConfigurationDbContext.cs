#nullable disable

namespace Aegis.Persistence.Migrations.IdentityServer.Configuration
{
	using System;

	using Microsoft.EntityFrameworkCore.Migrations;

	using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

	/// <inheritdoc />
	public partial class InitialConfigurationDbContext : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.EnsureSchema(
				name: "aegis-ids");

			migrationBuilder.CreateTable(
				name: "ApiResources",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					Enabled = table.Column<bool>(type: "boolean", nullable: false),
					Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
					DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
					Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
					AllowedAccessTokenSigningAlgorithms = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
					ShowInDiscoveryDocument = table.Column<bool>(type: "boolean", nullable: false),
					RequireResourceIndicator = table.Column<bool>(type: "boolean", nullable: false),
					Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					LastAccessed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					NonEditable = table.Column<bool>(type: "boolean", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ApiResources", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "ApiScopes",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					Enabled = table.Column<bool>(type: "boolean", nullable: false),
					Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
					DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
					Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
					Required = table.Column<bool>(type: "boolean", nullable: false),
					Emphasize = table.Column<bool>(type: "boolean", nullable: false),
					ShowInDiscoveryDocument = table.Column<bool>(type: "boolean", nullable: false),
					Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					LastAccessed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					NonEditable = table.Column<bool>(type: "boolean", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ApiScopes", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "Clients",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					Enabled = table.Column<bool>(type: "boolean", nullable: false),
					ClientId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
					ProtocolType = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
					RequireClientSecret = table.Column<bool>(type: "boolean", nullable: false),
					ClientName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
					Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
					ClientUri = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
					LogoUri = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
					RequireConsent = table.Column<bool>(type: "boolean", nullable: false),
					AllowRememberConsent = table.Column<bool>(type: "boolean", nullable: false),
					AlwaysIncludeUserClaimsInIdToken = table.Column<bool>(type: "boolean", nullable: false),
					RequirePkce = table.Column<bool>(type: "boolean", nullable: false),
					AllowPlainTextPkce = table.Column<bool>(type: "boolean", nullable: false),
					RequireRequestObject = table.Column<bool>(type: "boolean", nullable: false),
					AllowAccessTokensViaBrowser = table.Column<bool>(type: "boolean", nullable: false),
					FrontChannelLogoutUri = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
					FrontChannelLogoutSessionRequired = table.Column<bool>(type: "boolean", nullable: false),
					BackChannelLogoutUri = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
					BackChannelLogoutSessionRequired = table.Column<bool>(type: "boolean", nullable: false),
					AllowOfflineAccess = table.Column<bool>(type: "boolean", nullable: false),
					IdentityTokenLifetime = table.Column<int>(type: "integer", nullable: false),
					AllowedIdentityTokenSigningAlgorithms = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
					AccessTokenLifetime = table.Column<int>(type: "integer", nullable: false),
					AuthorizationCodeLifetime = table.Column<int>(type: "integer", nullable: false),
					ConsentLifetime = table.Column<int>(type: "integer", nullable: true),
					AbsoluteRefreshTokenLifetime = table.Column<int>(type: "integer", nullable: false),
					SlidingRefreshTokenLifetime = table.Column<int>(type: "integer", nullable: false),
					RefreshTokenUsage = table.Column<int>(type: "integer", nullable: false),
					UpdateAccessTokenClaimsOnRefresh = table.Column<bool>(type: "boolean", nullable: false),
					RefreshTokenExpiration = table.Column<int>(type: "integer", nullable: false),
					AccessTokenType = table.Column<int>(type: "integer", nullable: false),
					EnableLocalLogin = table.Column<bool>(type: "boolean", nullable: false),
					IncludeJwtId = table.Column<bool>(type: "boolean", nullable: false),
					AlwaysSendClientClaims = table.Column<bool>(type: "boolean", nullable: false),
					ClientClaimsPrefix = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
					PairWiseSubjectSalt = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
					UserSsoLifetime = table.Column<int>(type: "integer", nullable: true),
					UserCodeType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
					DeviceCodeLifetime = table.Column<int>(type: "integer", nullable: false),
					CibaLifetime = table.Column<int>(type: "integer", nullable: true),
					PollingInterval = table.Column<int>(type: "integer", nullable: true),
					CoordinateLifetimeWithUserSession = table.Column<bool>(type: "boolean", nullable: true),
					Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					LastAccessed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					NonEditable = table.Column<bool>(type: "boolean", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Clients", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "IdentityProviders",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					Scheme = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
					DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
					Enabled = table.Column<bool>(type: "boolean", nullable: false),
					Type = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
					Properties = table.Column<string>(type: "text", nullable: true),
					Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					LastAccessed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					NonEditable = table.Column<bool>(type: "boolean", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_IdentityProviders", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "IdentityResources",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					Enabled = table.Column<bool>(type: "boolean", nullable: false),
					Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
					DisplayName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
					Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
					Required = table.Column<bool>(type: "boolean", nullable: false),
					Emphasize = table.Column<bool>(type: "boolean", nullable: false),
					ShowInDiscoveryDocument = table.Column<bool>(type: "boolean", nullable: false),
					Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					Updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					NonEditable = table.Column<bool>(type: "boolean", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_IdentityResources", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "ApiResourceClaims",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					ApiResourceId = table.Column<int>(type: "integer", nullable: false),
					Type = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ApiResourceClaims", x => x.Id);
					table.ForeignKey(
						name: "FK_ApiResourceClaims_ApiResources_ApiResourceId",
						column: x => x.ApiResourceId,
						principalSchema: "aegis-ids",
						principalTable: "ApiResources",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ApiResourceProperties",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					ApiResourceId = table.Column<int>(type: "integer", nullable: false),
					Key = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
					Value = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ApiResourceProperties", x => x.Id);
					table.ForeignKey(
						name: "FK_ApiResourceProperties_ApiResources_ApiResourceId",
						column: x => x.ApiResourceId,
						principalSchema: "aegis-ids",
						principalTable: "ApiResources",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ApiResourceScopes",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					Scope = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
					ApiResourceId = table.Column<int>(type: "integer", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ApiResourceScopes", x => x.Id);
					table.ForeignKey(
						name: "FK_ApiResourceScopes_ApiResources_ApiResourceId",
						column: x => x.ApiResourceId,
						principalSchema: "aegis-ids",
						principalTable: "ApiResources",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ApiResourceSecrets",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					ApiResourceId = table.Column<int>(type: "integer", nullable: false),
					Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
					Value = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
					Expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					Type = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
					Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ApiResourceSecrets", x => x.Id);
					table.ForeignKey(
						name: "FK_ApiResourceSecrets_ApiResources_ApiResourceId",
						column: x => x.ApiResourceId,
						principalSchema: "aegis-ids",
						principalTable: "ApiResources",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ApiScopeClaims",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					ScopeId = table.Column<int>(type: "integer", nullable: false),
					Type = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ApiScopeClaims", x => x.Id);
					table.ForeignKey(
						name: "FK_ApiScopeClaims_ApiScopes_ScopeId",
						column: x => x.ScopeId,
						principalSchema: "aegis-ids",
						principalTable: "ApiScopes",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ApiScopeProperties",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					ScopeId = table.Column<int>(type: "integer", nullable: false),
					Key = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
					Value = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ApiScopeProperties", x => x.Id);
					table.ForeignKey(
						name: "FK_ApiScopeProperties_ApiScopes_ScopeId",
						column: x => x.ScopeId,
						principalSchema: "aegis-ids",
						principalTable: "ApiScopes",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ClientClaims",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					Type = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
					Value = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
					ClientId = table.Column<int>(type: "integer", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ClientClaims", x => x.Id);
					table.ForeignKey(
						name: "FK_ClientClaims_Clients_ClientId",
						column: x => x.ClientId,
						principalSchema: "aegis-ids",
						principalTable: "Clients",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ClientCorsOrigins",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					Origin = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
					ClientId = table.Column<int>(type: "integer", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ClientCorsOrigins", x => x.Id);
					table.ForeignKey(
						name: "FK_ClientCorsOrigins_Clients_ClientId",
						column: x => x.ClientId,
						principalSchema: "aegis-ids",
						principalTable: "Clients",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ClientGrantTypes",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					GrantType = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
					ClientId = table.Column<int>(type: "integer", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ClientGrantTypes", x => x.Id);
					table.ForeignKey(
						name: "FK_ClientGrantTypes_Clients_ClientId",
						column: x => x.ClientId,
						principalSchema: "aegis-ids",
						principalTable: "Clients",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ClientIdPRestrictions",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					Provider = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
					ClientId = table.Column<int>(type: "integer", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ClientIdPRestrictions", x => x.Id);
					table.ForeignKey(
						name: "FK_ClientIdPRestrictions_Clients_ClientId",
						column: x => x.ClientId,
						principalSchema: "aegis-ids",
						principalTable: "Clients",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ClientPostLogoutRedirectUris",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					PostLogoutRedirectUri = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
					ClientId = table.Column<int>(type: "integer", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ClientPostLogoutRedirectUris", x => x.Id);
					table.ForeignKey(
						name: "FK_ClientPostLogoutRedirectUris_Clients_ClientId",
						column: x => x.ClientId,
						principalSchema: "aegis-ids",
						principalTable: "Clients",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ClientProperties",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					ClientId = table.Column<int>(type: "integer", nullable: false),
					Key = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
					Value = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ClientProperties", x => x.Id);
					table.ForeignKey(
						name: "FK_ClientProperties_Clients_ClientId",
						column: x => x.ClientId,
						principalSchema: "aegis-ids",
						principalTable: "Clients",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ClientRedirectUris",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					RedirectUri = table.Column<string>(type: "character varying(400)", maxLength: 400, nullable: false),
					ClientId = table.Column<int>(type: "integer", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ClientRedirectUris", x => x.Id);
					table.ForeignKey(
						name: "FK_ClientRedirectUris_Clients_ClientId",
						column: x => x.ClientId,
						principalSchema: "aegis-ids",
						principalTable: "Clients",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ClientScopes",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					Scope = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
					ClientId = table.Column<int>(type: "integer", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ClientScopes", x => x.Id);
					table.ForeignKey(
						name: "FK_ClientScopes_Clients_ClientId",
						column: x => x.ClientId,
						principalSchema: "aegis-ids",
						principalTable: "Clients",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "ClientSecrets",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					ClientId = table.Column<int>(type: "integer", nullable: false),
					Description = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
					Value = table.Column<string>(type: "character varying(4000)", maxLength: 4000, nullable: false),
					Expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					Type = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
					Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ClientSecrets", x => x.Id);
					table.ForeignKey(
						name: "FK_ClientSecrets_Clients_ClientId",
						column: x => x.ClientId,
						principalSchema: "aegis-ids",
						principalTable: "Clients",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "IdentityResourceClaims",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					IdentityResourceId = table.Column<int>(type: "integer", nullable: false),
					Type = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_IdentityResourceClaims", x => x.Id);
					table.ForeignKey(
						name: "FK_IdentityResourceClaims_IdentityResources_IdentityResourceId",
						column: x => x.IdentityResourceId,
						principalSchema: "aegis-ids",
						principalTable: "IdentityResources",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "IdentityResourceProperties",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					IdentityResourceId = table.Column<int>(type: "integer", nullable: false),
					Key = table.Column<string>(type: "character varying(250)", maxLength: 250, nullable: false),
					Value = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_IdentityResourceProperties", x => x.Id);
					table.ForeignKey(
						name: "FK_IdentityResourceProperties_IdentityResources_IdentityResour~",
						column: x => x.IdentityResourceId,
						principalSchema: "aegis-ids",
						principalTable: "IdentityResources",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_ApiResourceClaims_ApiResourceId_Type",
				schema: "aegis-ids",
				table: "ApiResourceClaims",
				columns: new[] { "ApiResourceId", "Type" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_ApiResourceProperties_ApiResourceId_Key",
				schema: "aegis-ids",
				table: "ApiResourceProperties",
				columns: new[] { "ApiResourceId", "Key" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_ApiResources_Name",
				schema: "aegis-ids",
				table: "ApiResources",
				column: "Name",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_ApiResourceScopes_ApiResourceId_Scope",
				schema: "aegis-ids",
				table: "ApiResourceScopes",
				columns: new[] { "ApiResourceId", "Scope" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_ApiResourceSecrets_ApiResourceId",
				schema: "aegis-ids",
				table: "ApiResourceSecrets",
				column: "ApiResourceId");

			migrationBuilder.CreateIndex(
				name: "IX_ApiScopeClaims_ScopeId_Type",
				schema: "aegis-ids",
				table: "ApiScopeClaims",
				columns: new[] { "ScopeId", "Type" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_ApiScopeProperties_ScopeId_Key",
				schema: "aegis-ids",
				table: "ApiScopeProperties",
				columns: new[] { "ScopeId", "Key" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_ApiScopes_Name",
				schema: "aegis-ids",
				table: "ApiScopes",
				column: "Name",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_ClientClaims_ClientId_Type_Value",
				schema: "aegis-ids",
				table: "ClientClaims",
				columns: new[] { "ClientId", "Type", "Value" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_ClientCorsOrigins_ClientId_Origin",
				schema: "aegis-ids",
				table: "ClientCorsOrigins",
				columns: new[] { "ClientId", "Origin" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_ClientGrantTypes_ClientId_GrantType",
				schema: "aegis-ids",
				table: "ClientGrantTypes",
				columns: new[] { "ClientId", "GrantType" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_ClientIdPRestrictions_ClientId_Provider",
				schema: "aegis-ids",
				table: "ClientIdPRestrictions",
				columns: new[] { "ClientId", "Provider" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_ClientPostLogoutRedirectUris_ClientId_PostLogoutRedirectUri",
				schema: "aegis-ids",
				table: "ClientPostLogoutRedirectUris",
				columns: new[] { "ClientId", "PostLogoutRedirectUri" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_ClientProperties_ClientId_Key",
				schema: "aegis-ids",
				table: "ClientProperties",
				columns: new[] { "ClientId", "Key" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_ClientRedirectUris_ClientId_RedirectUri",
				schema: "aegis-ids",
				table: "ClientRedirectUris",
				columns: new[] { "ClientId", "RedirectUri" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_Clients_ClientId",
				schema: "aegis-ids",
				table: "Clients",
				column: "ClientId",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_ClientScopes_ClientId_Scope",
				schema: "aegis-ids",
				table: "ClientScopes",
				columns: new[] { "ClientId", "Scope" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_ClientSecrets_ClientId",
				schema: "aegis-ids",
				table: "ClientSecrets",
				column: "ClientId");

			migrationBuilder.CreateIndex(
				name: "IX_IdentityProviders_Scheme",
				schema: "aegis-ids",
				table: "IdentityProviders",
				column: "Scheme",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_IdentityResourceClaims_IdentityResourceId_Type",
				schema: "aegis-ids",
				table: "IdentityResourceClaims",
				columns: new[] { "IdentityResourceId", "Type" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_IdentityResourceProperties_IdentityResourceId_Key",
				schema: "aegis-ids",
				table: "IdentityResourceProperties",
				columns: new[] { "IdentityResourceId", "Key" },
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_IdentityResources_Name",
				schema: "aegis-ids",
				table: "IdentityResources",
				column: "Name",
				unique: true);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "ApiResourceClaims",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "ApiResourceProperties",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "ApiResourceScopes",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "ApiResourceSecrets",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "ApiScopeClaims",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "ApiScopeProperties",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "ClientClaims",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "ClientCorsOrigins",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "ClientGrantTypes",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "ClientIdPRestrictions",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "ClientPostLogoutRedirectUris",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "ClientProperties",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "ClientRedirectUris",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "ClientScopes",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "ClientSecrets",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "IdentityProviders",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "IdentityResourceClaims",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "IdentityResourceProperties",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "ApiResources",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "ApiScopes",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "Clients",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "IdentityResources",
				schema: "aegis-ids");
		}
	}
}

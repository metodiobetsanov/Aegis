﻿#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion
#nullable disable

namespace Aegis.Persistence.Migrations.IdentityServer.PersistedGrant
{
	using System;

	using Microsoft.EntityFrameworkCore.Migrations;

	using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

	/// <inheritdoc />
	public partial class InitialPersistedGrantDbContext : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.EnsureSchema(
				name: "aegis-ids");

			migrationBuilder.CreateTable(
				name: "DeviceCodes",
				schema: "aegis-ids",
				columns: table => new
				{
					UserCode = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
					DeviceCode = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
					SubjectId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
					SessionId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
					ClientId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
					Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
					CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					Expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					Data = table.Column<string>(type: "character varying(50000)", maxLength: 50000, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_DeviceCodes", x => x.UserCode);
				});

			migrationBuilder.CreateTable(
				name: "Keys",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<string>(type: "text", nullable: false),
					Version = table.Column<int>(type: "integer", nullable: false),
					Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					Use = table.Column<string>(type: "text", nullable: true),
					Algorithm = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
					IsX509Certificate = table.Column<bool>(type: "boolean", nullable: false),
					DataProtected = table.Column<bool>(type: "boolean", nullable: false),
					Data = table.Column<string>(type: "text", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Keys", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "PersistedGrants",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<long>(type: "bigint", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					Key = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
					Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
					SubjectId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
					SessionId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
					ClientId = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
					Description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
					CreationTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					Expiration = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					ConsumedTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					Data = table.Column<string>(type: "character varying(50000)", maxLength: 50000, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_PersistedGrants", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "ServerSideSessions",
				schema: "aegis-ids",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					Key = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
					Scheme = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
					SubjectId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
					SessionId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
					DisplayName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
					Created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					Renewed = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					Expires = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					Data = table.Column<string>(type: "text", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_ServerSideSessions", x => x.Id);
				});

			migrationBuilder.CreateIndex(
				name: "IX_DeviceCodes_DeviceCode",
				schema: "aegis-ids",
				table: "DeviceCodes",
				column: "DeviceCode",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_DeviceCodes_Expiration",
				schema: "aegis-ids",
				table: "DeviceCodes",
				column: "Expiration");

			migrationBuilder.CreateIndex(
				name: "IX_Keys_Use",
				schema: "aegis-ids",
				table: "Keys",
				column: "Use");

			migrationBuilder.CreateIndex(
				name: "IX_PersistedGrants_ConsumedTime",
				schema: "aegis-ids",
				table: "PersistedGrants",
				column: "ConsumedTime");

			migrationBuilder.CreateIndex(
				name: "IX_PersistedGrants_Expiration",
				schema: "aegis-ids",
				table: "PersistedGrants",
				column: "Expiration");

			migrationBuilder.CreateIndex(
				name: "IX_PersistedGrants_Key",
				schema: "aegis-ids",
				table: "PersistedGrants",
				column: "Key",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_PersistedGrants_SubjectId_ClientId_Type",
				schema: "aegis-ids",
				table: "PersistedGrants",
				columns: new[] { "SubjectId", "ClientId", "Type" });

			migrationBuilder.CreateIndex(
				name: "IX_PersistedGrants_SubjectId_SessionId_Type",
				schema: "aegis-ids",
				table: "PersistedGrants",
				columns: new[] { "SubjectId", "SessionId", "Type" });

			migrationBuilder.CreateIndex(
				name: "IX_ServerSideSessions_DisplayName",
				schema: "aegis-ids",
				table: "ServerSideSessions",
				column: "DisplayName");

			migrationBuilder.CreateIndex(
				name: "IX_ServerSideSessions_Expires",
				schema: "aegis-ids",
				table: "ServerSideSessions",
				column: "Expires");

			migrationBuilder.CreateIndex(
				name: "IX_ServerSideSessions_Key",
				schema: "aegis-ids",
				table: "ServerSideSessions",
				column: "Key",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_ServerSideSessions_SessionId",
				schema: "aegis-ids",
				table: "ServerSideSessions",
				column: "SessionId");

			migrationBuilder.CreateIndex(
				name: "IX_ServerSideSessions_SubjectId",
				schema: "aegis-ids",
				table: "ServerSideSessions",
				column: "SubjectId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "DeviceCodes",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "Keys",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "PersistedGrants",
				schema: "aegis-ids");

			migrationBuilder.DropTable(
				name: "ServerSideSessions",
				schema: "aegis-ids");
		}
	}
}

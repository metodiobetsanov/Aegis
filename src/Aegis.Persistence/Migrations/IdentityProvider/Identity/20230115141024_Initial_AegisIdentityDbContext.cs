#region copyright
//----------------------------------------------------------------------
// Copyright 2023 MNB Software
// Licensed under the Apache License, Version 2.0
// You may obtain a copy at http://www.apache.org/licenses/LICENSE-2.0
//----------------------------------------------------------------------
#endregion
#nullable disable

namespace Aegis.Persistence.Migrations.IdentityProvider.Identity
{
	using System;

	using Microsoft.EntityFrameworkCore.Migrations;

	using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

	/// <inheritdoc />
	public partial class InitialAegisIdentityDbContext : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.EnsureSchema(
				name: "aegis_idp");

			migrationBuilder.CreateTable(
				name: "AegisRole",
				schema: "aegis_idp",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					Description = table.Column<string>(type: "text", nullable: true),
					SystemRole = table.Column<bool>(type: "boolean", nullable: false),
					ProtectedRole = table.Column<bool>(type: "boolean", nullable: false),
					AssignByDefault = table.Column<bool>(type: "boolean", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					Name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
					NormalizedName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
					ConcurrencyStamp = table.Column<string>(type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AegisRole", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "AegisUser",
				schema: "aegis_idp",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					FirstName = table.Column<string>(type: "text", nullable: true),
					LastName = table.Column<string>(type: "text", nullable: true),
					ProfilePictureURL = table.Column<string>(type: "text", nullable: false),
					SystemUser = table.Column<bool>(type: "boolean", nullable: false),
					ProtectedUser = table.Column<bool>(type: "boolean", nullable: false),
					ActiveProfile = table.Column<bool>(type: "boolean", nullable: false),
					CompletedProfile = table.Column<bool>(type: "boolean", nullable: false),
					SoftDelete = table.Column<bool>(type: "boolean", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					UserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
					NormalizedUserName = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
					Email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
					NormalizedEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
					EmailConfirmed = table.Column<bool>(type: "boolean", nullable: false),
					PasswordHash = table.Column<string>(type: "text", nullable: true),
					SecurityStamp = table.Column<string>(type: "text", nullable: true),
					ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
					PhoneNumber = table.Column<string>(type: "text", nullable: true),
					PhoneNumberConfirmed = table.Column<bool>(type: "boolean", nullable: false),
					TwoFactorEnabled = table.Column<bool>(type: "boolean", nullable: false),
					LockoutEnd = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
					LockoutEnabled = table.Column<bool>(type: "boolean", nullable: false),
					AccessFailedCount = table.Column<int>(type: "integer", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AegisUser", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "AegisRoleClaim",
				schema: "aegis_idp",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
					CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					AegisRoleId = table.Column<Guid>(type: "uuid", nullable: true),
					RoleId = table.Column<Guid>(type: "uuid", nullable: false),
					ClaimType = table.Column<string>(type: "text", nullable: true),
					ClaimValue = table.Column<string>(type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AegisRoleClaim", x => x.Id);
					table.ForeignKey(
						name: "FK_AegisRoleClaim_AegisRole_AegisRoleId",
						column: x => x.AegisRoleId,
						principalSchema: "aegis_idp",
						principalTable: "AegisRole",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_AegisRoleClaim_AegisRole_RoleId",
						column: x => x.RoleId,
						principalSchema: "aegis_idp",
						principalTable: "AegisRole",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AegisUserClaims",
				schema: "aegis_idp",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
					CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					AegisUserId = table.Column<Guid>(type: "uuid", nullable: true),
					UserId = table.Column<Guid>(type: "uuid", nullable: false),
					ClaimType = table.Column<string>(type: "text", nullable: true),
					ClaimValue = table.Column<string>(type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AegisUserClaims", x => x.Id);
					table.ForeignKey(
						name: "FK_AegisUserClaims_AegisUser_AegisUserId",
						column: x => x.AegisUserId,
						principalSchema: "aegis_idp",
						principalTable: "AegisUser",
						principalColumn: "Id");
					table.ForeignKey(
						name: "FK_AegisUserClaims_AegisUser_UserId",
						column: x => x.UserId,
						principalSchema: "aegis_idp",
						principalTable: "AegisUser",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AegisUserLogin",
				schema: "aegis_idp",
				columns: table => new
				{
					LoginProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
					ProviderKey = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
					ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
					CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					ProviderDisplayName = table.Column<string>(type: "text", nullable: true),
					UserId = table.Column<Guid>(type: "uuid", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AegisUserLogin", x => new { x.LoginProvider, x.ProviderKey });
					table.ForeignKey(
						name: "FK_AegisUserLogin_AegisUser_UserId",
						column: x => x.UserId,
						principalSchema: "aegis_idp",
						principalTable: "AegisUser",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AegisUserRole",
				schema: "aegis_idp",
				columns: table => new
				{
					UserId = table.Column<Guid>(type: "uuid", nullable: false),
					RoleId = table.Column<Guid>(type: "uuid", nullable: false),
					ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
					CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AegisUserRole", x => new { x.UserId, x.RoleId });
					table.ForeignKey(
						name: "FK_AegisUserRole_AegisRole_RoleId",
						column: x => x.RoleId,
						principalSchema: "aegis_idp",
						principalTable: "AegisRole",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
					table.ForeignKey(
						name: "FK_AegisUserRole_AegisUser_UserId",
						column: x => x.UserId,
						principalSchema: "aegis_idp",
						principalTable: "AegisUser",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "AegisUserToken",
				schema: "aegis_idp",
				columns: table => new
				{
					UserId = table.Column<Guid>(type: "uuid", nullable: false),
					LoginProvider = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
					Name = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
					ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
					CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
					Value = table.Column<string>(type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AegisUserToken", x => new { x.UserId, x.LoginProvider, x.Name });
					table.ForeignKey(
						name: "FK_AegisUserToken_AegisUser_UserId",
						column: x => x.UserId,
						principalSchema: "aegis_idp",
						principalTable: "AegisUser",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "RoleNameIndex",
				schema: "aegis_idp",
				table: "AegisRole",
				column: "NormalizedName",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_AegisRoleClaim_AegisRoleId",
				schema: "aegis_idp",
				table: "AegisRoleClaim",
				column: "AegisRoleId");

			migrationBuilder.CreateIndex(
				name: "IX_AegisRoleClaim_RoleId",
				schema: "aegis_idp",
				table: "AegisRoleClaim",
				column: "RoleId");

			migrationBuilder.CreateIndex(
				name: "EmailIndex",
				schema: "aegis_idp",
				table: "AegisUser",
				column: "NormalizedEmail");

			migrationBuilder.CreateIndex(
				name: "UserNameIndex",
				schema: "aegis_idp",
				table: "AegisUser",
				column: "NormalizedUserName",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_AegisUserClaims_AegisUserId",
				schema: "aegis_idp",
				table: "AegisUserClaims",
				column: "AegisUserId");

			migrationBuilder.CreateIndex(
				name: "IX_AegisUserClaims_UserId",
				schema: "aegis_idp",
				table: "AegisUserClaims",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_AegisUserLogin_UserId",
				schema: "aegis_idp",
				table: "AegisUserLogin",
				column: "UserId");

			migrationBuilder.CreateIndex(
				name: "IX_AegisUserRole_RoleId",
				schema: "aegis_idp",
				table: "AegisUserRole",
				column: "RoleId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "AegisRoleClaim",
				schema: "aegis_idp");

			migrationBuilder.DropTable(
				name: "AegisUserClaims",
				schema: "aegis_idp");

			migrationBuilder.DropTable(
				name: "AegisUserLogin",
				schema: "aegis_idp");

			migrationBuilder.DropTable(
				name: "AegisUserRole",
				schema: "aegis_idp");

			migrationBuilder.DropTable(
				name: "AegisUserToken",
				schema: "aegis_idp");

			migrationBuilder.DropTable(
				name: "AegisRole",
				schema: "aegis_idp");

			migrationBuilder.DropTable(
				name: "AegisUser",
				schema: "aegis_idp");
		}
	}
}

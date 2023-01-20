#nullable disable

namespace Aegis.Persistence.Migrations.Application.Secure
{
	using System;

	using Microsoft.EntityFrameworkCore.Migrations;

	using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

	/// <inheritdoc />
	public partial class InitialSecureDbContext : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.EnsureSchema(
				name: "aegis-sc");

			migrationBuilder.CreateTable(
				name: "AuditLogs",
				schema: "aegis-sc",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					EventName = table.Column<string>(type: "text", nullable: false),
					Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					Succeeded = table.Column<bool>(type: "boolean", nullable: false),
					Module = table.Column<int>(type: "integer", nullable: false),
					Action = table.Column<int>(type: "integer", nullable: false),
					Subject = table.Column<int>(type: "integer", nullable: false),
					SubjectId = table.Column<Guid>(type: "uuid", nullable: false),
					UserId = table.Column<Guid>(type: "uuid", nullable: false),
					UserName = table.Column<string>(type: "text", nullable: true),
					UserIp = table.Column<string>(type: "text", nullable: true),
					UserAgent = table.Column<string>(type: "text", nullable: true),
					Summary = table.Column<string>(type: "text", nullable: false),
					OldValues = table.Column<string>(type: "text", nullable: true),
					NewValues = table.Column<string>(type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_AuditLogs", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "DataProtectionKeys",
				schema: "aegis-sc",
				columns: table => new
				{
					Id = table.Column<int>(type: "integer", nullable: false)
						.Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
					FriendlyName = table.Column<string>(type: "text", nullable: true),
					Xml = table.Column<string>(type: "text", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_DataProtectionKeys", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "PersonalDataProtectionKeys",
				schema: "aegis-sc",
				columns: table => new
				{
					Id = table.Column<Guid>(type: "uuid", nullable: false),
					Key = table.Column<string>(type: "text", nullable: false),
					KeyHash = table.Column<string>(type: "text", nullable: false),
					ExpiresOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					ConcurrencyStamp = table.Column<string>(type: "text", nullable: true),
					CreatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
					UpdatedOn = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_PersonalDataProtectionKeys", x => x.Id);
				});
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "AuditLogs",
				schema: "aegis-sc");

			migrationBuilder.DropTable(
				name: "DataProtectionKeys",
				schema: "aegis-sc");

			migrationBuilder.DropTable(
				name: "PersonalDataProtectionKeys",
				schema: "aegis-sc");
		}
	}
}

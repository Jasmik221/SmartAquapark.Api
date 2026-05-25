using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace SmartAquapark.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWristbands : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "wristbands",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    qr_code = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    activated_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    exit_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    status = table.Column<string>(type: "text", nullable: false),
                    ticket_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_wristbands", x => x.id);
                    table.ForeignKey(
                        name: "FK_wristbands_tickets_ticket_id",
                        column: x => x.ticket_id,
                        principalTable: "tickets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_wristbands_qr_code",
                table: "wristbands",
                column: "qr_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_wristbands_ticket_id",
                table: "wristbands",
                column: "ticket_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "wristbands");
        }
    }
}

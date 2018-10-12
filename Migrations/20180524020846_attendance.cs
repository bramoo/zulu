using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace zulu.Migrations
{
    public partial class attendance : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Attendance",
                columns: table => new
                {
                    EventId = table.Column<int>(nullable: false),
                    MemberId = table.Column<int>(nullable: false),
                    Attended = table.Column<bool>(nullable: false),
                    ServiceHours = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attendance", x => new { x.EventId, x.MemberId });
                    table.ForeignKey(
                        name: "FK_Attendance_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Attendance_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Attendance_MemberId",
                table: "Attendance",
                column: "MemberId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Attendance");
        }
    }
}

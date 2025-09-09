using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrewTaskApi.Migrations
{
    /// <inheritdoc />
    public partial class IniiUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "User",
                columns: new[] { "id", "CreateAt", "Email", "PasswordHash", "SoftDeleted", "Username" },
                values: new object[] { 1, new DateTime(2025, 9, 9, 16, 36, 58, 761, DateTimeKind.Utc).AddTicks(4198), "vino_kurov@inbox.ru", "$V1$10000$enG65fR/M8auKQvuhdytZoftXHFSYv1wkjlxBwtzZCKYTMp8", false, "vinokurov" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "User",
                keyColumn: "id",
                keyValue: 1);
        }
    }
}

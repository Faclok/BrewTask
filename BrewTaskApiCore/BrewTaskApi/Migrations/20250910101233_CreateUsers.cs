using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrewTaskApi.Migrations
{
    /// <inheritdoc />
    public partial class CreateUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subtasks_Task_ParentTaskId",
                table: "Subtasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Subtasks_Task_TaskId",
                table: "Subtasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_User_AssigneeId",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_Task_User_AuthorId",
                table: "Task");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskRelation_Task_TaskFromId",
                table: "TaskRelation");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskRelation_Task_TaskToId",
                table: "TaskRelation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                table: "User");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskRelation",
                table: "TaskRelation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Task",
                table: "Task");

            migrationBuilder.RenameTable(
                name: "User",
                newName: "Users");

            migrationBuilder.RenameTable(
                name: "TaskRelation",
                newName: "TaskRelations");

            migrationBuilder.RenameTable(
                name: "Task",
                newName: "Tasks");

            migrationBuilder.RenameIndex(
                name: "IX_TaskRelation_TaskToId",
                table: "TaskRelations",
                newName: "IX_TaskRelations_TaskToId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskRelation_TaskFromId",
                table: "TaskRelations",
                newName: "IX_TaskRelations_TaskFromId");

            migrationBuilder.RenameIndex(
                name: "IX_Task_AuthorId",
                table: "Tasks",
                newName: "IX_Tasks_AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Task_AssigneeId",
                table: "Tasks",
                newName: "IX_Tasks_AssigneeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                table: "Users",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskRelations",
                table: "TaskRelations",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks",
                column: "id");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "CreateAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 10, 10, 12, 29, 947, DateTimeKind.Utc).AddTicks(8196), "$V1$10000$U/97Rw6aiXmQ9AIauy8VbrbgZckvhwA6jKHDqm9naxQouwTH" });

            migrationBuilder.AddForeignKey(
                name: "FK_Subtasks_Tasks_ParentTaskId",
                table: "Subtasks",
                column: "ParentTaskId",
                principalTable: "Tasks",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subtasks_Tasks_TaskId",
                table: "Subtasks",
                column: "TaskId",
                principalTable: "Tasks",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskRelations_Tasks_TaskFromId",
                table: "TaskRelations",
                column: "TaskFromId",
                principalTable: "Tasks",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskRelations_Tasks_TaskToId",
                table: "TaskRelations",
                column: "TaskToId",
                principalTable: "Tasks",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_AssigneeId",
                table: "Tasks",
                column: "AssigneeId",
                principalTable: "Users",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_AuthorId",
                table: "Tasks",
                column: "AuthorId",
                principalTable: "Users",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Subtasks_Tasks_ParentTaskId",
                table: "Subtasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Subtasks_Tasks_TaskId",
                table: "Subtasks");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskRelations_Tasks_TaskFromId",
                table: "TaskRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskRelations_Tasks_TaskToId",
                table: "TaskRelations");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_AssigneeId",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_AuthorId",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tasks",
                table: "Tasks");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TaskRelations",
                table: "TaskRelations");

            migrationBuilder.RenameTable(
                name: "Users",
                newName: "User");

            migrationBuilder.RenameTable(
                name: "Tasks",
                newName: "Task");

            migrationBuilder.RenameTable(
                name: "TaskRelations",
                newName: "TaskRelation");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_AuthorId",
                table: "Task",
                newName: "IX_Task_AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_AssigneeId",
                table: "Task",
                newName: "IX_Task_AssigneeId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskRelations_TaskToId",
                table: "TaskRelation",
                newName: "IX_TaskRelation_TaskToId");

            migrationBuilder.RenameIndex(
                name: "IX_TaskRelations_TaskFromId",
                table: "TaskRelation",
                newName: "IX_TaskRelation_TaskFromId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                table: "User",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Task",
                table: "Task",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TaskRelation",
                table: "TaskRelation",
                column: "id");

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "CreateAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 9, 16, 36, 58, 761, DateTimeKind.Utc).AddTicks(4198), "$V1$10000$enG65fR/M8auKQvuhdytZoftXHFSYv1wkjlxBwtzZCKYTMp8" });

            migrationBuilder.AddForeignKey(
                name: "FK_Subtasks_Task_ParentTaskId",
                table: "Subtasks",
                column: "ParentTaskId",
                principalTable: "Task",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Subtasks_Task_TaskId",
                table: "Subtasks",
                column: "TaskId",
                principalTable: "Task",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_User_AssigneeId",
                table: "Task",
                column: "AssigneeId",
                principalTable: "User",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_Task_User_AuthorId",
                table: "Task",
                column: "AuthorId",
                principalTable: "User",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskRelation_Task_TaskFromId",
                table: "TaskRelation",
                column: "TaskFromId",
                principalTable: "Task",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskRelation_Task_TaskToId",
                table: "TaskRelation",
                column: "TaskToId",
                principalTable: "Task",
                principalColumn: "id");
        }
    }
}

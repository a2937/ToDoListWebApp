using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace ToDoListWebApp.Data.Migrations
{
    public partial class TrackUserId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "TodoList",
                table: "TodoListItem",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TodoListItem_UserId",
                schema: "TodoList",
                table: "TodoListItem",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TodoListItem_AspNetUsers_UserId",
                schema: "TodoList",
                table: "TodoListItem",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TodoListItem_AspNetUsers_UserId",
                schema: "TodoList",
                table: "TodoListItem");

            migrationBuilder.DropIndex(
                name: "IX_TodoListItem_UserId",
                schema: "TodoList",
                table: "TodoListItem");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "TodoList",
                table: "TodoListItem");
        }
    }
}

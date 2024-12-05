using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Threllaut.Data.Migrations.Application
{
    /// <inheritdoc />
    public partial class CreateSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "app");

            migrationBuilder.CreateTable(
                name: "Boards",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Boards_Users_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserBoard",
                schema: "app",
                columns: table => new
                {
                    BoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MembersId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserBoard", x => new { x.BoardId, x.MembersId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserBoard_Boards_BoardId",
                        column: x => x.BoardId,
                        principalSchema: "app",
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserBoard_Users_MembersId",
                        column: x => x.MembersId,
                        principalSchema: "identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Columns",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BoardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Columns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Columns_Boards_BoardId",
                        column: x => x.BoardId,
                        principalSchema: "app",
                        principalTable: "Boards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ColumnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tasks_Columns_ColumnId",
                        column: x => x.ColumnId,
                        principalSchema: "app",
                        principalTable: "Columns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserBoardTask",
                schema: "app",
                columns: table => new
                {
                    AssigneesId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BoardTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserBoardTask", x => new { x.AssigneesId, x.BoardTaskId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserBoardTask_Tasks_BoardTaskId",
                        column: x => x.BoardTaskId,
                        principalSchema: "app",
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserBoardTask_Users_AssigneesId",
                        column: x => x.AssigneesId,
                        principalSchema: "identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TaskEvents",
                schema: "app",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ColumnId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PreviousColumnId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TaskEvents_Columns_ColumnId",
                        column: x => x.ColumnId,
                        principalSchema: "app",
                        principalTable: "Columns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskEvents_Columns_PreviousColumnId",
                        column: x => x.PreviousColumnId,
                        principalSchema: "app",
                        principalTable: "Columns",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TaskEvents_Tasks_TaskId",
                        column: x => x.TaskId,
                        principalSchema: "app",
                        principalTable: "Tasks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TaskEvents_Users_UserId",
                        column: x => x.UserId,
                        principalSchema: "identity",
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserBoard_MembersId",
                schema: "app",
                table: "ApplicationUserBoard",
                column: "MembersId");

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserBoardTask_BoardTaskId",
                schema: "app",
                table: "ApplicationUserBoardTask",
                column: "BoardTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_Boards_OwnerId",
                schema: "app",
                table: "Boards",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Columns_BoardId",
                schema: "app",
                table: "Columns",
                column: "BoardId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskEvents_ColumnId",
                schema: "app",
                table: "TaskEvents",
                column: "ColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskEvents_PreviousColumnId",
                schema: "app",
                table: "TaskEvents",
                column: "PreviousColumnId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskEvents_TaskId",
                schema: "app",
                table: "TaskEvents",
                column: "TaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskEvents_UserId",
                schema: "app",
                table: "TaskEvents",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ColumnId",
                schema: "app",
                table: "Tasks",
                column: "ColumnId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserBoard",
                schema: "app");

            migrationBuilder.DropTable(
                name: "ApplicationUserBoardTask",
                schema: "app");

            migrationBuilder.DropTable(
                name: "TaskEvents",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Tasks",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Columns",
                schema: "app");

            migrationBuilder.DropTable(
                name: "Boards",
                schema: "app");
        }
    }
}

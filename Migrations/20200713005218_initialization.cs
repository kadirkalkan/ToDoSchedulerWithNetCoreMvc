using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PitonProject.Migrations
{
    public partial class initialization : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LOG_TYPE",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogTypeText = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOG_TYPE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TASK_TYPE",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TaskTypeText = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TASK_TYPE", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LOG",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogText = table.Column<string>(nullable: true),
                    ClassName = table.Column<string>(nullable: true),
                    MethodName = table.Column<string>(nullable: true),
                    LogTypeID = table.Column<int>(nullable: false),
                    CreatedTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LOG", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LOG_LOG_TYPE_LogTypeID",
                        column: x => x.LogTypeID,
                        principalTable: "LOG_TYPE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    TaskTypeID = table.Column<int>(nullable: true),
                    StartDate = table.Column<DateTime>(nullable: false),
                    EndDate = table.Column<DateTime>(nullable: false),
                    CreatedUserID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Tasks_USERS_CreatedUserID",
                        column: x => x.CreatedUserID,
                        principalTable: "USERS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tasks_TASK_TYPE_TaskTypeID",
                        column: x => x.TaskTypeID,
                        principalTable: "TASK_TYPE",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "LOG_TYPE",
                columns: new[] { "ID", "LogTypeText" },
                values: new object[,]
                {
                    { 1, "info" },
                    { 2, "error" }
                });

            migrationBuilder.InsertData(
                table: "TASK_TYPE",
                columns: new[] { "ID", "TaskTypeText" },
                values: new object[,]
                {
                    { 1, "day" },
                    { 2, "week" },
                    { 3, "month" }
                });

            migrationBuilder.InsertData(
                table: "USERS",
                columns: new[] { "ID", "Password", "UserName" },
                values: new object[] { 1, "14010511822918165421189233818977238212231771031692001152527518416831111421807216924", "admin" });

            migrationBuilder.CreateIndex(
                name: "IX_LOG_LogTypeID",
                table: "LOG",
                column: "LogTypeID");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_CreatedUserID",
                table: "Tasks",
                column: "CreatedUserID");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_TaskTypeID",
                table: "Tasks",
                column: "TaskTypeID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LOG");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "LOG_TYPE");

            migrationBuilder.DropTable(
                name: "USERS");

            migrationBuilder.DropTable(
                name: "TASK_TYPE");
        }
    }
}

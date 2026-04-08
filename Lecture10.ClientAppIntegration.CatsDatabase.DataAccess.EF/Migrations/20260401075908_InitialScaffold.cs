using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Lecture10.ClientAppIntegration.CatsDatabase.DataAccess.EF.Migrations
{
    /// <inheritdoc />
    public partial class InitialScaffold : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Uid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Password = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cats",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Uid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Breed = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Age = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cats_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Uid = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    PhotoUrl = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CatPost",
                columns: table => new
                {
                    CatsId = table.Column<long>(type: "bigint", nullable: false),
                    PostsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatPost", x => new { x.CatsId, x.PostsId });
                    table.ForeignKey(
                        name: "FK_CatPost_Cats_CatsId",
                        column: x => x.CatsId,
                        principalTable: "Cats",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CatPost_Posts_PostsId",
                        column: x => x.PostsId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatPost_PostsId",
                table: "CatPost",
                column: "PostsId");

            migrationBuilder.CreateIndex(
                name: "IX_Cats_Uid",
                table: "Cats",
                column: "Uid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Cats_UserId",
                table: "Cats",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_Uid",
                table: "Posts",
                column: "Uid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Uid",
                table: "Users",
                column: "Uid",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);


            // Demo user
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Uid", "Username", "Email", "Password" },
                values: new object[]
                {
                    100L,
                    new Guid("11111111-1111-1111-1111-111111111111"),
                    "Иванов Иван Иванович",
                    "ivanov.ivan.ivanovich@example.com",
                    "demo-password"
                });

            // Cats
            migrationBuilder.InsertData(
                table: "Cats",
                columns: new[] { "Id", "UserId", "Uid", "Name", "Breed", "Age" },
                values: new object[,]
                {
                    { 1001L, 100L, new Guid("20000000-0000-0000-0000-000000000001"), "Приемлемо", "Домашняя короткошёрстная, полностью белая", 5 },
                    { 1002L, 100L, new Guid("20000000-0000-0000-0000-000000000002"), "Рыжик", "Рыжий табби, домашняя короткошёрстная", 3 },
                    { 1003L, 100L, new Guid("20000000-0000-0000-0000-000000000003"), "Бу", "Порода тут интересная, конечно", 6 },
                    { 1004L, 100L, new Guid("20000000-0000-0000-0000-000000000004"), "Черешник", "Домашняя короткошёрстная", 4 },
                    { 1005L, 100L, new Guid("20000000-0000-0000-0000-000000000005"), "Студик", "Домашняя короткошёрстная", 1 },
                    { 1006L, 100L, new Guid("20000000-0000-0000-0000-000000000006"), "Котек", "Домашняя короткошёрстная", 1 },
                    { 1007L, 100L, new Guid("20000000-0000-0000-0000-000000000007"), "Тоже котек", "Домашняя короткошёрстная", 1 },
                    { 1008L, 100L, new Guid("20000000-0000-0000-0000-000000000008"), "Mean", "Манчкин", 3 },
                    { 1009L, 100L, new Guid("20000000-0000-0000-0000-000000000009"), "Кусалка", "Американская короткошёрстная", 4 },
                    { 1010L, 100L, new Guid("20000000-0000-0000-0000-000000000010"), "Рыжая кусалка", "Американская короткошёрстная", 4 }
                });

            // Posts
            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "Id", "UserId", "Uid", "Title", "Description", "PhotoUrl" },
                values: new object[,]
                {
                    {
                        2001L,
                        100L,
                        new Guid("a5f6c493-984f-4a80-8b0a-b634e7cf4082"),
                        "Интернетное",
                        "Бесконечно можно смотреть на три вещи. На остальные нельзя.",
                        "https://i.postimg.cc/kMwbmgg4/photo-2026-04-06-18-22-07.jpg"
                    },
                    {
                        2002L,
                        100L,
                        new Guid("04deadc3-19cb-4630-b634-182121ccf811"),
                        "Я ем",
                        "в 3 ночи",
                        "https://i.postimg.cc/50dRbmpv/2am.png"
                    },
                    {
                        2003L,
                        100L,
                        new Guid("9fdc4652-0911-4c96-90ee-81b92c67694e"),
                        "Bu",
                        "  ",
                        "https://i.postimg.cc/XJ4ks1QK/bu.png"
                    },
                    {
                        2004L,
                        100L,
                        new Guid("d51547d5-3495-4f3e-9201-99f357f279db"),
                        "Вопрос",
                        "как обстоят дела с черешней?",
                        "https://i.postimg.cc/zBzTCbGj/wr-960.png"
                    },
                    {
                        2005L,
                        100L,
                        new Guid("35918bc0-26d2-445e-b8d4-3c5453eb46dd"),
                        "жааль",
                        "что перефоткаться на студак уже поздно",
                        "https://i.postimg.cc/PqhDHpYk/photo-cat.jpg"
                    },
                    {
                        2006L,
                        100L,
                        new Guid("164518c5-a739-4266-88fe-df6a69201ce4"),
                        "котеки",
                        "котеки только что окончили вуз и понятия не имеют что делать дальше",
                        "https://i.postimg.cc/dtyZTm9s/graduation.jpg"
                    },
                    {
                        2007L,
                        100L,
                        new Guid("ab0dd92d-21df-4768-b725-4002a39bac56"),
                        "Mean",
                        "so mean",
                        "https://i.postimg.cc/wvM7xhvt/so-mean.jpg"
                    },
                    {
                        2008L,
                        100L,
                        new Guid("c1925ef5-39bd-448e-957b-d03236a93536"),
                        "Кусалки",
                        "Кусалки",
                        "https://i.postimg.cc/Mpw06F8X/kusalki.jpg"
                    }
                });

            // Post <-> Cat relations
            migrationBuilder.InsertData(
                table: "CatPost",
                columns: new[] { "CatsId", "PostsId" },
                values: new object[,]
                {
                    { 1001L, 2001L },
                    { 1002L, 2002L },
                    { 1003L, 2003L },
                    { 1004L, 2004L },
                    { 1005L, 2005L },
                    { 1006L, 2006L },
                    { 1007L, 2006L },
                    { 1008L, 2007L },
                    { 1009L, 2008L },
                    { 1010L, 2008L }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatPost");

            migrationBuilder.DropTable(
                name: "Cats");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

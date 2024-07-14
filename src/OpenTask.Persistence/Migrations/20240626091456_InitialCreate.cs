using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace OpenTask.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ot_locker",
                columns: table => new
                {
                    resource = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, comment: "资源唯唯一标识"),
                    version = table.Column<int>(type: "int", nullable: false, comment: "资源版本号"),
                    locked_by = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, defaultValueSql: "''", comment: "加锁者"),
                    locked_at = table.Column<DateTime>(type: "datetime", nullable: false, comment: "获取锁的开始时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.resource);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ot_server",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "自增主键")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    server_id = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, comment: "server唯一标识"),
                    end_point = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, comment: "集群内部访问地址"),
                    heart_at = table.Column<DateTime>(type: "datetime", nullable: false, comment: "最后一次心跳时间"),
                    slot_from = table.Column<int>(type: "int", nullable: false, comment: "0~16383"),
                    slot_end = table.Column<int>(type: "int", nullable: false, comment: "0~16383")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ot_task_info",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "自增主键")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    appid = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, defaultValueSql: "''", comment: "所属应用appid"),
                    name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, defaultValueSql: "''", comment: "任务名称"),
                    handler = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, defaultValueSql: "''", comment: "任务handler名"),
                    description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValueSql: "''", comment: "任务描述"),
                    time_type = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, defaultValueSql: "''", comment: "调度时间类型"),
                    time_conf = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false, defaultValueSql: "''", comment: "调度时间配置 根据time_type不同而不同"),
                    attempt_interval = table.Column<sbyte>(type: "tinyint", nullable: false, comment: "重试间隔 单位s"),
                    attempt_max = table.Column<sbyte>(type: "tinyint", nullable: false, comment: "重试次数"),
                    handle_params = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValueSql: "''", comment: "执行参数"),
                    schedule_mode = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, defaultValueSql: "''", comment: "调度类型，单机/广播"),
                    alarm_type = table.Column<string>(type: "varchar(16)", maxLength: 16, nullable: false, defaultValueSql: "''", comment: "报警类型"),
                    alarm_conf = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValueSql: "''", comment: "报警配置"),
                    trigger_next_time = table.Column<long>(type: "bigint", nullable: false, comment: "下次调度时间"),
                    trigger_last_time = table.Column<long>(type: "bigint", nullable: false, comment: "最后一次调度时间"),
                    slot = table.Column<int>(type: "int", nullable: false, comment: "槽"),
                    enabled = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "启用/禁用")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ot_task_log",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "自增主键")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    task_id = table.Column<long>(type: "bigint", nullable: false, comment: "任务id（taskinfo表主键）"),
                    handle_start = table.Column<DateTime>(type: "datetime", nullable: true, comment: "处理开始时间"),
                    handle_result = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValueSql: "''", comment: "处理结果"),
                    handle_status = table.Column<sbyte>(type: "tinyint", nullable: false, comment: "处理状态"),
                    handle_client = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, defaultValueSql: "''", comment: "处理任务客户端id"),
                    alarm_status = table.Column<sbyte>(type: "tinyint", nullable: false, comment: "报警状态 0-默认、1-无需告警、2-告警成功、3-告警失败")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ot_user",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false, comment: "用户id")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    user_name = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false, defaultValueSql: "''", comment: "用户名"),
                    password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, defaultValueSql: "''", comment: "密码"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, comment: "创建时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.InsertData(
                table: "ot_user",
                columns: new[] { "id", "created_at", "password", "user_name" },
                values: new object[] { 1, new DateTime(2024, 6, 26, 17, 14, 56, 497, DateTimeKind.Local).AddTicks(4529), "xhGgU5Uh435OAbPeJ0SV7w==", "admin" });

            migrationBuilder.CreateIndex(
                name: "idx_server_id",
                table: "ot_server",
                column: "server_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_name",
                table: "ot_task_info",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_slot",
                table: "ot_task_info",
                column: "slot");

            migrationBuilder.CreateIndex(
                name: "idx_task_id",
                table: "ot_task_log",
                column: "task_id");

            migrationBuilder.CreateIndex(
                name: "idx_user_name",
                table: "ot_user",
                column: "user_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ot_locker");

            migrationBuilder.DropTable(
                name: "ot_server");

            migrationBuilder.DropTable(
                name: "ot_task_info");

            migrationBuilder.DropTable(
                name: "ot_task_log");

            migrationBuilder.DropTable(
                name: "ot_user");
        }
    }
}

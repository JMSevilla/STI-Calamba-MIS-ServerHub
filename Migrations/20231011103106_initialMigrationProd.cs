using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sti_sys_backend.Migrations
{
    /// <inheritdoc />
    public partial class initialMigrationProd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accounts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    firstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    middlename = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    mobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    imgurl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    status = table.Column<int>(type: "int", nullable: false),
                    verified = table.Column<int>(type: "int", nullable: false),
                    access_level = table.Column<int>(type: "int", nullable: false),
                    section = table.Column<int>(type: "int", nullable: false),
                    isNewAccount = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    course_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "actions_logger",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    accountId = table.Column<int>(type: "int", nullable: false),
                    actionsMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_actions_logger", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "application_settings",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    roomSettings = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_application_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "com_laboratory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    comlabName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    totalComputers = table.Column<int>(type: "int", nullable: false),
                    totalWorkingComputers = table.Column<int>(type: "int", nullable: false),
                    totalNotWorkingComputers = table.Column<int>(type: "int", nullable: false),
                    totalNoNetworkComputers = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_com_laboratory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "conference_auth",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    firstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    access_token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    refresh_token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accountId = table.Column<int>(type: "int", nullable: false),
                    isValid = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conference_auth", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "courses",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    course = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    courseAcronym = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_courses", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "joined_participants",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    accountId = table.Column<int>(type: "int", nullable: false),
                    room_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    comlabId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    _joinedStatus = table.Column<int>(type: "int", nullable: false),
                    date_joined = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_left = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_joined_participants", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "leave_meeting_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    roomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    accountId = table.Column<int>(type: "int", nullable: false),
                    firstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    leaveDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_leave_meeting_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "meeting_actions_logs",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    log_message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    accountId = table.Column<int>(type: "int", nullable: false),
                    room_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    violations = table.Column<int>(type: "int", nullable: true),
                    _meetingAuthorization = table.Column<int>(type: "int", nullable: true),
                    logDateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meeting_actions_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "meeting_room",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    room_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    room_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    sectionId = table.Column<int>(type: "int", nullable: false),
                    comlabId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    room_link = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    room_status = table.Column<int>(type: "int", nullable: false),
                    room_creator = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    room_description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    room_password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    pushNotifs = table.Column<int>(type: "int", nullable: false),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meeting_room", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "pc",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    operatingSystem = table.Column<int>(type: "int", nullable: false),
                    computerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    comlabId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    computerStatus = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pc", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "productivity_management",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    accountId = table.Column<int>(type: "int", nullable: false),
                    _productivityStatus = table.Column<int>(type: "int", nullable: false),
                    TimeIn = table.Column<TimeSpan>(type: "time", nullable: false),
                    TimeOut = table.Column<TimeSpan>(type: "time", nullable: false),
                    _status = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_productivity_management", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "record_joined_participants",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    accountId = table.Column<int>(type: "int", nullable: false),
                    room_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    comlabId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    _RecordJoinedStatus = table.Column<int>(type: "int", nullable: false),
                    date_joined = table.Column<DateTime>(type: "datetime2", nullable: false),
                    date_left = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_record_joined_participants", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sections",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    section_id = table.Column<int>(type: "int", nullable: false),
                    sectionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    num_of_students = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    year = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    course_id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_sections", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "ticket_issues",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    issue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    issueKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticket_issues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ticketing",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ticketId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ticketSubject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    priority = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Assignee = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    specificAssignee = table.Column<int>(type: "int", nullable: false),
                    issue = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IssueStatuses = table.Column<int>(type: "int", nullable: false),
                    requester = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    requesterId = table.Column<int>(type: "int", nullable: false),
                    pc_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    comLab = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    pushNotif = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ticketing", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "verification",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    code = table.Column<int>(type: "int", nullable: false),
                    resendCount = table.Column<int>(type: "int", nullable: false),
                    isValid = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_verification", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "verification_cooldown",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    resendCount = table.Column<int>(type: "int", nullable: false),
                    cooldown = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_verification_cooldown", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accounts");

            migrationBuilder.DropTable(
                name: "actions_logger");

            migrationBuilder.DropTable(
                name: "application_settings");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "com_laboratory");

            migrationBuilder.DropTable(
                name: "conference_auth");

            migrationBuilder.DropTable(
                name: "courses");

            migrationBuilder.DropTable(
                name: "joined_participants");

            migrationBuilder.DropTable(
                name: "leave_meeting_logs");

            migrationBuilder.DropTable(
                name: "meeting_actions_logs");

            migrationBuilder.DropTable(
                name: "meeting_room");

            migrationBuilder.DropTable(
                name: "pc");

            migrationBuilder.DropTable(
                name: "productivity_management");

            migrationBuilder.DropTable(
                name: "record_joined_participants");

            migrationBuilder.DropTable(
                name: "sections");

            migrationBuilder.DropTable(
                name: "ticket_issues");

            migrationBuilder.DropTable(
                name: "ticketing");

            migrationBuilder.DropTable(
                name: "verification");

            migrationBuilder.DropTable(
                name: "verification_cooldown");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbleEaseInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Disabilities",
                columns: table => new
                {
                    SSN = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Disabilities", x => x.SSN);
                });

            migrationBuilder.CreateTable(
                name: "PhysiotherapyCenters",
                columns: table => new
                {
                    SSN = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactInfo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysiotherapyCenters", x => x.SSN);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    SSN = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactInfo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.SSN);
                });

            migrationBuilder.CreateTable(
                name: "Organizations",
                columns: table => new
                {
                    SSN = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Organizations", x => x.SSN);
                    table.ForeignKey(
                        name: "FK_Organizations_Users_SSN",
                        column: x => x.SSN,
                        principalTable: "Users",
                        principalColumn: "SSN");
                });

            migrationBuilder.CreateTable(
                name: "ReceivedMessages",
                columns: table => new
                {
                    ReceivedSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    SenderSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivedMessages", x => new { x.SenderSSN, x.Id, x.ReceivedSSN });
                    table.ForeignKey(
                        name: "FK_ReceivedMessages_Users_ReceivedSSN",
                        column: x => x.ReceivedSSN,
                        principalTable: "Users",
                        principalColumn: "SSN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Relatives",
                columns: table => new
                {
                    SSN = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Relatives", x => x.SSN);
                    table.ForeignKey(
                        name: "FK_Relatives_Users_SSN",
                        column: x => x.SSN,
                        principalTable: "Users",
                        principalColumn: "SSN");
                });

            migrationBuilder.CreateTable(
                name: "SentMessages",
                columns: table => new
                {
                    SenderSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    ReceiverSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SentMessages", x => new { x.SenderSSN, x.Id, x.ReceiverSSN });
                    table.ForeignKey(
                        name: "FK_SentMessages_Users_SenderSSN",
                        column: x => x.SenderSSN,
                        principalTable: "Users",
                        principalColumn: "SSN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Caregivers",
                columns: table => new
                {
                    SSN = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrganizationSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Caregivers", x => x.SSN);
                    table.ForeignKey(
                        name: "FK_Caregivers_Organizations_OrganizationSSN",
                        column: x => x.OrganizationSSN,
                        principalTable: "Organizations",
                        principalColumn: "SSN",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Caregivers_Users_SSN",
                        column: x => x.SSN,
                        principalTable: "Users",
                        principalColumn: "SSN");
                });

            migrationBuilder.CreateTable(
                name: "Programs",
                columns: table => new
                {
                    OrganizationSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    status = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Programs", x => new { x.OrganizationSSN, x.Id });
                    table.ForeignKey(
                        name: "FK_Programs_Organizations_OrganizationSSN",
                        column: x => x.OrganizationSSN,
                        principalTable: "Organizations",
                        principalColumn: "SSN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Assessments",
                columns: table => new
                {
                    ProgramOrganizationSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProgramId = table.Column<int>(type: "int", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessments", x => new { x.ProgramOrganizationSSN, x.ProgramId, x.Id });
                    table.ForeignKey(
                        name: "FK_Assessments_Programs_ProgramOrganizationSSN_ProgramId",
                        columns: x => new { x.ProgramOrganizationSSN, x.ProgramId },
                        principalTable: "Programs",
                        principalColumns: new[] { "OrganizationSSN", "Id" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Patients",
                columns: table => new
                {
                    SSN = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RelativeSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CaregiverSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProgramOrganizationSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProgramId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Patients", x => x.SSN);
                    table.ForeignKey(
                        name: "FK_Patients_Caregivers_CaregiverSSN",
                        column: x => x.CaregiverSSN,
                        principalTable: "Caregivers",
                        principalColumn: "SSN");
                    table.ForeignKey(
                        name: "FK_Patients_Programs_ProgramOrganizationSSN_ProgramId",
                        columns: x => new { x.ProgramOrganizationSSN, x.ProgramId },
                        principalTable: "Programs",
                        principalColumns: new[] { "OrganizationSSN", "Id" });
                    table.ForeignKey(
                        name: "FK_Patients_Relatives_RelativeSSN",
                        column: x => x.RelativeSSN,
                        principalTable: "Relatives",
                        principalColumn: "SSN");
                    table.ForeignKey(
                        name: "FK_Patients_Users_SSN",
                        column: x => x.SSN,
                        principalTable: "Users",
                        principalColumn: "SSN");
                });

            migrationBuilder.CreateTable(
                name: "AssessmentPatients",
                columns: table => new
                {
                    PatientSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssessmentProgramOrganizationSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AssessmentProgramId = table.Column<int>(type: "int", nullable: false),
                    AssessmentId = table.Column<int>(type: "int", nullable: false),
                    Grade = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AssessmentPatients", x => new { x.AssessmentProgramOrganizationSSN, x.AssessmentProgramId, x.AssessmentId, x.PatientSSN });
                    table.ForeignKey(
                        name: "FK_AssessmentPatients_Assessments_AssessmentProgramOrganizationSSN_AssessmentProgramId_AssessmentId",
                        columns: x => new { x.AssessmentProgramOrganizationSSN, x.AssessmentProgramId, x.AssessmentId },
                        principalTable: "Assessments",
                        principalColumns: new[] { "ProgramOrganizationSSN", "ProgramId", "Id" },
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AssessmentPatients_Patients_PatientSSN",
                        column: x => x.PatientSSN,
                        principalTable: "Patients",
                        principalColumn: "SSN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FinancialAids",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApprovalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    percentage = table.Column<double>(type: "float", nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PatientSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OrganizationSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialAids", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialAids_Organizations_OrganizationSSN",
                        column: x => x.OrganizationSSN,
                        principalTable: "Organizations",
                        principalColumn: "SSN",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FinancialAids_Patients_PatientSSN",
                        column: x => x.PatientSSN,
                        principalTable: "Patients",
                        principalColumn: "SSN",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "MedicalInfos",
                columns: table => new
                {
                    PatientSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Id = table.Column<int>(type: "int", nullable: false),
                    DoctorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Diagnosis = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TherapyDeatils = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    endDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RelativeSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalInfos", x => new { x.PatientSSN, x.Id });
                    table.ForeignKey(
                        name: "FK_MedicalInfos_Patients_PatientSSN",
                        column: x => x.PatientSSN,
                        principalTable: "Patients",
                        principalColumn: "SSN",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MedicalInfos_Relatives_RelativeSSN",
                        column: x => x.RelativeSSN,
                        principalTable: "Relatives",
                        principalColumn: "SSN");
                });

            migrationBuilder.CreateTable(
                name: "PatientDisabilities",
                columns: table => new
                {
                    PatientSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisabilityID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    level = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientDisabilities", x => new { x.PatientSSN, x.DisabilityID });
                    table.ForeignKey(
                        name: "FK_PatientDisabilities_Disabilities_DisabilityID",
                        column: x => x.DisabilityID,
                        principalTable: "Disabilities",
                        principalColumn: "SSN",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientDisabilities_Patients_PatientSSN",
                        column: x => x.PatientSSN,
                        principalTable: "Patients",
                        principalColumn: "SSN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PatientWorksAtOrganizations",
                columns: table => new
                {
                    PatientSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    jobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    salary = table.Column<double>(type: "float", nullable: false),
                    startDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientWorksAtOrganizations", x => x.PatientSSN);
                    table.ForeignKey(
                        name: "FK_PatientWorksAtOrganizations_Organizations_OrganizationSSN",
                        column: x => x.OrganizationSSN,
                        principalTable: "Organizations",
                        principalColumn: "SSN",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PatientWorksAtOrganizations_Patients_PatientSSN",
                        column: x => x.PatientSSN,
                        principalTable: "Patients",
                        principalColumn: "SSN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PhysicalTherapies",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    duration = table.Column<int>(type: "int", nullable: false),
                    Doctorname = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    therapyDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PatientSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CenterID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PhysicalTherapies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PhysicalTherapies_Patients_PatientSSN",
                        column: x => x.PatientSSN,
                        principalTable: "Patients",
                        principalColumn: "SSN",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PhysicalTherapies_PhysiotherapyCenters_CenterID",
                        column: x => x.CenterID,
                        principalTable: "PhysiotherapyCenters",
                        principalColumn: "SSN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reports",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CaregiverSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PatientSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProgramOrganizationSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProgramId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reports", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reports_Caregivers_CaregiverSSN",
                        column: x => x.CaregiverSSN,
                        principalTable: "Caregivers",
                        principalColumn: "SSN",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Reports_Patients_PatientSSN",
                        column: x => x.PatientSSN,
                        principalTable: "Patients",
                        principalColumn: "SSN",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reports_Programs_ProgramOrganizationSSN_ProgramId",
                        columns: x => new { x.ProgramOrganizationSSN, x.ProgramId },
                        principalTable: "Programs",
                        principalColumns: new[] { "OrganizationSSN", "Id" },
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    amount = table.Column<double>(type: "float", nullable: false),
                    approvalStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientSSN = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FinancialId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_FinancialAids_FinancialId",
                        column: x => x.FinancialId,
                        principalTable: "FinancialAids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Payments_Patients_PatientSSN",
                        column: x => x.PatientSSN,
                        principalTable: "Patients",
                        principalColumn: "SSN",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AssessmentPatients_PatientSSN",
                table: "AssessmentPatients",
                column: "PatientSSN");

            migrationBuilder.CreateIndex(
                name: "IX_Assessments_ProgramOrganizationSSN_ProgramId",
                table: "Assessments",
                columns: new[] { "ProgramOrganizationSSN", "ProgramId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Caregivers_OrganizationSSN",
                table: "Caregivers",
                column: "OrganizationSSN");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAids_OrganizationSSN",
                table: "FinancialAids",
                column: "OrganizationSSN");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialAids_PatientSSN",
                table: "FinancialAids",
                column: "PatientSSN");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalInfos_RelativeSSN",
                table: "MedicalInfos",
                column: "RelativeSSN");

            migrationBuilder.CreateIndex(
                name: "IX_PatientDisabilities_DisabilityID",
                table: "PatientDisabilities",
                column: "DisabilityID");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_CaregiverSSN",
                table: "Patients",
                column: "CaregiverSSN");

            migrationBuilder.CreateIndex(
                name: "IX_Patients_ProgramOrganizationSSN_ProgramId",
                table: "Patients",
                columns: new[] { "ProgramOrganizationSSN", "ProgramId" });

            migrationBuilder.CreateIndex(
                name: "IX_Patients_RelativeSSN",
                table: "Patients",
                column: "RelativeSSN",
                unique: true,
                filter: "[RelativeSSN] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PatientWorksAtOrganizations_OrganizationSSN",
                table: "PatientWorksAtOrganizations",
                column: "OrganizationSSN");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_FinancialId",
                table: "Payments",
                column: "FinancialId",
                unique: true,
                filter: "[FinancialId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PatientSSN",
                table: "Payments",
                column: "PatientSSN");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalTherapies_CenterID",
                table: "PhysicalTherapies",
                column: "CenterID");

            migrationBuilder.CreateIndex(
                name: "IX_PhysicalTherapies_PatientSSN",
                table: "PhysicalTherapies",
                column: "PatientSSN");

            migrationBuilder.CreateIndex(
                name: "IX_Programs_status",
                table: "Programs",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedMessages_ReceivedSSN",
                table: "ReceivedMessages",
                column: "ReceivedSSN");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivedMessages_SentDate",
                table: "ReceivedMessages",
                column: "SentDate");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_CaregiverSSN",
                table: "Reports",
                column: "CaregiverSSN");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_PatientSSN",
                table: "Reports",
                column: "PatientSSN");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_ProgramOrganizationSSN_ProgramId",
                table: "Reports",
                columns: new[] { "ProgramOrganizationSSN", "ProgramId" });

            migrationBuilder.CreateIndex(
                name: "IX_SentMessages_SentDate",
                table: "SentMessages",
                column: "SentDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AssessmentPatients");

            migrationBuilder.DropTable(
                name: "MedicalInfos");

            migrationBuilder.DropTable(
                name: "PatientDisabilities");

            migrationBuilder.DropTable(
                name: "PatientWorksAtOrganizations");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "PhysicalTherapies");

            migrationBuilder.DropTable(
                name: "ReceivedMessages");

            migrationBuilder.DropTable(
                name: "Reports");

            migrationBuilder.DropTable(
                name: "SentMessages");

            migrationBuilder.DropTable(
                name: "Assessments");

            migrationBuilder.DropTable(
                name: "Disabilities");

            migrationBuilder.DropTable(
                name: "FinancialAids");

            migrationBuilder.DropTable(
                name: "PhysiotherapyCenters");

            migrationBuilder.DropTable(
                name: "Patients");

            migrationBuilder.DropTable(
                name: "Caregivers");

            migrationBuilder.DropTable(
                name: "Programs");

            migrationBuilder.DropTable(
                name: "Relatives");

            migrationBuilder.DropTable(
                name: "Organizations");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}

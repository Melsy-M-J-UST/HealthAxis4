using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HealthAxis3.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAppDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "DoctorId",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Patients",
                keyColumn: "PatientId",
                keyValue: 5);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "DoctorId", "DoctorName", "Experience", "Fees", "IsActive", "Specialisation", "UserId" },
                values: new object[,]
                {
                    { 1, "Dr. Priya Sharma", 12, 800, true, "Cardiologist", null },
                    { 2, "Dr. Suresh Mathew", 9, 600, true, "Dermatologist", null },
                    { 3, "Dr. Neha Iyer", 10, 700, true, "Pediatrician", null },
                    { 4, "Dr. Thomas George", 15, 900, true, "OrthopedicSurgeon", null },
                    { 5, "Dr. Kavitha Rao", 14, 1000, true, "Neurologist", null },
                    { 6, "Dr. Mohammed Ali", 11, 500, true, "GeneralPractitioner", null },
                    { 7, "Dr. Lakshmi Menon", 8, 550, true, "Endocrinologist", null },
                    { 8, "Dr. Rajesh Nambiar", 13, 650, true, "Oncologist", null }
                });

            migrationBuilder.InsertData(
                table: "Patients",
                columns: new[] { "PatientId", "DateOfBirth", "Email", "Gender", "InsuranceId", "IsActive", "PatientName", "PhoneNumber", "RegisteredDate", "UserId" },
                values: new object[,]
                {
                    { 1, new DateTime(1992, 5, 14, 12, 24, 33, 0, DateTimeKind.Unspecified), "arun.kumar@example.com", "Male", "INS1001", true, "Arun Kumar", "9876543210", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 2, new DateTime(1988, 9, 22, 22, 15, 30, 0, DateTimeKind.Unspecified), "meera.nair@example.com", "Female", "INS1002", true, "Meera Nair", "9876543211", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 3, new DateTime(2000, 1, 10, 16, 17, 18, 0, DateTimeKind.Unspecified), "rahul.menon@example.com", "Male", "INS1003", true, "Rahul Menon", "9876543212", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 4, new DateTime(1995, 12, 3, 1, 2, 3, 0, DateTimeKind.Unspecified), "anjali.thomas@example.com", "Female", "INS1004", true, "Anjali Thomas", "9876543213", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null },
                    { 5, new DateTime(1983, 7, 19, 5, 6, 7, 0, DateTimeKind.Unspecified), "vivek.pillai@example.com", "Male", "INS1005", true, "Vivek Pillai", "9876543214", new DateTime(2026, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), null }
                });
        }
    }
}

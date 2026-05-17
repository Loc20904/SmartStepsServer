using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartStepsServer.Migrations
{
    /// <inheritdoc />
    public partial class AddValidationConstraints : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcards_Situations_SituationId",
                table: "Flashcards");

            migrationBuilder.DropForeignKey(
                name: "FK_ParentReviewQuestions_Situations_SituationId",
                table: "ParentReviewQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_ParentReviewQuestions_Skills_SkillId",
                table: "ParentReviewQuestions");

            migrationBuilder.DropForeignKey(
                name: "FK_Situations_Islands_IslandId",
                table: "Situations");

            migrationBuilder.DropForeignKey(
                name: "FK_SituationSkills_Situations_SituationId",
                table: "SituationSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_SituationSkills_Skills_SkillId",
                table: "SituationSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_SituationSteps_Situations_SituationId",
                table: "SituationSteps");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_Flashcards_FlashcardId",
                table: "UserAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswers_Users_UserId",
                table: "UserAnswers");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgresses_Islands_IslandId",
                table: "UserProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgresses_SituationSteps_CurrentStep",
                table: "UserProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgresses_Situations_SituationId",
                table: "UserProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgresses_Users_UserId",
                table: "UserProgresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_ParentId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProgresses",
                table: "UserProgresses");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAnswers",
                table: "UserAnswers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skills",
                table: "Skills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SituationSteps",
                table: "SituationSteps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SituationSkills",
                table: "SituationSkills");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Situations",
                table: "Situations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParentReviewQuestions",
                table: "ParentReviewQuestions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Islands",
                table: "Islands");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Flashcards",
                table: "Flashcards");

            migrationBuilder.RenameTable(
                name: "UserProgresses",
                newName: "UserProgress");

            migrationBuilder.RenameTable(
                name: "UserAnswers",
                newName: "UserAnswer");

            migrationBuilder.RenameTable(
                name: "Skills",
                newName: "Skill");

            migrationBuilder.RenameTable(
                name: "SituationSteps",
                newName: "SituationStep");

            migrationBuilder.RenameTable(
                name: "SituationSkills",
                newName: "SituationSkill");

            migrationBuilder.RenameTable(
                name: "Situations",
                newName: "Situation");

            migrationBuilder.RenameTable(
                name: "ParentReviewQuestions",
                newName: "ParentReviewQuestion");

            migrationBuilder.RenameTable(
                name: "Islands",
                newName: "Island");

            migrationBuilder.RenameTable(
                name: "Flashcards",
                newName: "Flashcard");

            migrationBuilder.RenameIndex(
                name: "IX_UserProgresses_UserId",
                table: "UserProgress",
                newName: "IX_UserProgress_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProgresses_SituationId",
                table: "UserProgress",
                newName: "IX_UserProgress_SituationId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProgresses_IslandId",
                table: "UserProgress",
                newName: "IX_UserProgress_IslandId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProgresses_CurrentStep",
                table: "UserProgress",
                newName: "IX_UserProgress_CurrentStep");

            migrationBuilder.RenameIndex(
                name: "IX_UserAnswers_UserId",
                table: "UserAnswer",
                newName: "IX_UserAnswer_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAnswers_FlashcardId",
                table: "UserAnswer",
                newName: "IX_UserAnswer_FlashcardId");

            migrationBuilder.RenameIndex(
                name: "IX_SituationSteps_SituationId",
                table: "SituationStep",
                newName: "IX_SituationStep_SituationId");

            migrationBuilder.RenameIndex(
                name: "IX_SituationSkills_SkillId",
                table: "SituationSkill",
                newName: "IX_SituationSkill_SkillId");

            migrationBuilder.RenameIndex(
                name: "IX_Situations_IslandId",
                table: "Situation",
                newName: "IX_Situation_IslandId");

            migrationBuilder.RenameIndex(
                name: "IX_ParentReviewQuestions_SkillId",
                table: "ParentReviewQuestion",
                newName: "IX_ParentReviewQuestion_SkillId");

            migrationBuilder.RenameIndex(
                name: "IX_ParentReviewQuestions_SituationId",
                table: "ParentReviewQuestion",
                newName: "IX_ParentReviewQuestion_SituationId");

            migrationBuilder.RenameIndex(
                name: "IX_Flashcards_SituationId",
                table: "Flashcard",
                newName: "IX_Flashcard_SituationId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "UserProgress",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "UserProgress",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastAccessedAt",
                table: "UserProgress",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "UserProgress",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "UserAnswer",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SelectedAnswer",
                table: "UserAnswer",
                type: "char(1)",
                fixedLength: true,
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "UserAnswer",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AnsweredAt",
                table: "UserAnswer",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Skill",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Skill",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "SituationStep",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StepType",
                table: "SituationStep",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "MediaUrl",
                table: "SituationStep",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "SituationStep",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "SkillId",
                table: "SituationSkill",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "SituationId",
                table: "SituationSkill",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Situation",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Situation",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Situation",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ParentReviewQuestion",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ParentReviewQuestion",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Island",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Island",
                type: "varchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Island",
                type: "varchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Island",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Flashcard",
                type: "datetime",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Flashcard",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "CorrectAnswer",
                table: "Flashcard",
                type: "char(1)",
                fixedLength: true,
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1)",
                oldMaxLength: 1);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProgress",
                table: "UserProgress",
                column: "ProgressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAnswer",
                table: "UserAnswer",
                column: "AnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skill",
                table: "Skill",
                column: "SkillId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SituationStep",
                table: "SituationStep",
                column: "StepId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SituationSkill",
                table: "SituationSkill",
                columns: new[] { "SituationId", "SkillId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Situation",
                table: "Situation",
                column: "SituationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParentReviewQuestion",
                table: "ParentReviewQuestion",
                column: "QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Island",
                table: "Island",
                column: "IslandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Flashcard",
                table: "Flashcard",
                column: "FlashcardId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Users_Role",
                table: "Users",
                sql: "[Role] IN ('Child', 'Parent', 'Admin', 'ContentCreator')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_UserProgress_Status",
                table: "UserProgress",
                sql: "[Status] IN ('InProgress', 'Completed')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_UserAnswer_AttemptCount",
                table: "UserAnswer",
                sql: "[AttemptCount] >= 1");

            migrationBuilder.AddCheckConstraint(
                name: "CK_UserAnswer_SelectedAnswer",
                table: "UserAnswer",
                sql: "[SelectedAnswer] IN ('A', 'B')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_SituationStep_StepType",
                table: "SituationStep",
                sql: "[StepType] IN ('Intro', 'Story', 'Flashcard', 'Result')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Situation_Status",
                table: "Situation",
                sql: "[Status] IN ('Draft', 'Pending', 'Approved', 'Rejected', 'Published', 'Hidden')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Island_Status",
                table: "Island",
                sql: "[Status] IN ('Active', 'Hidden')");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Flashcard_CorrectAnswer",
                table: "Flashcard",
                sql: "[CorrectAnswer] IN ('A', 'B')");

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcard_Situation_SituationId",
                table: "Flashcard",
                column: "SituationId",
                principalTable: "Situation",
                principalColumn: "SituationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParentReviewQuestion_Situation_SituationId",
                table: "ParentReviewQuestion",
                column: "SituationId",
                principalTable: "Situation",
                principalColumn: "SituationId");

            migrationBuilder.AddForeignKey(
                name: "FK_ParentReviewQuestion_Skill_SkillId",
                table: "ParentReviewQuestion",
                column: "SkillId",
                principalTable: "Skill",
                principalColumn: "SkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_Situation_Island_IslandId",
                table: "Situation",
                column: "IslandId",
                principalTable: "Island",
                principalColumn: "IslandId");

            migrationBuilder.AddForeignKey(
                name: "FK_SituationSkill_Situation_SituationId",
                table: "SituationSkill",
                column: "SituationId",
                principalTable: "Situation",
                principalColumn: "SituationId");

            migrationBuilder.AddForeignKey(
                name: "FK_SituationSkill_Skill_SkillId",
                table: "SituationSkill",
                column: "SkillId",
                principalTable: "Skill",
                principalColumn: "SkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_SituationStep_Situation_SituationId",
                table: "SituationStep",
                column: "SituationId",
                principalTable: "Situation",
                principalColumn: "SituationId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswer_Flashcard_FlashcardId",
                table: "UserAnswer",
                column: "FlashcardId",
                principalTable: "Flashcard",
                principalColumn: "FlashcardId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswer_Users_UserId",
                table: "UserAnswer",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgress_Island_IslandId",
                table: "UserProgress",
                column: "IslandId",
                principalTable: "Island",
                principalColumn: "IslandId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgress_SituationStep_CurrentStep",
                table: "UserProgress",
                column: "CurrentStep",
                principalTable: "SituationStep",
                principalColumn: "StepId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgress_Situation_SituationId",
                table: "UserProgress",
                column: "SituationId",
                principalTable: "Situation",
                principalColumn: "SituationId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgress_Users_UserId",
                table: "UserProgress",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_ParentId",
                table: "Users",
                column: "ParentId",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcard_Situation_SituationId",
                table: "Flashcard");

            migrationBuilder.DropForeignKey(
                name: "FK_ParentReviewQuestion_Situation_SituationId",
                table: "ParentReviewQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_ParentReviewQuestion_Skill_SkillId",
                table: "ParentReviewQuestion");

            migrationBuilder.DropForeignKey(
                name: "FK_Situation_Island_IslandId",
                table: "Situation");

            migrationBuilder.DropForeignKey(
                name: "FK_SituationSkill_Situation_SituationId",
                table: "SituationSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_SituationSkill_Skill_SkillId",
                table: "SituationSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_SituationStep_Situation_SituationId",
                table: "SituationStep");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswer_Flashcard_FlashcardId",
                table: "UserAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAnswer_Users_UserId",
                table: "UserAnswer");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgress_Island_IslandId",
                table: "UserProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgress_SituationStep_CurrentStep",
                table: "UserProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgress_Situation_SituationId",
                table: "UserProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_UserProgress_Users_UserId",
                table: "UserProgress");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_ParentId",
                table: "Users");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Users_Role",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProgress",
                table: "UserProgress");

            migrationBuilder.DropCheckConstraint(
                name: "CK_UserProgress_Status",
                table: "UserProgress");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAnswer",
                table: "UserAnswer");

            migrationBuilder.DropCheckConstraint(
                name: "CK_UserAnswer_AttemptCount",
                table: "UserAnswer");

            migrationBuilder.DropCheckConstraint(
                name: "CK_UserAnswer_SelectedAnswer",
                table: "UserAnswer");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Skill",
                table: "Skill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SituationStep",
                table: "SituationStep");

            migrationBuilder.DropCheckConstraint(
                name: "CK_SituationStep_StepType",
                table: "SituationStep");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SituationSkill",
                table: "SituationSkill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Situation",
                table: "Situation");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Situation_Status",
                table: "Situation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ParentReviewQuestion",
                table: "ParentReviewQuestion");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Island",
                table: "Island");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Island_Status",
                table: "Island");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Flashcard",
                table: "Flashcard");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Flashcard_CorrectAnswer",
                table: "Flashcard");

            migrationBuilder.RenameTable(
                name: "UserProgress",
                newName: "UserProgresses");

            migrationBuilder.RenameTable(
                name: "UserAnswer",
                newName: "UserAnswers");

            migrationBuilder.RenameTable(
                name: "Skill",
                newName: "Skills");

            migrationBuilder.RenameTable(
                name: "SituationStep",
                newName: "SituationSteps");

            migrationBuilder.RenameTable(
                name: "SituationSkill",
                newName: "SituationSkills");

            migrationBuilder.RenameTable(
                name: "Situation",
                newName: "Situations");

            migrationBuilder.RenameTable(
                name: "ParentReviewQuestion",
                newName: "ParentReviewQuestions");

            migrationBuilder.RenameTable(
                name: "Island",
                newName: "Islands");

            migrationBuilder.RenameTable(
                name: "Flashcard",
                newName: "Flashcards");

            migrationBuilder.RenameIndex(
                name: "IX_UserProgress_UserId",
                table: "UserProgresses",
                newName: "IX_UserProgresses_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProgress_SituationId",
                table: "UserProgresses",
                newName: "IX_UserProgresses_SituationId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProgress_IslandId",
                table: "UserProgresses",
                newName: "IX_UserProgresses_IslandId");

            migrationBuilder.RenameIndex(
                name: "IX_UserProgress_CurrentStep",
                table: "UserProgresses",
                newName: "IX_UserProgresses_CurrentStep");

            migrationBuilder.RenameIndex(
                name: "IX_UserAnswer_UserId",
                table: "UserAnswers",
                newName: "IX_UserAnswers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserAnswer_FlashcardId",
                table: "UserAnswers",
                newName: "IX_UserAnswers_FlashcardId");

            migrationBuilder.RenameIndex(
                name: "IX_SituationStep_SituationId",
                table: "SituationSteps",
                newName: "IX_SituationSteps_SituationId");

            migrationBuilder.RenameIndex(
                name: "IX_SituationSkill_SkillId",
                table: "SituationSkills",
                newName: "IX_SituationSkills_SkillId");

            migrationBuilder.RenameIndex(
                name: "IX_Situation_IslandId",
                table: "Situations",
                newName: "IX_Situations_IslandId");

            migrationBuilder.RenameIndex(
                name: "IX_ParentReviewQuestion_SkillId",
                table: "ParentReviewQuestions",
                newName: "IX_ParentReviewQuestions_SkillId");

            migrationBuilder.RenameIndex(
                name: "IX_ParentReviewQuestion_SituationId",
                table: "ParentReviewQuestions",
                newName: "IX_ParentReviewQuestions_SituationId");

            migrationBuilder.RenameIndex(
                name: "IX_Flashcard_SituationId",
                table: "Flashcards",
                newName: "IX_Flashcards_SituationId");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Users",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Users",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "UserProgresses",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "UserProgresses",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastAccessedAt",
                table: "UserProgresses",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "UserProgresses",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "UserAnswers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SelectedAnswer",
                table: "UserAnswers",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(1)",
                oldFixedLength: true,
                oldMaxLength: 1);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "UserAnswers",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AnsweredAt",
                table: "UserAnswers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Skills",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Skills",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "SituationSteps",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StepType",
                table: "SituationSteps",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "MediaUrl",
                table: "SituationSteps",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "SituationSteps",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<int>(
                name: "SkillId",
                table: "SituationSkills",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 1);

            migrationBuilder.AlterColumn<int>(
                name: "SituationId",
                table: "SituationSkills",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("Relational:ColumnOrder", 0);

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Situations",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Situations",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Situations",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "ParentReviewQuestions",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "ParentReviewQuestions",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Islands",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Islands",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Islands",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Islands",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdatedAt",
                table: "Flashcards",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Flashcards",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<string>(
                name: "CorrectAnswer",
                table: "Flashcards",
                type: "nvarchar(1)",
                maxLength: 1,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "char(1)",
                oldFixedLength: true,
                oldMaxLength: 1);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProgresses",
                table: "UserProgresses",
                column: "ProgressId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAnswers",
                table: "UserAnswers",
                column: "AnswerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Skills",
                table: "Skills",
                column: "SkillId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SituationSteps",
                table: "SituationSteps",
                column: "StepId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SituationSkills",
                table: "SituationSkills",
                columns: new[] { "SituationId", "SkillId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Situations",
                table: "Situations",
                column: "SituationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ParentReviewQuestions",
                table: "ParentReviewQuestions",
                column: "QuestionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Islands",
                table: "Islands",
                column: "IslandId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Flashcards",
                table: "Flashcards",
                column: "FlashcardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcards_Situations_SituationId",
                table: "Flashcards",
                column: "SituationId",
                principalTable: "Situations",
                principalColumn: "SituationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParentReviewQuestions_Situations_SituationId",
                table: "ParentReviewQuestions",
                column: "SituationId",
                principalTable: "Situations",
                principalColumn: "SituationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ParentReviewQuestions_Skills_SkillId",
                table: "ParentReviewQuestions",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "SkillId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Situations_Islands_IslandId",
                table: "Situations",
                column: "IslandId",
                principalTable: "Islands",
                principalColumn: "IslandId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SituationSkills_Situations_SituationId",
                table: "SituationSkills",
                column: "SituationId",
                principalTable: "Situations",
                principalColumn: "SituationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SituationSkills_Skills_SkillId",
                table: "SituationSkills",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "SkillId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SituationSteps_Situations_SituationId",
                table: "SituationSteps",
                column: "SituationId",
                principalTable: "Situations",
                principalColumn: "SituationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_Flashcards_FlashcardId",
                table: "UserAnswers",
                column: "FlashcardId",
                principalTable: "Flashcards",
                principalColumn: "FlashcardId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAnswers_Users_UserId",
                table: "UserAnswers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgresses_Islands_IslandId",
                table: "UserProgresses",
                column: "IslandId",
                principalTable: "Islands",
                principalColumn: "IslandId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgresses_SituationSteps_CurrentStep",
                table: "UserProgresses",
                column: "CurrentStep",
                principalTable: "SituationSteps",
                principalColumn: "StepId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgresses_Situations_SituationId",
                table: "UserProgresses",
                column: "SituationId",
                principalTable: "Situations",
                principalColumn: "SituationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProgresses_Users_UserId",
                table: "UserProgresses",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_ParentId",
                table: "Users",
                column: "ParentId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

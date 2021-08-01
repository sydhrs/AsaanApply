namespace WebProject_NetFramework.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FinalizedMVCcode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AcademicDegreeBoardTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PrerequisiteAcademicDegreeTypeIds = c.String(),
                        Name = c.String(),
                        Value = c.String(),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AcademicDegreeTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        PrerequisiteDegreeTypeId = c.Long(),
                        Name = c.String(),
                        Value = c.String(),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DegreePursuingTypes", t => t.PrerequisiteDegreeTypeId)
                .Index(t => t.PrerequisiteDegreeTypeId);
            
            CreateTable(
                "dbo.DegreePursuingTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Addresses",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Address = c.String(),
                        StateOfRegion = c.String(),
                        ZipCode = c.String(),
                        Email = c.String(),
                        City = c.String(),
                        MobilePhone = c.String(),
                        CreatedBy = c.Long(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Long(),
                        UpdatedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AdmissionChallans",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RefNo = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FeeConcession = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NetAmount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VerifiedBy = c.Long(),
                        IsPaid = c.Boolean(nullable: false),
                        DatePaid = c.DateTime(),
                        StatusReason = c.String(),
                        EntityStatusTypeId = c.Long(),
                        AdmissionId = c.Long(nullable: false),
                        DueDate = c.DateTime(),
                        MimeType = c.String(),
                        PaidChallan = c.Binary(),
                        CreatedBy = c.Long(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Long(),
                        UpdatedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admissions", t => t.AdmissionId, cascadeDelete: true)
                .ForeignKey("dbo.EntityStatusTypes", t => t.EntityStatusTypeId)
                .Index(t => t.EntityStatusTypeId)
                .Index(t => t.AdmissionId);
            
            CreateTable(
                "dbo.Admissions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StudentTypeId = c.Long(nullable: false),
                        DegreePursuingTypeId = c.Long(nullable: false),
                        IsMorningShift = c.Boolean(nullable: false),
                        StudentId = c.Long(nullable: false),
                        EntityStatusTypeId = c.Long(),
                        StatusReason = c.String(),
                        TestGradeId = c.Long(),
                        CreatedBy = c.Long(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Long(),
                        UpdatedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DegreePursuingTypes", t => t.DegreePursuingTypeId, cascadeDelete: true)
                .ForeignKey("dbo.EntityStatusTypes", t => t.EntityStatusTypeId)
                .ForeignKey("dbo.People", t => t.StudentId)
                .ForeignKey("dbo.StudentTypes", t => t.StudentTypeId, cascadeDelete: true)
                .Index(t => t.StudentTypeId)
                .Index(t => t.DegreePursuingTypeId)
                .Index(t => t.StudentId)
                .Index(t => t.EntityStatusTypeId);
            
            CreateTable(
                "dbo.EntityStatusTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        Photo = c.Binary(),
                        DateOfBirth = c.DateTime(),
                        CnicNo = c.String(),
                        GenderTypeId = c.Long(nullable: false),
                        MaritalStatusTypeId = c.Long(),
                        ApplicationUserId = c.Long(),
                        AddressId = c.Long(),
                        PersonContactId = c.Long(),
                        CreatedBy = c.Long(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Long(),
                        UpdatedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        EnrollmentDate = c.DateTime(),
                        SectionId = c.Long(),
                        Designation = c.String(),
                        Discriminator = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sections", t => t.SectionId)
                .ForeignKey("dbo.Addresses", t => t.AddressId)
                .ForeignKey("dbo.ApplicationUsers", t => t.ApplicationUserId)
                .ForeignKey("dbo.GenderTypes", t => t.GenderTypeId, cascadeDelete: true)
                .ForeignKey("dbo.MaritalStatusTypes", t => t.MaritalStatusTypeId)
                .ForeignKey("dbo.PersonContacts", t => t.PersonContactId)
                .Index(t => t.GenderTypeId)
                .Index(t => t.MaritalStatusTypeId)
                .Index(t => t.ApplicationUserId)
                .Index(t => t.AddressId)
                .Index(t => t.PersonContactId)
                .Index(t => t.SectionId);
            
            CreateTable(
                "dbo.ApplicationUsers",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Username = c.String(),
                        Email = c.String(),
                        FirstName = c.String(),
                        MiddleName = c.String(),
                        LastName = c.String(),
                        Password = c.String(),
                        Role = c.String(),
                        ProfilePic = c.Binary(),
                        PersonId = c.Long(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GenderTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MaritalStatusTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.PersonContacts",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        GuardianName = c.String(),
                        GuardianAddress = c.String(),
                        GuardianContact = c.String(),
                        EmergencyName = c.String(),
                        EmergencyContact = c.String(),
                        CreatedBy = c.Long(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Long(),
                        UpdatedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sections",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SectionName = c.String(),
                        Capacity = c.Int(nullable: false),
                        IsMorningShift = c.Boolean(nullable: false),
                        CreatedBy = c.Long(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Long(),
                        UpdatedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StudentTestSchedules",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        StudentId = c.Long(nullable: false),
                        AdmissionId = c.Long(nullable: false),
                        TestScheduleId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Long(),
                        UpdatedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TestSchedules", t => t.TestScheduleId)
                .ForeignKey("dbo.People", t => t.StudentId)
                .ForeignKey("dbo.Admissions", t => t.AdmissionId)
                .Index(t => t.StudentId)
                .Index(t => t.AdmissionId)
                .Index(t => t.TestScheduleId);
            
            CreateTable(
                "dbo.TestSchedules",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        TotalSeat = c.Int(nullable: false),
                        ReservedSeat = c.Int(nullable: false),
                        ExamDate = c.DateTime(nullable: false),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        EntranceTestVenueId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Long(),
                        UpdatedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                        StudentTestSchedule_Id = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EntranceTestVenues", t => t.EntranceTestVenueId, cascadeDelete: true)
                .ForeignKey("dbo.StudentTestSchedules", t => t.StudentTestSchedule_Id)
                .Index(t => t.EntranceTestVenueId)
                .Index(t => t.StudentTestSchedule_Id);
            
            CreateTable(
                "dbo.EntranceTestVenues",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        VenueName = c.String(),
                        VenueAddress = c.String(),
                        OpeningTime = c.DateTime(),
                        ClosingTime = c.DateTime(),
                        CreatedBy = c.Long(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Long(),
                        UpdatedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StudentAcademics",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RollNo = c.String(),
                        MarksObtained = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TotalMarks = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProjectedPercentage = c.Decimal(nullable: false, precision: 18, scale: 2),
                        YearOfPassing = c.String(),
                        AcademicDegreeTypeId = c.Long(nullable: false),
                        AcademicDegreeBoardTypeId = c.Long(),
                        AdmissionId = c.Long(nullable: false),
                        CreatedBy = c.Long(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Long(),
                        UpdatedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AcademicDegreeBoardTypes", t => t.AcademicDegreeBoardTypeId)
                .ForeignKey("dbo.AcademicDegreeTypes", t => t.AcademicDegreeTypeId, cascadeDelete: true)
                .ForeignKey("dbo.Admissions", t => t.AdmissionId)
                .Index(t => t.AcademicDegreeTypeId)
                .Index(t => t.AcademicDegreeBoardTypeId)
                .Index(t => t.AdmissionId);
            
            CreateTable(
                "dbo.StudentTypes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Value = c.String(),
                        Description = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AdmissionCriterias",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Criteria = c.String(),
                        DegreePursuingTypeId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.FacultyInfoes",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        PhoneNo = c.String(),
                        Position = c.String(),
                        Email = c.String(),
                        FacebookName = c.String(),
                        TwitterName = c.String(),
                        SkypeName = c.String(),
                        LinkedInName = c.String(),
                        PicName = c.String(),
                        StaffId = c.Long(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.People", t => t.StaffId)
                .Index(t => t.StaffId);
            
            CreateTable(
                "dbo.LoginSessions",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        SessionToken = c.String(),
                        RequestToken = c.String(),
                        AppUserId = c.Long(nullable: false),
                        Agent = c.String(),
                        Browser = c.String(),
                        OS = c.String(),
                        Device = c.String(),
                        IP = c.String(),
                        CreationDate = c.DateTime(nullable: false),
                        ExpiryDate = c.DateTime(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MeritLists",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        DegreePursuingTypeId = c.Long(),
                        StudentId = c.Long(),
                        TestGradeId = c.Long(),
                        Aggregate = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DegreePursuingTypes", t => t.DegreePursuingTypeId)
                .ForeignKey("dbo.People", t => t.StudentId)
                .ForeignKey("dbo.TestGrades", t => t.TestGradeId)
                .Index(t => t.DegreePursuingTypeId)
                .Index(t => t.StudentId)
                .Index(t => t.TestGradeId);
            
            CreateTable(
                "dbo.TestGrades",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Score = c.String(),
                        Grade = c.String(),
                        Comment = c.String(),
                        StudentId = c.Long(nullable: false),
                        AdmissionId = c.Long(),
                        CreatedBy = c.Long(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Long(),
                        UpdatedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admissions", t => t.AdmissionId)
                .ForeignKey("dbo.People", t => t.StudentId, cascadeDelete: true)
                .Index(t => t.StudentId)
                .Index(t => t.AdmissionId);
            
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Subject = c.String(),
                        Message = c.String(),
                        EntityStatusTypeId = c.Long(),
                        StatusReason = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.EntityStatusTypes", t => t.EntityStatusTypeId)
                .Index(t => t.EntityStatusTypeId);
            
            CreateTable(
                "dbo.News",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Title = c.String(),
                        Detail = c.String(),
                        Date = c.DateTime(),
                        PictureName = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.RoadMaps",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        Name = c.String(),
                        Details = c.String(),
                        DegreePursuingTypeId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.StudentChallans",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        RefNo = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        VerifiedBy = c.Long(),
                        IsPaid = c.Boolean(nullable: false),
                        DatePaid = c.DateTime(),
                        StatusReason = c.String(),
                        EntityStatusTypeId = c.Long(),
                        AdmissionId = c.Long(nullable: false),
                        LastDate = c.DateTime(),
                        MimeType = c.String(),
                        PaidChallan = c.Binary(),
                        CreatedBy = c.Long(),
                        CreatedDate = c.DateTime(),
                        UpdatedBy = c.Long(),
                        UpdatedDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Admissions", t => t.AdmissionId, cascadeDelete: true)
                .ForeignKey("dbo.EntityStatusTypes", t => t.EntityStatusTypeId)
                .Index(t => t.EntityStatusTypeId)
                .Index(t => t.AdmissionId);
            
            CreateTable(
                "dbo.SystemSettings",
                c => new
                    {
                        Id = c.Long(nullable: false, identity: true),
                        AdmissionsOpen = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.StudentChallans", "EntityStatusTypeId", "dbo.EntityStatusTypes");
            DropForeignKey("dbo.StudentChallans", "AdmissionId", "dbo.Admissions");
            DropForeignKey("dbo.People", "PersonContactId", "dbo.PersonContacts");
            DropForeignKey("dbo.People", "MaritalStatusTypeId", "dbo.MaritalStatusTypes");
            DropForeignKey("dbo.People", "GenderTypeId", "dbo.GenderTypes");
            DropForeignKey("dbo.People", "ApplicationUserId", "dbo.ApplicationUsers");
            DropForeignKey("dbo.People", "AddressId", "dbo.Addresses");
            DropForeignKey("dbo.Messages", "EntityStatusTypeId", "dbo.EntityStatusTypes");
            DropForeignKey("dbo.MeritLists", "TestGradeId", "dbo.TestGrades");
            DropForeignKey("dbo.TestGrades", "StudentId", "dbo.People");
            DropForeignKey("dbo.TestGrades", "AdmissionId", "dbo.Admissions");
            DropForeignKey("dbo.MeritLists", "StudentId", "dbo.People");
            DropForeignKey("dbo.MeritLists", "DegreePursuingTypeId", "dbo.DegreePursuingTypes");
            DropForeignKey("dbo.FacultyInfoes", "StaffId", "dbo.People");
            DropForeignKey("dbo.AdmissionChallans", "EntityStatusTypeId", "dbo.EntityStatusTypes");
            DropForeignKey("dbo.AdmissionChallans", "AdmissionId", "dbo.Admissions");
            DropForeignKey("dbo.Admissions", "StudentTypeId", "dbo.StudentTypes");
            DropForeignKey("dbo.StudentTestSchedules", "AdmissionId", "dbo.Admissions");
            DropForeignKey("dbo.StudentAcademics", "AdmissionId", "dbo.Admissions");
            DropForeignKey("dbo.StudentAcademics", "AcademicDegreeTypeId", "dbo.AcademicDegreeTypes");
            DropForeignKey("dbo.StudentAcademics", "AcademicDegreeBoardTypeId", "dbo.AcademicDegreeBoardTypes");
            DropForeignKey("dbo.StudentTestSchedules", "StudentId", "dbo.People");
            DropForeignKey("dbo.TestSchedules", "StudentTestSchedule_Id", "dbo.StudentTestSchedules");
            DropForeignKey("dbo.StudentTestSchedules", "TestScheduleId", "dbo.TestSchedules");
            DropForeignKey("dbo.TestSchedules", "EntranceTestVenueId", "dbo.EntranceTestVenues");
            DropForeignKey("dbo.People", "SectionId", "dbo.Sections");
            DropForeignKey("dbo.Admissions", "StudentId", "dbo.People");
            DropForeignKey("dbo.Admissions", "EntityStatusTypeId", "dbo.EntityStatusTypes");
            DropForeignKey("dbo.Admissions", "DegreePursuingTypeId", "dbo.DegreePursuingTypes");
            DropForeignKey("dbo.AcademicDegreeTypes", "PrerequisiteDegreeTypeId", "dbo.DegreePursuingTypes");
            DropIndex("dbo.StudentChallans", new[] { "AdmissionId" });
            DropIndex("dbo.StudentChallans", new[] { "EntityStatusTypeId" });
            DropIndex("dbo.Messages", new[] { "EntityStatusTypeId" });
            DropIndex("dbo.TestGrades", new[] { "AdmissionId" });
            DropIndex("dbo.TestGrades", new[] { "StudentId" });
            DropIndex("dbo.MeritLists", new[] { "TestGradeId" });
            DropIndex("dbo.MeritLists", new[] { "StudentId" });
            DropIndex("dbo.MeritLists", new[] { "DegreePursuingTypeId" });
            DropIndex("dbo.FacultyInfoes", new[] { "StaffId" });
            DropIndex("dbo.StudentAcademics", new[] { "AdmissionId" });
            DropIndex("dbo.StudentAcademics", new[] { "AcademicDegreeBoardTypeId" });
            DropIndex("dbo.StudentAcademics", new[] { "AcademicDegreeTypeId" });
            DropIndex("dbo.TestSchedules", new[] { "StudentTestSchedule_Id" });
            DropIndex("dbo.TestSchedules", new[] { "EntranceTestVenueId" });
            DropIndex("dbo.StudentTestSchedules", new[] { "TestScheduleId" });
            DropIndex("dbo.StudentTestSchedules", new[] { "AdmissionId" });
            DropIndex("dbo.StudentTestSchedules", new[] { "StudentId" });
            DropIndex("dbo.People", new[] { "SectionId" });
            DropIndex("dbo.People", new[] { "PersonContactId" });
            DropIndex("dbo.People", new[] { "AddressId" });
            DropIndex("dbo.People", new[] { "ApplicationUserId" });
            DropIndex("dbo.People", new[] { "MaritalStatusTypeId" });
            DropIndex("dbo.People", new[] { "GenderTypeId" });
            DropIndex("dbo.Admissions", new[] { "EntityStatusTypeId" });
            DropIndex("dbo.Admissions", new[] { "StudentId" });
            DropIndex("dbo.Admissions", new[] { "DegreePursuingTypeId" });
            DropIndex("dbo.Admissions", new[] { "StudentTypeId" });
            DropIndex("dbo.AdmissionChallans", new[] { "AdmissionId" });
            DropIndex("dbo.AdmissionChallans", new[] { "EntityStatusTypeId" });
            DropIndex("dbo.AcademicDegreeTypes", new[] { "PrerequisiteDegreeTypeId" });
            DropTable("dbo.SystemSettings");
            DropTable("dbo.StudentChallans");
            DropTable("dbo.RoadMaps");
            DropTable("dbo.News");
            DropTable("dbo.Messages");
            DropTable("dbo.TestGrades");
            DropTable("dbo.MeritLists");
            DropTable("dbo.LoginSessions");
            DropTable("dbo.FacultyInfoes");
            DropTable("dbo.AdmissionCriterias");
            DropTable("dbo.StudentTypes");
            DropTable("dbo.StudentAcademics");
            DropTable("dbo.EntranceTestVenues");
            DropTable("dbo.TestSchedules");
            DropTable("dbo.StudentTestSchedules");
            DropTable("dbo.Sections");
            DropTable("dbo.PersonContacts");
            DropTable("dbo.MaritalStatusTypes");
            DropTable("dbo.GenderTypes");
            DropTable("dbo.ApplicationUsers");
            DropTable("dbo.People");
            DropTable("dbo.EntityStatusTypes");
            DropTable("dbo.Admissions");
            DropTable("dbo.AdmissionChallans");
            DropTable("dbo.Addresses");
            DropTable("dbo.DegreePursuingTypes");
            DropTable("dbo.AcademicDegreeTypes");
            DropTable("dbo.AcademicDegreeBoardTypes");
        }
    }
}

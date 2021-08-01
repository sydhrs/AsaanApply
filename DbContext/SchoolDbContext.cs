using System.Data.Entity;
using WebProject_NetFramework.DbContext.DbModels;
//using WebProject_NetFramework.Migrations;

namespace WebProject_NetFramework.DbContext
{
    public class SchoolDbContext : System.Data.Entity.DbContext
    {
        public SchoolDbContext() : base("name=SchoolDBConnectionString")
        {
           // Database.SetInitializer(
            //    new MigrateDatabaseToLatestVersion<SchoolDbContext, Configuration>());
        }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<PersonContact> PersonContacts { get; set; }
        public DbSet<Admission> Admissions { get; set; }
        public DbSet<StudentAcademic> StudentAcademics { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<FacultyInfo> FacultyInfos { get; set; }
        public DbSet<EntityStatusType> EntityStatusTypes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<TestGrade> TestGrades { get; set; }
        public DbSet<TestSchedule> TestSchedules { get; set; }
        public DbSet<EntranceTestVenue> EntranceTestVenues { get; set; }
        public DbSet<StudentChallan> StudentChallans { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<MeritList> MeritLists { get; set; }
        public DbSet<SystemSetting> SystemSettings { get; set; }
        public DbSet<MaritalStatusType> MaritalStatusTypes { get; set; }
        public DbSet<GenderType> GenderTypes { get; set; }
        public DbSet<DegreePursuingType> DegreePursuingTypes { get; set; }
        public DbSet<AcademicDegreeType> AcademicDegreeTypes { get; set; }
        public DbSet<AcademicDegreeBoardType> AcademicDegreeBoardTypes { get; set; }
        public DbSet<StudentType> StudentTypes { get; set; }
        public DbSet<LoginSession> LoginSessions { get; set; }
        public DbSet<RoadMap> RoadMaps { get; set; }
        public DbSet<AdmissionCriteria> AdmissionCriteria { get; set; }
        public DbSet<StudentTestSchedule> StudentTestSchedules { get; set; }
        public DbSet<AdmissionChallan> AdmissionChallans { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Admission>()
                .HasMany(f => f.StudentAcademics)
                .WithRequired(f => f.Admission)
                .HasForeignKey(f => f.AdmissionId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany(f => f.Admissions)
                .WithRequired(f => f.Student)
                .HasForeignKey(f => f.StudentId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Student>()
                .HasMany(f => f.StudentTestSchedules)
                .WithRequired(f => f.Student)
                .HasForeignKey(f => f.StudentId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Admission>()
                .HasMany(f => f.StudentTestSchedules)
                .WithRequired(f => f.Admission)
                .HasForeignKey(f => f.AdmissionId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TestSchedule>()
                .HasMany(f => f.StudentTestSchedules)
                .WithRequired(f => f.TestSchedule)
                .HasForeignKey(f => f.TestScheduleId)
                .WillCascadeOnDelete(false);
        }
    }
}
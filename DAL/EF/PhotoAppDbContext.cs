using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using POC.BL.Domain.delivery;
using POC.BL.Domain.profile;
using POC.BL.Domain.setup;
using POC.BL.Domain.task;
using POC.BL.Domain.user;

namespace POC.DAL.EF
{
    namespace SC.DAL.EF
    {
        public class PhotoAppDbContext : IdentityDbContext<UserAccount>
        {
            public PhotoAppDbContext(DbContextOptions<PhotoAppDbContext> options) : base(options)
            {
                //Database.EnsureCreated();
                Initializer.Initialize(this, dropCreateDatabase: true);
                
            }

            #region DbSetRegion

            #region DeliveryPackage

            public DbSet<Answer> Answers { get; set; }
            public DbSet<Photo> Photos { get; set; }
            public DbSet<QualityScoreEvent> QualityScoreEvents { get; set; }
            public DbSet<SetTaskDelivery> SetTaskDeliveries { get; set; }
            public DbSet<SideAnswer> SideAnswers { get; set; }
            public DbSet<Tag> Tags { get; set; } 
            public DbSet<TaskDelivery> Deliveries { get; set; }
            public DbSet<TeacherTaskDelivery> TeacherTaskDeliveries { get; set; }

            #endregion

            #region ProfilePackage

            public DbSet<GroupMCProfileQuestion> GroupMcProfileQuestions { get; set; }
            public DbSet<GroupProfileOption> GroupProfileOptions { get; set; }
            public DbSet<GroupProfileQuestion> GroupProfileQuestions { get; set; }
            public DbSet<ProfileOption> ProfileOptions { get; set; }
            public DbSet<ProfileQuestion> ProfileQuestions { get; set; }
            public DbSet<StudentMCProfileQuestion> StudentMcProfileQuestions { get; set; }
            public DbSet<StudentProfileOption> StudentProfileOptions { get; set; }
            public DbSet<StudentProfileQuestion> StudentProfileQuestions { get; set; }
            
            

            #endregion

            #region SetupPackage

            public DbSet<SetUp> SetUps { get; set; }
            public DbSet<SetUpAdmin> SetUpAdmins { get; set; }

            #endregion

            #region TaskPackage

            public DbSet<GroupTask> GroupTasks { get; set; }
            public DbSet<Location> Locations { get; set; }
            public DbSet<PhotoQuestion> PhotoQuestions { get; set; }
            public DbSet<SetTask> SetTasks { get; set; }
            public DbSet<SideQuestion> SideQuestions { get; set; }
            public DbSet<SideQuestionOption> SideQuestionOptions { get; set; }
            public DbSet<Task> Tasks { get; set; }
            public DbSet<TeacherTask> TeacherTasks { get; set; }
            

            #endregion

            #region UserPackage

            //public DbSet<Account> Accounts { get; set; }
            public DbSet<Admin> Admins { get; set; }
            public DbSet<Group> Groups { get; set; }
            public DbSet<GroupInfo> GroupInfos { get; set; }
            public DbSet<GroupProfileAnswer> GroupProfileAnswers { get; set; }
            public DbSet<Student> Students { get; set; }
            public DbSet<StudentProfileAnswer> StudentProfileAnswers { get; set; }
            public DbSet<Teacher> Teachers { get; set; }
            public DbSet<User> AppUsers { get; set; }
            public DbSet<FilterProfile> FilterProfiles { get; set; }
            public DbSet<Filter> Filters { get; set; }

            #endregion

            #endregion

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                //optionsBuilder.UseSqlite("Data Source=..\\PhotoAppDb_EFCodeFirst.db");

                // configure logging-information
                optionsBuilder.UseLoggerFactory(LoggerFactory.Create(
                    builder => builder.AddDebug()
                ));
            }
       protected override void OnModelCreating(ModelBuilder modelBuilder)
            {  
                base.OnModelCreating(modelBuilder);
                
                #region Primary keys

                #region delivery
                modelBuilder.Entity<Answer>().HasKey(ans => ans.AnswerId);
                modelBuilder.Entity<SideAnswer>().HasKey(sideAnswer => sideAnswer.SideAnswerId);
                modelBuilder.Entity<Photo>().HasKey(photo => photo.PhotoId);
                modelBuilder.Entity<QualityScoreEvent>().HasKey(qse => qse.QualityScoreEventId);
                modelBuilder.Entity<Tag>().HasKey(tag => tag.TagId);
                modelBuilder.Entity<TaskDelivery>().HasKey(taskDelivery => taskDelivery.TaskDeliveryId);
                #endregion delivery

                #region profile
                modelBuilder.Entity<ProfileOption>().HasKey(po => po.ProfileOptionId);
                modelBuilder.Entity<ProfileQuestion>().HasKey(pq => pq.ProfileQuestionId);
                #endregion profile

                #region setup
                modelBuilder.Entity<SetUp>().HasKey(setup => setup.SetUpId);
                #endregion setup

                #region task
                modelBuilder.Entity<Location>().HasKey(location => location.LocationId);
                modelBuilder.Entity<PhotoQuestion>().HasKey(photoQuestion => photoQuestion.PhotoQuestionId);
                modelBuilder.Entity<SideQuestion>().HasKey(sideQuestion => sideQuestion.SideQuestionId);
                modelBuilder.Entity<SideQuestionOption>().HasKey(sqo => sqo.SideQuestionOptionId);
                modelBuilder.Entity<Task>().HasKey(task => task.TaskId);
                #endregion task

                #region user
                modelBuilder.Entity<Group>().HasKey(group => group.GroupId);
                modelBuilder.Entity<GroupInfo>().HasKey(groupInfo => groupInfo.GroupInfoId);
                modelBuilder.Entity<GroupProfileAnswer>().HasKey(gpa => gpa.GroupProfileAnswerId);
                modelBuilder.Entity<Student>().HasKey(student => student.StudentId);
                modelBuilder.Entity<User>().HasKey(user => user.UserId);
                modelBuilder.Entity<FilterProfile>().HasKey(p => p.FilterProfileId);
                modelBuilder.Entity<Filter>().HasKey(f => f.FilterId);
                #endregion user
                #endregion Primary keys

                #region MyRegion

                modelBuilder.Entity<SetUp>()
                    .HasIndex(s => s.loginIndentifier)
                    .IsUnique();

                modelBuilder.Entity<Group>()
                    .HasIndex(g => g.GroupCode)
                    .IsUnique();
                

                #endregion

                #region UserPackage
                
                
                #region Group
                modelBuilder.Entity<Group>().Property<int>("FK_INFO");
                modelBuilder.Entity<Group>()
                    .HasOne(g => g.Info)
                    .WithOne(gi => gi.Group)
                    .HasForeignKey<Group>("FK_INFO")
                    .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<GroupTask>().Property<int>("TaskFK");
                modelBuilder.Entity<GroupTask>().Property<int>("GroupFK");
                modelBuilder.Entity<GroupTask>().HasKey("TaskFK", "GroupFK");

                modelBuilder.Entity<GroupTask>()
                    .HasOne(gt => gt.Group)
                    .WithMany(g => g.Tasks)
                    .HasForeignKey("GroupFK")
                    .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<GroupTask>()
                    .HasOne(gt => gt.Task)
                    .WithMany(t => t.Groups)
                    .HasForeignKey("TaskFK")
                    .OnDelete(DeleteBehavior.Cascade);
                
                #endregion 

                #region qualityScoreEvent
                /*
                modelBuilder.Entity<QualityScoreEvent>()
                    .HasOne(qScore => qScore.Answer)
                    .WithMany(answer => answer.QualityScoreEvents)
                    .OnDelete(DeleteBehavior.Cascade);
                
                modelBuilder.Entity<QualityScoreEvent>()
                    .HasOne(qScore => qScore.Teacher)
                    .WithMany(t => t.QualityScoreEvents)
                    .OnDelete(DeleteBehavior.SetNull);
                    */
                #endregion 
                #endregion

                #region SetUpPackage

                #region SetUp
                modelBuilder.Entity<SetUpAdmin>().Property<int>("AdminFK");
                modelBuilder.Entity<SetUpAdmin>().Property<int>("SetUpFK");
                modelBuilder.Entity<SetUpAdmin>()
                    .HasKey("SetUpFK", "AdminFK");

                modelBuilder.Entity<SetUpAdmin>()
                    .HasOne(sa => sa.SetUp)
                    .WithMany(a => a.Admins)
                    .HasForeignKey("SetUpFK")
                    .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<SetUpAdmin>()
                    .HasOne(sa => sa.Admin)
                    .WithMany(s => s.SetUps)
                    .HasForeignKey("AdminFK")
                    .OnDelete(DeleteBehavior.Cascade);

                modelBuilder.Entity<SetUp>().Property<int>("PhotoFK");
                modelBuilder.Entity<SetUp>()
                    .HasOne(s => s.Logo)
                    .WithOne(p => p.SetUp)
                    .HasForeignKey<SetUp>("PhotoFK")
                    .IsRequired(false);
                
                    
                #endregion setup
                #endregion SetUpPackage

                modelBuilder.Entity<TaskDelivery>()
                    .HasOne(d => d.Student)
                    .WithMany(s => s.Deliveries)
                    .OnDelete(DeleteBehavior.SetNull);
            }     
            
        }
    }
}
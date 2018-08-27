using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace PERI.Agenda.EF
{
    public partial class AARSContext : DbContext
    {
        public AARSContext()
        {
        }

        public AARSContext(DbContextOptions<AARSContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attendance> Attendance { get; set; }
        public virtual DbSet<Community> Community { get; set; }
        public virtual DbSet<EndUser> EndUser { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<EventCategory> EventCategory { get; set; }
        public virtual DbSet<EventCategoryReport> EventCategoryReport { get; set; }
        public virtual DbSet<EventSection> EventSection { get; set; }
        public virtual DbSet<FirstTimer> FirstTimer { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<GroupCategory> GroupCategory { get; set; }
        public virtual DbSet<GroupMember> GroupMember { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<LookUp> LookUp { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<Recurrence> Recurrence { get; set; }
        public virtual DbSet<Registrant> Registrant { get; set; }
        public virtual DbSet<Report> Report { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Rsvp> Rsvp { get; set; }
        public virtual DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Core.Setting.Configuration.GetValue<string>("ConnectionStrings:DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Attendance>(entity =>
            {
                entity.HasIndex(e => new { e.EventId, e.EventSectionId, e.MemberId })
                    .HasName("IX_Attendance_0")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.DateTimeLogged).HasColumnType("datetime");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.EventSectionId).HasColumnName("EventSectionID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Attendance)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK_Attendance_Event");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Attendance)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_Attendance_Member");
            });

            modelBuilder.Entity<Community>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).HasMaxLength(50);

                entity.Property(e => e.ContactNumber).HasMaxLength(50);

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateExpiration).HasColumnType("datetime");

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Remarks).HasMaxLength(50);

                entity.Property(e => e.Utc).HasColumnName("UTC");
            });

            modelBuilder.Entity<EndUser>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("EndUser", "prompt");

                entity.HasIndex(e => e.MemberId)
                    .HasName("IX_EndUser")
                    .IsUnique();

                entity.Property(e => e.ConfirmationCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ConfirmationExpiry).HasColumnType("datetime");

                entity.Property(e => e.DateConfirmed).HasColumnType("datetime");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateInactive).HasColumnType("datetime");

                entity.Property(e => e.LastFailedPasswordAttempt).HasColumnType("datetime");

                entity.Property(e => e.LastLoginDate).HasColumnType("datetime");

                entity.Property(e => e.LastPasswordChanged).HasColumnType("datetime");

                entity.Property(e => e.LastSessionId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordExpiry).HasColumnType("datetime");

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.Member)
                    .WithOne(p => p.EndUser)
                    .HasForeignKey<EndUser>(d => d.MemberId)
                    .HasConstraintName("FK_EndUser_Member");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.EndUser)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_User_Role");
            });

            modelBuilder.Entity<Event>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.EventCategoryId, e.DateTimeStart, e.LocationId })
                    .HasName("IX_Event")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.DateTimeCreated).HasColumnType("datetime");

                entity.Property(e => e.DateTimeEnd).HasColumnType("datetime");

                entity.Property(e => e.DateTimeModified).HasColumnType("datetime");

                entity.Property(e => e.DateTimeStart).HasColumnType("datetime");

                entity.Property(e => e.EventCategoryId).HasColumnName("EventCategoryID");

                entity.Property(e => e.LocationId).HasColumnName("LocationID");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RecurrenceId).HasColumnName("RecurrenceID");

                entity.HasOne(d => d.EventCategory)
                    .WithMany(p => p.Event)
                    .HasForeignKey(d => d.EventCategoryId)
                    .HasConstraintName("FK_Event_EventCategory");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Event)
                    .HasForeignKey(d => d.LocationId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Event_Location");
            });

            modelBuilder.Entity<EventCategory>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.CommunityId })
                    .HasName("IX_EventCategory")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CommunityId).HasColumnName("CommunityID");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.DateTimeCreated).HasColumnType("datetime");

                entity.Property(e => e.DateTimeModified).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<EventCategoryReport>(entity =>
            {
                entity.HasKey(e => new { e.EventCategoryId, e.ReportId });

                entity.ToTable("EventCategoryReport", "prompt");

                entity.HasOne(d => d.EventCategory)
                    .WithMany(p => p.EventCategoryReport)
                    .HasForeignKey(d => d.EventCategoryId)
                    .HasConstraintName("FK_EventCategoryReport_EventCategory");

                entity.HasOne(d => d.Report)
                    .WithMany(p => p.EventCategoryReport)
                    .HasForeignKey(d => d.ReportId)
                    .HasConstraintName("FK_EventCategoryReport_Report");
            });

            modelBuilder.Entity<EventSection>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.EventCategoryId })
                    .HasName("IX_EventTopic")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.DateTimeCreated).HasColumnType("datetime");

                entity.Property(e => e.DateTimeModified).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.EventCategoryId).HasColumnName("EventCategoryID");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.EventCategory)
                    .WithMany(p => p.EventSection)
                    .HasForeignKey(d => d.EventCategoryId)
                    .HasConstraintName("FK_EventSection_EventCategory");
            });

            modelBuilder.Entity<FirstTimer>(entity =>
            {
                entity.HasKey(e => e.AttendanceId);

                entity.ToTable("FirstTimer", "prompt");

                entity.Property(e => e.AttendanceId).ValueGeneratedNever();

                entity.HasOne(d => d.Attendance)
                    .WithOne(p => p.FirstTimer)
                    .HasForeignKey<FirstTimer>(d => d.AttendanceId)
                    .HasConstraintName("FK_AttendanceId_Attendance");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.GroupCategoryId })
                    .HasName("IX_Group")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.GroupCategoryId).HasColumnName("GroupCategoryID");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.GroupCategory)
                    .WithMany(p => p.Group)
                    .HasForeignKey(d => d.GroupCategoryId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Group_GroupCategory");
            });

            modelBuilder.Entity<GroupCategory>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.CommunityId })
                    .HasName("IX_GroupCategory")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CommunityId).HasColumnName("CommunityID");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.DateTimeCreated).HasColumnType("datetime");

                entity.Property(e => e.DateTimeModified).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<GroupMember>(entity =>
            {
                entity.HasIndex(e => new { e.GroupId, e.MemberId })
                    .HasName("IX_GroupMember")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.GroupMember)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_GroupMember_Group");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.GroupMember)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_GroupMember_Member");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.CommunityId })
                    .HasName("IX_Location")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).IsRequired();

                entity.Property(e => e.CommunityId).HasColumnName("CommunityID");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.DateTimeCreated).HasColumnType("datetime");

                entity.Property(e => e.DateTimeModified).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<LookUp>(entity =>
            {
                entity.HasIndex(e => e.Id)
                    .HasName("IX_LookUp")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Group).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);

                entity.Property(e => e.Value).HasMaxLength(50);
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasIndex(e => new { e.Name, e.CommunityId })
                    .HasName("IX_Member")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.BirthDate).HasColumnType("datetime");

                entity.Property(e => e.CommunityId).HasColumnName("CommunityID");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Mobile).HasMaxLength(50);

                entity.Property(e => e.ModifiedBy).HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.NickName).HasMaxLength(50);
            });

            modelBuilder.Entity<Recurrence>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.End).HasColumnType("datetime");

                entity.Property(e => e.Pattern).HasMaxLength(50);

                entity.Property(e => e.Start).HasColumnType("datetime");
            });

            modelBuilder.Entity<Registrant>(entity =>
            {
                entity.HasIndex(e => new { e.MemberId, e.EventId })
                    .HasName("IX_Registrant")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.EventId).HasColumnName("EventID");

                entity.Property(e => e.GroupId).HasColumnName("GroupID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Registrant)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK_Registrant_Event");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Registrant)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_Registrant_Member");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("Report", "prompt");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Community)
                    .WithMany(p => p.Report)
                    .HasForeignKey(d => d.CommunityId)
                    .HasConstraintName("FK_Report_Community");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role", "prompt");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Rsvp>(entity =>
            {
                entity.HasKey(e => new { e.EventId, e.MemberId });

                entity.ToTable("RSVP", "prompt");

                entity.HasIndex(e => new { e.EventId, e.MemberId })
                    .HasName("UQ_RSVP_EventId_MemberId")
                    .IsUnique();

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateModified).HasColumnType("datetime");

                entity.HasOne(d => d.Event)
                    .WithMany(p => p.Rsvp)
                    .HasForeignKey(d => d.EventId)
                    .HasConstraintName("FK_RSVP_Event");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Rsvp)
                    .HasForeignKey(d => d.MemberId)
                    .HasConstraintName("FK_RSVP_Member");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.CommunityId).HasColumnName("CommunityID");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.UserName).HasMaxLength(50);
            });
        }
    }
}

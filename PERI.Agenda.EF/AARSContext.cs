using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace PERI.Agenda.EF
{
    public partial class AARSContext : DbContext
    {
        public virtual DbSet<AspnetApplications> AspnetApplications { get; set; }
        public virtual DbSet<AspnetMembership> AspnetMembership { get; set; }
        public virtual DbSet<AspnetPaths> AspnetPaths { get; set; }
        public virtual DbSet<AspnetPersonalizationAllUsers> AspnetPersonalizationAllUsers { get; set; }
        public virtual DbSet<AspnetPersonalizationPerUser> AspnetPersonalizationPerUser { get; set; }
        public virtual DbSet<AspnetProfile> AspnetProfile { get; set; }
        public virtual DbSet<AspnetRoles> AspnetRoles { get; set; }
        public virtual DbSet<AspnetSchemaVersions> AspnetSchemaVersions { get; set; }
        public virtual DbSet<AspnetUsers> AspnetUsers { get; set; }
        public virtual DbSet<AspnetUsersInRoles> AspnetUsersInRoles { get; set; }
        public virtual DbSet<AspnetWebEventEvents> AspnetWebEventEvents { get; set; }
        public virtual DbSet<Attendance> Attendance { get; set; }
        public virtual DbSet<Community> Community { get; set; }
        public virtual DbSet<EndUser> EndUser { get; set; }
        public virtual DbSet<Event> Event { get; set; }
        public virtual DbSet<EventCategory> EventCategory { get; set; }
        public virtual DbSet<EventSection> EventSection { get; set; }
        public virtual DbSet<Group> Group { get; set; }
        public virtual DbSet<GroupCategory> GroupCategory { get; set; }
        public virtual DbSet<GroupMember> GroupMember { get; set; }
        public virtual DbSet<Location> Location { get; set; }
        public virtual DbSet<LookUp> LookUp { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<Recurrence> Recurrence { get; set; }
        public virtual DbSet<Registrant> Registrant { get; set; }
        public virtual DbSet<Role> Role { get; set; }
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
            modelBuilder.Entity<AspnetApplications>(entity =>
            {
                entity.HasKey(e => e.ApplicationId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("aspnet_Applications");

                entity.HasIndex(e => e.ApplicationName)
                    .HasName("UQ__aspnet_A__309103318F4C5A18")
                    .IsUnique();

                entity.HasIndex(e => e.LoweredApplicationName)
                    .HasName("aspnet_Applications_Index")
                    .ForSqlServerIsClustered();

                entity.Property(e => e.ApplicationId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.ApplicationName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Description).HasMaxLength(256);

                entity.Property(e => e.LoweredApplicationName)
                    .IsRequired()
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<AspnetMembership>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("aspnet_Membership");

                entity.HasIndex(e => new { e.ApplicationId, e.LoweredEmail })
                    .HasName("aspnet_Membership_index")
                    .ForSqlServerIsClustered();

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.Comment).HasColumnType("ntext");

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FailedPasswordAnswerAttemptWindowStart).HasColumnType("datetime");

                entity.Property(e => e.FailedPasswordAttemptWindowStart).HasColumnType("datetime");

                entity.Property(e => e.LastLockoutDate).HasColumnType("datetime");

                entity.Property(e => e.LastLoginDate).HasColumnType("datetime");

                entity.Property(e => e.LastPasswordChangedDate).HasColumnType("datetime");

                entity.Property(e => e.LoweredEmail).HasMaxLength(256);

                entity.Property(e => e.MobilePin)
                    .HasColumnName("MobilePIN")
                    .HasMaxLength(16);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.PasswordAnswer).HasMaxLength(128);

                entity.Property(e => e.PasswordFormat).HasDefaultValueSql("((0))");

                entity.Property(e => e.PasswordQuestion).HasMaxLength(256);

                entity.Property(e => e.PasswordSalt)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.AspnetMembership)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Me__Appli__6442E2C9");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.AspnetMembership)
                    .HasForeignKey<AspnetMembership>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Me__UserI__65370702");
            });

            modelBuilder.Entity<AspnetPaths>(entity =>
            {
                entity.HasKey(e => e.PathId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("aspnet_Paths");

                entity.HasIndex(e => new { e.ApplicationId, e.LoweredPath })
                    .HasName("aspnet_Paths_index")
                    .IsUnique()
                    .ForSqlServerIsClustered();

                entity.Property(e => e.PathId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.LoweredPath)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Path)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.AspnetPaths)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Pa__Appli__662B2B3B");
            });

            modelBuilder.Entity<AspnetPersonalizationAllUsers>(entity =>
            {
                entity.HasKey(e => e.PathId);

                entity.ToTable("aspnet_PersonalizationAllUsers");

                entity.Property(e => e.PathId).ValueGeneratedNever();

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.PageSettings)
                    .IsRequired()
                    .HasColumnType("image");

                entity.HasOne(d => d.Path)
                    .WithOne(p => p.AspnetPersonalizationAllUsers)
                    .HasForeignKey<AspnetPersonalizationAllUsers>(d => d.PathId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Pe__PathI__671F4F74");
            });

            modelBuilder.Entity<AspnetPersonalizationPerUser>(entity =>
            {
                entity.ToTable("aspnet_PersonalizationPerUser");

                entity.HasIndex(e => new { e.PathId, e.UserId })
                    .HasName("aspnet_PersonalizationPerUser_index1")
                    .IsUnique()
                    .ForSqlServerIsClustered();

                entity.HasIndex(e => new { e.UserId, e.PathId })
                    .HasName("aspnet_PersonalizationPerUser_ncindex2")
                    .IsUnique();

                entity.Property(e => e.Id).HasDefaultValueSql("(newid())");

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.PageSettings)
                    .IsRequired()
                    .HasColumnType("image");

                entity.HasOne(d => d.Path)
                    .WithMany(p => p.AspnetPersonalizationPerUser)
                    .HasForeignKey(d => d.PathId)
                    .HasConstraintName("FK__aspnet_Pe__PathI__681373AD");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspnetPersonalizationPerUser)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__aspnet_Pe__UserI__690797E6");
            });

            modelBuilder.Entity<AspnetProfile>(entity =>
            {
                entity.HasKey(e => e.UserId);

                entity.ToTable("aspnet_Profile");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.LastUpdatedDate).HasColumnType("datetime");

                entity.Property(e => e.PropertyNames)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.PropertyValuesBinary)
                    .IsRequired()
                    .HasColumnType("image");

                entity.Property(e => e.PropertyValuesString)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.AspnetProfile)
                    .HasForeignKey<AspnetProfile>(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Pr__UserI__69FBBC1F");
            });

            modelBuilder.Entity<AspnetRoles>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("aspnet_Roles");

                entity.HasIndex(e => new { e.ApplicationId, e.LoweredRoleName })
                    .HasName("aspnet_Roles_index1")
                    .IsUnique()
                    .ForSqlServerIsClustered();

                entity.Property(e => e.RoleId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Description).HasMaxLength(256);

                entity.Property(e => e.LoweredRoleName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.AspnetRoles)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Ro__Appli__6AEFE058");
            });

            modelBuilder.Entity<AspnetSchemaVersions>(entity =>
            {
                entity.HasKey(e => new { e.Feature, e.CompatibleSchemaVersion });

                entity.ToTable("aspnet_SchemaVersions");

                entity.Property(e => e.Feature).HasMaxLength(128);

                entity.Property(e => e.CompatibleSchemaVersion).HasMaxLength(128);
            });

            modelBuilder.Entity<AspnetUsers>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .ForSqlServerIsClustered(false);

                entity.ToTable("aspnet_Users");

                entity.HasIndex(e => new { e.ApplicationId, e.LastActivityDate })
                    .HasName("aspnet_Users_Index2");

                entity.HasIndex(e => new { e.ApplicationId, e.LoweredUserName })
                    .HasName("aspnet_Users_Index")
                    .IsUnique()
                    .ForSqlServerIsClustered();

                entity.Property(e => e.UserId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.LastActivityDate).HasColumnType("datetime");

                entity.Property(e => e.LoweredUserName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.MobileAlias).HasMaxLength(16);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.AspnetUsers)
                    .HasForeignKey(d => d.ApplicationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Us__Appli__6BE40491");
            });

            modelBuilder.Entity<AspnetUsersInRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.ToTable("aspnet_UsersInRoles");

                entity.HasIndex(e => e.RoleId)
                    .HasName("aspnet_UsersInRoles_index");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspnetUsersInRoles)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Us__RoleI__6CD828CA");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspnetUsersInRoles)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__aspnet_Us__UserI__6DCC4D03");
            });

            modelBuilder.Entity<AspnetWebEventEvents>(entity =>
            {
                entity.HasKey(e => e.EventId);

                entity.ToTable("aspnet_WebEvent_Events");

                entity.Property(e => e.EventId)
                    .HasColumnType("char(32)")
                    .ValueGeneratedNever();

                entity.Property(e => e.ApplicationPath).HasMaxLength(256);

                entity.Property(e => e.ApplicationVirtualPath).HasMaxLength(256);

                entity.Property(e => e.Details).HasColumnType("ntext");

                entity.Property(e => e.EventOccurrence).HasColumnType("decimal(19, 0)");

                entity.Property(e => e.EventSequence).HasColumnType("decimal(19, 0)");

                entity.Property(e => e.EventTime).HasColumnType("datetime");

                entity.Property(e => e.EventTimeUtc).HasColumnType("datetime");

                entity.Property(e => e.EventType)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.ExceptionType).HasMaxLength(256);

                entity.Property(e => e.MachineName)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.Message).HasMaxLength(1024);

                entity.Property(e => e.RequestUrl).HasMaxLength(1024);
            });

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

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role", "prompt");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false);
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

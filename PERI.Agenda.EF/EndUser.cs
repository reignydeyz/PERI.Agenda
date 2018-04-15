using System;
using System.Collections.Generic;

namespace PERI.Agenda.EF
{
    public partial class EndUser
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int RoleId { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public DateTime? PasswordExpiry { get; set; }
        public string LastSessionId { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public DateTime? LastPasswordChanged { get; set; }
        public int FailedPasswordCount { get; set; }
        public DateTime? LastFailedPasswordAttempt { get; set; }
        public string ConfirmationCode { get; set; }
        public DateTime? ConfirmationExpiry { get; set; }
        public DateTime? DateConfirmed { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateInactive { get; set; }
        public int? CommunityId { get; set; }

        public Role Role { get; set; }
    }
}

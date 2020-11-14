using System.Collections.Generic;
using ApplicationCore.Entities.AnswerAggregate;
using ApplicationCore.Entities.NotificationAggregate;
using Microsoft.AspNetCore.Identity;

namespace ApplicationCore.Entities.Identity
{
    public class User : IdentityUser<int>
    {
        public virtual ICollection<UserClaim> Claims { get; set; }
        public virtual ICollection<UserLogin> Logins { get; set; }
        public virtual ICollection<UserToken> Tokens { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Address { get; set; }
        public virtual List<Answer> Answers { get; set; }
        public virtual List<Notification> Notifications { get; set; }
    }
}

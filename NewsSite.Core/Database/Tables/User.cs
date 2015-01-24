using System;
using System.Collections.Generic;

namespace NewsSite.Core.Database.Tables
{
    public class User : BaseEntity
    {
        public User()
        {
            Roles = new HashSet<Role>();
        }

        public string UserName { get; set; }
        public string RealName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsConfirmed { get; set; }
        public Guid ConfirmationId { get; set; }
        public string ImgUrl { get; set; }
        public string ImgUrlSmall { get; set; }
        public string ImgUrlMiddle { get; set; }
        public string ImgUrlBig { get; set; }
        public DateTime LastLoginDate { get; set; }
        public string LastLoginIP { get; set; }
        public string Website { get; set; }

        public virtual ICollection<Role> Roles { get; set; }
    }
}

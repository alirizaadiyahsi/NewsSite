using NewsSite.Core.Application;
using FormsAuthenticationExtensions;
using System;
using System.Web;
using System.Web.Security;

namespace NewsSite.Web.Framework.Membership
{
    public static class CustomMembership
    {
        public static CurrentUser CurrentUser()
        {
            var currentUser = HttpContext.Current.User;
            var ticketData = ((FormsIdentity)currentUser.Identity).Ticket.GetStructuredUserData();
            var user = new CurrentUser();

            user.Id = Int32.Parse(ticketData[Keys.Id]);
            user.Email = ticketData[Keys.Email];
            user.RealName = ticketData[Keys.RealName];
            user.Name = currentUser.Identity.Name;

            return user;
        }
    }

    public class CurrentUser
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string RealName { get; set; }
        public string Name { get; set; }
    }
}

using NewsSite.Core.Database.Tables;
using System;
using System.Linq;

namespace NewsSite.Service.MembershipServices
{
    public interface IMembershipService
    {
        /// <summary>
        /// Kullanıcı bul.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        User FindByUserNameAndPassword(string userName, string password);

        /// <summary>
        /// Kullanıcı bul.
        /// </summary>
        /// <param name="confirmationId"></param>
        /// <returns></returns>
        User FindByConfirmationId(Guid confirmationId);

        /// <summary>
        /// Yeni üye olan kullanıcıya onay mesajı gönder.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ConfirmationUrl"></param>
        /// <returns>Email send success status</returns>
        bool SendConfirmationMail(User user, string ConfirmationUrl);

        /// <summary>
        /// Eposta sistemde kayıtlı mı.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        bool ValidateEmail(string email);

        /// <summary>
        /// Kullanıcı adı sistemde kayıtlı mı.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        bool ValidateUserName(string userName);

        /// <summary>
        /// Rol role sahip mi.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        bool IsUserInRole(string userName, string roleName);

        /// <summary>
        /// Kullanıcıya göre roller.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        IQueryable<Role> GetRolesByUserName(string userName);

        /// <summary>
        /// Kullanıcı ekle.
        /// </summary>
        /// <param name="user"></param>
        void Insert(User user);

        /// <summary>
        /// Kullanıcı güncelle.
        /// </summary>
        /// <param name="user"></param>
        void Update(User user);

        IQueryable<User> GetAllUsers();
    }
}

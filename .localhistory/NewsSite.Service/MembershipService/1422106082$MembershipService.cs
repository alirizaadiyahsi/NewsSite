using NewsSite.Core.Database.Tables;
using NewsSite.Data.Repository;
using NewsSite.Data.UnitOfWork;
using System;
using System.Linq;
using System.Net.Mail;

namespace NewsSite.Service.MembershipService
{
    public class MembershipService : IMembershipService
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IGenericRepository<Role> _roleRepository;

        public MembershipService(IUnitOfWork uow)
        {
            _userRepository = uow.GetRepository<User>();
            _roleRepository = uow.GetRepository<Role>();
        }

        /// <summary>
        /// Kullanıcı bul.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public User FindByUserNameAndPassword(string userName, string password)
        {
            return _userRepository.GetAll().FirstOrDefault(x => x.UserName == userName && x.Password == password);
        }

        /// <summary>
        /// Kullanıcı bul.
        /// </summary>
        /// <param name="confirmationId"></param>
        /// <returns></returns>
        public User FindByConfirmationId(Guid confirmationId)
        {
            return _userRepository.GetAll().FirstOrDefault(x => x.ConfirmationId == confirmationId);
        }

        /// <summary>
        /// Yeni üye olan kullanıcıya onay mesajı gönder.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ConfirmationUrl"></param>
        /// <returns>Email send success status</returns>
        public bool SendConfirmationMail(User user, string ConfirmationUrl)
        {
            var status = false;
            string confirmationId = user.ConfirmationId.ToString();
            ConfirmationUrl += "/Account/ConfirmUser?confirmationId=" + confirmationId;

            var message = new MailMessage("info@gundemdisi.net", user.Email)
            {
                Subject = "Lütfen e-posta adresinizi onaylayınız.",
                Body = ConfirmationUrl
            };

            var client = new SmtpClient();
            try
            {
                client.Send(message);
                status = true;
            }
            catch (System.Exception)
            {
                return status;
            }

            return status;
        }

        /// <summary>
        /// Eposta sistemde kayıtlı mı.
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool ValidateEmail(string email)
        {
            return _userRepository.GetAll().Any(x => x.Email == email);
        }

        /// <summary>
        /// Kullanıcı adı sistemde kayıtlı mı.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool ValidateUserName(string userName)
        {
            return _userRepository.GetAll().Any(x => x.UserName == userName);
        }

        /// <summary>
        /// Kullanıcıya göre roller.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public IQueryable<Role> GetRolesByUserName(string userName)
        {
            return _userRepository.GetAll().FirstOrDefault(x => x.UserName == userName).Roles.AsQueryable();
        }

        /// <summary>
        /// Rol role sahip mi.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public bool IsUserInRole(string userName, string roleName)
        {
            return _userRepository.GetAll().FirstOrDefault(x => x.UserName == userName).Roles.Any(x => x.Name == roleName);
        }

        /// <summary>
        /// Kullanıcı ekle.
        /// </summary>
        /// <param name="user"></param>
        public void Insert(User user)
        {
            _userRepository.Insert(user);
        }

        /// <summary>
        /// Kullanıcı güncelle.
        /// </summary>
        /// <param name="user"></param>
        public void Update(User user)
        {
            _userRepository.Update(user);
        }
    }
}

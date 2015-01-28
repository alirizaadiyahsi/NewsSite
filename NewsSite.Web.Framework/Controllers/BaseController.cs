using NewsSite.Core.Application;
using NewsSite.Data.UnitOfWork;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NewsSite.Web.Framework.Controllers
{
    public class BaseController : Controller
    {
        protected readonly IUnitOfWork _uow;
        public List<string> messagesForView = new List<string>();

        public BaseController(IUnitOfWork uow)
        {
            _uow = uow;
        }

        //protected override void OnException(ExceptionContext filterContext)
        //{
        //    if (filterContext.ExceptionHandled)
        //        return;

        //    //Let the request know what went wrong
        //    Error(filterContext.Exception.Message);

        //    //redirect to error handler
        //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(
        //        new { controller = "Common", action = "HandleError", area = "" }));

        //    // Stop any other exception handlers from running
        //    filterContext.ExceptionHandled = true;

        //    // CLear out anything already in the response
        //    filterContext.HttpContext.Response.Clear();

        //    base.OnException(filterContext);
        //}

        #region messages for view
        public void Default(List<string> messages)
        {
            AddMessage(messages, MessageTypes.Default);
        }

        public void Info(List<string> messages)
        {
            AddMessage(messages, MessageTypes.Info);
        }

        public void Success(List<string> messages)
        {
            AddMessage(messages, MessageTypes.Success);
        }

        public void Warning(List<string> messages)
        {
            AddMessage(messages, MessageTypes.Warning);
        }

        public void Error(List<string> messages)
        {
            AddMessage(messages, MessageTypes.Danger);
        }
        #endregion

        #region private methods
        private void AddMessage(List<string> msgList, MessageTypes type)
        {
            var messages = new List<MessageForView>();

            if (TempData[Keys.MessageForView] != null)
                messages = (List<MessageForView>)TempData[Keys.MessageForView];

            foreach (var msg in msgList)
            {
                messages.Add(new MessageForView { MessageType = type, Message = msg });
            }

            TempData[Keys.MessageForView] = messages;
        }
        #endregion
    }
}

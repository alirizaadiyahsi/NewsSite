using NewsSite.Core.Application;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NewsSite.Web.Framework.HtmlHelpers
{
    public static class DisplayHelpers
    {
        public static MvcHtmlString DisplayMessage(this HtmlHelper htmlHelper)
        {
            var tempMessages = htmlHelper.ViewContext.TempData[Keys.MessageForView];
            if (tempMessages == null) return MvcHtmlString.Create(string.Empty);

            var messages = (List<MessageForView>)tempMessages;

            var messageToDisplay = "<div class=\"msg\">";

            messageToDisplay = messages.Aggregate(messageToDisplay, (s, msg) =>
            {
                s += "<div class=\"alert alert-" + msg.MessageType.ToString().ToLower() + " alert-dismissable\">" +
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-hidden=\"true\">&times;</button>" +
                    msg.Message +
                    "</div>";
                return s;
            });

            messageToDisplay += "</div>";

            return MvcHtmlString.Create(messageToDisplay);
        }
    }
}

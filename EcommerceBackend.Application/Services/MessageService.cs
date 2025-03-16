
using EcommerceBackend.Application.Resources;

namespace EcommerceBackend.Application.Services
{
    public class MessageService
    {
        public string GetMessage(string messageCode)
        {
            var culture = Thread.CurrentThread.CurrentUICulture;
             return MessagesEn.ResourceManager.GetString(messageCode, culture);
           // return "";
        }
    }
}
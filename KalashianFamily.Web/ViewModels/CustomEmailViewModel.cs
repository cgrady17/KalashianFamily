using System.Web.Mvc;

namespace KalashianFamily.Web.ViewModels
{
    public class CustomEmailViewModel
    {
        [AllowHtml]
        public string Body { get; set; }

        public string Subject { get; set; }

        public string RecipientSelection { get; set; }
    }
}
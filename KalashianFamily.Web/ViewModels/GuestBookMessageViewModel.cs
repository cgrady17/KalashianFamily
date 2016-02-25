using System.ComponentModel.DataAnnotations;

namespace KalashianFamily.Web.ViewModels
{
    public class GuestBookMessageViewModel
    {
        [Display(Name = "Your Name")]
        [Required]
        [StringLength(150)]
        public string Name { get; set; }

        [Display(Name = "Your Message")]
        [Required]
        [StringLength(500, MinimumLength = 5)]
        public string Message { get; set; }

        #region Anti-Bot
        public string Website { get; set; }
        public string CaptchaPass { get; set; }
        #endregion
    }
}
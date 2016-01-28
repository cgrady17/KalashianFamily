using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace KalashianFamily.Web.ViewModels
{
    public class RsvpViewModel
    {
        public RsvpViewModel()
        {
            Attendees = new List<RsvpAttendeeViewModel>();
        }

        [Display(Name = "Will your Party be attending the Wedding?")]
        [Required]
        public bool Attending { get; set; }

        [Display(Name = "Are there others in our Party beside yourself?")]
        [Required]
        public bool OtherAttendees { get; set; }

        [Display(Name = "What's your estimated date of arrival?")]
        [Required]
        public DateTime ArrivalDate { get; set; }

        [Display(Name = "What's an Email Address at which we can contact you?")]
        [EmailAddress]
        [Required]
        [StringLength(75)]
        public string EmailAddress { get; set; }

        [Display(Name = "Attendee Name")]
        [Required]
        [StringLength(150, MinimumLength = 5)]
        public string Name { get; set; }

        public List<RsvpAttendeeViewModel> Attendees { get; set; } 
    }
}
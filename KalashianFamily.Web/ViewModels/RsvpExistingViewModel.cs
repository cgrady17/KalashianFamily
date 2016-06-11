using System;

namespace KalashianFamily.Web.ViewModels
{
    public class RsvpExistingViewModel
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public string EmailAddress { get; set; }

        public bool Attending { get; set; }

        public DateTime? ArrivalDate { get; set; }
    }
}
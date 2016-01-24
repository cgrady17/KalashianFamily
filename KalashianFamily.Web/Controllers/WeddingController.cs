using KalashianFamily.Web.Models;
using KalashianFamily.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace KalashianFamily.Web.Controllers
{
    public class WeddingController : Controller
    {
        // GET: Wedding
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Rsvp()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Rsvp(RsvpViewModel model)
        {
            using (NickBeckyWedding db = new NickBeckyWedding())
            using (DbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                RSVP rsvp = new RSVP
                {
                    ArrivalDate = model.ArrivalDate,
                    PrimaryEmail = model.EmailAddress,
                    Attending = model.Attending
                };

                db.RSVPs.Add(rsvp);

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();

                    return Json(new { status = 0, error = ex.Message });
                }

                foreach (RSVPAttendee dbAttendee in model.Attendees.Select(attendee => new RSVPAttendee
                {
                    EmailAddress = attendee.EmailAddress,
                    Name = attendee.Name,
                    RSVP_ID = rsvp.ID
                }))
                {
                    db.RSVPAttendees.Add(dbAttendee);
                }

                try
                {
                    await db.SaveChangesAsync();
                    dbTrans.Commit();
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();

                    return Json(new { status = 0, error = ex.Message });
                }
            }

            return Json(new { status = 1 });
        }

        [HttpGet]
        public ActionResult Destination()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TravelInfo()
        {
            return View();
        }

        [HttpGet]
        public async Task<ActionResult> GuestBook()
        {
            using (NickBeckyWedding db = new NickBeckyWedding())
            {
                IReadOnlyList<GuestBookMessage> messages =
                    await db.GuestBookMessages.OrderByDescending(msg => msg.SignedDate).ToListAsync();

                return View(messages);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> GuestBook(GuestBookMessageViewModel model)
        {
            using (NickBeckyWedding db = new NickBeckyWedding())
            {
                GuestBookMessage guestBook = new GuestBookMessage
                {
                    Name = model.Name,
                    Message = model.Message,
                    SignedDate = DateTime.Now
                };

                db.GuestBookMessages.Add(guestBook);

                try
                {
                    await db.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    return Json(new { status = 0, error = ex.Message });
                }
            }

            return Json(new { status = 1 });
        }
    }
}
﻿using Grady.Framework.Mvc;
using KalashianFamily.Web.Models;
using KalashianFamily.Web.ViewModels;
using SendGrid;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web.Mvc;
using SendGrid.Helpers.Mail;

namespace KalashianFamily.Web.Controllers
{
    public class WeddingController : Controller
    {
        // GET: Wedding
        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Rsvp()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Rsvp(RsvpViewModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Website) || model.CaptchaPass == null || model.CaptchaPass != "KALASHIAN_NOT_ROBOT")
            {
                return Json(new { status = 0, error = "Only humans can submit this form." });
            }

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

                RSVPAttendee primeAttendee = new RSVPAttendee
                {
                    EmailAddress = model.EmailAddress,
                    Name = model.Name,
                    RSVP_ID = rsvp.ID
                };

                db.RSVPAttendees.Add(primeAttendee);

                foreach (RSVPAttendee dbAttendee in model.Attendees.Where(attendee => attendee.EmailAddress != null).Select(attendee => new RSVPAttendee
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

                await SendRsvpEmailAsync(model);
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
        public ActionResult OurStory()
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

                ViewBag.Messages = messages;

                return View();
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> GuestBook(GuestBookMessageViewModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.Website) || model.CaptchaPass == null || model.CaptchaPass != "KALASHIAN_NOT_ROBOT")
            {
                return Json(new { status = 0, error = "Only humans can submit this form." });
            }

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

        //public async Task<ActionResult> TestEmail()
        //{
        //    RsvpViewModel model = new RsvpViewModel { EmailAddress = "cgrady17@outlook.com", Name = "Connor Grady", Attending = false, ArrivalDate = new DateTime(2016, 11, 11), OtherAttendees = false, Attendees = new List<RsvpAttendeeViewModel> { new RsvpAttendeeViewModel { Name = "Robin Grady", EmailAddress = "rgrady611@gmail.com" } } };
        //    Dictionary<string, string> recipients = new Dictionary<string, string>
        //    {
        //        {"Connor Grady", "cgrady17@outlook.com"}
        //    };

        //    await SendRsvpEmailAsync(recipients, model);

        //    return RedirectToAction("Index", "Home");
        //}

        private async Task SendRsvpEmailAsync(RsvpViewModel model)
        {
            //SendGridMessage message = new SendGridMessage
            //{
            //    From = new MailAddress("becky-nick@kalashianfamily.com", "Becky & Nick"),
            //    Subject = "RSVP Confirmation | Becky & Nick's Wedding",
            //    Html = this.RenderPartialViewToString("RSVPEmail", model)
            //};

            //message.AddTo($"{model.Name} <{model.EmailAddress}>");

            //SendGrid.Web transportWeb = new SendGrid.Web(ConfigurationManager.AppSettings["sendgrid:APIKey"]);

            //await transportWeb.DeliverAsync(message);

            SendGridAPIClient sendGrid = new SendGridAPIClient(ConfigurationManager.AppSettings["sendgrid:APIKey"]);
            Mail mail = new Mail(new Email("becky-nick@kalashianfamily.com", "Nick & Rebecca"), "RSVP Confirmation | Becky & Nick's Wedding", new Email(model.EmailAddress, model.Name), new Content("text/html", this.RenderPartialViewToString("RSVPEmail", model)));

            await sendGrid.client.mail.send.post(requestBody: mail.Get());
        }

        //private static async Task<bool> VerifyReCaptcha(string response, string ipAddr)
        //{
        //    using (HttpClient client = new HttpClient())
        //    using (MultipartFormDataContent formData = new MultipartFormDataContent())
        //    {
        //        Uri uri = new Uri("https://www.google.com/recaptcha/api/siteverify");

        //        formData.Add(new StringContent("6LfSLRgTAAAAAF_zbdu93EVUV1cHuZ_XV09JOzc0"), "secret");
        //        formData.Add(new StringContent(response), "response");
        //        formData.Add(new StringContent(ipAddr), "remoteip");

        //        HttpResponseMessage result = await client.PostAsync(uri, formData);

        //        if (!result.IsSuccessStatusCode) return false;
        //    }
        //}

        //private string RenderRazorViewToString(string viewName, object model)
        //{
        //    ViewData.Model = model;
        //    using (StringWriter sw = new StringWriter())
        //    {
        //        ViewEngineResult viewResult = ViewEngines.Engines.FindPartialView(ControllerContext, viewName);
        //        ViewContext viewContext = new ViewContext(ControllerContext, viewResult.View, ViewData, TempData, sw);
        //        viewResult.View.Render(viewContext, sw);
        //        viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
        //        return sw.GetStringBuilder().ToString();
        //    }
        //}
    }
}
using Grady.Framework.Mvc;
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
using System.Web.WebPages;
using Microsoft.Ajax.Utilities;
using SendGrid.Helpers.Mail;

namespace KalashianFamily.Web.Controllers
{
    public class RSVPController : Controller
    {
        // GET: RSVP
        public ActionResult Index()
        {
            bool? isAuthenticated = Session["IsAuthenticated"] as bool?;

            bool? badPass = TempData["BadPass"] as bool?;

            if (badPass.HasValue && badPass.Value) ViewBag.BadPass = true;

            return View(isAuthenticated.HasValue && isAuthenticated.Value ? "Index" : "SecurityCheck");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Index(string password)
        {
            if (!string.IsNullOrWhiteSpace(password) && password == "Ace2016")
            {
                Session["IsAuthenticated"] = true;
            }
            else
            {
                TempData["BadPass"] = true;
            }

            return Index();
        }

        public ActionResult MassEmail()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> MassEmail(CustomEmailViewModel model)
        {
            if (model.Subject.IsEmpty() || model.Body.IsEmpty())
                return Json(new { status = 0, error = "Both a Subject and a Body must be provided." });

            int numSent;

            try
            {
                Dictionary<string, string> recipients;

                using (NickBeckyWedding db = new NickBeckyWedding())
                {
                    var query = from ra in db.RSVPAttendees
                                join r in db.RSVPs on ra.RSVP_ID equals r.ID
                                select new
                                {
                                    ra.Name,
                                    Email = ra.EmailAddress,
                                    r.Attending
                                };

                    if (model.RecipientSelection != "All") query = query.Where(r => r.Attending);

                    query = query.Distinct();

                    recipients = await query.ToDictionaryAsync(r => r.Name, r => r.Email);
                }

                numSent = await SendMassEmailAsync(model, recipients);
            }
            catch (Exception ex)
            {
                return Json(new { status = 0, error = ex.Message });
            }

            return Json(new { status = 1, message = "All " + numSent + " emails successfully sent." });
        }

        public async Task<ActionResult> TestMassEmail()
        {
            CustomEmailViewModel model = new CustomEmailViewModel
            {
                Body = "This is a test email message. We'll want to test some HTML in here as well. <p>This is a separate paragraph</p><br /><br />That was two line breaks.<br /><br /><h3>This is an H3</h3><h2>H2</h2><h1>This is an H1</h1><a href='http://kalashianfamily.com/'>This is a link to KalashianFamily.com</a>. <p>Thanks,</p><p>Connor Gray</p>",
                Subject = "Test Email"
            };

            Dictionary<string, string> recipients = new Dictionary<string, string>
            {
                { "Connor Outlook", "cgrady17@outlook.com" },
                { "Connor Gmail", "cgrady17@gmail.com" },
                { "Connor School", "gradycp17@uww.edu" },
                { "Connor Work", "connorgrady@landmarkcu.com" }
            };

            await SendMassEmailAsync(model, recipients);

            return HttpNotFound();
        }

        private async Task<int> SendMassEmailAsync(CustomEmailViewModel model, Dictionary<string, string> recipients)
        {
            string baseBody = this.RenderPartialViewToString("EmailShell", model);

            //SendGrid.Web transportWeb = new SendGrid.Web(ConfigurationManager.AppSettings["sendgrid:APIKey"]);
            SendGridAPIClient sendGrid = new SendGridAPIClient(ConfigurationManager.AppSettings["sendgrid:APIKey"]);

            int count = 0;

            foreach (KeyValuePair<string, string> recipient in recipients)
            {
                //SendGridMessage message = new SendGridMessage
                //{
                //    From = new MailAddress("becky-nick@kalashianfamily.com", "Becky & Nick"),
                //    Subject = model.Subject,
                //    Html = baseBody.Replace("-recipientName-", recipient.Key)
                //};

                Mail mail = new Mail(new Email("becky-nick@kalashianfamily.com", "Nick & Rebecca"), model.Subject, new Email(recipient.Value, recipient.Key), new Content("text/html", baseBody.Replace("-recipientName-", recipient.Key)));

                await sendGrid.client.mail.send.post(requestBody: mail.Get());

                //message.AddTo($"{recipient.Key} <{recipient.Value}>");

                //await transportWeb.DeliverAsync(message);

                count++;
            }

            return count;
        }

        public async Task<JsonResult> Read(int? id)
        {
            using (NickBeckyWedding db = new NickBeckyWedding())
            {
                switch (id)
                {
                    case 1:
                        return Json(await db.RSVPs.CountAsync(), JsonRequestBehavior.AllowGet);

                    case 2:
                        return Json(await db.RSVPs.CountAsync(x => x.Attending), JsonRequestBehavior.AllowGet);

                    case 3:
                        return Json(await db.RSVPs.CountAsync(x => !x.Attending), JsonRequestBehavior.AllowGet);
                }

                List<RsvpExistingViewModel> existingRsvps = await (from ra in db.RSVPAttendees
                                                                   join r in db.RSVPs on ra.RSVP_ID equals r.ID
                                                                   select new RsvpExistingViewModel
                                                                   {
                                                                       ID = r.ID,
                                                                       Name = ra.Name,
                                                                       EmailAddress = ra.EmailAddress,
                                                                       Attending = r.Attending,
                                                                       ArrivalDate = r.ArrivalDate
                                                                   }).ToListAsync();

                return Json(new
                {
                    data = existingRsvps
                        .Distinct()
                        .Select(x => new
                        {
                            x.ID,
                            x.Name,
                            EmailAddress = "<a href='mailto:" + x.EmailAddress + "'>" + x.EmailAddress + "</a>",
                            Attending = x.Attending ? "Yes" : "No",
                            ArrivalDate = x.ArrivalDate?.ToShortDateString() ?? "N/A"
                        })
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}
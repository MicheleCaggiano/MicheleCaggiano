using MicheleCaggiano.Web.Model;
using MicheleCaggiano.Web.Models;
using System;
using System.Configuration;
using System.Linq;
using System.Net.Mail;
using System.Web.Mvc;

namespace MicheleCaggiano.Web.Controllers
{

  public class HomeController : Controller
  {
    private readonly ModelContainer ctx = new ModelContainer();

    public ActionResult Index()
    {
      ViewBag.BlogPost = ctx.Articolo.Take(3).OrderByDescending(o => o.DataPubblicazione).ToList();
      return View();
    }

    public ActionResult Curriculum()
    {
      return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Contact(EmailViewModel emailvm)
    {
      if (ModelState.IsValid)
      {
        var isSended = SendEmail(emailvm.Email, emailvm.Nome, emailvm.Oggetto, emailvm.Messaggio, null);
      }
      return View("Curriculum");
    }

    /// <summary>
    /// Send Email. Needed 3 app settings in web.config:
    ///  - emailTo: email address where send email
    ///  - emailCC: email addresses comma separated, where send email CC
    ///  - emailSubjectPrefix: email subject prefix
    /// </summary>
    /// <param name="emailFrom"></param>
    /// <param name="fromName"></param>
    /// <param name="subject"></param>
    /// <param name="message"></param>
    /// <param name="pathAllegato"></param>
    /// <returns></returns>
    private bool SendEmail(string emailFrom, string fromName, string subject, string message, string pathAllegato)
    {
      var result = false;

      MailAddress EmailFrom = new MailAddress(emailFrom, fromName);
      MailAddress EmailTo = new MailAddress(ConfigurationManager.AppSettings["emailTo"]);
      MailMessage Email = new MailMessage(EmailFrom, EmailTo);

      // Assegna le proprietà all'oggetto MailMessage
      if (ConfigurationManager.AppSettings["emailCC"] != null)
      {
        foreach (var address in ConfigurationManager.AppSettings["emailCC"].Split(','))
        {
          MailAddress EmailCC = new MailAddress(address);
          Email.CC.Add(EmailCC);
        }
      }
      Email.Subject = ConfigurationManager.AppSettings["emailSubjectPrefix"] + subject;
      Email.Body = message;
      Email.IsBodyHtml = false;

      if (!string.IsNullOrEmpty(pathAllegato))
      {
        if (System.IO.File.Exists(System.Web.HttpContext.Current.Server.MapPath(".") + "/" + pathAllegato))
        {
          Attachment _allegato = new Attachment(System.Web.HttpContext.Current.Server.MapPath(".") + "/" + pathAllegato);
          Email.Attachments.Add(_allegato);
        }
      }

      Email.BodyEncoding = System.Text.Encoding.UTF8;
      // Creates SmtpClient object
      SmtpClient smtp = new SmtpClient();
      //smtp.Credentials = new System.Net.NetworkCredential(username, password);
      smtp.Timeout = 20000; // ms => 20s. Default 100000
      try
      {
        smtp.Send(Email);
        result = true;
      }
      catch (Exception ex)
      {
        //System.Web.HttpContext.Current.Response.Write(ex.ToString());
      }

      return result;
    }

  }
}
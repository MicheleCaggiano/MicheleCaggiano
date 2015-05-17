using MicheleCaggiano.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace MicheleCaggiano.Controllers
{

  public class HomeController : Controller
  {
    public ActionResult Index()
    {
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
        var isSended = SendEmail("michele.caggiano@gmail.com", "Michele Caggiano", "michele.caggiano@gmail.com", emailvm.Oggetto, emailvm.Email + "\n" + emailvm.Messaggio, null, "michele.caggiano@gmail.com", "@Miky115923");
      }
      return View("Curriculum");
    }

    private bool SendEmail(string emailFrom, string fromName, string emailReceiver, string subject, string message, string pathAllegato, string username, string password)
    {
      var result = false;

      System.Net.Mail.MailMessage Email = null;

      MailAddress EmailFrom = new MailAddress(emailFrom, fromName);
      MailAddress EmailTo = new MailAddress(emailReceiver);
      Email = new System.Net.Mail.MailMessage(EmailFrom, EmailTo);

      // Assegna le proprietà all'oggetto MailMessage
      Email.Subject = subject;

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
      System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
      smtp.Credentials = new System.Net.NetworkCredential(username, password);
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
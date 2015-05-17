using MicheleCaggiano.Web;
using MicheleCaggiano.Web.Model;
using MicheleCaggiano.Web.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MicheleCaggiano.Controllers
{
  [Authorize]
  public class CategoriaController : Controller
  {
    private ModelContainer ctx = new ModelContainer();

    // GET: Categoria
    public ActionResult Index()
    {
      return View(ctx.Categoria.OrderBy(c => c.Nome));
    }

    // GET: Categoria/Details/5
    public ActionResult Details(int id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Categoria categoria = GetCategoriaById(id);
      if (categoria == null)
      {
        return HttpNotFound();
      }
      return View(categoria);
    }

    // GET: Categoria/Create
    public ActionResult Create()
    {
      return View();
    }

    // POST: Categoria/Create
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Categoria categoria)
    {
      try
      {
        var userCompleteName = User.Identity.GetUserName();
        categoria.AuthInfo = new AuthInfo()
        {
          Created = DateTime.Now,
          CreatedBy = userCompleteName,
          Modified = DateTime.Now,
          ModifiedBy = userCompleteName, //HttpContext.User.Identity.Name,
          UserId = User.Identity.GetUserId()
        };
        ctx.Categoria.Add(categoria);
        ctx.SaveChanges();
        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    // GET: Categoria/Edit/5
    public ActionResult Edit(int id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }

      Categoria categoria = GetCategoriaById(id);
      if (categoria == null)
      {
        return HttpNotFound();
      }
      return View(categoria);
    }

    // POST: Categoria/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(Categoria categoria)
    {
      //var user = GetUserManager();
      if (categoria.AuthInfo != default(AuthInfo))
      {
        categoria.AuthInfo.Modified = DateTime.Now;
        categoria.AuthInfo.ModifiedBy = User.Identity.GetUserName();
      }
      ctx.Entry(categoria).State = EntityState.Modified;
      ctx.SaveChanges();
      return RedirectToAction("Index");
    }

    // GET: Categoria/Delete/5
    public ActionResult Delete(int id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }

      Categoria categoria = GetCategoriaById(id);
      if (categoria == null)
      {
        return HttpNotFound();
      }
      return View(categoria);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(Categoria categoria)
    {
      try
      {
        var result = ctx.Categoria.Find(categoria.Nome);
        ctx.Categoria.Remove(result);
        ctx.SaveChanges();
        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    #region Internal Methods

    private Categoria GetCategoriaById(int id)
    {
      Categoria categoria = ctx.Categoria.Find(id);
      return categoria;
    }
    //private ApplicationUser GetUserManager()
    //{
    //  var manager = new ApplicationUserManager(new UserStore<ApplicationUser>());
    //  var user = manager.FindById(User.Identity.GetUserId());
    //  return user;
    //}
    #endregion Internal Methods
  }
}

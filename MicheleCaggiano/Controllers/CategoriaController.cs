using MicheleCaggiano.MongoModel;
using MicheleCaggiano.Properties;
using Microsoft.AspNet.Identity;
using MongoDB.AspNet.Identity;
using MongoDB.Bson;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using System;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MicheleCaggiano.Controllers
{
  [Authorize]
  public class CategoriaController : Controller
  {
    public readonly ModelContext ctx;// = new ModelContext(Settings.Default.MicheleCaggianoConnectionString, Settings.Default.MicheleCaggianoDatabaseName);

    // GET: Categoria
    public ActionResult Index()
    {
      return View(ctx.Categorie.FindAll().SetSortOrder(SortBy.Ascending("Nome")));
    }

    // GET: Categoria/Details/5
    public ActionResult Details(string id)
    {
      if (string.IsNullOrEmpty(id))
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
        var user = GetUserManager();
        var userCompleteName = string.Format("{0} {1}", user.Name, user.LastName);
        categoria.AuthInfo = new AuthInfo()
        {
          Created = DateTime.Now,
          CreatedBy = userCompleteName,
          Modified = DateTime.Now,
          ModifiedBy = userCompleteName, //HttpContext.User.Identity.Name,
          UserId = user.Id
        };
        ctx.Categorie.Insert(categoria);
        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    // GET: Categoria/Edit/5
    public ActionResult Edit(string id)
    {
      if (string.IsNullOrEmpty(id))
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
      var user = GetUserManager();
      if (categoria.AuthInfo != default(AuthInfo))
      {
        categoria.AuthInfo.Modified = DateTime.Now;
        categoria.AuthInfo.ModifiedBy = string.Format("{0} {1}", user.Name, user.LastName);
      }

      ctx.Categorie.Save(categoria);
      return RedirectToAction("Index");
    }

    // GET: Categoria/Delete/5
    public ActionResult Delete(string id)
    {
      if (string.IsNullOrEmpty(id))
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
        ctx.Categorie.Remove(Query.EQ("Nome", new ObjectId(categoria.Nome)));
        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    #region Internal Methods

    private Categoria GetCategoriaById(string id)
    {
      Categoria categoria = ctx.Categorie.FindOneById(id);
      return categoria;
    }
    private Models.ApplicationUser GetUserManager()
    {
      var manager = new ApplicationUserManager(new UserStore<MicheleCaggiano.Models.ApplicationUser>(Settings.Default.MicheleCaggianoConnectionString));
      var user = manager.FindById(User.Identity.GetUserId());
      return user;
    }
    #endregion Internal Methods
  }
}

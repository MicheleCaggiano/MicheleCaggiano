using MicheleCaggiano.Models;
using MicheleCaggiano.Web.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace MicheleCaggiano.Web.Controllers
{
  [Authorize]
  public class BlogController : Controller
  {
    //public readonly ModelContext ctx = new ModelContext(System.Configuration.ConfigurationManager.ConnectionStrings["MicheleCaggianoConnectionString"].ConnectionString, System.Configuration.ConfigurationManager.AppSettings["MicheleCaggianoDatabaseName"]);
    private ModelContainer ctx = new ModelContainer();

    [AllowAnonymous]
    // GET: ArticoloMongo
    public ActionResult Index(BlogViewModel.TipologiaArticoli? tipologiaArticoli)
    {
      BlogViewModel viewModel = GetBlogViewModel(tipologiaArticoli, null);
      return View(viewModel); // Query non utilizzando Linq: return View(ctx.Articoli.FindAll().SetSortOrder(SortBy.Descending("AuthInfo.Created")));
    }

    [AllowAnonymous]
    public ActionResult Details(int id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }
      Articolo articolo = GetArticoloById(id);
      if (articolo == null)
      {
        return HttpNotFound();
      }
      return View(articolo);
    }

    // GET: ArticoloMongo/Create
    public ActionResult Create()
    {
      // Recupera tutte le categorie
      ViewBag.CategorieList = ctx.Categoria
        .OrderBy(o => o.Nome)
        .Select(c => c.Nome).ToList();

      ViewBag.CategorieMS = new MultiSelectList(ctx.Categoria.OrderBy(o => o.Nome), "Id", "Nome");
      return View();
    }

    // POST: ArticoloMongo/Create
    [HttpPost]
    [ValidateInput(false)]
    [ValidateAntiForgeryToken]
    public ActionResult Create(FormCollection form, Articolo articolo)
    {
      if (ModelState.IsValid)
      {
        //var user = GetUserManager();
        var userCompleteName = User.Identity.GetUserName();
        articolo.AuthInfo = new AuthInfo()
        {
          Created = DateTime.Now,
          CreatedBy = userCompleteName,
          Modified = DateTime.Now,
          ModifiedBy = userCompleteName, //HttpContext.User.Identity.Name,
          UserId = User.Identity.GetUserId()
        };

        // Controlla se l'articolo deve essere pubblicato
        if (articolo.Pubblicato)
        {
          articolo.DataPubblicazione = DateTime.Now;
        }

        // Bind Categorie
        articolo.BindCategorie(form["CategorieScelte"], ctx);

        ctx.Articolo.Add(articolo);
        ctx.SaveChanges();
        return RedirectToAction("Index");
      }

      return View(articolo);
    }

    // GET: Blog/Edit/5
    public ActionResult Edit(int id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }

      Articolo articolo = GetArticoloById(id);
      if (articolo == null)
      {
        return HttpNotFound();
      }

      // Recupera tutte le categorie
      ViewBag.CategorieList = ctx.Categoria.OrderBy(o => o.Nome);
      ViewBag.CategorieSelected = new SelectList(ctx.Categoria);

      //ViewBag.CategorieMS = new MultiSelectList(ctx.Categoria.OrderBy(o => o.Nome).ToList(), "Id", "Nome", selectedList);

      //if (articolo.Categorie.Count > 0)
      //{
      //  ViewBag.CategorieArticolo = articolo.Categorie.OrderBy(o => o);
      //}
      //else
      //{
      //  ViewBag.CategorieArticolo = new IEnumerable<Categoria>();
      //}
      return View(articolo);
    }

    // POST: Blog/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateInput(false)]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(FormCollection form, Articolo articolo)
    {

      //  ctx.Entry(articolo).State = EntityState.Modified;
      //  db.SaveChanges();
      //  return RedirectToAction("Index");
      //}
      //var user = GetUserManager();

      if (ModelState.IsValid)
      {
        var articoloToSave = GetArticoloById(articolo.Id);

        if (articoloToSave.AuthInfo != default(AuthInfo))
        {
          articoloToSave.AuthInfo.Modified = DateTime.Now;
          articoloToSave.AuthInfo.ModifiedBy = User.Identity.GetUserName();
          articoloToSave.AuthInfo.UserId = User.Identity.GetUserId();
        }

        // Controlla se l'articolo deve essere pubblicato per la prima volta
        if (articolo.Pubblicato && articolo.DataPubblicazione == null)
        {
          articoloToSave.DataPubblicazione = DateTime.UtcNow;
        }

        // Bind Categorie
        articoloToSave.BindCategorie(form["CategorieScelte"], ctx);
        articoloToSave.Testo = articolo.Testo;
        articoloToSave.Titolo = articolo.Titolo;

        ctx.Entry(articoloToSave).State = EntityState.Modified;
        ctx.SaveChanges();
        return RedirectToAction("Index");
      }

      return View(articolo);
    }

    // GET: ArticoloMongo/Delete/5
    public ActionResult Delete(int id)
    {
      if (id == null)
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }

      Articolo articolo = GetArticoloById(id);
      if (articolo == null)
      {
        return HttpNotFound();
      }
      return View(articolo);
    }

    // POST: ArticoloMongo/Delete/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Delete(Articolo articolo)
    {
      try
      {
        var articoloToDelete = ctx.Articolo.Find(articolo.Id);
        ctx.Articolo.Remove(articoloToDelete);
        ctx.SaveChanges();
        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    [AllowAnonymous]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Search(FormCollection data)
    {
      BlogViewModel viewModel = GetBlogViewModel(BlogViewModel.TipologiaArticoli.Pubblicati, data["TestoRicerca"]);
      return View("Index", viewModel);
    }

    [AllowAnonymous]
    [HttpGet]
    public ActionResult PostsScroll(BlogViewModel.TipologiaArticoli? tipologiaArticoli, string testoRicerca, int page)
    {

      var blogViewModel = GetBlogViewModel(tipologiaArticoli, testoRicerca, page);

      ViewBag.Page = page;
      return PartialView("_Articles", blogViewModel.Articoli);
    }

    #region Internal Methods

    private Articolo GetArticoloById(int id)
    {
      Articolo articolo = ctx.Articolo.Find(id);
      return articolo;
    }

    //private Models.ApplicationUser GetUserManager()
    //{
    //  var manager = new ApplicationUserManager(new UserStore<MicheleCaggiano.Web.Models.ApplicationUser>());
    //  var user = manager.FindById(User.Identity.GetUserId());
    //  return user;
    //}

    private BlogViewModel GetBlogViewModel(BlogViewModel.TipologiaArticoli? tipologiaArticoli, string testoRicerca, int page = -1)
    {
      BlogViewModel viewModel = new BlogViewModel();
      if (Request.IsAuthenticated && User.IsInRole("Admin"))
      {
        IQueryable<Articolo> articoli;
        articoli = ctx.Articolo.OrderByDescending(a => a.AuthInfo.Created);

        viewModel.TipoArticoli = tipologiaArticoli ?? BlogViewModel.TipologiaArticoli.Pubblicati;
        switch (viewModel.TipoArticoli)
        {
          case BlogViewModel.TipologiaArticoli.Pubblicati:
          default:
            // Visualizza articoli pubblicati
            viewModel.Articoli = articoli.Where(a => a.Pubblicato);
            break;
          case BlogViewModel.TipologiaArticoli.Bozze:
            // Visualizza bozze
            viewModel.Articoli = articoli.Where(a => !a.Pubblicato);
            break;
          case BlogViewModel.TipologiaArticoli.Tutti:
            // Visualizza tutti gli articoli
            viewModel.Articoli = articoli;
            break;
        }

        viewModel.ArticoliPubblicati = articoli.Where(a => a.Pubblicato).Count();
        viewModel.Bozze = articoli.Where(a => !a.Pubblicato).Count();
        viewModel.NumeroCategorie = ctx.Categoria.Count();
      }
      else
      {
        viewModel.Articoli = ctx.Articolo.Where(a => a.Pubblicato).OrderByDescending(a => a.AuthInfo.Created);
      }

      // Checks if lazy loading
      if (page > 0)
      {
        viewModel.Articoli = viewModel.Articoli.Skip(page * 10).Take(10);
      }
      else
      {
        viewModel.Articoli = viewModel.Articoli.Take(10);
      }

      // Controlla filtro ricerca
      if (!string.IsNullOrEmpty(testoRicerca))
      {
        viewModel.Articoli = viewModel.Articoli.Where(a => a.Titolo.ToLower().Contains(testoRicerca.ToLower()));
      }
      return viewModel;
    }

    #endregion Internal Methods
  }
}

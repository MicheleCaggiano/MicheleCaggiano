using MicheleCaggiano.Models;
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
  public class BlogController : Controller
  {
    public readonly ModelContext ctx = new ModelContext(System.Configuration.ConfigurationManager.ConnectionStrings["MicheleCaggianoConnectionString"].ConnectionString, System.Configuration.ConfigurationManager.AppSettings["MicheleCaggianoDatabaseName"]);

    [AllowAnonymous]
    // GET: ArticoloMongo
    public ActionResult Index(BlogViewModel.TipologiaArticoli? tipologiaArticoli)
    {
      BlogViewModel viewModel = GetBlogViewModel(tipologiaArticoli, null);
      return View(viewModel); // Query non utilizzando Linq: return View(ctx.Articoli.FindAll().SetSortOrder(SortBy.Descending("AuthInfo.Created")));
    }

    [AllowAnonymous]
    public ActionResult Details(string id)
    {
      if (string.IsNullOrEmpty(id))
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
      ViewBag.CategorieList = ctx.Categorie.FindAll().OrderBy(o => o.Nome).Select(c => c.Nome).ToList();
      return View();
    }

    // POST: ArticoloMongo/Create
    [HttpPost]
    [ValidateInput(false)]
    [ValidateAntiForgeryToken]
    public ActionResult Create(Articolo articolo)
    {
      try
      {
        var user = GetUserManager();
        var userCompleteName = string.Format("{0} {1}", user.Name, user.LastName);
        articolo.AuthInfo = new AuthInfo()
        {
          Created = DateTime.Now,
          CreatedBy = userCompleteName,
          Modified = DateTime.Now,
          ModifiedBy = userCompleteName, //HttpContext.User.Identity.Name,
          UserId = user.Id
        };

        // Controlla se l'articolo deve essere pubblicato
        if (articolo.Pubblicato)
        {
          articolo.DataPubblicazione = DateTime.Now;
        }

        ctx.Articoli.Insert(articolo);
        return RedirectToAction("Index");
      }
      catch
      {
        return View();
      }
    }

    // GET: Blog/Edit/5
    public ActionResult Edit(string id)
    {
      if (string.IsNullOrEmpty(id))
      {
        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
      }

      Articolo articolo = GetArticoloById(id);
      if (articolo == null)
      {
        return HttpNotFound();
      }

      // Recupera tutte le categorie
      ViewBag.CategorieList = ctx.Categorie.FindAll().OrderBy(o => o.Nome).Select(c => c.Nome).ToList();
      if (articolo.Categorie != null)
      {
        ViewBag.CategorieArticolo = articolo.Categorie.OrderBy(o => o).ToJson();
      }
      return View(articolo);
    }

    // POST: Blog/Edit/5
    // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
    // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
    [HttpPost]
    [ValidateInput(false)]
    [ValidateAntiForgeryToken]
    public ActionResult Edit(Articolo articolo)
    {
      //if (ModelState.IsValid)
      //{
      //  ctx.Entry(articolo).State = EntityState.Modified;
      //  db.SaveChanges();
      //  return RedirectToAction("Index");
      //}
      var user = GetUserManager();
      if (articolo.AuthInfo != default(AuthInfo))
      {
        articolo.AuthInfo.Modified = DateTime.Now;
        articolo.AuthInfo.ModifiedBy = string.Format("{0} {1}", user.Name, user.LastName);
      }

      // Controlla se l'articolo deve essere pubblicato per la prima volta
      if (articolo.Pubblicato && articolo.DataPubblicazione == null)
      {
        articolo.DataPubblicazione = DateTime.UtcNow;
      }

      ctx.Articoli.Save(articolo);
      return RedirectToAction("Index");
    }

    // GET: ArticoloMongo/Delete/5
    public ActionResult Delete(string id)
    {
      if (string.IsNullOrEmpty(id))
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
        ctx.Articoli.Remove(Query.EQ("_id", new ObjectId(articolo.Id)));
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

    private Articolo GetArticoloById(string id)
    {
      Articolo articolo = ctx.Articoli.FindOneById(new ObjectId(id));
      return articolo;
    }

    private Models.ApplicationUser GetUserManager()
    {
      var manager = new ApplicationUserManager(new UserStore<MicheleCaggiano.Models.ApplicationUser>(Settings.Default.MicheleCaggianoConnectionString));
      var user = manager.FindById(User.Identity.GetUserId());
      return user;
    }

    private BlogViewModel GetBlogViewModel(BlogViewModel.TipologiaArticoli? tipologiaArticoli, string testoRicerca, int page = -1)
    {
      BlogViewModel viewModel = new BlogViewModel();
      if (Request.IsAuthenticated && User.IsInRole("Admin"))
      {
        IQueryable<Articolo> articoli;
        articoli = ctx.Articoli.AsQueryable().OrderByDescending(a => a.AuthInfo.Created);

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
        viewModel.NumeroCategorie = (int)ctx.Categorie.Count();
      }
      else
      {
        viewModel.Articoli = ctx.Articoli.AsQueryable().Where(a => a.Pubblicato).OrderByDescending(a => a.AuthInfo.Created);
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

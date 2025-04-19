using Futebol.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Net;

namespace Futebol.Controllers
{
    public class ComissaoTecnicaController : Controller
    {
        private LigaTabajaraContext db = new LigaTabajaraContext();

        public ActionResult Index(string searchNome, Cargo? searchCargo)
        {
            var comissaoTecnica = db.ComissoesTecnicas.Include("Time").AsQueryable();

            if (!string.IsNullOrEmpty(searchNome))
            {
                comissaoTecnica = comissaoTecnica.Where(j => j.Nome.Contains(searchNome));
            }

            if (searchCargo.HasValue)
            {
                comissaoTecnica = comissaoTecnica.Where(j => j.Cargo == searchCargo.Value);
            }

            return View(comissaoTecnica.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ComissaoTecnica comissao = db.ComissoesTecnicas.Find(id);
            if (comissao == null)
                return HttpNotFound();

            return View(comissao);
        }

        public ActionResult Create()
        {
            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Cargo,DataNascimento,TimeId")] ComissaoTecnica comissao)
        {
            if (ModelState.IsValid)
            {
                db.ComissoesTecnicas.Add(comissao);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome", comissao.TimeId);
            return View(comissao);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ComissaoTecnica comissao = db.ComissoesTecnicas.Find(id);
            if (comissao == null)
                return HttpNotFound();

            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome", comissao.TimeId);
            return View(comissao);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Cargo,DataNascimento,TimeId")] Jogador comissao)
        {
            if (ModelState.IsValid)
            {
                db.Entry(comissao).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome", comissao.TimeId);
            return View(comissao);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ComissaoTecnica comissao = db.ComissoesTecnicas.Find(id);
            if (comissao == null)
                return HttpNotFound();

            return View(comissao);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ComissaoTecnica comissao = db.ComissoesTecnicas.Find(id);
            db.ComissoesTecnicas.Remove(comissao);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
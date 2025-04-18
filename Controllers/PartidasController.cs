using Futebol.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Futebol.Controllers
{
    public class PartidasController : Controller
    {
        private LigaTabajaraContext db = new LigaTabajaraContext();

        // GET: Partidas
        public ActionResult Index()
        {
            var partidas = db.Partidas.Include(p => p.TimeCasa).Include(p => p.TimeFora);
            return View(partidas.ToList());
        }

        // GET: Partidas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Partida partida = db.Partidas.Include(p => p.TimeCasa).Include(p => p.TimeFora).FirstOrDefault(p => p.Id == id);

            if (partida == null)
                return HttpNotFound();

            return View(partida);
        }

        // GET: Partidas/Create
        public ActionResult Create()
        {
            ViewBag.TimeCasaId = new SelectList(db.Times, "Id", "Nome");
            ViewBag.TimeForaId = new SelectList(db.Times, "Id", "Nome");
            return View();
        }

        // POST: Partidas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,DataHora,Local,TimeCasaId,TimeForaId,PlacarCasa,PlacarFora")] Partida partida)
        {
            if (ModelState.IsValid)
            {
                db.Partidas.Add(partida);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TimeCasaId = new SelectList(db.Times, "Id", "Nome", partida.TimeCasaId);
            ViewBag.TimeForaId = new SelectList(db.Times, "Id", "Nome", partida.TimeForaId);
            return View(partida);
        }

        // GET: Partidas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Partida partida = db.Partidas.Find(id);
            if (partida == null)
                return HttpNotFound();

            ViewBag.TimeCasaId = new SelectList(db.Times, "Id", "Nome", partida.TimeCasaId);
            ViewBag.TimeForaId = new SelectList(db.Times, "Id", "Nome", partida.TimeForaId);
            return View(partida);
        }

        // POST: Partidas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,DataHora,Local,TimeCasaId,TimeForaId,PlacarCasa,PlacarFora")] Partida partida)
        {
            if (ModelState.IsValid)
            {
                db.Entry(partida).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TimeCasaId = new SelectList(db.Times, "Id", "Nome", partida.TimeCasaId);
            ViewBag.TimeForaId = new SelectList(db.Times, "Id", "Nome", partida.TimeForaId);
            return View(partida);
        }

        // GET: Partidas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Partida partida = db.Partidas.Find(id);
            if (partida == null)
                return HttpNotFound();

            return View(partida);
        }

        // POST: Partidas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Partida partida = db.Partidas.Find(id);
            db.Partidas.Remove(partida);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
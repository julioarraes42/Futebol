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
    public class EstatisticasController : Controller
    {
        private LigaTabajaraContext db = new LigaTabajaraContext();

        // GET: Estatisticas
        public ActionResult Index()
        {
            var estatisticas = db.Estatisticas.Include(e => e.Jogador).Include(e => e.Partida);
            return View(estatisticas.ToList());
        }

        // GET: Estatisticas/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Estatistica estatistica = db.Estatisticas.Include(e => e.Jogador).Include(e => e.Partida).FirstOrDefault(e => e.Id == id);

            if (estatistica == null)
                return HttpNotFound();

            return View(estatistica);
        }

        // GET: Estatisticas/Create
        public ActionResult Create()
        {
            ViewBag.JogadorId = new SelectList(db.Jogadores, "Id", "Nome");
            ViewBag.PartidaId = new SelectList(db.Partidas, "Id", "Id"); // Ou mostra outra info da partida
            return View();
        }

        // POST: Estatisticas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,JogadorId,PartidaId,GolsAssistencias,Faltas,CartoesAmarelos,CartoesVermelhos")] Estatistica estatistica)
        {
            if (ModelState.IsValid)
            {
                db.Estatisticas.Add(estatistica);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JogadorId = new SelectList(db.Jogadores, "Id", "Nome", estatistica.JogadorId);
            ViewBag.PartidaId = new SelectList(db.Partidas, "Id", "Id", estatistica.PartidaId);
            return View(estatistica);
        }

        // GET: Estatisticas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Estatistica estatistica = db.Estatisticas.Find(id);
            if (estatistica == null)
                return HttpNotFound();

            ViewBag.JogadorId = new SelectList(db.Jogadores, "Id", "Nome", estatistica.JogadorId);
            ViewBag.PartidaId = new SelectList(db.Partidas, "Id", "Id", estatistica.PartidaId);
            return View(estatistica);
        }

        // POST: Estatisticas/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,JogadorId,PartidaId,GolsAssistencias,Faltas,CartoesAmarelos,CartoesVermelhos")] Estatistica estatistica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(estatistica).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.JogadorId = new SelectList(db.Jogadores, "Id", "Nome", estatistica.JogadorId);
            ViewBag.PartidaId = new SelectList(db.Partidas, "Id", "Id", estatistica.PartidaId);
            return View(estatistica);
        }

        // GET: Estatisticas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Estatistica estatistica = db.Estatisticas.Find(id);
            if (estatistica == null)
                return HttpNotFound();

            return View(estatistica);
        }

        // POST: Estatisticas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Estatistica estatistica = db.Estatisticas.Find(id);
            db.Estatisticas.Remove(estatistica);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
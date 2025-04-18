using Futebol.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Futebol.Controllers
{
    public class TimesController : Controller
    {
        private LigaTabajaraContext db = new LigaTabajaraContext();

        // GET: Times
        public ActionResult Index()
        {
            return View(db.Times.ToList());
        }

        // GET: Times/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Time time = db.Times
                .Include(t => t.Jogadores)
                .Include(t => t.ComissaoTecnica)
                .FirstOrDefault(t => t.Id == id);

            if (time == null)
            {
                return HttpNotFound();
            }

            return View(time);
        }

        // GET: Times/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Times/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Cidade,Estado,AnoFundacao,Estadio,CapacidadeEstadio,CorUniformePrimaria,CorUniformeSecundaria,AptoParaLiga")] Time time)
        {
            if (ModelState.IsValid)
            {
                db.Times.Add(time);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(time);
        }

        // GET: Times/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Time time = db.Times.Find(id);
            if (time == null)
                return HttpNotFound();

            return View(time);
        }

        // POST: Times/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Cidade,Estado,AnoFundacao,Estadio,CapacidadeEstadio,CorUniformePrimaria,CorUniformeSecundaria,AptoParaLiga")] Time time)
        {
            if (ModelState.IsValid)
            {
                db.Entry(time).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(time);
        }

        // GET: Times/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Time time = db.Times.Find(id);
            if (time == null)
                return HttpNotFound();

            return View(time);
        }

        // POST: Times/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Time time = db.Times.Find(id);
            db.Times.Remove(time);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                db.Dispose();
            base.Dispose(disposing);
        }

        public ActionResult Classificacao()
        {
            var times = db.Times.ToList();

            var partidas = db.Partidas
                .Where(p => p.GolsCasa != null && p.GolsFora != null)
                .ToList();

            var classificacao = times.Select(time => new ClassificacaoViewModel
            {
                TimeId = time.Id,
                NomeTime = time.Nome,
                Pontos = CalcularPontos(time.Id, partidas),
                Vitorias = partidas.Count(p => (p.TimeCasaId == time.Id && p.GolsCasa > p.GolsFora) ||
                                               (p.TimeForaId == time.Id && p.GolsFora > p.GolsCasa)),
                Empates = partidas.Count(p => (p.TimeCasaId == time.Id || p.TimeForaId == time.Id) && p.GolsCasa == p.GolsFora),
                Derrotas = partidas.Count(p => (p.TimeCasaId == time.Id && p.GolsCasa < p.GolsFora) ||
                                               (p.TimeForaId == time.Id && p.GolsFora < p.GolsCasa)),
                GolsMarcados = partidas.Where(p => p.TimeCasaId == time.Id).Sum(p => p.GolsCasa ?? 0) +
                               partidas.Where(p => p.TimeForaId == time.Id).Sum(p => p.GolsFora ?? 0),
                GolsSofridos = partidas.Where(p => p.TimeCasaId == time.Id).Sum(p => p.GolsFora ?? 0) +
                               partidas.Where(p => p.TimeForaId == time.Id).Sum(p => p.GolsCasa ?? 0)
            })
            .OrderByDescending(c => c.Pontos)
            .ThenByDescending(c => c.SaldoGols)
            .ThenByDescending(c => c.GolsMarcados)
            .ToList();

            return View(classificacao);
        }

        private int CalcularPontos(int timeId, List<Partida> partidas)
        {
            int pontos = 0;
            foreach (var partida in partidas)
            {
                if (partida.TimeCasaId == timeId)
                {
                    if (partida.GolsCasa > partida.GolsFora)
                        pontos += 3;
                    else if (partida.GolsCasa == partida.GolsFora)
                        pontos += 1;
                }
                else if (partida.TimeForaId == timeId)
                {
                    if (partida.GolsFora > partida.GolsCasa)
                        pontos += 3;
                    else if (partida.GolsFora == partida.GolsCasa)
                        pontos += 1;
                }
            }
            return pontos;
        }
    }
}
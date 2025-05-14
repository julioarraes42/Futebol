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
    public class JogadoresController : Controller
    {
        private LigaTabajaraContext db = new LigaTabajaraContext();

        public ActionResult Index(string searchNome, Posicao? searchPosicao, PePreferido? searchPePreferido)
        {
            var jogadores = db.Jogadores.Include("Time").AsQueryable();

            if (!string.IsNullOrEmpty(searchNome))
            {
                jogadores = jogadores.Where(j => j.Nome.Contains(searchNome));
            }

            if (searchPosicao.HasValue)
            {
                jogadores = jogadores.Where(j => j.Posicao == searchPosicao.Value);
            }

            if (searchPePreferido.HasValue)
            {
                jogadores = jogadores.Where(j => j.PePreferido == searchPePreferido.Value);
            }

            return View(jogadores.ToList());
        }

        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Jogador jogador = db.Jogadores.Find(id);
            if (jogador == null)
                return HttpNotFound();

            return View(jogador);
        }

        public ActionResult Create()
        {
            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,DataNascimento,Nacionalidade,Posicao,NumeroCamisa,Altura,Peso,PePreferido,TimeId")] Jogador jogador)
        {
            if (ModelState.IsValid)
            {
                db.Jogadores.Add(jogador);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome", jogador.TimeId);
            return View(jogador);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Jogador jogador = db.Jogadores.Find(id);
            if (jogador == null)
                return HttpNotFound();

            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome", jogador.TimeId);
            return View(jogador);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,DataNascimento,Nacionalidade,Posicao,NumeroCamisa,Altura,Peso,PePreferido,TimeId")] Jogador jogador)
        {
            if (ModelState.IsValid)
            {
                db.Entry(jogador).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TimeId = new SelectList(db.Times, "Id", "Nome", jogador.TimeId);
            return View(jogador);
        }

        public ActionResult Delete(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Jogador jogador = db.Jogadores.Find(id);
            if (jogador == null)
                return HttpNotFound();

            return View(jogador);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Jogador jogador = db.Jogadores.Find(id);
            db.Jogadores.Remove(jogador);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult RankingArtilheiros()
        {
            var jogadores = db.Jogadores.ToList();
            var estatisticas = db.Estatisticas.ToList();

            var ranking = jogadores
                .Select(j => new RankingArtilheiroViewModel
                {
                    JogadorNome = j.Nome,
                    TimeNome = db.Times.FirstOrDefault(t => t.Id == j.TimeId)?.Nome ?? "Sem Time",
                    TotalGols = estatisticas.Where(e => e.JogadorId == j.Id).Sum(e => e.Gols)
                })
                .GroupBy(x => x.TimeNome)
                .Select(g => g.OrderByDescending(x => x.TotalGols).First()) 
                .ToList();

            return View(ranking);
        }
    }
}
using Futebol.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Futebol.Controllers
{
    public class HomeController : Controller
    {
        private LigaTabajaraContext db = new LigaTabajaraContext();

        public ActionResult Index()
        {
            var times = db.Times.Include(t => t.Jogadores).Include(t => t.ComissaoTecnica).ToList();

            bool ligaApta = VerificarLigaApta(times);

            ViewBag.LigaApta = ligaApta;
            return View(times);
        }

        private bool VerificarLigaApta(List<Time> times)
        {
            if (times.Count != 20)
                return false;

            foreach (var time in times)
            {
                if (time.Jogadores == null || time.Jogadores.Count < 30)
                    return false;

                if (time.ComissaoTecnica == null || time.ComissaoTecnica.Count < 5)
                    return false;

                var cargosDistintos = time.ComissaoTecnica.Select(c => c.Cargo).Distinct().Count();
                if (cargosDistintos < 5)
                    return false;
            }

            return true;
        }

        public ActionResult GerarPartidas()
        {
            db.Estatisticas.RemoveRange(db.Estatisticas);
            db.Partidas.RemoveRange(db.Partidas);
            db.SaveChanges();

            var times = db.Times.Where(t => t.AptoParaLiga == true).ToList();

            if (times.Count != 20)
            {
                TempData["Erro"] = "É necessário ter exatamente 20 times aptos para gerar o campeonato.";
                return RedirectToAction("Index");
            }

            var random = new Random();
            var rodadas = new List<Tuple<Time, Time>>();

            foreach (var timeCasa in times)
            {
                foreach (var timeFora in times)
                {
                    if (timeCasa.Id != timeFora.Id)
                    {
                        rodadas.Add(Tuple.Create(timeCasa, timeFora));
                    }
                }
            }

            foreach (var rodada in rodadas)
            {
                var golsCasa = random.Next(0, 6); 
                var golsFora = random.Next(0, 6);

                var partida = new Partida
                {
                    TimeCasaId = rodada.Item1.Id,
                    TimeForaId = rodada.Item2.Id,
                    Data = DateTime.Now.AddDays(random.Next(1, 100)), 
                    GolsCasa = golsCasa,
                    GolsFora = golsFora
                };

                db.Partidas.Add(partida);
                db.SaveChanges(); 


                var jogadoresCasa = db.Jogadores.Where(j => j.TimeId == rodada.Item1.Id).ToList();

                var jogadoresFora = db.Jogadores.Where(j => j.TimeId == rodada.Item2.Id).ToList();

                for (int i = 0; i < golsCasa; i++)
                {
                    if (jogadoresCasa.Any())
                    {
                        var jogador = jogadoresCasa[random.Next(jogadoresCasa.Count)];
                        db.Estatisticas.Add(new Estatistica
                        {
                            JogadorId = jogador.Id,
                            PartidaId = partida.Id,
                            Gols = 1
                        });
                    }
                }

                for (int i = 0; i < golsFora; i++)
                {
                    if (jogadoresFora.Any())
                    {
                        var jogador = jogadoresFora[random.Next(jogadoresFora.Count)];
                        db.Estatisticas.Add(new Estatistica
                        {
                            JogadorId = jogador.Id,
                            PartidaId = partida.Id,
                            Gols = 1
                        });
                    }
                }

                db.SaveChanges();
            }

            TempData["Sucesso"] = "Partidas e resultados gerados com sucesso!";
            return RedirectToAction("Index");
        }

        public ActionResult Classificacao()
        {
            var times = db.Times.ToList();
            var partidas = db.Partidas.ToList();

            var classificacao = new List<ClassificacaoTime>();

            foreach (var time in times)
            {
                int pontos = 0, vitorias = 0, empates = 0, derrotas = 0, golsPro = 0, golsContra = 0;

                var partidasCasa = partidas.Where(p => p.TimeCasaId == time.Id && p.GolsCasa.HasValue && p.GolsFora.HasValue);
                var partidasFora = partidas.Where(p => p.TimeForaId == time.Id && p.GolsCasa.HasValue && p.GolsFora.HasValue);

                foreach (var partida in partidasCasa)
                {
                    golsPro += partida.GolsCasa.Value;
                    golsContra += partida.GolsFora.Value;

                    if (partida.GolsCasa > partida.GolsFora)
                        vitorias++;
                    else if (partida.GolsCasa == partida.GolsFora)
                        empates++;
                    else
                        derrotas++;
                }

                foreach (var partida in partidasFora)
                {
                    golsPro += partida.GolsFora.Value;
                    golsContra += partida.GolsCasa.Value;

                    if (partida.GolsFora > partida.GolsCasa)
                        vitorias++;
                    else if (partida.GolsFora == partida.GolsCasa)
                        empates++;
                    else
                        derrotas++;
                }

                pontos = (vitorias * 3) + (empates * 1);

                classificacao.Add(new ClassificacaoTime
                {
                    NomeTime = time.Nome,
                    Pontos = pontos,
                    Vitorias = vitorias,
                    Empates = empates,
                    Derrotas = derrotas,
                    GolsPro = golsPro,
                    GolsContra = golsContra,
                    SaldoGols = golsPro - golsContra
                });
            }

            var classificacaoOrdenada = classificacao
                .OrderByDescending(c => c.Pontos)
                .ThenByDescending(c => c.SaldoGols)
                .ThenByDescending(c => c.GolsPro)
                .ToList();

            return View(classificacaoOrdenada);
        }
    }
}
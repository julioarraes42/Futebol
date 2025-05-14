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
                if (time.Jogadores == null || time.Jogadores.Count < 20)
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

        public ActionResult PopularBancoDeDados()
        {
            // Remover todas as entidades relacionadas
            db.Jogadores.RemoveRange(db.Jogadores);
            db.ComissoesTecnicas.RemoveRange(db.ComissoesTecnicas);
            db.Times.RemoveRange(db.Times);
            db.SaveChanges();

            Random random = new Random();

            var nomesTimes = new List<string>
    {
        "Tabajara FC", "Galácticos", "Fúria Azul", "Trovões", "Leões do Norte",
        "Águias da Montanha", "Falcões Vermelhos", "Tigres do Cerrado", "Lobos da Serra", "Guerreiros Urbanos",
        "Dragões Dourados", "Fênix Negra", "Cavaleiros do Vale", "Santos de Aço", "Vikings Tropicais",
        "Piratas do Sul", "Espartanos", "Corvos Brancos", "Samurais do Sertão", "Gladiadores Modernos"
    };

            var posicoes = Enum.GetValues(typeof(Posicao)).Cast<Posicao>().ToList();
            var pes = Enum.GetValues(typeof(PePreferido)).Cast<PePreferido>().ToList();
            var cargos = Enum.GetValues(typeof(Cargo)).Cast<Cargo>().ToList();

            var times = new List<Time>();
            var jogadores = new List<Jogador>();
            var comissoes = new List<ComissaoTecnica>();

            foreach (var nomeTime in nomesTimes)
            {
                var time = new Time
                {
                    Nome = nomeTime,
                    Cidade = "Cidade " + random.Next(1, 100),
                    Estado = "Estado " + random.Next(1, 27),
                    AnoFundacao = random.Next(1900, 2022),
                    Estadio = "Estádio " + nomeTime,
                    CapacidadeEstadio = random.Next(10000, 80000),
                    CorUniformePrimaria = "Cor" + random.Next(1, 10),
                    CorUniformeSecundaria = "Cor" + random.Next(10, 20),
                    AptoParaLiga = true
                };

                times.Add(time);

                // Jogadores
                for (int i = 1; i <= 30; i++)
                {
                    var jogador = new Jogador
                    {
                        Nome = $"Jogador_{nomeTime}_{i}",
                        DataNascimento = DateTime.Now.AddYears(-random.Next(18, 35)),
                        Nacionalidade = "Brasileiro",
                        Posicao = posicoes[random.Next(posicoes.Count)],
                        NumeroCamisa = i,
                        Altura = (float)(1.60 + random.NextDouble() * 0.4),
                        Peso = (float)(60 + random.NextDouble() * 30),
                        PePreferido = pes[random.Next(pes.Count)],
                        Time = time // Relaciona o jogador ao time
                    };

                    jogadores.Add(jogador);
                }

                // Comissão Técnica
                foreach (var cargo in cargos)
                {
                    var membro = new ComissaoTecnica
                    {
                        Nome = $"Comissao_{nomeTime}_{cargo}",
                        Cargo = cargo,
                        DataNascimento = DateTime.Now.AddYears(-random.Next(30, 60)),
                        Time = time // Relaciona o membro ao time
                    };

                    comissoes.Add(membro);
                }
            }

            // Adicionar todas as entidades ao contexto
            db.Times.AddRange(times);
            db.Jogadores.AddRange(jogadores);
            db.ComissoesTecnicas.AddRange(comissoes);

            // Salvar todas as alterações de uma vez
            db.SaveChanges();


            return Content("Banco de dados populado com sucesso!");
        }

        public ActionResult PopularBancoDeDados2()
        {
            db.Jogadores.RemoveRange(db.Jogadores);
            db.ComissoesTecnicas.RemoveRange(db.ComissoesTecnicas);
            db.Times.RemoveRange(db.Times);

            Random random = new Random();

            var nomesTimes = new List<string>
            {
                "Tabajara FC", "Galácticos", "Fúria Azul", "Trovões", "Leões do Norte",
                "Águias da Montanha", "Falcões Vermelhos", "Tigres do Cerrado", "Lobos da Serra", "Guerreiros Urbanos",
                "Dragões Dourados", "Fênix Negra", "Cavaleiros do Vale", "Santos de Aço", "Vikings Tropicais",
                "Piratas do Sul", "Espartanos", "Corvos Brancos", "Samurais do Sertão", "Gladiadores Modernos"
            };

            var posicoes = Enum.GetValues(typeof(Posicao)).Cast<Posicao>().ToList();
            var pes = Enum.GetValues(typeof(PePreferido)).Cast<PePreferido>().ToList();
            var cargos = Enum.GetValues(typeof(Cargo)).Cast<Cargo>().ToList();

            foreach (var nomeTime in nomesTimes)
            {
                var time = new Time
                {
                    Nome = nomeTime,
                    Cidade = "Cidade " + random.Next(1, 100),
                    Estado = "Estado " + random.Next(1, 27),
                    AnoFundacao = random.Next(1900, 2022),
                    Estadio = "Estádio " + nomeTime,
                    CapacidadeEstadio = random.Next(10000, 80000),
                    CorUniformePrimaria = "Cor" + random.Next(1, 10),
                    CorUniformeSecundaria = "Cor" + random.Next(10, 20),
                    AptoParaLiga = true
                };

                db.Times.Add(time); 

                for (int i = 1; i <= 30; i++)
                {
                    var jogador = new Jogador
                    {
                        Nome = $"Jogador_{nomeTime}_{i}",
                        DataNascimento = DateTime.Now.AddYears(-random.Next(18, 35)),
                        Nacionalidade = "Brasileiro",
                        Posicao = posicoes[random.Next(posicoes.Count)],
                        NumeroCamisa = i,
                        Altura = (float)(1.60 + random.NextDouble() * 0.4),
                        Peso = (float)(60 + random.NextDouble() * 30),
                        PePreferido = pes[random.Next(pes.Count)],
                        TimeId = time.Id
                    };

                    db.Jogadores.Add(jogador);
                }

                foreach (var cargo in cargos)
                {
                    var membro = new ComissaoTecnica
                    {
                        Nome = $"Comissao_{nomeTime}_{cargo}",
                        Cargo = cargo,
                        DataNascimento = DateTime.Now.AddYears(-random.Next(30, 60)),
                        TimeId = time.Id
                    };

                    db.ComissoesTecnicas.Add(membro);
                }

            }

            return Content("Banco de dados populado com sucesso!");
        }
    }
}
using System.Collections.Generic;
using MercadoriaRecebimentoApi.Data;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MercadoriaRecebimentoApi.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class AgendamentoController : ControllerBase
    {
        private readonly IConfiguration _config;
        private IMemoryCache _cache;

        public AgendamentoController(IConfiguration config, IMemoryCache memoryCache)
        {
            _config = config;
            _cache = memoryCache;
        }

        // GET: api/<AgendamentoController>
        [HttpGet]
        public IEnumerable<Agendamento> Get()
        {
            List<Agendamento> agendamentoLista = _cache.Get<List<Agendamento>>("AgendamentoLista");
            if (agendamentoLista == null)
            {
                agendamentoLista = new List<Agendamento>();
            }
            return agendamentoLista;
        }

        // POST api/<AgendamentoController>
        [HttpPost]
        public IActionResult Post([FromBody] Agendamento agendamentoAdicionar)
        {
            return Ok(AgendamentoAdicionar(agendamentoAdicionar));
        }

        private IActionResult AgendamentoAdicionar(Agendamento agendamentoAdicionar)
        {
            string adicionarSituacao = "Falha";
            List<Agendamento> agendamentoLista = _cache.Get<List<Agendamento>>("AgendamentoLista");
            if (agendamentoLista == null)
            {
                agendamentoLista = new List<Agendamento>();
            }
            if (agendamentoLista != null &&
                ValidarVagaDisponivel(agendamentoLista, agendamentoAdicionar) &&
                !ValidarVagaConflito(agendamentoLista, agendamentoAdicionar) &&
                ValidarVagaTempoOcupacao(agendamentoAdicionar) &&
                !ValidarVagaSimultaneo(agendamentoLista, agendamentoAdicionar) &&
                !ValidarVagaLimite(agendamentoLista) &&
                ValidarVagaHorario(agendamentoAdicionar)
                )
            {
                agendamentoLista.Add(agendamentoAdicionar);
                adicionarSituacao = "Sucesso";
            }
            _cache.Set<List<Agendamento>>("AgendamentoLista", agendamentoLista);
            return Ok($"Registro {agendamentoAdicionar.AgendamentoCarreta} Adicionado -- {adicionarSituacao} --");
        }

        private bool ValidarVagaDisponivel(List<Agendamento> agendamentoLista, Agendamento agendamentoAdicionar)
        {
            //Tratamento 1a
            return agendamentoLista.FindAll(agendamento =>
                agendamentoAdicionar.AgendamentoInicio > agendamento.AgendamentoInicio.AddMinutes(-30) &&
                agendamentoAdicionar.AgendamentoInicio < agendamento.AgendamentoTermino.AddMinutes(+30)
                ).Count <= 3;
        }

        private bool ValidarVagaConflito(List<Agendamento> agendamentoLista, Agendamento agendamentoAdicionar)
        {
            //Tratamento 1b, 1c
            return agendamentoLista.FindAll(agendamento =>
                agendamentoAdicionar.AgendamentoInicio > agendamento.AgendamentoInicio.AddMinutes(-30) &&
                agendamentoAdicionar.AgendamentoInicio < agendamento.AgendamentoTermino.AddMinutes(+30) &&
                int.Parse(agendamentoAdicionar.AgendamentoVaga.ToString()) == int.Parse(agendamento.AgendamentoVaga.ToString()) &&
                int.Parse(agendamentoAdicionar.AgendamentoVaga.ToString()) > 0 &&
                int.Parse(agendamentoAdicionar.AgendamentoVaga.ToString()) <= 3
                ).Count > 0;
        }

        private bool ValidarVagaTempoOcupacao(Agendamento agendamentoAdicionar)
        {
            //Tratamento 1d
            return (agendamentoAdicionar.AgendamentoTermino - agendamentoAdicionar.AgendamentoInicio).TotalMinutes <= 60;
        }

        private bool ValidarVagaSimultaneo(List<Agendamento> agendamentoLista, Agendamento agendamentoAdicionar)
        {
            //Tratamento 1e
            return agendamentoLista.FindAll(agendamento =>
                agendamentoAdicionar.AgendamentoInicio > agendamento.AgendamentoInicio.AddMinutes(-30) &&
                agendamentoAdicionar.AgendamentoInicio < agendamento.AgendamentoTermino.AddMinutes(+30) &&
                agendamentoAdicionar.AgendamentoFornecedor == agendamento.AgendamentoFornecedor
                ).Count >= 2;
        }

        private bool ValidarVagaLimite(List<Agendamento> agendamentoLista)
        {
            //Tratamento 1f
            return agendamentoLista.Count > 11;
        }

        private bool ValidarVagaHorario(Agendamento agendamentoAdicionar)
        {
            //Tratamento 1g
            return (agendamentoAdicionar.AgendamentoInicio.Hour >= 8 && agendamentoAdicionar.AgendamentoInicio.Hour < 12 && agendamentoAdicionar.AgendamentoTermino.Hour > 8 && agendamentoAdicionar.AgendamentoTermino.Hour <= 12) ||
                (agendamentoAdicionar.AgendamentoInicio.Hour >= 14 && agendamentoAdicionar.AgendamentoInicio.Hour < 18 && agendamentoAdicionar.AgendamentoTermino.Hour > 14 && agendamentoAdicionar.AgendamentoTermino.Hour <= 18);
        }
    }
}

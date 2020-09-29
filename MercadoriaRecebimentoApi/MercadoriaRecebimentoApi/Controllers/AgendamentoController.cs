using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MercadoriaRecebimentoApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MercadoriaRecebimentoApi.Controllers
{
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
            List<Agendamento> agendamentoLista;
            try
            {
                if (!_cache.TryGetValue<List<Agendamento>>("AgendamentoLista", out agendamentoLista))
                {
                    agendamentoLista = ConfiguracaoInicial();
                    _cache.Set("AgendamentoLista", agendamentoLista);
                }
            }
            catch (Exception ex)
            {
                agendamentoLista = new List<Agendamento>();
                Debug.WriteLine(ex);
            }
            //return new string[] { _config.GetValue<string>("EstoqueParametros:TotalDia"), _config.GetValue<string>("EstoqueParametros:TotalVagas") };
            return agendamentoLista;
        }

        // POST api/<AgendamentoController>
        [HttpPost]
        public void Post([FromBody] Agendamento agendamentoAdicionar)
        {
            Debug.WriteLine(agendamentoAdicionar);
        }

        private List<Agendamento> ConfiguracaoInicial()
        {
            List<Agendamento> agendamentoLista = new List<Agendamento>();
            for (int agendamentoNumero = 1; agendamentoNumero <= 10; agendamentoNumero++)
            {
                agendamentoLista.Add(new Agendamento
                {
                    AgendamentoCarreta = $"Carreta{agendamentoNumero.ToString().PadLeft(2, '0')}",
                    AgendamentoFornecedor = $"Fornacedor{(agendamentoNumero % 5).ToString().PadLeft(2, '0')}",
                    AgendamentoInicio = DateTime.Today.AddHours(8),
                    AgendamentoTermino = DateTime.Today.AddHours(8),
                    AgendamentoVaga = agendamentoNumero % 4,
                });
            }
            return agendamentoLista;
        }
    }
}

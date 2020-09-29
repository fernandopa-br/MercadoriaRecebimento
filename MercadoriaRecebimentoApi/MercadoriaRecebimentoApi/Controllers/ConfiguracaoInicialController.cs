using System;
using System.Collections.Generic;
using MercadoriaRecebimentoApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MercadoriaRecebimentoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfiguracaoInicialController : ControllerBase
    {
        private readonly IConfiguration _config;
        private IMemoryCache _cache;

        public ConfiguracaoInicialController(IConfiguration config, IMemoryCache memoryCache)
        {
            _config = config;
            _cache = memoryCache;
        }
        
        // GET: api/<ConfiguracaoInicialController>
        [HttpGet]
        public IEnumerable<Agendamento> Get()
        {
            List<Agendamento> agendamentoLista;
            if (!_cache.TryGetValue<List<Agendamento>>("AgendamentoLista", out agendamentoLista))
            {
                agendamentoLista = ConfiguracaoInicial();
                _cache.Set("AgendamentoLista", agendamentoLista);
            }
            return agendamentoLista;
        }

        private List<Agendamento> ConfiguracaoInicial()
        {
            List<Agendamento> agendamentoLista = new List<Agendamento>();
            int horaInicio = 6;
            int vagaNumero = 1;

            for (int agendamentoNumero = 1; agendamentoNumero <= 10; agendamentoNumero++)
            {
                horaInicio += 2;
                if (horaInicio > 11 && horaInicio < 14)
                    horaInicio = 14;

                if (horaInicio > 17)
                {
                    vagaNumero++;
                    horaInicio = 8;
                }

                agendamentoLista.Add(new Agendamento
                {
                    AgendamentoCarreta = $"Carreta{agendamentoNumero.ToString().PadLeft(2, '0')}",
                    AgendamentoFornecedor = $"Fornacedor{vagaNumero.ToString().PadLeft(2, '0')}",
                    AgendamentoInicio = DateTime.Today.AddHours(horaInicio),
                    AgendamentoTermino = DateTime.Today.AddHours(horaInicio + 1),
                    AgendamentoVaga = vagaNumero
                });
            }
            return agendamentoLista;
        }


    }
}

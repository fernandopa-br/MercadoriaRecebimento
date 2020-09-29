using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MercadoriaRecebimentoApi.Data
{
    public class Agendamento
    {
        public String AgendamentoCarreta { get; set; }
        public String AgendamentoFornecedor { get; set; }
        public DateTime AgendamentoInicio { get; set; }
        public DateTime AgendamentoTermino { get; set; }
        public object AgendamentoVaga { get; set; }
    }
}

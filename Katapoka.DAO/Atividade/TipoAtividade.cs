using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Katapoka.DAO.Atividade
{
    public class TipoAtividade
    {
        public int? IdTipoAtividade { get; set; }
        public string DsTituloAtividade { get; set; }
        public decimal QtTempoEstimado { get; set; }
        public string DsAtividade { get; set; }
        public int? IdTipoAtividadePredecessora { get; set; }
        public int IdTipoProjeto { get; set; }
    }
}

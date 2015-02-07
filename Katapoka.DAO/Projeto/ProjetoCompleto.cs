using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Katapoka.DAO.Projeto
{
    public class ProjetoCompleto
    {
        public int IdProjeto { get; set; }
        public int IdEmpresa { get; set; }
        public int IdTipoProjeto { get; set; }
        public string DsNome { get; set; }
        public DateTime? DtInicioEstimado { get; set; }
        public DateTime? DtTerminoEstimado { get; set; }
        public string DsCodigoReferencia { get; set; }
        public string FlStatus { get; set; }
        public int IdUsuarioCriacao { get; set; }
        public DateTime DtCriacao { get; set; }

    }
}

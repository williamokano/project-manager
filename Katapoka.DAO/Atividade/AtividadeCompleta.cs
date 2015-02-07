using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Katapoka.DAO.Atividade
{
    public class AtividadeCompleta
    {
        public int? IdAtividade { get; set; }
        public int IdAtividadeLocal { get; set; }
        public string DsNomeAtividade { get; set; }
        public int[] IdUsuariosResponsaveis { get; set; }
        public Decimal QtHoras { get; set; }
        public int? IdPreAtividade { get; set; }
    }
}

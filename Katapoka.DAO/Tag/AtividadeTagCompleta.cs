using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Katapoka.DAO.Tag
{
    public class AtividadeTagCompleta
    {
        public int? IdAtividadeTag { get; set; }
        public int IdAtividade { get; set; }
        public int IdTag { get; set; }
        public string DsTag { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Katapoka.DAO.Atividade
{
    public class AtividadeAjaxPost
    {
        public string DsAtividade { get; set; }
        public string DsNomeAtividade { get; set; }
        public string DtInicioStr { get; set; }
        public string DtTerminoStr { get; set; }
        public int? IdAtividade { get; set; }
        public int IdAtividadeLocal { get; set; }
        public int? IdAtividadePredecessora { get; set; }
        public string QtTempoEstimado { get; set; }
        public string QtTempoExecutado { get; set; }
        public int? VrCompletoPorcentagem { get; set; }
        public List<Tag.TagCompleta> Tags { get; set; }
        public List<UsuarioCompleto> Usuarios { get; set; }
    }
}

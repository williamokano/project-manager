using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Katapoka.DAO;

namespace Katapoka.BLL.Projeto
{
    public class TipoProjetoTipoAtividadeBLL : AbstractBLLModel<TipoProjetoTipoAtividade_Tb>
    {
        /// <summary>
        /// Recupera um TipoProjetoTipoAtividade_Tb caso exista ou então um null
        /// </summary>
        /// <param name="idTipoProjeto"></param>
        /// <param name="idTipoAtividade"></param>
        /// <returns></returns>
        public TipoProjetoTipoAtividade_Tb GetTipoProjetoTipoAtividade(int idTipoProjeto, int idTipoAtividade)
        {
            return this.Context.TipoProjetoTipoAtividade_Tb
                .Where(p => p.IdTipoAtividade == idTipoAtividade && p.IdTipoProjeto == idTipoProjeto)
                .FirstOrDefault();
        }
    }
}

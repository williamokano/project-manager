using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Katapoka.DAO;

namespace Katapoka.BLL.Tag
{ 
    public class TipoAtividadeTagBLL : AbstractBLLModel<TipoAtividadeTag_Tb>
    {
        public IList<Katapoka.DAO.Tag_Tb> GetTagsTipoAtividade(int idTipoAtividade)
        {
            return this.Context.TipoAtividadeTag_Tb
                .Join(this.Context.Tag_Tb, p => p.IdTag, p => p.IdTag, (tb1, tb2) => new { TipoAtividadeTab_Tb = tb1, Tag_Tb = tb2 })
                .Where(p => p.TipoAtividadeTab_Tb.IdTipoAtividade == idTipoAtividade)
                .Select(p => p.Tag_Tb)
                .ToList();
        }
    }
}

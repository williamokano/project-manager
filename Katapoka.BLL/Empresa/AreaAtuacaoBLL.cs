using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Katapoka.BLL.Empresa
{
    public class AreaAtuacaoBLL :  AbstractBLLModel<Katapoka.DAO.AreaAtuacao_Tb>
    {
        public IList<Katapoka.DAO.AreaAtuacao_Tb> GetAll()
        {
            return this.Context.AreaAtuacao_Tb
                .OrderBy(p => p.DsAreaAtuacao)
                .ToList();
        }
    }
}

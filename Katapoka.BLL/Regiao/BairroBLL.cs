using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Katapoka.BLL.Regiao
{
    public class BairroBLL : AbstractBLLModel<Katapoka.DAO.Bairro_Tb>
    {
        /// <summary>
        /// Return all the neighborhoods of the city
        /// </summary>
        /// <param name="idCidade">The id of the selected city</param>
        /// <returns></returns>
        public IList<Katapoka.DAO.Bairro_Tb> GetAll(int idCidade)
        {
            return this.Context.Bairro_Tb
                .Where(p => p.IdCidade == idCidade)
                .OrderBy(p => p.DsNome)
                .ToList();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Katapoka.BLL.Regiao
{
    public class CidadeBLL : AbstractBLLModel<Katapoka.DAO.Cidade_Tb>
    {
        /// <summary>
        /// Get all the cities of the selected state
        /// </summary>
        /// <param name="CdUF">id</param>
        /// <returns>List of the cities</returns>
        public IList<Katapoka.DAO.Cidade_Tb> GetAll(string CdUF)
        {
            return this.Context.Cidade_Tb
                .Where(p => p.CdUF == CdUF)
                .OrderBy(p => p.DsNome)
                .ToList();
        }
        public IList<Katapoka.DAO.Cidade_Tb> GetAll()
        {
            return this.Context.Cidade_Tb
                .OrderBy(p => p.DsNome)
                .ToList();
        }
    }
}

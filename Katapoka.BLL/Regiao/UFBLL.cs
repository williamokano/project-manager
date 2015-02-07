using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Katapoka.BLL.Regiao
{
    public class UFBLL : AbstractBLLModel<Katapoka.DAO.UF_Tb>
    {

        /// <summary>
        /// Return all the states ordered by their name
        /// </summary>
        /// <returns>List containing all states</returns>
        public IList<Katapoka.DAO.UF_Tb> GetAll()
        {
            return this.Context
                .UF_Tb
                .OrderBy(p => p.CdUF)
                .ToList();
        }
    }
}

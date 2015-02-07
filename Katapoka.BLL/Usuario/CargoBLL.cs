using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Katapoka.BLL.Usuario
{
    public class CargoBLL:AbstractBLLModel<Katapoka.DAO.Cargo_Tb>
    {
        public IList<Katapoka.DAO.Cargo_Tb> GetAll()
        {
            return this.Context.Cargo_Tb
                .OrderBy(p => p.DsCargo)
                .ToList();
        }
    }
}
